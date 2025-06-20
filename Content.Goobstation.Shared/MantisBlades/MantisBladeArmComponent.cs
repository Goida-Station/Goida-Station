// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;
using Robust.Shared.Audio;

namespace Content.Goobstation.Shared.MantisBlades;

[RegisterComponent]
public sealed partial class MantisBladeArmComponent : Component
{
    [DataField]
    public string ActionProto;

    [DataField]
    public EntityUid? ActionUid;

    [DataField]
    public string BladeProto = "MantisBlade";

    [DataField]
    public EntityUid? BladeUid;

    [DataField]
    public SoundSpecifier? ExtendSound = new SoundPathSpecifier("/Audio/_Goobstation/Weapons/MantisBlades/mantis_extend.ogg");

    [DataField]
    public SoundSpecifier? RetractSound = new SoundCollectionSpecifier("MantisBladeRetract");
}

public sealed partial class ToggleMantisBladeEvent : InstantActionEvent;
