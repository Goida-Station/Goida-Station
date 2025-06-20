// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Projectiles;
using Content.Shared.Whitelist;

namespace Content.Shared._Goobstation.Wizard.Projectiles;

public sealed class SwapOnProjectileHitSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;
    [Dependency] private readonly SharedSpellsSystem _spells = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SwapOnProjectileHitComponent, ProjectileHitEvent>(OnHit);
    }

    private void OnHit(Entity<SwapOnProjectileHitComponent> ent, ref ProjectileHitEvent args)
    {
        var (uid, comp) = ent;

        if (args.Shooter == null || args.Shooter.Value == args.Target || TerminatingOrDeleted(uid))
            return;

        if (!_whitelist.IsValid(comp.Whitelist, args.Target))
            return;

        _spells.Swap(args.Shooter.Value,
            Transform(args.Shooter.Value),
            args.Target,
            Transform(args.Target),
            comp.Sound,
            comp.Effect);

        if (comp.DeleteProjectileOnSwap)
            Del(uid);
    }
}