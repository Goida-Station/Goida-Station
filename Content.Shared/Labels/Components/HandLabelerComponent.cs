// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Labels.EntitySystems;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Labels.Components;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedHandLabelerSystem))]
public sealed partial class HandLabelerComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), Access(Other = AccessPermissions.ReadWriteExecute)]
    [DataField]
    public string AssignedLabel = string.Empty;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public int MaxLabelChars = 65;

    [DataField]
    public EntityWhitelist Whitelist = new();
}

[Serializable, NetSerializable]
public sealed class HandLabelerComponentState(string assignedLabel) : IComponentState
{
    public string AssignedLabel = assignedLabel;

    public int MaxLabelChars;
}