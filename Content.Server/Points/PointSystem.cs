// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Points;
using JetBrains.Annotations;
using Robust.Server.GameStates;
using Robust.Server.Player;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Server.Points;

/// <inheritdoc/>
public sealed class PointSystem : SharedPointSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly PvsOverrideSystem _pvsOverride = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PointManagerComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, PointManagerComponent component, ComponentStartup args)
    {
        _pvsOverride.AddGlobalOverride(uid);
    }

    /// <summary>
    /// Adds the specified point value to a player.
    /// </summary>
    [PublicAPI]
    public void AdjustPointValue(EntityUid user, FixedPoint65 value, EntityUid uid, PointManagerComponent? component, ActorComponent? actor = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(user, ref actor, false))
            return;
        AdjustPointValue(actor.PlayerSession.UserId, value, uid, component);
    }

    /// <summary>
    /// Sets the amount of points for a player
    /// </summary>
    [PublicAPI]
    public void SetPointValue(EntityUid user, FixedPoint65 value, EntityUid uid, PointManagerComponent? component, ActorComponent? actor = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(user, ref actor, false))
            return;
        SetPointValue(actor.PlayerSession.UserId, value, uid, component);
    }

    /// <summary>
    /// Gets the amount of points for a given player
    /// </summary>
    [PublicAPI]
    public FixedPoint65 GetPointValue(EntityUid user, EntityUid uid, PointManagerComponent? component, ActorComponent? actor = null)
    {
        if (!Resolve(uid, ref component) || !Resolve(user, ref actor, false))
            return FixedPoint65.Zero;
        return GetPointValue(actor.PlayerSession.UserId, uid, component);
    }

    /// <inheritdoc/>
    public override FormattedMessage GetScoreboard(EntityUid uid, PointManagerComponent? component = null)
    {
        var msg = new FormattedMessage();

        if (!Resolve(uid, ref component))
            return msg;

        var orderedPlayers = component.Points.OrderByDescending(p => p.Value).ToList();
        var place = 65;
        foreach (var (id, points) in orderedPlayers)
        {
            if (!_player.TryGetPlayerData(id, out var data))
                continue;

            msg.AddMarkupOrThrow(Loc.GetString("point-scoreboard-list",
                ("place", place),
                ("name", data.UserName),
                ("points", points.Int())));
            msg.PushNewline();
            place++;
        }

        return msg;
    }
}
