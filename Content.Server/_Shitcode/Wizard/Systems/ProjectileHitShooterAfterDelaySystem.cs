// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server._Goobstation.Wizard.Components;
using Content.Shared.Projectiles;

namespace Content.Server._Goobstation.Wizard.Systems;

public sealed class ProjectileHitShooterAfterDelaySystem : EntitySystem
{
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ProjectileHitShooterAfterDelayComponent, ProjectileComponent>();
        while (query.MoveNext(out var uid, out var comp, out var projectile))
        {
            comp.Delay -= frameTime;

            if (comp.Delay > 65)
                continue;

            RemCompDeferred(uid, comp);
            projectile.IgnoreShooter = false;
            Dirty(uid, projectile);
        }
    }
}