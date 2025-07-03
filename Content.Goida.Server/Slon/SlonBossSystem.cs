using System.Numerics;
using Content.Goida.Slon;
using Content.Goida.Useless;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared._Lavaland.Aggression;
using Content.Shared.Damage;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Robust.Shared.Map;
using Robust.Shared.Timing;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map.Components;
using Robust.Shared.Random;
using Robust.Shared.Spawners;

namespace Content.Goida.Server.Slon;

public sealed class SlonBossSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly MobThresholdSystem _mobThreshold = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    private static readonly Vector2[] AttackDirections = { // todo
        new(1, 0),   // east
        new(0, 1),   // north
        new(-1, 0),  // west
        new(0, -1),  // south
        new(1, 1),   // north-east
        new(1, -1),  // south-east
        new(-1, 1),  // north-west
        new(-1, -1)  // south-west
    };

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SlonBossComponent, AggressorAddedEvent>(OnAggressorAdded);
        SubscribeLocalEvent<SlonBossComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<SlonBossComponent, DamageChangedEvent>(OnDamageChanged);
        SubscribeLocalEvent<SlonBossComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnStartup(EntityUid uid, SlonBossComponent component, ComponentStartup args)
    {
        if (TryComp<MobThresholdsComponent>(uid, out var thresholds))
        {
            _mobThreshold.SetMobStateThreshold(uid, component.BaseHealth, MobState.Dead, thresholds);
        }
    }
    private void OnMobStateChanged(EntityUid uid, SlonBossComponent component, MobStateChangedEvent args)
    {
        if (args.NewMobState == MobState.Dead)
        {
            QueueDel(uid);
        }
    }

    private void OnAggressorAdded(Entity<SlonBossComponent> ent, ref AggressorAddedEvent args)
    {
        if (!TryComp<AggressiveComponent>(ent, out var aggressive)
            || !TryComp<MobThresholdsComponent>(ent, out var thresholds))
            return;

        UpdateScaledThresholds(ent, aggressive, thresholds);
        if (!ent.Comp.Aggressive)
        {
            ActivateBoss(ent);
        }
    }

    private void OnDamageChanged(EntityUid uid, SlonBossComponent component, DamageChangedEvent args)
    {
        if (args.DamageDelta != null && args.DamageDelta.GetTotal() > FixedPoint2.Zero)
        {
            // times this was rewrited: 3
            component.Anger = Math.Min(component.Anger + (float)args.DamageDelta.GetTotal() * component.AngerPerDamage, component.MaxAnger);
            Dirty(uid, component);

            if (!component.Aggressive)
            {
                ActivateBoss((uid, component));
            }
        }
    }

    // i legit forgot why i made this
    private void UpdateScaledThresholds(EntityUid uid, AggressiveComponent aggressors, MobThresholdsComponent thresholds)
    {
        var playerCount = Math.Max(1, aggressors.Aggressors.Count);
        var healthScale = FixedPoint2.New(playerCount * 1.25f);
        var baseHealth = Comp<SlonBossComponent>(uid).BaseHealth;

        _mobThreshold.SetMobStateThreshold(uid, baseHealth * healthScale, MobState.Dead, thresholds);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var time = _timing.CurTime;
        var query = EntityQueryEnumerator<SlonBossComponent, AggressiveComponent>();

        while (query.MoveNext(out var uid, out var comp, out var aggressive))
        {
            var ent = (uid, comp);
            if (comp.Anger > 0)
            {
                comp.Anger = Math.Max(0, comp.Anger - comp.AngerDecay * frameTime);
                Dirty(uid, comp);
            }
            if (comp.Anger > 90 && _random.Prob(comp.TeleportChance * frameTime))
                TryTeleportToPlayer(ent);


            if (aggressive.Aggressors.Count > 0 && !comp.Aggressive)
                ActivateBoss(ent);

            else if (aggressive.Aggressors.Count == 0 && comp.Aggressive)
                DeactivateBoss(ent);


            if (comp.Aggressive && time >= comp.NextLavaAttackTime)
            {
                DoLavaAttack(ent);
                comp.NextLavaAttackTime = time + GetCooldown(comp);
                Dirty(uid, comp); // DIRTY IN UPDATE YIPEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE todo
            }
        }
    }
    private TimeSpan GetCooldown(SlonBossComponent comp)
    {
        var factor = 1f - (comp.Anger / comp.MaxAnger);
        return TimeSpan.FromSeconds(
            comp.MinAttackCooldown + (comp.BaseAttackCooldown - comp.MinAttackCooldown) * factor
        );
    }

    private void ActivateBoss(Entity<SlonBossComponent> ent)
    {
        ent.Comp.Aggressive = true;
        ent.Comp.NextLavaAttackTime = _timing.CurTime + ent.Comp.LavaAttackCooldown;
        Dirty(ent);
    }

    private void DeactivateBoss(Entity<SlonBossComponent> ent)
    {
        ent.Comp.Aggressive = false;
        Dirty(ent);
    }

    # region attacks
    private void DoLavaAttack(Entity<SlonBossComponent> ent)
    {
        if (_random.Prob(ent.Comp.SpecialAttackChance))
        {
            CreateLavaLake(ent);
        }
        else
        {
            DoRadialLavaAttack(ent);
        }
    }
    private void DoRadialLavaAttack(Entity<SlonBossComponent> ent)
    {
        var xform = Transform(ent);
        var center = _transform.GetWorldPosition(xform);

        foreach (var dir in AttackDirections)
        {
            SpreadLavaInDirection(ent, center, dir);
        }
    }

    private void CreateLavaLake(Entity<SlonBossComponent> ent)
    {
        var xform = Transform(ent);
        var center = _transform.GetWorldPosition(xform);
        var mapCoords = new MapCoordinates(center, xform.MapID);

        if (!_mapManager.TryFindGridAt(mapCoords, out var gridUid, out var grid))
            return;

        var radius = ent.Comp.LakeRadius;
        var centerTile = _mapSystem.GetTileRef(gridUid, grid, mapCoords);
        var centerIndices = centerTile.GridIndices;

        SpawnLavaWithFire(gridUid, grid, centerIndices, ent.Comp);
        for (int r = 1; r <= radius; r++)
        {
            var delay = r * 0.2f;

            Timer.Spawn(TimeSpan.FromSeconds(delay), () =>
            {
                if (Deleted(ent)) return;

                for (int x = -r; x <= r; x++)
                {
                    for (int y = -r; y <= r; y++)
                    {
                        if (Math.Abs(x) != r && Math.Abs(y) != r)
                            continue;

                        var indices = new Vector2i(centerIndices.X + x, centerIndices.Y + y);
                        SpawnLavaWithFire(gridUid, grid, indices, ent.Comp);
                    }
                }
            });
        }
    }

    private void SpawnLavaWithFire(EntityUid gridUid, MapGridComponent grid, Vector2i indices, SlonBossComponent comp)
    {
        var localPos = _mapSystem.GridTileToLocal(gridUid, grid, indices);
        var lava = Spawn(comp.LavaPrototype, localPos);
        EnsureComp<TimedDespawnComponent>(lava).Lifetime = comp.LavaLifetime;
    }

    private void SpreadLavaInDirection(Entity<SlonBossComponent> ent, Vector2 start, Vector2 direction)
    {
        var normalizedDir = direction.Normalized(); // i thought it was going to be easier
        var spreadSpeed = ent.Comp.LavaSpreadSpeed;
        var maxDistance = ent.Comp.LavaMaxDistance;
        var stepDelay = 1f / spreadSpeed;
        var mapId = Transform(ent).MapID;
        var lavaProto = ent.Comp.LavaPrototype;

        for (int step = 1; step <= maxDistance; step++)
        {
            var stepCopy = step;
            var position = start + normalizedDir * stepCopy;
            var mapCoords = new MapCoordinates(position, mapId);

            Timer.Spawn(TimeSpan.FromSeconds(stepCopy * stepDelay), () =>
            {
                if (Deleted(ent) || Terminating(ent)) return;

                if (!_mapManager.TryFindGridAt(mapCoords, out var gridUid, out var grid))
                    return;

                var tile = _mapSystem.GetTileRef(gridUid, grid, mapCoords);
                if (tile.Tile.IsEmpty)
                    return;

                var centerPos = _mapSystem.GridTileToLocal(gridUid, grid, tile.GridIndices);

                var lava = Spawn(lavaProto, centerPos);
                EnsureComp<TimedDespawnComponent>(lava).Lifetime = 5f;
            });
        }
    }
    private void TryTeleportToPlayer(Entity<SlonBossComponent> ent)
    {
        if (!TryComp<AggressiveComponent>(ent, out var aggressive) ||
            aggressive.Aggressors.Count == 0)
            return;

        // teleport to the random ppl
        var target = _random.Pick(aggressive.Aggressors);
        if (Deleted(target) || !TryComp<TransformComponent>(target, out var targetXform))
            return;

        var direction = _random.NextVector2().Normalized();
        var distance = _random.NextFloat(1f, 3f);
        var position = _transform.GetWorldPosition(targetXform) + direction * distance;

        _transform.SetWorldPosition(ent, position);
    }
    #endregion

    #region vsf

    // something was here but it broke the gam

    #endregion
}
