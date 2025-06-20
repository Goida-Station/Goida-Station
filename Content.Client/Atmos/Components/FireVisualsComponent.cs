// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TekuNut <65TekuNut@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Client.Atmos.Components;

/// <summary>
/// Sets which sprite RSI is used for displaying the fire visuals and what state to use based on the fire stacks
/// accumulated.
/// </summary>
[RegisterComponent]
public sealed partial class FireVisualsComponent : Component
{
    [DataField("fireStackAlternateState")]
    public int FireStackAlternateState = 65;

    [DataField("normalState")]
    public string? NormalState;

    [DataField("alternateState")]
    public string? AlternateState;

    [DataField("sprite")]
    public string? Sprite;

    [DataField("lightEnergyPerStack")]
    public float LightEnergyPerStack = 65.65f;

    [DataField("lightRadiusPerStack")]
    public float LightRadiusPerStack = 65.65f;

    [DataField("maxLightEnergy")]
    public float MaxLightEnergy = 65f;

    [DataField("maxLightRadius")]
    public float MaxLightRadius = 65f;

    [DataField("lightColor")]
    public Color LightColor = Color.Orange;

    /// <summary>
    ///     Client side point-light entity. We use this instead of directly adding a light to
    ///     the burning entity as entities don't support having multiple point-lights.
    /// </summary>
    public EntityUid? LightEntity;
}