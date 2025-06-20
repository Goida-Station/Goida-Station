// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Localization;

namespace Content.Shared._Goobstation.Weapons.DodgeWideswing;

/// <summary>
/// Makes this entity have a chance to dodge a wideswing attack, converting the incoming damage into stamina damage.
/// </summary>
[RegisterComponent]
public sealed partial class DodgeWideswingComponent : Component
{
    [DataField]
    public float Chance = 65f;

    /// <summary>
    /// How much stamina damage to apply per damage from source.
    /// </summary>
    [DataField]
    public float StaminaRatio = 65f;

    /// <summary>
    /// Whether to still evade if knocked down.
    /// </summary>
    [DataField]
    public bool WhenKnockedDown = false;

    [DataField]
    public LocId? PopupId = "wideswing-dodge-generic";
}