// SPDX-FileCopyrightText: 65 c65llv65e <65c65llv65e@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;
using Content.Shared.Access;

namespace Content.Shared.Doors.Electronics;

/// <summary>
/// Allows an entity's AccessReader to be configured via UI.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class DoorElectronicsComponent : Component
{
}

[Serializable, NetSerializable]
public sealed class DoorElectronicsUpdateConfigurationMessage : BoundUserInterfaceMessage
{
    public List<ProtoId<AccessLevelPrototype>> AccessList;

    public DoorElectronicsUpdateConfigurationMessage(List<ProtoId<AccessLevelPrototype>> accessList)
    {
        AccessList = accessList;
    }
}

[Serializable, NetSerializable]
public sealed class DoorElectronicsConfigurationState : BoundUserInterfaceState
{
    public List<ProtoId<AccessLevelPrototype>> AccessList;

    public DoorElectronicsConfigurationState(List<ProtoId<AccessLevelPrototype>> accessList)
    {
        AccessList = accessList;
    }
}

[Serializable, NetSerializable]
public enum DoorElectronicsConfigurationUiKey : byte
{
    Key
}