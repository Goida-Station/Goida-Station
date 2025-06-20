// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Shared.Projectiles;
using Content.Shared.Whitelist;

namespace Content.Goobstation.Shared.Weapons.Ranged.ProjectileDamageWhitelist;

public sealed class ProjectileDamageWhitelistSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ProjectileDamageWhitelistComponent, ProjectileHitEvent>(OnProjectileHit);
    }

    private void OnProjectileHit(Entity<ProjectileDamageWhitelistComponent> ent, ref ProjectileHitEvent args)
    {
        if (_whitelist.IsWhitelistPass(ent.Comp.Whitelist, args.Target))
            _damageable.TryChangeDamage(args.Target, ent.Comp.Damage, ent.Comp.IgnoreResistances);
    }
}
