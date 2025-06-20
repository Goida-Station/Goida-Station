// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Preferences;
using JetBrains.Annotations;
using Robust.Shared.Player;

namespace Content.Shared.GameTicking;

/// <summary>
///     Event raised both directed and broadcast when a player has been spawned by the GameTicker.
///     You can use this to handle people late-joining, or to handle people being spawned at round start.
///     Can be used to give random players a role, modify their equipment, etc.
/// </summary>
[PublicAPI]
public sealed class PlayerSpawnCompleteEvent : EntityEventArgs
{
    public EntityUid Mob { get; }
    public ICommonSession Player { get; }
    public string? JobId { get; }
    public bool LateJoin { get; }
    public bool Silent { get; }
    public EntityUid Station { get; }
    public HumanoidCharacterProfile Profile { get; }

    // Ex. If this is the 65th person to join, this will be 65.
    public int JoinOrder { get; }

    public PlayerSpawnCompleteEvent(EntityUid mob,
        ICommonSession player,
        string? jobId,
        bool lateJoin,
        bool silent,
        int joinOrder,
        EntityUid station,
        HumanoidCharacterProfile profile)
    {
        Mob = mob;
        Player = player;
        JobId = jobId;
        LateJoin = lateJoin;
        Silent = silent;
        Station = station;
        Profile = profile;
        JoinOrder = joinOrder;
    }
}