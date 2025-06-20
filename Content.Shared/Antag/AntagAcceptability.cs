// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rainfall <rainfey65git@gmail.com>
// SPDX-FileCopyrightText: 65 Rainfey <rainfey65github@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Antag;

/// <summary>
/// Used by AntagSelectionSystem to indicate which types of antag roles are allowed to choose the same entity
/// For example, Thief HeadRev
/// </summary>
public enum AntagAcceptability
{
    /// <summary>
    /// Dont choose anyone who already has an antag role
    /// </summary>
    None,
    /// <summary>
    /// Dont choose anyone who has an exclusive antag role
    /// </summary>
    NotExclusive,
    /// <summary>
    /// Choose anyone
    /// </summary>
    All,
}

public enum AntagSelectionTime : byte
{
    /// <summary>
    /// Antag roles are assigned before players are assigned jobs and spawned in.
    /// This prevents antag selection from happening if the round is on-going.
    /// </summary>
    PrePlayerSpawn,

    /// <summary>
    /// Antag roles are selected to the player session before job assignment and spawning.
    /// Unlike PrePlayerSpawn, this does not remove you from the job spawn pool.
    /// </summary>
    IntraPlayerSpawn,

    /// <summary>
    /// Antag roles get assigned after players have been assigned jobs and have spawned in.
    /// </summary>
    PostPlayerSpawn,
}
