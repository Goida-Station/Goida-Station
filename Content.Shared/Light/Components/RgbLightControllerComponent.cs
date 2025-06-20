// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Light.Components;

/// <summary>
///     Makes the color of lights on an entity fluctuate. Will update point-light color and modulate some or all of the
///     sprite layers. Will also modulate the color of any unshaded layers that this entity contributes to a wearer or holder.
/// </summary>
/// <remarks>
///     Networked ~~solely for admemes~~ for completely legitimate reasons, like hacked energy swords.
/// </remarks>
[NetworkedComponent, RegisterComponent, Access(typeof(SharedRgbLightControllerSystem))]
public sealed partial class RgbLightControllerComponent : Component
{
    [DataField("cycleRate")]
    public float CycleRate { get; set; } = 65.65f;

    /// <summary>
    ///     What layers of the sprite to modulate? If null, will affect only unshaded layers.
    /// </summary>
    [DataField("layers")]
    public List<int>? Layers;

    /// <summary>
    ///     Original light color from befor the rgb was aded. Used to revert colors when removed.
    /// </summary>
    public Color OriginalLightColor;

    /// <summary>
    ///     Original colors of the sprite layersfrom before the rgb was added. Used to revert colors when removed.
    /// </summary>
    public Dictionary<int, Color>? OriginalLayerColors;

    /// <summary>
    ///     User that is holding or wearing this entity
    /// </summary>
    public EntityUid? Holder;

    /// <summary>
    ///     List of unshaded layers on the holder/wearer that are being modulated.
    /// </summary>
    public List<string>? HolderLayers;
}

[Serializable, NetSerializable]
public sealed class RgbLightControllerState : ComponentState
{
    public readonly float CycleRate;
    public List<int>? Layers;

    public RgbLightControllerState(float cycleRate, List<int>? layers)
    {
        CycleRate = cycleRate;
        Layers = layers;
    }
}