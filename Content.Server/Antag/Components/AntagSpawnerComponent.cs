// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Server.Antag.Components;

/// <summary>
/// Spawns a prototype for antags created with a spawner.
/// </summary>
[RegisterComponent, Access(typeof(AntagSpawnerSystem))]
public sealed partial class AntagSpawnerComponent : Component
{
    /// <summary>
    /// The entity to spawn.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Prototype = string.Empty;
}