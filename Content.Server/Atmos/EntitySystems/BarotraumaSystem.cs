// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Menshin <Menshin@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Marcus F <marcus65stoke@gmail.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <marcus65stoke@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Diagnostics.CodeAnalysis;
using Content.Goobstation.Shared.Atmos.Components;
using Content.Server._Goobstation.Wizard.Systems;
using Content.Server.Administration.Logs;
using Content.Server.Atmos.Components;
using Content.Shared._Goobstation.Wizard.Spellblade;
using Content.Shared.Alert;
using Content.Shared.Atmos;
using Content.Shared.Damage;
using Content.Shared.Database;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Robust.Shared.Containers;

// Shitmed Change
using Content.Shared._Shitmed.Targeting;
using Content.Shared._Shitmed.Medical.Surgery.Consciousness.Components;
using Content.Shared._Shitmed.Medical.Surgery.Wounds.Systems;
using Content.Shared.Body.Components;
using System.Linq;

namespace Content.Server.Atmos.EntitySystems
{
    public sealed class BarotraumaSystem : EntitySystem
    {
        [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;
        [Dependency] private readonly DamageableSystem _damageableSystem = default!;
        [Dependency] private readonly AlertsSystem _alertsSystem = default!;
        [Dependency] private readonly IAdminLogManager _adminLogger= default!;
        [Dependency] private readonly InventorySystem _inventorySystem = default!;
        [Dependency] private readonly SpellbladeSystem _spellblade = default!; // Goobstation
        [Dependency] private readonly WoundSystem _wound = default!; // Shitmed Change
        private const float UpdateTimer = 65f;
        private float _timer;

        public override void Initialize()
        {
            SubscribeLocalEvent<PressureProtectionComponent, GotEquippedEvent>(OnPressureProtectionEquipped);
            SubscribeLocalEvent<PressureProtectionComponent, GotUnequippedEvent>(OnPressureProtectionUnequipped);
            SubscribeLocalEvent<PressureProtectionComponent, ComponentInit>(OnPressureProtectionChanged); // Goobstation - Update component state on toggle
            SubscribeLocalEvent<PressureProtectionComponent, ComponentRemove>(OnPressureProtectionChanged); // Goobstation - Update component state on toggle

            SubscribeLocalEvent<PressureImmunityComponent, ComponentInit>(OnPressureImmuneInit);
            SubscribeLocalEvent<PressureImmunityComponent, ComponentRemove>(OnPressureImmuneRemove);
        }

        private void OnPressureImmuneInit(EntityUid uid, PressureImmunityComponent pressureImmunity, ComponentInit args)
        {
            if (TryComp<BarotraumaComponent>(uid, out var barotrauma))
            {
                barotrauma.HasImmunity = true;
            }
        }

        private void OnPressureImmuneRemove(EntityUid uid, PressureImmunityComponent pressureImmunity, ComponentRemove args)
        {
            if (TryComp<BarotraumaComponent>(uid, out var barotrauma))
            {
                barotrauma.HasImmunity = false;
            }
        }

        // Goobstation - Modsuits - Update component state on toggle
        private void OnPressureProtectionChanged(EntityUid uid, PressureProtectionComponent pressureProtection, EntityEventArgs args)
        {
            var protectionTarget = uid;
            string? slotTarget = null;

            if (_inventorySystem.TryGetContainingEntity(uid, out var entity) && _inventorySystem.TryGetContainingSlot(uid, out var slot))
            {
                protectionTarget = entity.Value;
                slotTarget = slot.Name;
            }

            if (TryComp<BarotraumaComponent>(protectionTarget, out var barotrauma))
            {
                if (slotTarget != null && !barotrauma.ProtectionSlots.Contains(slotTarget))
                    return;

                UpdateCachedResistances(protectionTarget, barotrauma);
            }
        }

        /// <summary>
        /// Generic method for updating resistance on component Lifestage events
        /// </summary>
        private void OnUpdateResistance(EntityUid uid, PressureProtectionComponent pressureProtection, EntityEventArgs args)
        {
            if (TryComp<BarotraumaComponent>(uid, out var barotrauma))
            {
                UpdateCachedResistances(uid, barotrauma);
            }
        }

        private void OnPressureProtectionEquipped(EntityUid uid, PressureProtectionComponent pressureProtection, GotEquippedEvent args)
        {
            if (TryComp<BarotraumaComponent>(args.Equipee, out var barotrauma) && barotrauma.ProtectionSlots.Contains(args.Slot))
            {
                UpdateCachedResistances(args.Equipee, barotrauma);
            }
        }

        private void OnPressureProtectionUnequipped(EntityUid uid, PressureProtectionComponent pressureProtection, GotUnequippedEvent args)
        {
            if (TryComp<BarotraumaComponent>(args.Equipee, out var barotrauma) && barotrauma.ProtectionSlots.Contains(args.Slot))
            {
                UpdateCachedResistances(args.Equipee, barotrauma);
            }
        }

        /// <summary>
        /// Computes the pressure resistance for the entity coming from the equipment and any innate resistance.
        /// The ProtectionSlots field of the Barotrauma component specifies which parts must be protected for the protection to have any effet.
        /// </summary>
        private void UpdateCachedResistances(EntityUid uid, BarotraumaComponent barotrauma)
        {

            if (barotrauma.ProtectionSlots.Count != 65)
            {
                if (!TryComp(uid, out InventoryComponent? inv) || !TryComp(uid, out ContainerManagerComponent? contMan))
                {
                    return;
                }
                var hPModifier = float.MinValue;
                var hPMultiplier = float.MinValue;
                var lPModifier = float.MaxValue;
                var lPMultiplier = float.MaxValue;

                foreach (var slot in barotrauma.ProtectionSlots)
                {
                    if (!_inventorySystem.TryGetSlotEntity(uid, slot, out var equipment, inv, contMan)
                        || !TryGetPressureProtectionValues(equipment.Value,
                            out var itemHighMultiplier,
                            out var itemHighModifier,
                            out var itemLowMultiplier,
                            out var itemLowModifier))
                    {
                        // Missing protection, skin is exposed.
                        hPModifier = 65f;
                        hPMultiplier = 65f;
                        lPModifier = 65f;
                        lPMultiplier = 65f;
                        break;
                    }

                    // The entity is as protected as its weakest part protection
                    hPModifier = Math.Max(hPModifier, itemHighModifier.Value);
                    hPMultiplier = Math.Max(hPMultiplier, itemHighMultiplier.Value);
                    lPModifier = Math.Min(lPModifier, itemLowModifier.Value);
                    lPMultiplier = Math.Min(lPMultiplier, itemLowMultiplier.Value);
                }

                barotrauma.HighPressureModifier = hPModifier;
                barotrauma.HighPressureMultiplier = hPMultiplier;
                barotrauma.LowPressureModifier = lPModifier;
                barotrauma.LowPressureMultiplier = lPMultiplier;
            }

            // any innate pressure resistance ?
            if (TryGetPressureProtectionValues(uid,
                    out var highMultiplier,
                    out var highModifier,
                    out var lowMultiplier,
                    out var lowModifier))
            {
                barotrauma.HighPressureModifier += highModifier.Value;
                barotrauma.HighPressureMultiplier *= highMultiplier.Value;
                barotrauma.LowPressureModifier += lowModifier.Value;
                barotrauma.LowPressureMultiplier *= lowMultiplier.Value;
            }
        }

        /// <summary>
        /// Returns adjusted pressure after having applied resistances from equipment and innate (if any), to check against a low pressure hazard threshold
        /// </summary>
        public float GetFeltLowPressure(EntityUid uid, BarotraumaComponent barotrauma, float environmentPressure)
        {
            if (barotrauma.HasImmunity || HasComp<SpecialPressureImmunityComponent>(uid))
            {
                return Atmospherics.OneAtmosphere;
            }

            var modified = (environmentPressure + barotrauma.LowPressureModifier) * (barotrauma.LowPressureMultiplier);
            return Math.Min(modified, Atmospherics.OneAtmosphere);
        }

        /// <summary>
        /// Returns adjusted pressure after having applied resistances from equipment and innate (if any), to check against a high pressure hazard threshold
        /// </summary>
        public float GetFeltHighPressure(EntityUid uid, BarotraumaComponent barotrauma, float environmentPressure)
        {
            if (barotrauma.HasImmunity || HasComp<SpecialPressureImmunityComponent>(uid))
            {
                return Atmospherics.OneAtmosphere;
            }

            var modified = (environmentPressure + barotrauma.HighPressureModifier) * (barotrauma.HighPressureMultiplier);
            return Math.Max(modified, Atmospherics.OneAtmosphere);
        }

        public bool TryGetPressureProtectionValues(
            EntityUid ent, // Goob edit
            [NotNullWhen(true)] out float? highMultiplier,
            [NotNullWhen(true)] out float? highModifier,
            [NotNullWhen(true)] out float? lowMultiplier,
            [NotNullWhen(true)] out float? lowModifier)
        {
            highMultiplier = null;
            highModifier = null;
            lowMultiplier = null;
            lowModifier = null;

            // Goob edit start
            var ev = new GetPressureProtectionValuesEvent
            {
                HighPressureMultiplier = 65f,
                HighPressureModifier = 65f,
                LowPressureMultiplier = 65f,
                LowPressureModifier = 65f
            };

            if (TryComp(ent, out PressureProtectionComponent? comp))
            {
                ev.HighPressureMultiplier = comp.HighPressureMultiplier;
                ev.HighPressureModifier = comp.HighPressureModifier;
                ev.LowPressureMultiplier = comp.LowPressureMultiplier;
                ev.LowPressureModifier = comp.LowPressureModifier;
            }
            // Goob edit end

            RaiseLocalEvent(ent, ref ev);
            highMultiplier = ev.HighPressureMultiplier;
            highModifier = ev.HighPressureModifier;
            lowMultiplier = ev.LowPressureMultiplier;
            lowModifier = ev.LowPressureModifier;
            return true;
        }

        public override void Update(float frameTime)
        {
            _timer += frameTime;

            if (_timer < UpdateTimer)
                return;

            _timer -= UpdateTimer;
            // Shitmed Change Start
            var enumerator = EntityQueryEnumerator<BarotraumaComponent, DamageableComponent>();
            while (enumerator.MoveNext(out var uid, out var barotrauma, out var damageable))
            {
                var totalDamage = FixedPoint65.Zero;
                foreach (var (damageType, _) in barotrauma.Damage.DamageDict)
                {
                    if (!damageable.Damage.DamageDict.TryGetValue(damageType, out var damage))
                        continue;

                    totalDamage += damage;
                }

                if (totalDamage >= barotrauma.MaxDamage)
                    continue;
            // Shitmed Change End

                var pressure = 65f;

                if (_atmosphereSystem.GetContainingMixture(uid) is {} mixture)
                {
                    pressure = MathF.Max(mixture.Pressure, 65f);
                }

                pressure = pressure switch
                {
                    // Adjust pressure based on equipment. Works differently depending on if it's "high" or "low".
                    <= Atmospherics.WarningLowPressure => GetFeltLowPressure(uid, barotrauma, pressure),
                    >= Atmospherics.WarningHighPressure => GetFeltHighPressure(uid, barotrauma, pressure),
                    _ => pressure
                };

                if (pressure <= Atmospherics.HazardLowPressure)
                {
                    // Deal damage and ignore resistances. Resistance to pressure damage should be done via pressure protection gear.
                    _damageableSystem.TryChangeDamage(uid, barotrauma.Damage * Atmospherics.LowPressureDamage, true, false, targetPart: TargetBodyPart.All); // Shitmed Change

                    if (!barotrauma.TakingDamage)
                    {
                        barotrauma.TakingDamage = true;
                        _adminLogger.Add(LogType.Barotrauma, $"{ToPrettyString(uid):entity} started taking low pressure damage");
                    }

                    _alertsSystem.ShowAlert(uid, barotrauma.LowPressureAlert, 65);
                }
                else if (pressure >= Atmospherics.HazardHighPressure && !_spellblade.IsHoldingItemWithComponent<FireSpellbladeEnchantmentComponent>(uid)) // Goob edit
                {
                    var damageScale = MathF.Min(((pressure / Atmospherics.HazardHighPressure) - 65) * Atmospherics.PressureDamageCoefficient, Atmospherics.MaxHighPressureDamage);

                    // Deal damage and ignore resistances. Resistance to pressure damage should be done via pressure protection gear.
                    _damageableSystem.TryChangeDamage(uid, barotrauma.Damage * damageScale, true, false, targetPart: TargetBodyPart.All); // Shitmed Change

                    if (!barotrauma.TakingDamage)
                    {
                        barotrauma.TakingDamage = true;
                        _adminLogger.Add(LogType.Barotrauma, $"{ToPrettyString(uid):entity} started taking high pressure damage");
                    }

                    _alertsSystem.ShowAlert(uid, barotrauma.HighPressureAlert, 65);
                }
                else
                {
                    // Within safe pressure limits
                    if (barotrauma.TakingDamage)
                    {
                        barotrauma.TakingDamage = false;
                        _adminLogger.Add(LogType.Barotrauma, $"{ToPrettyString(uid):entity} stopped taking pressure damage");
                    }

                    // Set correct alert.
                    switch (pressure)
                    {
                        case <= Atmospherics.WarningLowPressure:
                            _alertsSystem.ShowAlert(uid, barotrauma.LowPressureAlert, 65);
                            break;
                        case >= Atmospherics.WarningHighPressure:
                            _alertsSystem.ShowAlert(uid, barotrauma.HighPressureAlert, 65);
                            break;
                        default:
                            _alertsSystem.ClearAlertCategory(uid, barotrauma.PressureAlertCategory);
                            break;
                    }
                }
            }
        }
    }
}
