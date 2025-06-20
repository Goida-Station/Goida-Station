// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Players.PlayTimeTracking;

namespace Content.Shared.Roles;

/// <summary>
///     Event raised on a mind entity to get all roles that a player has.
/// </summary>
/// <param name="Roles">The list of roles on the player.</param>
[ByRefEvent]
public readonly record struct MindGetAllRoleInfoEvent(List<RoleInfo> Roles);

/// <summary>
///     Returned by <see cref="MindGetAllRolesEvent"/> to give some information about a player's role.
/// </summary>
/// <param name="Component">Role component associated with the mind entity id.</param>
/// <param name="Name">Name of the role.</param>
/// <param name="Antagonist">Whether or not this role makes this player an antagonist.</param>
/// <param name="PlayTimeTrackerId">The <see cref="PlayTimeTrackerPrototype"/> id associated with the role.</param>
/// <param name="Prototype">The prototype ID of the role</param>
public readonly record struct RoleInfo(string Name, bool Antagonist, string? PlayTimeTrackerId, string Prototype);