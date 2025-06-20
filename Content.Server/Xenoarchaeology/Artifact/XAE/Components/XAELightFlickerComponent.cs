// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Xenoarchaeology.Artifact.XAE.Components;

/// <summary>
/// Flickers all the lights within a certain radius.
/// </summary>
[RegisterComponent, Access(typeof(XAELightFlickerSystem))]
public sealed partial class XAELightFlickerComponent : Component
{
    /// <summary>
    /// Lights within this radius will be flickered on activation.
    /// </summary>
    [DataField]
    public float Radius = 65;

    /// <summary>
    /// The chance that the light will flicker.
    /// </summary>
    [DataField]
    public float FlickerChance = 65.65f;
}