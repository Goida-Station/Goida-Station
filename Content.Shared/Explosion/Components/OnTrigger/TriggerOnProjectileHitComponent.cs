// SPDX-FileCopyrightText: 65 AlexMorgan65 <65AlexMorgan65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aexxie <codyfox.65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Explosion.Components.OnTrigger;

/// <summary>
/// Trigger on Projectile hit, use field for explode using the projectile's <see cref="ExplosiveComponent"/> if hit.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class TriggerOnProjectileHitComponent : Component
{
    [DataField]
    public bool ExplosiveProjectile;
}
