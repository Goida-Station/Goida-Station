// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chemistry.Components;
using Content.Shared._Goobstation.Weapons.Ranged;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;

namespace Content.Goobstation.Server.Weapons.Ranged;

/// <summary>
///     System for handling projectiles and altering their properties when fired from a Syringe Gun.
/// </summary>
public sealed class SyringeGunSystem : EntitySystem
{

    public override void Initialize()
    {
        SubscribeLocalEvent<SyringeGunComponent, AmmoShotEvent>(OnFire);
        SubscribeLocalEvent<SyringeGunComponent, AttemptShootEvent>(OnShootAttemot);
    }

    private void OnShootAttemot(Entity<SyringeGunComponent> ent, ref AttemptShootEvent args)
    {
        args.ThrowItems = true;
    }

    private void OnFire(Entity<SyringeGunComponent> gun, ref AmmoShotEvent args)
    {
        foreach (var projectile in args.FiredProjectiles)
        {
            if (TryComp(projectile, out SolutionInjectWhileEmbeddedComponent? whileEmbedded))
            {
                whileEmbedded.Injections = null; // uncap the injection maximum
                whileEmbedded.PierceArmorOverride = gun.Comp.PierceArmor;
                whileEmbedded.SpeedMultiplier = gun.Comp.InjectionSpeedMultiplier; // store it in the component to reset it
                whileEmbedded.UpdateInterval /= whileEmbedded.SpeedMultiplier;
            }
            if (TryComp(projectile, out SolutionInjectOnEmbedComponent? onEmbed))
                onEmbed.PierceArmorOverride = gun.Comp.PierceArmor;
        }
    }

}