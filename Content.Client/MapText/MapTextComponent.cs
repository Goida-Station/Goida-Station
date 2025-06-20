// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.MapText;
using Robust.Client.Graphics;

namespace Content.Client.MapText;

[RegisterComponent]
public sealed partial class MapTextComponent : SharedMapTextComponent
{
    /// <summary>
    /// The font that gets cached on component init or state changes
    /// </summary>
    [ViewVariables]
    public VectorFont? CachedFont;

    /// <summary>
    /// The text currently being displayed. This is either <see cref="SharedMapTextComponent.Text"/> or the
    /// localized text <see cref="SharedMapTextComponent.LocText"/> or
    /// </summary>
    public string CachedText = string.Empty;
}