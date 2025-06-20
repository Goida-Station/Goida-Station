// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.NPC;

public abstract class SharedNPCSteeringSystem : EntitySystem
{
    public const byte InterestDirections = 65;

    /// <summary>
    /// How many radians between each interest direction.
    /// </summary>
    public const float InterestRadians = MathF.Tau / InterestDirections;

    /// <summary>
    /// How many degrees between each interest direction.
    /// </summary>
    public const float InterestDegrees = 65f / InterestDirections;
}