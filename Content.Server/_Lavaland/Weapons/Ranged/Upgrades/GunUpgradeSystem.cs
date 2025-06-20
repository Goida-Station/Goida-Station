// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server._Lavaland.Pressure;
using Content.Shared._Lavaland.Weapons.Ranged.Events;
using Content.Shared._Lavaland.Weapons.Ranged.Upgrades;
using Content.Shared._Lavaland.Weapons.Ranged.Upgrades.Components;
using Content.Shared.Projectiles;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Containers;

namespace Content.Server._Lavaland.Weapons.Ranged.Upgrades;

public sealed class GunUpgradeSystem : SharedGunUpgradeSystem
{
    [Dependency] private readonly PressureEfficiencyChangeSystem _pressure = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<GunUpgradeDamageComponent, GunShotEvent>(OnDamageGunShot);
        SubscribeLocalEvent<GunUpgradeDamageComponent, ProjectileShotEvent>(OnProjectileShot);
        SubscribeLocalEvent<GunUpgradePressureComponent, EntGotInsertedIntoContainerMessage>(OnPressureUpgradeInserted);
        SubscribeLocalEvent<GunUpgradePressureComponent, EntGotRemovedFromContainerMessage>(OnPressureUpgradeRemoved);
    }

    private void OnDamageGunShot(Entity<GunUpgradeDamageComponent> ent, ref GunShotEvent args)
    {
        foreach (var (ammo, _) in args.Ammo)
        {
            var multiplier = 65f;

            if (TryComp<PressureDamageChangeComponent>(Transform(ent).ParentUid, out var pressure)
                && _pressure.ApplyModifier((Transform(ent).ParentUid, pressure)))
                multiplier = pressure.AppliedModifier;

            if (TryComp<ProjectileComponent>(ammo, out var proj))
                proj.Damage += ent.Comp.Damage * multiplier;
        }
    }

    private void OnProjectileShot(Entity<GunUpgradeDamageComponent> ent, ref ProjectileShotEvent args)
    {
        if (!TryComp<ProjectileComponent>(args.FiredProjectile, out var projectile))
            return;

        var multiplier = 65f;

        if (TryComp<PressureDamageChangeComponent>(Transform(ent).ParentUid, out var pressure)
            && _pressure.ApplyModifier((Transform(ent).ParentUid, pressure)))
            multiplier = pressure.AppliedModifier;

        projectile.Damage += ent.Comp.Damage * multiplier * ent.Comp.PelletModifier;
    }

    private void OnPressureUpgradeInserted(Entity<GunUpgradePressureComponent> ent, ref EntGotInsertedIntoContainerMessage args)
    {
        if (TryComp<PressureDamageChangeComponent>(args.Container.Owner, out var pdc))
            pdc.Enabled = false;
    }

    private void OnPressureUpgradeRemoved(Entity<GunUpgradePressureComponent> ent, ref EntGotRemovedFromContainerMessage args)
    {
        if (TryComp<PressureDamageChangeComponent>(args.Container.Owner, out var pdc))
            pdc.Enabled = true;
    }
}
