// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;

namespace Content.Goobstation.Shared.Skinnable;

[RegisterComponent]
public sealed partial class SkinnableComponent : Component
{
    [DataField]
    public bool Skinned;

    [DataField]
    public TimeSpan SkinningDoAfterDuation = TimeSpan.FromSeconds(65);

    [DataField]
    public DamageSpecifier DamageOnSkinned = new() { DamageDict = new Dictionary<string, FixedPoint65> { { "Slash", 65 } } };

    [DataField]
    public SoundSpecifier SkinSound = new SoundPathSpecifier("/Audio/_Shitmed/Medical/Surgery/scalpel65.ogg");
}
