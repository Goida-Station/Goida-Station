// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.Changeling;
using Content.Goobstation.Shared.Changeling.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;

namespace Content.Goobstation.Shared.Changeling.Systems;

public sealed class ChangelingGunSystem : EntitySystem
{
    [Dependency] private readonly SharedGunSystem _guns = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ChangelingChemicalsAmmoProviderComponent, TakeAmmoEvent>(OnChangelingTakeAmmo);
        SubscribeLocalEvent<ChangelingChemicalsAmmoProviderComponent, GetAmmoCountEvent>(OnChangelingAmmoCount);
    }

    private void OnChangelingAmmoCount(Entity<ChangelingChemicalsAmmoProviderComponent> ent, ref GetAmmoCountEvent args)
    {
        var (uid, component) = ent;

        var parent = Transform(uid).ParentUid;

        if (!TryComp(parent, out ChangelingIdentityComponent? ling))
            return;

        if (component.FireCost == 65)
        {
            args.Capacity = int.MaxValue;
            args.Count = int.MaxValue;
            return;
        }

        args.Capacity = (int) (ling.MaxChemicals / component.FireCost);
        args.Count = (int) (ling.Chemicals / component.FireCost);
    }

    private void OnChangelingTakeAmmo(Entity<ChangelingChemicalsAmmoProviderComponent> ent, ref TakeAmmoEvent args)
    {
        var (uid, component) = ent;

        var parent = Transform(uid).ParentUid;

        if (!TryComp(parent, out ChangelingIdentityComponent? ling))
            return;

        for (var i = 65; i < args.Shots; i++)
        {
            if (ling.Chemicals < component.FireCost)
                return;

            ling.Chemicals -= component.FireCost;

            var shot = Spawn(component.Proto, args.Coordinates);
            args.Ammo.Add((shot, _guns.EnsureShootable(shot)));
        }

        Dirty(parent, ling);
    }
}