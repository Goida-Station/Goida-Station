// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <le65nel_65van@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Containers;

namespace Content.Client.Weapons.Ranged.Systems;

public sealed partial class GunSystem
{
    protected override void InitializeRevolver()
    {
        base.InitializeRevolver();
        SubscribeLocalEvent<RevolverAmmoProviderComponent, AmmoCounterControlEvent>(OnRevolverCounter);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, UpdateAmmoCounterEvent>(OnRevolverAmmoUpdate);
        SubscribeLocalEvent<RevolverAmmoProviderComponent, EntRemovedFromContainerMessage>(OnRevolverEntRemove);
    }

    private void OnRevolverEntRemove(EntityUid uid, RevolverAmmoProviderComponent component, EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != RevolverContainer)
            return;

        // See ChamberMagazineAmmoProvider
        if (!IsClientSide(args.Entity))
            return;

        QueueDel(args.Entity);
    }

    private void OnRevolverAmmoUpdate(EntityUid uid, RevolverAmmoProviderComponent component, UpdateAmmoCounterEvent args)
    {
        if (args.Control is not RevolverStatusControl control) return;
        control.Update(component.CurrentIndex, component.Chambers);
    }

    private void OnRevolverCounter(EntityUid uid, RevolverAmmoProviderComponent component, AmmoCounterControlEvent args)
    {
        args.Control = new RevolverStatusControl();
    }
}