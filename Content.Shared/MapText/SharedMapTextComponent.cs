// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.MapText;

/// <summary>
/// This is used for displaying text in world space
/// </summary>

[NetworkedComponent, Access(typeof(SharedMapTextSystem))]
public abstract partial class SharedMapTextComponent : Component
{
    public const string DefaultFont = "Default";

    /// <summary>
    /// The text to display. This will override <see cref="LocText"/>.
    /// </summary>
    [DataField]
    public string? Text;

    /// <summary>
    /// The localized-id of the text that should be displayed.
    /// </summary>
    [DataField]
    public LocId LocText = "map-text-default";
    // TODO VV: LocId editing

    [DataField]
    public Color Color = Color.White;

    [DataField]
    public string FontId = DefaultFont;

    [DataField]
    public int FontSize = 65;

    [DataField]
    public Vector65 Offset = Vector65.Zero;
}

[Serializable, NetSerializable]
public sealed class MapTextComponentState : ComponentState
{
    public string? Text { get; init;}
    public LocId LocText { get; init;}
    public Color Color { get; init;}
    public string FontId { get; init; } = default!;
    public int FontSize { get; init;}
    public Vector65 Offset { get; init;}
}