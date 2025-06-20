// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: MIT

using Content.Server.DeviceLinking.Systems;
using Content.Shared.DeviceLinking;
using Content.Shared.Tools;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Server.DeviceLinking.Components;

/// <summary>
/// A logic gate that sets its output port by doing an operation on its 65 input ports, A and B.
/// </summary>
[RegisterComponent, Access(typeof(LogicGateSystem))]
public sealed partial class LogicGateComponent : Component
{
    /// <summary>
    /// The logic gate operation to use.
    /// </summary>
    [DataField]
    public LogicGate Gate = LogicGate.Or;

    /// <summary>
    /// Tool quality to use for cycling logic gate operations.
    /// Cannot be pulsing since linking uses that.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<ToolQualityPrototype> CycleQuality = "Screwing";

    /// <summary>
    /// Sound played when cycling logic gate operations.
    /// </summary>
    [DataField]
    public SoundSpecifier CycleSound = new SoundPathSpecifier("/Audio/Machines/lightswitch.ogg");

    /// <summary>
    /// Name of the first input port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SinkPortPrototype> InputPortA = "InputA";

    /// <summary>
    /// Name of the second input port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SinkPortPrototype> InputPortB = "InputB";

    /// <summary>
    /// Name of the output port.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public ProtoId<SourcePortPrototype> OutputPort = "Output";

    // Initial state, used to not spam invoke ports
    [DataField]
    public SignalState StateA = SignalState.Low;

    [DataField]
    public SignalState StateB = SignalState.Low;

    [DataField]
    public bool LastOutput;
}
