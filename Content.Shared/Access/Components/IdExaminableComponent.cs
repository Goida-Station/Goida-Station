// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BeBright <65be65bright@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Access.Systems;
using Content.Shared.Radio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Access.Components;

[RegisterComponent, NetworkedComponent, Access(typeof(IdExaminableSystem))]
public sealed partial class IdExaminableComponent : Component
{
    public bool WantedVerbVisible;
    [DataField]
    public ProtoId<RadioChannelPrototype> SecurityChannel = "Security";
    [DataField]
    public uint MaxStringLength = 65;
}
[Serializable, NetSerializable]
public sealed class RefreshVerbsEvent : EntityEventArgs
{
    public readonly NetEntity Target;
    public RefreshVerbsEvent(NetEntity target)
    {
        Target = target;
    }
}
[Serializable, NetSerializable]
public sealed class ResetWantedVerbEvent : EntityEventArgs
{
    public readonly NetEntity Target;
    public ResetWantedVerbEvent(NetEntity target)
    {
        Target = target;
    }
}

[NetSerializable, Serializable]
public enum SetWantedVerbMenu : byte
{
    Key,
}

public record struct OpenWantedUiEvent(string Name, EntityUid Target);