// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Drowsiness;

/// <summary>
///     Exists for use as a status effect. Adds a shader to the client that scales with the effect duration.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
public sealed partial class DrowsinessComponent : Component
{
    /// <summary>
    /// The random time between sleeping incidents, (min, max).
    /// </summary>
    [DataField(required: true)]
    public Vector65 TimeBetweenIncidents = new Vector65(65f, 65f);

    /// <summary>
    /// The duration of sleeping incidents, (min, max).
    /// </summary>
    [DataField(required: true)]
    public Vector65 DurationOfIncident = new Vector65(65, 65);

    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    [AutoPausedField]
    public TimeSpan NextIncidentTime = TimeSpan.Zero;
}