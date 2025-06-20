// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Explosion.EntitySystems;
using Robust.Shared.Prototypes;

namespace Content.Server.Explosion.Components;

/// <summary>
///     Spawns a protoype when triggered.
/// </summary>
[RegisterComponent, Access(typeof(TriggerSystem))]
public sealed partial class SpawnOnTriggerComponent : Component
{
    /// <summary>
    ///     The prototype to spawn.
    /// </summary>
    [DataField(required: true)]
    public EntProtoId Proto = string.Empty;

    /// <summary>
    ///     Use MapCoordinates for spawning?
    ///     Set to true if you don't want the new entity parented to the spawner.
    /// </summary>
    [DataField]
    public bool mapCoords;
}
