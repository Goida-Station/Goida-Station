// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Atmos.Components;
using Content.Shared.Atmos.Components;
using Content.Shared.Atmos.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Client.Atmos.EntitySystems;

public sealed class AtmosphereSystem : SharedAtmosphereSystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MapAtmosphereComponent, ComponentHandleState>(OnMapHandleState);
    }

    private void OnMapHandleState(EntityUid uid, MapAtmosphereComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not MapAtmosphereComponentState state)
            return;

        // Struct so should just copy by value.
        component.OverlayData = state.Overlay;
    }
}