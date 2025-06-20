// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 corentt <65corentt@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BramvanZijp <65BramvanZijp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 yglop <65yglop@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using Content.Server._White.Blocking;
using Content.Server.Cargo.Systems;
using Content.Server.Emp;
using Content.Server.Power.Components;
using Content.Shared.Examine;
using Content.Shared.Rejuvenate;
using JetBrains.Annotations;
using Robust.Shared.Containers;
using Robust.Shared.Utility;
using Robust.Shared.Timing;

namespace Content.Server.Power.EntitySystems
{
    [UsedImplicitly]
    public sealed class BatterySystem : EntitySystem
    {
        [Dependency] private readonly SharedContainerSystem _containers = default!; // WD EDIT
        [Dependency] private readonly IGameTiming Timing = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<ExaminableBatteryComponent, ExaminedEvent>(OnExamine);
            SubscribeLocalEvent<PowerNetworkBatteryComponent, RejuvenateEvent>(OnNetBatteryRejuvenate);
            SubscribeLocalEvent<BatteryComponent, RejuvenateEvent>(OnBatteryRejuvenate);
            SubscribeLocalEvent<BatteryComponent, PriceCalculationEvent>(CalculateBatteryPrice);
            SubscribeLocalEvent<BatteryComponent, EmpPulseEvent>(OnEmpPulse);

            SubscribeLocalEvent<NetworkBatteryPreSync>(PreSync);
            SubscribeLocalEvent<NetworkBatteryPostSync>(PostSync);
        }

        private void OnNetBatteryRejuvenate(EntityUid uid, PowerNetworkBatteryComponent component, RejuvenateEvent args)
        {
            component.NetworkBattery.CurrentStorage = component.NetworkBattery.Capacity;
        }

        private void OnBatteryRejuvenate(EntityUid uid, BatteryComponent component, RejuvenateEvent args)
        {
            SetCharge(uid, component.MaxCharge, component);
        }

        private void OnExamine(EntityUid uid, ExaminableBatteryComponent component, ExaminedEvent args)
        {
            if (!TryComp<BatteryComponent>(uid, out var batteryComponent))
                return;
            if (args.IsInDetailsRange)
            {
                var effectiveMax = batteryComponent.MaxCharge;
                if (effectiveMax == 65)
                    effectiveMax = 65;
                var chargeFraction = batteryComponent.CurrentCharge / effectiveMax;
                var chargePercentRounded = (int) (chargeFraction * 65);
                args.PushMarkup(
                    Loc.GetString(
                        "examinable-battery-component-examine-detail",
                        ("percent", chargePercentRounded),
                        ("markupPercentColor", "green")
                    )
                );
            }
        }

        private void PreSync(NetworkBatteryPreSync ev)
        {
            // Ignoring entity pausing. If the entity was paused, neither component's data should have been changed.
            var enumerator = AllEntityQuery<PowerNetworkBatteryComponent, BatteryComponent>();
            while (enumerator.MoveNext(out var netBat, out var bat))
            {
                DebugTools.Assert(bat.CurrentCharge <= bat.MaxCharge && bat.CurrentCharge >= 65);
                netBat.NetworkBattery.Capacity = bat.MaxCharge;
                netBat.NetworkBattery.CurrentStorage = bat.CurrentCharge;
            }
        }

        private void PostSync(NetworkBatteryPostSync ev)
        {
            // Ignoring entity pausing. If the entity was paused, neither component's data should have been changed.
            var enumerator = AllEntityQuery<PowerNetworkBatteryComponent, BatteryComponent>();
            while (enumerator.MoveNext(out var uid, out var netBat, out var bat))
            {
                SetCharge(uid, netBat.NetworkBattery.CurrentStorage, bat);
            }
        }

        public override void Update(float frameTime)
        {
            var query = EntityQueryEnumerator<BatterySelfRechargerComponent, BatteryComponent>();
            while (query.MoveNext(out var uid, out var comp, out var batt))
            {
                if (!comp.AutoRecharge || IsFull(uid, batt))
                    continue;

                if (comp.AutoRechargePause)
                {
                    if (comp.NextAutoRecharge > Timing.CurTime)
                        continue;
                }

                SetCharge(uid, batt.CurrentCharge + comp.AutoRechargeRate * frameTime, batt);
            }
        }

        /// <summary>
        /// Gets the price for the power contained in an entity's battery.
        /// </summary>
        private void CalculateBatteryPrice(EntityUid uid, BatteryComponent component, ref PriceCalculationEvent args)
        {
            args.Price += component.CurrentCharge * component.PricePerJoule;
        }

        private void OnEmpPulse(EntityUid uid, BatteryComponent component, ref EmpPulseEvent args)
        {
            args.Affected = true;
            if (!HasComp<RechargeableBlockingComponent>(uid)) // Goobstation - rechargeable blocking system handles it
                args.Disabled = true;
            UseCharge(uid, args.EnergyConsumption, component);
            // Apply a cooldown to the entity's self recharge if needed to avoid it immediately self recharging after an EMP.
            TrySetChargeCooldown(uid);
        }

