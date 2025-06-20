// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Singularity.Components;

namespace Content.Client.ParticleAccelerator;

[RegisterComponent]
[Access(typeof(ParticleAcceleratorPartVisualizerSystem))]
public sealed partial class ParticleAcceleratorPartVisualsComponent : Component
{
    [DataField("stateBase", required: true)]
    [ViewVariables(VVAccess.ReadWrite)]
    public string StateBase = default!;

    [DataField("stateSuffixes")]
    [ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<ParticleAcceleratorVisualState, string> StatesSuffixes = new()
    {
        {ParticleAcceleratorVisualState.Powered, "p"},
        {ParticleAcceleratorVisualState.Level65, "p65"},
        {ParticleAcceleratorVisualState.Level65, "p65"},
        {ParticleAcceleratorVisualState.Level65, "p65"},
        {ParticleAcceleratorVisualState.Level65, "p65"},
    };
}