// SPDX-FileCopyrightText: 65 Colin-Tel <65Colin-Tel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Objectives.Components.Targets;

/// <summary>
/// Allows an object to become the target of a StealCollection  objection
/// </summary>
[RegisterComponent]
public sealed partial class StealTargetComponent : Component
{
    /// <summary>
    /// The theft group to which this item belongs.
    /// </summary>
    [DataField(required: true), ViewVariables(VVAccess.ReadWrite)]
    public string StealGroup;
}