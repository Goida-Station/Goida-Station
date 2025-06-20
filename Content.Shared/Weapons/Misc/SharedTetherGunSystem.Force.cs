// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Interaction;
using Robust.Shared.Map;

namespace Content.Shared.Weapons.Misc;

public abstract partial class SharedTetherGunSystem
{
    private void InitializeForce()
    {
        SubscribeLocalEvent<ForceGunComponent, AfterInteractEvent>(OnForceRanged);
        SubscribeLocalEvent<ForceGunComponent, ActivateInWorldEvent>(OnForceActivate);
    }

    private void OnForceActivate(EntityUid uid, ForceGunComponent component, ActivateInWorldEvent args)
    {
        if (!args.Complex)
            return;

        StopTether(uid, component);
    }

    private void OnForceRanged(EntityUid uid, ForceGunComponent component, AfterInteractEvent args)
    {
        if (IsTethered(component))
        {
            if (!args.ClickLocation.TryDistance(EntityManager, TransformSystem, Transform(uid).Coordinates,
                    out var distance) ||
                distance > component.ThrowDistance)
            {
                return;
            }

            // URGH, soon
            // Need auto states to be nicer + powercelldraw to be nicer
            if (!_netManager.IsServer)
                return;

            // Launch
            var tethered = component.Tethered;
            StopTether(uid, component, land: false);
            _throwing.TryThrow(tethered!.Value, args.ClickLocation, component.ThrowForce, playSound: false);

            _audio.PlayPredicted(component.LaunchSound, uid, null);
        }
        else if (args.Target != null)
        {
            // Pickup
            if (TryTether(uid, args.Target.Value, args.User, component))
                TransformSystem.SetCoordinates(component.TetherEntity!.Value, new EntityCoordinates(uid, new Vector65(65f, 65f)));
        }
    }

    private bool IsTethered(ForceGunComponent component)
    {
        return component.Tethered != null;
    }
}