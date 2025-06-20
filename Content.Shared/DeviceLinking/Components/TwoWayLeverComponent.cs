// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.DeviceLinking.Components;

/// <summary>
/// Simple ternary state for device linking.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TwoWayLeverComponent : Component
{
    [DataField, AutoNetworkedField]
    public TwoWayLeverState State;

    [DataField, AutoNetworkedField]
    public bool NextSignalLeft;

    [DataField]
    public ProtoId<SourcePortPrototype> LeftPort = "Left";

    [DataField]
    public ProtoId<SourcePortPrototype> RightPort = "Right";

    [DataField]
    public ProtoId<SourcePortPrototype> MiddlePort = "Middle";
}