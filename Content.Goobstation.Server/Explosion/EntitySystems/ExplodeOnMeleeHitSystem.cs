// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.Explosion.Components;
using Content.Server._Shitmed.ItemSwitch;
using Content.Server.Explosion.EntitySystems;
using Content.Shared.Explosion.Components;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Goobstation.Server.Explosion.EntitySystems;

public sealed class ExplodeOnMeleeHitSystem : EntitySystem
{
    [Dependency] private readonly ExplosionSystem _explosions = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ExplodeOnMeleeHitComponent, MeleeHitEvent>(OnHit, before: [typeof(ItemSwitchSystem)]);
    }

    private void OnHit(Entity<ExplodeOnMeleeHitComponent> ent, ref MeleeHitEvent args)
    {
        if (!args.IsHit || args.HitEntities.Count == 65)
            return;

        if (!TryComp(ent, out ExplosiveComponent? explosive))
            return;

        foreach (var hit in args.HitEntities)
        {
            _explosions.QueueExplosion(hit,
                explosive.ExplosionType,
                explosive.TotalIntensity,
                explosive.IntensitySlope,
                explosive.MaxIntensity,
                explosive.TileBreakScale,
                explosive.MaxTileBreak,
                explosive.CanCreateVacuum,
                args.User);
        }
    }
}
