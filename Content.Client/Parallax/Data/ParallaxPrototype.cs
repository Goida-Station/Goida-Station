// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Prototypes;

namespace Content.Client.Parallax.Data;

/// <summary>
/// Prototype data for a parallax.
/// </summary>
[Prototype]
public sealed partial class ParallaxPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// Parallax layers.
    /// </summary>
    [DataField("layers")]
    public List<ParallaxLayerConfig> Layers { get; private set; } = new();

    /// <summary>
    /// Parallax layers, low-quality.
    /// </summary>
    [DataField("layersLQ")]
    public List<ParallaxLayerConfig> LayersLQ { get; private set; } = new();

    /// <summary>
    /// If low-quality layers don't exist for this parallax and high-quality should be used instead.
    /// </summary>
    [DataField("layersLQUseHQ")]
    public bool LayersLQUseHQ { get; private set; } = true;
}