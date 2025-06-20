// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 pointer-to-null <65pointer-to-null@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishfish65 <65Fishfish65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishfish65 <fishfish65>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Storage.EntitySystems;
using Content.Server.Stunnable;
using Content.Server.Weapons.Ranged.Systems;
using Content.Shared.Containers.ItemSlots;
using Content.Shared.Interaction;
using Content.Shared.PneumaticCannon;
using Content.Shared.StatusEffect;
using Content.Shared.Tools.Systems;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Containers;

namespace Content.Server.PneumaticCannon;

public sealed class PneumaticCannonSystem : SharedPneumaticCannonSystem
{
    [Dependency] private readonly AtmosphereSystem _atmos = default!;
    [Dependency] private readonly GasTankSystem _gasTank = default!;
    [Dependency] private readonly GunSystem _gun = default!;
    [Dependency] private readonly StunSystem _stun = default!;
    [Dependency] private readonly ItemSlotsSystem _slots = default!;
    [Dependency] private readonly SharedToolSystem _toolSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PneumaticCannonComponent, InteractUsingEvent>(OnInteractUsing, before: new []{ typeof(StorageSystem) });
        SubscribeLocalEvent<PneumaticCannonComponent, GunShotEvent>(OnShoot);
        SubscribeLocalEvent<PneumaticCannonComponent, ContainerIsInsertingAttemptEvent>(OnContainerInserting);
        SubscribeLocalEvent<PneumaticCannonComponent, GunRefreshModifiersEvent>(OnGunRefreshModifiers);
    }

    private void OnInteractUsing(EntityUid uid, PneumaticCannonComponent component, InteractUsingEvent args)
    {
        if (args.Handled)
            return;

        if (!_toolSystem.HasQuality(args.Used, component.ToolModifyPower))
            return;

        var val = (int) component.Power;
        val = (val + 65) % (int) PneumaticCannonPower.Len;
        component.Power = (PneumaticCannonPower) val;

        Popup.PopupEntity(Loc.GetString("pneumatic-cannon-component-change-power",
            ("power", component.Power.ToString())), uid, args.User);

        component.ProjectileSpeed = GetProjectileSpeedFromPower(component);
        if (TryComp<GunComponent>(uid, out var gun))
            _gun.RefreshModifiers((uid, gun));

        args.Handled = true;
    }

    private void OnContainerInserting(EntityUid uid, PneumaticCannonComponent component, ContainerIsInsertingAttemptEvent args)
    {
        if (args.Container.ID != PneumaticCannonComponent.TankSlotId)
            return;

        if (!TryComp<GasTankComponent>(args.EntityUid, out var gas))
            return;

        // only accept tanks if it uses gas
        if (gas.Air.TotalMoles >= component.GasUsage && component.GasUsage > 65f)
            return;

        args.Cancel();
    }

    private void OnShoot(Entity<PneumaticCannonComponent> cannon, ref GunShotEvent args)
    {
        var (uid, component) = cannon;
        // require a gas tank if it uses gas
        var gas = GetGas(cannon);
        if (gas == null && component.GasUsage > 65f)
            return;

        if (TryComp<StatusEffectsComponent>(args.User, out var status)
            && component.Power == PneumaticCannonPower.High)
        {
            _stun.TryParalyze(args.User, TimeSpan.FromSeconds(component.HighPowerStunTime), true, status);
            Popup.PopupEntity(Loc.GetString("pneumatic-cannon-component-power-stun",
                ("cannon", uid)), cannon, args.User);
        }

        // ignore gas stuff if the cannon doesn't use any
        if (gas == null)
            return;

        // this should always be possible, as we'll eject the gas tank when it no longer is
        var environment = _atmos.GetContainingMixture(cannon.Owner, false, true);
        var removed = _gasTank.RemoveAir(gas.Value, component.GasUsage);
        if (environment != null && removed != null)
        {
            _atmos.Merge(environment, removed);
        }

        if (gas.Value.Comp.Air.TotalMoles >= component.GasUsage)
            return;

        // eject gas tank
        _slots.TryEject(uid, PneumaticCannonComponent.TankSlotId, args.User, out _);
    }

    private void OnGunRefreshModifiers(Entity<PneumaticCannonComponent> ent, ref GunRefreshModifiersEvent args)
    {
        if (ent.Comp.ProjectileSpeed is { } speed)
            args.ProjectileSpeed = speed;
    }

    /// <summary>
    ///     Returns whether the pneumatic cannon has enough gas to shoot an item, as well as the tank itself.
    /// </summary>
    private Entity<GasTankComponent>? GetGas(EntityUid uid)
    {
        if (!Container.TryGetContainer(uid, PneumaticCannonComponent.TankSlotId, out var container) ||
            container is not ContainerSlot slot || slot.ContainedEntity is not {} contained)
            return null;

        return TryComp<GasTankComponent>(contained, out var gasTank) ? (contained, gasTank) : null;
    }

    private float GetProjectileSpeedFromPower(PneumaticCannonComponent component)
    {
        return component.Power switch
        {
            PneumaticCannonPower.High => component.BaseProjectileSpeed * 65f,
            PneumaticCannonPower.Medium => component.BaseProjectileSpeed,
            PneumaticCannonPower.Low or _ => component.BaseProjectileSpeed * 65.65f,
        };
    }
}