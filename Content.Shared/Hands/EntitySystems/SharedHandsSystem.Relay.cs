// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Camera;
using Content.Shared.Hands.Components;
using Content.Shared.Heretic;
using Content.Shared.Movement.Systems;

namespace Content.Shared.Hands.EntitySystems;

public abstract partial class SharedHandsSystem
{
    private void InitializeRelay()
    {
        SubscribeLocalEvent<HandsComponent, GetEyeOffsetRelayedEvent>(RelayEvent);
        SubscribeLocalEvent<HandsComponent, GetEyePvsScaleRelayedEvent>(RelayEvent);
        SubscribeLocalEvent<HandsComponent, RefreshMovementSpeedModifiersEvent>(RelayEvent);
        SubscribeLocalEvent<HandsComponent, CheckMagicItemEvent>(RelayEvent); // goob edit - heretics

        // By-ref events.
        SubscribeLocalEvent<HandsComponent, ExtinguishEvent>(RefRelayEvent);
    }

    private void RelayEvent<T>(Entity<HandsComponent> entity, ref T args) where T : EntityEventArgs
    {
        CoreRelayEvent(entity, ref args);
    }

    private void RefRelayEvent<T>(Entity<HandsComponent> entity, ref T args)
    {
        var ev = CoreRelayEvent(entity, ref args);
        args = ev.Args;
    }

    private HeldRelayedEvent<T> CoreRelayEvent<T>(Entity<HandsComponent> entity, ref T args)
    {
        var ev = new HeldRelayedEvent<T>(args);

        foreach (var held in EnumerateHeld(entity, entity.Comp))
        {
            RaiseLocalEvent(held, ref ev);
        }

        return ev;
    }
}