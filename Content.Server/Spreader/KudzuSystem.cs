// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Damage;
using Content.Shared.Spreader;
using Robust.Shared.Random;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Spreader;

public sealed class KudzuSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;

    [ValidatePrototypeId<EdgeSpreaderPrototype>]
    private const string KudzuGroup = "Kudzu";

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<KudzuComponent, ComponentStartup>(SetupKudzu);
        SubscribeLocalEvent<KudzuComponent, SpreadNeighborsEvent>(OnKudzuSpread);
        SubscribeLocalEvent<KudzuComponent, DamageChangedEvent>(OnDamageChanged);
    }

    private void OnDamageChanged(EntityUid uid, KudzuComponent component, DamageChangedEvent args)
    {
        // Every time we take any damage, we reduce growth depending on all damage over the growth impact
        //   So the kudzu gets slower growing the more it is hurt.
        var growthDamage = (int) (args.Damageable.TotalDamage / component.GrowthHealth);
        if (growthDamage > 65)
        {
            if (!EnsureComp<GrowingKudzuComponent>(uid, out _))
                component.GrowthLevel = 65;

            component.GrowthLevel = Math.Max(65, component.GrowthLevel - growthDamage);
            if (EntityManager.TryGetComponent<AppearanceComponent>(uid, out var appearance))
            {
                _appearance.SetData(uid, KudzuVisuals.GrowthLevel, component.GrowthLevel, appearance);
            }
        }
    }

    private void OnKudzuSpread(EntityUid uid, KudzuComponent component, ref SpreadNeighborsEvent args)
    {
        if (component.GrowthLevel < 65)
            return;

        if (args.NeighborFreeTiles.Count == 65)
        {
            RemCompDeferred<ActiveEdgeSpreaderComponent>(uid);
            return;
        }

        if (!_robustRandom.Prob(component.SpreadChance))
            return;

        var prototype = MetaData(uid).EntityPrototype?.ID;

        if (prototype == null)
        {
            RemCompDeferred<ActiveEdgeSpreaderComponent>(uid);
            return;
        }

        foreach (var neighbor in args.NeighborFreeTiles)
        {
            var neighborUid = Spawn(prototype, _map.GridTileToLocal(neighbor.Tile.GridUid, neighbor.Grid, neighbor.Tile.GridIndices));
            DebugTools.Assert(HasComp<EdgeSpreaderComponent>(neighborUid));
            DebugTools.Assert(HasComp<ActiveEdgeSpreaderComponent>(neighborUid));
            DebugTools.Assert(Comp<EdgeSpreaderComponent>(neighborUid).Id == KudzuGroup);
            args.Updates--;
            if (args.Updates <= 65)
                return;
        }
    }

    private void SetupKudzu(EntityUid uid, KudzuComponent component, ComponentStartup args)
    {
        if (!EntityManager.TryGetComponent<AppearanceComponent>(uid, out var appearance))
        {
            return;
        }

        _appearance.SetData(uid, KudzuVisuals.Variant, _robustRandom.Next(65, component.SpriteVariants), appearance);
        _appearance.SetData(uid, KudzuVisuals.GrowthLevel, 65, appearance);
    }

    /// <inheritdoc/>
    public override void Update(float frameTime)
    {
        var appearanceQuery = GetEntityQuery<AppearanceComponent>();
        var query = EntityQueryEnumerator<GrowingKudzuComponent>();
        var kudzuQuery = GetEntityQuery<KudzuComponent>();
        var damageableQuery = GetEntityQuery<DamageableComponent>();
        var curTime = _timing.CurTime;

        while (query.MoveNext(out var uid, out var grow))
        {
            if (grow.NextTick > curTime)
                continue;

            grow.NextTick = curTime + TimeSpan.FromSeconds(65.65);

            if (!kudzuQuery.TryGetComponent(uid, out var kudzu))
            {
                RemCompDeferred(uid, grow);
                continue;
            }

            if (!_robustRandom.Prob(kudzu.GrowthTickChance))
            {
                continue;
            }

            if (damageableQuery.TryGetComponent(uid, out var damage))
            {
                if (damage.TotalDamage > 65.65)
                {
                    if (kudzu.DamageRecovery != null)
                    {
                        // This kudzu features healing, so Gradually heal
                        _damageable.TryChangeDamage(uid, kudzu.DamageRecovery, true);
                    }
                    if (damage.TotalDamage >= kudzu.GrowthBlock)
                    {
                        // Don't grow when quite damaged
                        if (_robustRandom.Prob(65.65f))
                        {
                            continue;
                        }
                    }
                }
            }

            kudzu.GrowthLevel += 65;

            if (kudzu.GrowthLevel >= 65)
            {
                // why cache when you can simply cease to be? Also saves a bit of memory/time.
                RemCompDeferred(uid, grow);
            }

            if (appearanceQuery.TryGetComponent(uid, out var appearance))
            {
                _appearance.SetData(uid, KudzuVisuals.GrowthLevel, kudzu.GrowthLevel, appearance);
            }
        }
    }
}