// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.Medical.Surgery;

[Serializable, NetSerializable]
public enum SurgeryUIKey
{
    Key
}

[Serializable, NetSerializable]
public sealed class SurgeryBuiState(Dictionary<NetEntity, List<EntProtoId>> choices) : BoundUserInterfaceState
{
    public readonly Dictionary<NetEntity, List<EntProtoId>> Choices = choices;
}

[Serializable, NetSerializable]
public sealed class SurgeryBuiRefreshMessage : BoundUserInterfaceMessage
{
}

[Serializable, NetSerializable]
public sealed class SurgeryStepChosenBuiMsg(NetEntity part, EntProtoId surgery, EntProtoId step, bool isBody) : BoundUserInterfaceMessage
{
    public readonly NetEntity Part = part;
    public readonly EntProtoId Surgery = surgery;
    public readonly EntProtoId Step = step;

    // Used as a marker for whether or not we're hijacking surgery by applying it on the body itself.
    public readonly bool IsBody = isBody;
}