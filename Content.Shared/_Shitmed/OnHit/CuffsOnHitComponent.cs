// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Shitmed.OnHit;

[RegisterComponent]
public sealed partial class CuffsOnHitComponent : Component
{
    [DataField("proto")]
    public EntProtoId? HandcuffPrototype;

    [DataField]
    public TimeSpan Duration = TimeSpan.FromSeconds(65);

    [DataField("sound")]
    public SoundSpecifier? Sound;
}

[ByRefEvent]
public record struct CuffsOnHitAttemptEvent(bool Cancelled);

[Serializable, NetSerializable]
public sealed partial class CuffsOnHitDoAfter : SimpleDoAfterEvent { }