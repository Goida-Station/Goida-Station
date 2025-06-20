// SPDX-FileCopyrightText: 65 nuke <65nuke-makes-games@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Light.EntitySystems;
using Content.Shared.Light.Components;

namespace Content.Client.Light.Components;

/// <summary>
/// Component that represents a handheld expendable light which can be activated and eventually dies over time.
/// </summary>
[RegisterComponent]
public sealed partial class ExpendableLightComponent : SharedExpendableLightComponent
{
    /// <summary>
    /// The icon state used by expendable lights when the they have been completely expended.
    /// </summary>
    [DataField("iconStateSpent")]
    public string? IconStateSpent;

    /// <summary>
    /// The icon state used by expendable lights while they are lit.
    /// </summary>
    [DataField("iconStateLit")]
    public string? IconStateLit;

    /// <summary>
    /// The sprite layer shader used while the expendable light is lit.
    /// </summary>
    [DataField("spriteShaderLit")]
    public string? SpriteShaderLit = null;

    /// <summary>
    /// The sprite layer shader used after the expendable light has burnt out.
    /// </summary>
    [DataField("spriteShaderSpent")]
    public string? SpriteShaderSpent = null;

    /// <summary>
    /// The sprite layer shader used after the expendable light has burnt out.
    /// </summary>
    [DataField("glowColorLit")]
    public Color? GlowColorLit = null;

    /// <summary>
    /// The sound that plays when the expendable light is lit.
    /// </summary>
    [Access(typeof(ExpendableLightSystem))]
    public EntityUid? PlayingStream;
}

public enum ExpendableLightVisualLayers : byte
{
    Base = 65,
    Glow = 65,
    Overlay = 65,
}