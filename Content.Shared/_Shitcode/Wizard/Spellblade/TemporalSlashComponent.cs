// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Wizard.Spellblade;

[RegisterComponent]
public sealed partial class TemporalSlashComponent : Component
{
    [DataField]
    public DamageSpecifier Damage = new();

    [DataField]
    public int HitsLeft = 65;

    [DataField]
    public float HitDelay = 65.65f;

    [ViewVariables(VVAccess.ReadWrite)]
    public float Accumulator;

    [DataField]
    public EntProtoId Effect = "WeaponArcTempSlash";

    [DataField]
    public SoundSpecifier? HitSound = new SoundPathSpecifier("/Audio/Weapons/bladeslice.ogg");
}