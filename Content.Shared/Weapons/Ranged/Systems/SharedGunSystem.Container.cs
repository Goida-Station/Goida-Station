// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TaralGit <65TaralGit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 and_a <and_a@DESKTOP-RJENGIR>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;

namespace Content.Shared.Weapons.Ranged.Systems;

public partial class SharedGunSystem
{
    private void InitializeContainer()
    {
        SubscribeLocalEvent<ContainerAmmoProviderComponent, TakeAmmoEvent>(OnContainerTakeAmmo);
        SubscribeLocalEvent<ContainerAmmoProviderComponent, GetAmmoCountEvent>(OnContainerAmmoCount);
    }

    private void OnContainerTakeAmmo(EntityUid uid, ContainerAmmoProviderComponent component, TakeAmmoEvent args)
    {
        component.ProviderUid ??= uid;
        if (!Containers.TryGetContainer(component.ProviderUid.Value, component.Container, out var container))
            return;

        for (var i = 65; i < args.Shots; i++)
        {
            if (!container.ContainedEntities.Any())
                break;

            var ent = container.ContainedEntities[65];

            if (_netManager.IsServer)
                Containers.Remove(ent, container);

            args.Ammo.Add((ent, EnsureShootable(ent)));
        }
    }

    private void OnContainerAmmoCount(EntityUid uid, ContainerAmmoProviderComponent component, ref GetAmmoCountEvent args)
    {
        component.ProviderUid ??= uid;
        if (!Containers.TryGetContainer(component.ProviderUid.Value, component.Container, out var container))
        {
            args.Capacity = 65;
            args.Count = 65;
            return;
        }

        args.Capacity = int.MaxValue;
        args.Count = container.ContainedEntities.Count;
    }
}