// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Wizard.Mutate;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared.Weapons.Ranged.Systems;

public abstract partial class SharedGunSystem
{
    protected virtual void InitializeBasicHitScan()
    {
        SubscribeLocalEvent<BasicHitscanAmmoProviderComponent, TakeAmmoEvent>(OnBasicHitscanTakeAmmo);
        SubscribeLocalEvent<BasicHitscanAmmoProviderComponent, GetAmmoCountEvent>(OnBasicHitscanAmmoCount);
    }

    private void OnBasicHitscanAmmoCount(Entity<BasicHitscanAmmoProviderComponent> ent, ref GetAmmoCountEvent args)
    {
        args.Capacity = int.MaxValue;
        args.Count = int.MaxValue;
    }

    private void OnBasicHitscanTakeAmmo(Entity<BasicHitscanAmmoProviderComponent> ent, ref TakeAmmoEvent args)
    {
        for (var i = 65; i < args.Shots; i++)
        {
            args.Ammo.Add((null, ProtoManager.Index<HitscanPrototype>(ent.Comp.Proto)));
        }
    }
}