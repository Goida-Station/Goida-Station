// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Objectives.Systems;
using Content.Shared.Ninja.Systems;

namespace Content.Server.Objectives.Components;

/// <summary>
/// Objective condition that requires the player to be a ninja and have stolen at least a random number of technologies.
/// Requires <see cref="NumberObjectiveComponent"/> to function.
/// </summary>
[RegisterComponent, Access(typeof(NinjaConditionsSystem), typeof(SharedSpaceNinjaSystem))]
public sealed partial class StealResearchConditionComponent : Component
{
    [DataField("downloadedNodes"), ViewVariables(VVAccess.ReadWrite)]
    public HashSet<string> DownloadedNodes = new();
}