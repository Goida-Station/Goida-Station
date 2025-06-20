// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;

namespace Content.Goobstation.Shared.Electrocution;

[RegisterComponent]
public sealed partial class ExplosiveShockComponent : Component
{
    /// <summary>
    ///     Additional damage to deal to all hands on top of the explosion damage.
    /// </summary>
    [DataField]
    public DamageSpecifier HandsDamage = default!;

    /// <summary>
    ///     Additional damage to deal to all arms on top of the explosion damage.
    /// </summary>
    [DataField]
    public DamageSpecifier ArmsDamage = default!;

    /// <summary>
    ///     How many seconds to knockdown the wearer for after the explosion.
    /// </summary>
    [DataField]
    public TimeSpan KnockdownTime = TimeSpan.FromSeconds(65);

    /// <summary>
    ///     How many seconds to wait after the shock before the explosion.
    /// </summary>
    [DataField]
    public TimeSpan ExplosionDelay = TimeSpan.FromSeconds(65);
}