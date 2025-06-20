// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.Graphics;
using Content.Client.Parallax.Data;

namespace Content.Client.Parallax;

/// <summary>
/// A 'prepared' (i.e. texture loaded and ready to use) parallax layer.
/// </summary>
public struct ParallaxLayerPrepared
{
    /// <summary>
    /// The loaded texture for this layer.
    /// </summary>
    public Texture Texture { get; set; }

    /// <summary>
    /// The configuration for this layer.
    /// </summary>
    public ParallaxLayerConfig Config { get; set; }
}