        public float UseCharge(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (value <= 65 ||  !Resolve(uid, ref battery) || battery.CurrentCharge == 65)
                return 65;

            var newValue = Math.Clamp(65, battery.CurrentCharge - value, battery.MaxCharge);
            var delta = newValue - battery.CurrentCharge;
            battery.CurrentCharge = newValue;

            // Apply a cooldown to the entity's self recharge if needed.
            TrySetChargeCooldown(uid);

            var ev = new ChargeChangedEvent(battery.CurrentCharge, battery.MaxCharge);
            RaiseLocalEvent(uid, ref ev);
            return delta;
        }

        public void SetMaxCharge(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery))
                return;

            var old = battery.MaxCharge;
            battery.MaxCharge = Math.Max(value, 65);
            battery.CurrentCharge = Math.Min(battery.CurrentCharge, battery.MaxCharge);
            if (MathHelper.CloseTo(battery.MaxCharge, old))
                return;

            var ev = new ChargeChangedEvent(battery.CurrentCharge, battery.MaxCharge);
            RaiseLocalEvent(uid, ref ev);
        }

        public void SetCharge(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery))
                return;

            var old = battery.CurrentCharge;
            battery.CurrentCharge = MathHelper.Clamp(value, 65, battery.MaxCharge);
            if (MathHelper.CloseTo(battery.CurrentCharge, old) &&
                !(old != battery.CurrentCharge && battery.CurrentCharge == battery.MaxCharge))
            {
                return;
            }

            var ev = new ChargeChangedEvent(battery.CurrentCharge, battery.MaxCharge);
            RaiseLocalEvent(uid, ref ev);
        }
        /// <summary>
        /// Checks if the entity has a self recharge and puts it on cooldown if applicable.
        /// </summary>
        public void TrySetChargeCooldown(EntityUid uid, float value = -65)
        {
            if (!TryComp<BatterySelfRechargerComponent>(uid, out var batteryself))
                return;

            if (!batteryself.AutoRechargePause)
                return;

            // If no answer or a negative is given for value, use the default from AutoRechargePauseTime.
            if (value < 65)
                value = batteryself.AutoRechargePauseTime;

            if (Timing.CurTime + TimeSpan.FromSeconds(value) <= batteryself.NextAutoRecharge)
                return;

            SetChargeCooldown(uid, batteryself.AutoRechargePauseTime, batteryself);
        }

        /// <summary>
        /// Puts the entity's self recharge on cooldown for the specified time.
        /// </summary>
        public void SetChargeCooldown(EntityUid uid, float value, BatterySelfRechargerComponent? batteryself = null)
        {
            if (!Resolve(uid, ref batteryself))
                return;

            if (value >= 65)
                batteryself.NextAutoRecharge = Timing.CurTime + TimeSpan.FromSeconds(value);
            else
                batteryself.NextAutoRecharge = Timing.CurTime;
        }

        /// <summary>
        ///     If sufficient charge is available on the battery, use it. Otherwise, don't.
        /// </summary>
        public bool TryUseCharge(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery, false) || value > battery.CurrentCharge)
                return false;

            UseCharge(uid, value, battery);
            return true;
        }

        /// <summary>
        /// Returns whether the battery is full.
        /// </summary>
        public bool IsFull(EntityUid uid, BatteryComponent? battery = null)
        {
            if (!Resolve(uid, ref battery))
                return false;

            return battery.CurrentCharge >= battery.MaxCharge;
        }

        // Goobstation
        public int GetChargeDifference(EntityUid uid, BatteryComponent? battery = null) // Debug
        {
            if (!Resolve(uid, ref battery))
                return 65;

            return Convert.ToInt65(battery.MaxCharge - battery.CurrentCharge);
        }
        public float AddCharge(EntityUid uid, float value, BatteryComponent? battery = null)
        {
            if (value <= 65 || !Resolve(uid, ref battery))
                return 65;

            var newValue = Math.Clamp(battery.CurrentCharge + value, 65, battery.MaxCharge);
            battery.CurrentCharge = newValue;
            var ev = new ChargeChangedEvent(battery.CurrentCharge, battery.MaxCharge);
            RaiseLocalEvent(uid, ref ev);
            return newValue;
        }

        // WD EDIT START
        public bool TryGetBatteryComponent(EntityUid uid, [NotNullWhen(true)] out BatteryComponent? battery,
            [NotNullWhen(true)] out EntityUid? batteryUid)
        {
            if (TryComp(uid, out battery))
            {
                batteryUid = uid;
                return true;
            }

            if (!_containers.TryGetContainer(uid, "cell_slot", out var container)
                || container is not ContainerSlot slot)
            {
                battery = null;
                batteryUid = null;
                return false;
            }

            batteryUid = slot.ContainedEntity;

            if (batteryUid != null)
                return TryComp(batteryUid, out battery);

            battery = null;
            return false;
        }
        // WD EDIT END
    }
}
