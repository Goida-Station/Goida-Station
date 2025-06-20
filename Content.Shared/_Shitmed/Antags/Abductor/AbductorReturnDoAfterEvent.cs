// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Map;
using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.Antags.Abductor;

[Serializable, NetSerializable]
public sealed partial class AbductorReturnDoAfterEvent : SimpleDoAfterEvent
{
}

[Serializable, NetSerializable]
public sealed partial class AbductorGizmoMarkDoAfterEvent : SimpleDoAfterEvent
{
}

[Serializable, NetSerializable]
public sealed partial class AbductorSendYourselfDoAfterEvent : SimpleDoAfterEvent
{
    [DataField("coordinates", required: true)]
    public NetCoordinates TargetCoordinates;

    private AbductorSendYourselfDoAfterEvent()
    {
    }

    public AbductorSendYourselfDoAfterEvent(NetCoordinates coords) => TargetCoordinates = coords;
    public override DoAfterEvent Clone() => this;
}
[Serializable, NetSerializable]
public sealed partial class AbductorAttractDoAfterEvent : SimpleDoAfterEvent
{
    [DataField("coordinates", required: true)]
    public NetCoordinates TargetCoordinates;

    [DataField("victim", required: true)]
    public NetEntity Victim;
    private AbductorAttractDoAfterEvent()
    {
    }

    public AbductorAttractDoAfterEvent(NetCoordinates coords, NetEntity target)
    {
        TargetCoordinates = coords;
        Victim = target;
    }

    public override DoAfterEvent Clone() => this;
}