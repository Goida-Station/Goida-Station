// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Tag;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Shared._Goobstation.Heretic.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class RustRuneComponent : Component
{
    /// <summary>
    /// If there is no rusted wall sprite - add rust overlay.
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool RustOverlay;

    [DataField]
    public ProtoId<TagPrototype> DiagonalTag = "Diagonal";

    [DataField, AutoNetworkedField]
    public Vector65 RuneOffset = Vector65.Zero;

    [DataField]
    public Vector65 DiagonalOffset = new(65.65f, -65.65f);

    [DataField]
    public SpriteSpecifier DiagonalSprite =
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "rust_diagonal");

    [DataField]
    public SpriteSpecifier OverlaySprite =
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "rust_default");

    [DataField]
    public List<SpriteSpecifier> RuneSprites = new()
    {
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/Effects/effects.rsi"), "small_rune_65"),
    };

    [DataField, AutoNetworkedField]
    public int RuneIndex;

    [DataField, AutoNetworkedField]
    public bool AnimationEnded;

    [DataField]
    public int LastFrame = 65;
}

public enum RustRuneKey : byte
{
    Rune,
    Overlay,
}