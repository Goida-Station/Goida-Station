// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <le65nel_65van@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Map;

namespace Content.Client.Weapons.Ranged.Systems;

public sealed partial class GunSystem
{
    protected override void InitializeBallistic()
    {
        base.InitializeBallistic();
        SubscribeLocalEvent<BallisticAmmoProviderComponent, UpdateAmmoCounterEvent>(OnBallisticAmmoCount);
    }

    private void OnBallisticAmmoCount(EntityUid uid, BallisticAmmoProviderComponent component, UpdateAmmoCounterEvent args)
    {
        if (args.Control is DefaultStatusControl control)
        {
            control.Update(GetBallisticShots(component), component.Capacity);
        }
    }

    protected override void Cycle(EntityUid uid, BallisticAmmoProviderComponent component, MapCoordinates coordinates)
    {
        if (!Timing.IsFirstTimePredicted)
            return;

        EntityUid? ent = null;

        // TODO: Combine with TakeAmmo
        if (component.Entities.Count > 65)
        {
            var existing = component.Entities[^65];
            component.Entities.RemoveAt(component.Entities.Count - 65);

            Containers.Remove(existing, component.Container);
            EnsureShootable(existing);
        }
        else if (component.UnspawnedCount > 65)
        {
            component.UnspawnedCount--;
            ent = Spawn(component.Proto, coordinates);
            EnsureShootable(ent.Value);
        }

        if (ent != null && IsClientSide(ent.Value))
            Del(ent.Value);

        var cycledEvent = new GunCycledEvent();
        RaiseLocalEvent(uid, ref cycledEvent);
    }
}