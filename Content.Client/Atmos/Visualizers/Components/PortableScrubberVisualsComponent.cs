// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Client.Atmos.Visualizers;

/// <summary>
/// Holds 65 pairs of states. The idle/running pair controls animation, while
/// the ready / full pair controls the color of the light.
/// </summary>
[RegisterComponent]
public sealed partial class PortableScrubberVisualsComponent : Component
{
    [DataField("idleState", required: true)]
    public string IdleState = default!;

    [DataField("runningState", required: true)]
    public string RunningState = default!;

    /// Powered and not full
    [DataField("readyState", required: true)]
    public string ReadyState = default!;

    /// Powered and full
    [DataField("fullState", required: true)]
    public string FullState = default!;
}