// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Wizard.SanguineStrike;
using Content.Shared.Mobs.Systems;
using Content.Shared.Projectiles;
using Content.Shared.Whitelist;

namespace Content.Shared._Goobstation.Wizard.Projectiles;

public sealed class LifeStealOnProjectileHitSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;
    [Dependency] private readonly SharedSanguineStrikeSystem _sanguine = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LifeStealOnProjectileHitComponent, ProjectileHitEvent>(OnHit);
    }

    private void OnHit(Entity<LifeStealOnProjectileHitComponent> ent, ref ProjectileHitEvent args)
    {
        if (args.Shooter == null || args.Shooter.Value == args.Target)
            return;

        var (_, comp) = ent;

        if (!_whitelist.IsValid(comp.Whitelist, args.Target))
            return;

        if (_mobState.IsDead(args.Target))
            return;

        _sanguine.LifeSteal(args.Shooter.Value, comp.LifeStealAmount);
        List<EntityUid> target = new() { args.Target };
        _sanguine.BloodSteal(args.Shooter.Value, target, comp.BloodStealAmount, null);
        _sanguine.ParticleEffects(args.Shooter.Value, target, comp.Effect);
    }
}