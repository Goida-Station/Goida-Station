// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 coolmankid65 <65coolmankid65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coolmankid65 <coolmankid65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Theodore Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking.Rules;
using Content.Server.Mindshield; // GoobStation

namespace Content.Server.Revolutionary.Components;

/// <summary>
/// Given to heads at round start. Used for assigning traitors to kill heads and for revs to check if the heads died or not.
/// </summary>
[RegisterComponent, Access(typeof(RevolutionaryRuleSystem), typeof(MindShieldSystem))] // GoobStation - typeof MindshieldSystem
public sealed partial class CommandStaffComponent : Component
{
    // Goobstation
    /// <summary>
    /// Check for removing mindshield implant from command.
    /// </summary>
    [DataField]
    public bool Enabled = true;
}

//TODO this should probably be on a mind role, not the mob