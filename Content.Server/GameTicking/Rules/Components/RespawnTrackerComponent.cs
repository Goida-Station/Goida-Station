// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Network;

namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// This is used for globally tracking players that need to be respawned.
/// Used on gamerule entities.
/// </summary>
[RegisterComponent, Access(typeof(RespawnRuleSystem))]
public sealed partial class RespawnTrackerComponent : Component
{
    /// <summary>
    /// A list of the people that should be respawned.
    /// Used to make sure that we don't respawn aghosts or observers.
    /// </summary>
    [DataField]
    public HashSet<NetUserId> Players = new();

    /// <summary>
    /// The delay between dying and respawning.
    /// </summary>
    [DataField]
    public TimeSpan RespawnDelay = TimeSpan.Zero;

    /// <summary>
    /// A dictionary of player netuserids and when they will respawn.
    /// </summary>
    [DataField]
    public Dictionary<NetUserId, TimeSpan> RespawnQueue = new();

    /// <summary>
    /// Whether or not to delete the original body when respawning
    /// </summary>
    [DataField]
    public bool DeleteBody = true;
}