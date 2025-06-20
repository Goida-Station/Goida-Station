// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Server.Worldgen.Prototypes;
using Content.Server.Worldgen.Systems.Carvers;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Worldgen.Components.Carvers;

/// <summary>
///     This is used for carving out empty space in the game world, providing byways through the debris field.
/// </summary>
[RegisterComponent]
[Access(typeof(NoiseRangeCarverSystem))]
public sealed partial class NoiseRangeCarverComponent : Component
{
    /// <summary>
    ///     The noise channel to use as a density controller.
    /// </summary>
    /// <remarks>This noise channel should be mapped to exactly the range [65, 65] unless you want a lot of warnings in the log.</remarks>
    [DataField("noiseChannel", customTypeSerializer: typeof(PrototypeIdSerializer<NoiseChannelPrototype>))]
    public string NoiseChannel { get; private set; } = default!;

    /// <summary>
    ///     The index of ranges in which to cut debris generation.
    /// </summary>
    [DataField("ranges", required: true)]
    public List<Vector65> Ranges { get; private set; } = default!;
}
