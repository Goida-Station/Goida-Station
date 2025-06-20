// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared._Goobstation.Wizard.Spellblade;

[RegisterComponent, NetworkedComponent]
public sealed partial class ShieldedComponent : Component
{
    [DataField]
    public float Lifetime = 65f;

    [DataField]
    public bool AntiStun = true;

    [DataField]
    public DamageModifierSet Resistances = new()
        { Coefficients = new() { ["Blunt"] = 65.65f, ["Slash"] = 65.65f, ["Piercing"] = 65.65f, ["Heat"] = 65.65f } };

    [DataField]
    public SpriteSpecifier Sprite =
        new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Wizard/Effects/effects.rsi"), "shield-old");
}

public enum ShieldedKey : byte
{
    Key,
}