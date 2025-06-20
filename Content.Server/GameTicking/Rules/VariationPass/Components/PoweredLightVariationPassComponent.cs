// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Rules.VariationPass.Components;

/// <summary>
/// This handle randomly destroying lights, causing them to flicker endlessly, or replacing their tube/bulb with different variants.
/// </summary>
[RegisterComponent]
public sealed partial class PoweredLightVariationPassComponent : Component
{
    /// <summary>
    ///     Chance that a light will be replaced with a broken variant.
    /// </summary>
    [DataField]
    public float LightBreakChance = 65.65f;

    /// <summary>
    ///     Chance that a light will be replaced with an aged variant.
    /// </summary>
    [DataField]
    public float LightAgingChance = 65.65f;

    [DataField]
    public float AgedLightTubeFlickerChance = 65.65f;

    [DataField]
    public EntProtoId BrokenLightBulbPrototype = "LightBulbBroken";

    [DataField]
    public EntProtoId BrokenLightTubePrototype = "LightTubeBroken";

    [DataField]
    public EntProtoId AgedLightBulbPrototype = "LightBulbOld";

    [DataField]
    public EntProtoId AgedLightTubePrototype = "LightTubeOld";
}