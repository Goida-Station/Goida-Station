// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Voting;

/// <summary>
/// Standard vote types that players can initiate themselves from the escape menu.
/// </summary>
public enum StandardVoteType : byte
{
    /// <summary>
    /// Vote to restart the round.
    /// </summary>
    Restart,

    /// <summary>
    /// Vote to change the game preset for next round.
    /// </summary>
    Preset,

    /// <summary>
    /// Vote to change the map for the next round.
    /// </summary>
    Map,

    /// <summary>
    /// Vote to kick a player.
    /// </summary>
    Votekick
}

/// <summary>
/// Reasons available to initiate a votekick.
/// </summary>
public enum VotekickReasonType : byte
{
    Raiding,
    Cheating,
    Spam
}