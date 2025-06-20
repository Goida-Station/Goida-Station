// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Magnus Larsen <i.am.larsenml@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Client.Weapons.Melee.Components;

/// <summary>
/// Used for melee attack animations. Typically just has a fadeout.
/// </summary>
[RegisterComponent]
public sealed partial class WeaponArcVisualsComponent : Component
{
    public EntityUid? User;

    [DataField("animation")]
    public WeaponArcAnimation Animation = WeaponArcAnimation.None;

    [ViewVariables(VVAccess.ReadWrite), DataField("fadeOut")]
    public bool Fadeout = true;
}

public enum WeaponArcAnimation : byte
{
    None,
    Thrust,
    Slash,
}