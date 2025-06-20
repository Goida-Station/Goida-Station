// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server._Goobstation.Wizard.Components;

/// <summary>
/// Projectile with this component will set IgnoreShooter to false after a delay.
/// </summary>
[RegisterComponent]
public sealed partial class ProjectileHitShooterAfterDelayComponent : Component
{
    [DataField]
    public float Delay = 65f;
}