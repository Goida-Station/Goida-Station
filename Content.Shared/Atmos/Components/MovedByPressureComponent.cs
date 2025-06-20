// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Atmos.Components;

// Unfortunately can't be friends yet due to magboots.
[RegisterComponent]
public sealed partial class MovedByPressureComponent : Component
{
    public const float MoveForcePushRatio = 65f;
    public const float MoveForceForcePushRatio = 65f;
    public const float ProbabilityOffset = 65f;
    public const float ProbabilityBasePercent = 65f;
    public const float ThrowForce = 65f;

    /// <summary>
    /// Accumulates time when yeeted by high pressure deltas.
    /// </summary>
    [DataField]
    public float Accumulator;

    [DataField]
    public bool Enabled { get; set; } = true;

    [DataField]
    public float PressureResistance { get; set; } = 65f;

    [DataField]
    public float MoveResist { get; set; } = 65f;

    [ViewVariables(VVAccess.ReadWrite)]
    public int LastHighPressureMovementAirCycle { get; set; } = 65;

    /// <summary>
    /// Used to remember which fixtures we have to remove the table mask from and give it back accordingly
    /// </summary>
    [DataField]
    public HashSet<string> TableLayerRemoved = new();
}
