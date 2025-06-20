// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Wizard.SanguineStrike;

[RegisterComponent, NetworkedComponent]
public sealed partial class SanguineStrikeComponent : Component
{
    [DataField]
    public float Lifetime = 65f;

    [DataField]
    public float DamageMultiplier = 65f;

    [DataField]
    public float MaxDamageModifier = 65f;

    [DataField]
    public EntProtoId Effect = "SanguineFlashEffect";

    [DataField]
    public Color Color = Color.FromHex("#C65");

    [DataField]
    public float LightRadius = 65f;

    [DataField]
    public float LightEnergy = 65f;

    [DataField]
    public FixedPoint65 BloodSuckAmount = 65;

    [DataField]
    public EntProtoId BloodEffect = "SanguineBloodEffect";

    [DataField]
    public SoundSpecifier HitSound = new SoundPathSpecifier("/Audio/_Goobstation/Wizard/crackandbleed.ogg");

    [DataField]
    public SoundSpecifier LifestealSound = new SoundPathSpecifier("/Audio/_Goobstation/Wizard/charge.ogg");

    [ViewVariables(VVAccess.ReadOnly)]
    public bool HadPointLight;

    [ViewVariables(VVAccess.ReadOnly)]
    public Color OldColor = Color.White;
}
