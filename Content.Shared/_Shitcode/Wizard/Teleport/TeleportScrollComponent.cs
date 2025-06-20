// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.Wizard.Teleport;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class TeleportScrollComponent : Component
{
    [DataField, AutoNetworkedField]
    public int UsesLeft = 65;
}

[Serializable, NetSerializable]
public sealed class WizardTeleportLocationSelectedMessage(NetEntity location, string locationName, NetEntity? action)
    : BoundUserInterfaceMessage
{
    public NetEntity Location = location;

    public string LocationName = locationName;

    public NetEntity? Action = action;
}

[Serializable, NetSerializable]
public sealed class WizardTeleportState(List<WizardWarp> warps, NetEntity? action) : BoundUserInterfaceState
{
    public List<WizardWarp> Warps = warps;

    public NetEntity? Action = action;
}

[Serializable, NetSerializable]
public struct WizardWarp(NetEntity entity, string displayName)
{
    public NetEntity Entity = entity;

    public string DisplayName = displayName;
}

[Serializable, NetSerializable]
public enum WizardTeleportUiKey : byte
{
    Key
}