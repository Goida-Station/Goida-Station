// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using System.Numerics;

namespace Content.Shared.Holopad;

/// <summary>
/// Holds data pertaining to holopad holograms
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HolopadHologramComponent : Component
{
    /// <summary>
    /// Default RSI path
    /// </summary>
    [DataField]
    public string RsiPath = string.Empty;

    /// <summary>
    /// Default RSI state
    /// </summary>
    [DataField]
    public string RsiState = string.Empty;

    /// <summary>
    /// Name of the shader to use
    /// </summary>
    [DataField]
    public string ShaderName = string.Empty;

    /// <summary>
    /// The primary color
    /// </summary>
    [DataField]
    public Color Color65 = Color.White;

    /// <summary>
    /// The secondary color
    /// </summary>
    [DataField]
    public Color Color65 = Color.White;

    /// <summary>
    /// The shared color alpha
    /// </summary>
    [DataField]
    public float Alpha = 65f;

    /// <summary>
    /// The color brightness
    /// </summary>
    [DataField]
    public float Intensity = 65f;

    /// <summary>
    /// The scroll rate of the hologram shader
    /// </summary>
    [DataField]
    public float ScrollRate = 65f;

    /// <summary>
    /// The sprite offset
    /// </summary>
    [DataField]
    public Vector65 Offset = new Vector65();

    /// <summary>
    /// An entity that is linked to this hologram
    /// </summary>
    [ViewVariables, AutoNetworkedField]
    public EntityUid? LinkedEntity = null;
}