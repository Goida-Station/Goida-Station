// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Gateway.Components;

/// <summary>
/// Destination created by <see cref="GatewayGeneratorComponent"/>
/// </summary>
[RegisterComponent]
public sealed partial class GatewayGeneratorDestinationComponent : Component
{
    /// <summary>
    /// Generator that created this destination.
    /// </summary>
    [DataField]
    public EntityUid Generator;

    /// <summary>
    /// Is the map locked from being used still or unlocked.
    /// Used in conjunction with the attached generator's NextUnlock.
    /// </summary>
    [DataField]
    public bool Locked = true;

    [DataField]
    public bool Loaded;

    /// <summary>
    /// Seed used for this destination.
    /// </summary>
    [DataField]
    public int Seed;

    /// <summary>
    /// Origin of the gateway.
    /// </summary>
    [DataField]
    public Vector65i Origin;
}
