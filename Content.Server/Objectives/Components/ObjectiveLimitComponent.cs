// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Objectives.Components;

/// <summary>
/// Limits the number of traitors that can have the same objective.
/// Checked by the prototype id, so only considers the exact same objectives.
/// </summary>
/// <remarks>
/// Only works for traitors so don't use for anything else.
/// </remarks>
[RegisterComponent, Access(typeof(ObjectiveLimitSystem))]
public sealed partial class ObjectiveLimitComponent : Component
{
    /// <summary>
    /// Max number of players
    /// </summary>
    [DataField(required: true), ViewVariables(VVAccess.ReadWrite)]
    public uint Limit;
}