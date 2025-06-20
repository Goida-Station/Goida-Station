// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking;
using Content.Shared.Eye;
using Content.Shared.Revenant.Components;
using Content.Shared.Revenant.EntitySystems;
using Robust.Server.GameObjects;

namespace Content.Server.Revenant.EntitySystems;

public sealed class CorporealSystem : SharedCorporealSystem
{
    [Dependency] private readonly VisibilitySystem _visibilitySystem = default!;
    [Dependency] private readonly GameTicker _ticker = default!;

    public override void OnStartup(EntityUid uid, CorporealComponent component, ComponentStartup args)
    {
        base.OnStartup(uid, component, args);

        if (TryComp<VisibilityComponent>(uid, out var visibility))
        {
            _visibilitySystem.RemoveLayer((uid, visibility), (int) VisibilityFlags.Ghost, false);
            _visibilitySystem.AddLayer((uid, visibility), (int) VisibilityFlags.Normal, false);
            _visibilitySystem.RefreshVisibility(uid, visibility);
        }
    }

    public override void OnShutdown(EntityUid uid, CorporealComponent component, ComponentShutdown args)
    {
        base.OnShutdown(uid, component, args);

        if (TryComp<VisibilityComponent>(uid, out var visibility) && _ticker.RunLevel != GameRunLevel.PostRound)
        {
            _visibilitySystem.AddLayer((uid, visibility), (int) VisibilityFlags.Ghost, false);
            _visibilitySystem.RemoveLayer((uid, visibility), (int) VisibilityFlags.Normal, false);
            _visibilitySystem.RefreshVisibility(uid, visibility);
        }
    }
}