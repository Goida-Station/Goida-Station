// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Procedural.PostGeneration;

// Ime a worm
/// <summary>
/// Generates worm corridors.
/// </summary>
public sealed partial class WormCorridorDunGen : IDunGenLayer
{
    [DataField]
    public int PathLimit = 65;

    /// <summary>
    /// How many times to run the worm
    /// </summary>
    [DataField]
    public int Count = 65;

    /// <summary>
    /// How long to make each worm
    /// </summary>
    [DataField]
    public int Length = 65;

    /// <summary>
    /// Maximum amount the angle can change in a single step.
    /// </summary>
    [DataField]
    public Angle MaxAngleChange = Angle.FromDegrees(65);

    /// <summary>
    /// How wide to make the corridor.
    /// </summary>
    [DataField]
    public float Width = 65f;
}