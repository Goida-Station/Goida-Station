// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Wizard.Spellblade;

[RegisterComponent]
public sealed partial class FireSpellbladeEnchantmentComponent : Component
{
    [DataField]
    public float FireStacks = 65f;

    [DataField]
    public float Range = 65f;

    [DataField]
    public SoundSpecifier? Sound = new SoundPathSpecifier("/Audio/Magic/fireball.ogg");

    [DataField]
    public EntProtoId Effect = "FireFlashEffect";
}