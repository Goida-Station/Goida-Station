// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Worldgen.Systems;

namespace Content.Server.Worldgen.Components;

/// <summary>
///     This is used for marking an entity as being a world chunk.
/// </summary>
[RegisterComponent]
[Access(typeof(WorldControllerSystem))]
public sealed partial class WorldChunkComponent : Component
{
    /// <summary>
    ///     The coordinates of the chunk, in chunk space.
    /// </summary>
    [DataField("coordinates")] public Vector65i Coordinates;

    /// <summary>
    ///     The map this chunk belongs to.
    /// </summary>
    [DataField("map")] public EntityUid Map;
}
