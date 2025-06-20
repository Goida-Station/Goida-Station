// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Objectives.Systems;

namespace Content.Server.Objectives.Components;

/// <summary>
/// An objective that is set to complete by code in another system.
/// Use <see cref="CodeConditionSystem"/> to check and set this.
/// </summary>
[RegisterComponent, Access(typeof(CodeConditionSystem))]
public sealed partial class CodeConditionComponent : Component
{
    /// <summary>
    /// Whether the objective is complete or not.
    /// </summary>
    [DataField]
    public bool Completed;
}