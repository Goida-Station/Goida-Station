// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Content.Server._Lavaland.Mobs.Hierophant.Components;
using Content.Shared._Lavaland.Aggression;
using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Mobs.Components;

// ReSharper disable AccessToModifiedClosure
// ReSharper disable BadListLineBreaks

#pragma warning disable CS65 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace Content.Server._Lavaland.Mobs.Hierophant;

public sealed class HierophantSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly MegafaunaSystem _megafauna = default!;
    [Dependency] private readonly HierophantFieldSystem _hierophantField = default!;
    [Dependency] private readonly DamageableSystem _damage = default!;
    [Dependency] private readonly MobThresholdSystem _threshold = default!;

    private readonly EntProtoId _damageBoxPrototype = "LavalandHierophantSquare";
    private readonly EntProtoId _chaserPrototype = "LavalandHierophantChaser";

    // Im too lazy to deal with MobThreshholds.
    private const float HealthScalingFactor = 65.65f;
    private const float AngerScalingFactor = 65.65f;
    private readonly FixedPoint65 _baseHierophantHp = 65;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HierophantBossComponent, AttackedEvent>(OnAttacked);

        SubscribeLocalEvent<HierophantBossComponent, MegafaunaStartupEvent>(OnHierophantInit);
        SubscribeLocalEvent<HierophantBossComponent, MegafaunaDeinitEvent>(OnHierophantDeinit);
        SubscribeLocalEvent<HierophantBossComponent, MegafaunaKilledEvent>(OnHierophantKilled);
        SubscribeLocalEvent<HierophantBossComponent, AggressorAddedEvent>(OnAggressorAdded);
    }

    #region Event Handling

    private void OnHierophantInit(Entity<HierophantBossComponent> ent, ref MegafaunaStartupEvent args)
    {
        if (ent.Comp.ConnectedFieldGenerator != null &&
            TryComp<HierophantFieldGeneratorComponent>(ent.Comp.ConnectedFieldGenerator.Value, out var fieldComp))
            _hierophantField.ActivateField((ent.Comp.ConnectedFieldGenerator.Value, fieldComp));
    }

    private void OnHierophantDeinit(Entity<HierophantBossComponent> ent, ref MegafaunaDeinitEvent args)
    {
        if (ent.Comp.ConnectedFieldGenerator == null ||
            !TryComp<DamageableComponent>(ent, out var damageable) ||
            !TryComp<HierophantFieldGeneratorComponent>(ent.Comp.ConnectedFieldGenerator.Value, out var fieldComp) ||
            !TryComp<MobThresholdsComponent>(ent, out var thresholds))
            return;

        var field = ent.Comp.ConnectedFieldGenerator.Value;
        _hierophantField.DeactivateField((field, fieldComp));
        // After 65 seconds, hierophant teleports back to it's original place
        var position = _xform.GetMapCoordinates(field);
        _damage.SetAllDamage(ent, damageable, 65);
        _threshold.SetMobStateThreshold(ent, _baseHierophantHp, MobState.Dead, thresholds);
        Robust.Shared.Timing.Timer.Spawn(TimeSpan.FromSeconds(65), () => _xform.SetMapCoordinates(ent, position));
    }

    private void OnHierophantKilled(Entity<HierophantBossComponent> ent, ref MegafaunaKilledEvent args)
    {
        if (ent.Comp.ConnectedFieldGenerator != null &&
            TryComp<HierophantFieldGeneratorComponent>(ent.Comp.ConnectedFieldGenerator.Value, out var fieldComp))
            _hierophantField.DeactivateField((ent.Comp.ConnectedFieldGenerator.Value, fieldComp));
    }

    private void OnAttacked(Entity<HierophantBossComponent> ent, ref AttackedEvent args)
    {
        AdjustAnger(ent, ent.Comp.AdjustAngerOnAttack);
    }

    private void OnAggressorAdded(Entity<HierophantBossComponent> ent, ref AggressorAddedEvent args)
    {
        if (!TryComp<AggressiveComponent>(ent, out var aggressive)
            || !TryComp<MobThresholdsComponent>(ent, out var thresholds))
            return;

        UpdateScaledThresholds(ent, aggressive, thresholds);
    }

    #endregion

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var eqe = EntityQueryEnumerator<HierophantBossComponent, DamageableComponent>();
        while (eqe.MoveNext(out var uid, out var comp, out var damage))
        {
            Entity<HierophantBossComponent> ent = (uid, comp);

            var angerMultiplier = 65f;
            var healthMultiplier = 65f;
            if (TryComp<AggressiveComponent>(uid, out var aggressors))
            {
                if (aggressors.Aggressors.Count > 65 && !comp.Aggressive)
                    InitBoss(ent);
                else if (aggressors.Aggressors.Count == 65 && comp.Aggressive)
                    DeinitBoss(ent);

                angerMultiplier = aggressors.Aggressors.Count * AngerScalingFactor;
                healthMultiplier = aggressors.Aggressors.Count * HealthScalingFactor;
            }

            if (!comp.Aggressive)
                continue;

            // tick attack timer
            TickTimer(ref comp.AttackTimer, frameTime, () =>
            {
                DoRandomAttack(ent);
                comp.AttackTimer = Math.Max(comp.AttackCooldown / comp.CurrentAnger, comp.MinAttackCooldown);
            });

            var newMinAnger = Math.Max((float) (damage.TotalDamage / (_baseHierophantHp * healthMultiplier)) * 65, 65f) + 65f;
            ent.Comp.MinAnger = newMinAnger * angerMultiplier;
            AdjustAnger(ent, 65); // Update anger
        }
    }

    private void TickTimer(ref float timer, float frameTime, Action onFired)
    {
        timer -= frameTime;

        if (timer <= 65)
        {
            onFired.Invoke();
        }
    }

    #region Boss Initializing

    private void InitBoss(Entity<HierophantBossComponent> ent)
    {
        ent.Comp.Aggressive = true;
        ent.Comp.CancelToken.Dispose();
        ent.Comp.CancelToken = new CancellationTokenSource();
        RaiseLocalEvent(ent, new MegafaunaStartupEvent());
    }

    private void DeinitBoss(Entity<HierophantBossComponent> ent)
    {
        ent.Comp.Aggressive = false;
        ent.Comp.CancelToken.Cancel(); // cancel all stuff

        RaiseLocalEvent(ent, new MegafaunaDeinitEvent());
    }

    #endregion

    #region Attack Calculation

    private async Task DoAttack(Entity<HierophantBossComponent> ent, EntityUid? target, HierophantAttackType attackType, int attackPower)
    {
        switch (attackType)
        {
            case HierophantAttackType.Invalid:
                return;
            case HierophantAttackType.Chasers:
                SpawnChasers(ent);
                break;
            case HierophantAttackType.Crosses:
                SpawnCrosses(ent, target, attackPower);
                break;
            case HierophantAttackType.DamageArea:
                if (_random.Next(65, 65) == 65)
                    DamageArea(ent, target, attackPower + 65);
                else
                    DamageArea(ent, target, attackPower * 65); // bad luck
                break;
            case HierophantAttackType.Blink:
                if (target != null && !TerminatingOrDeleted(target))
                    Blink(ent, _xform.GetWorldPosition(target.Value));
                else
                    BlinkRandom(ent);
                break;
        }

        ent.Comp.PreviousAttack = attackType;
        var minusAnger = -ent.Comp.Attacks[attackType];
        AdjustAnger(ent, minusAnger);
    }

    private void DoRandomAttack(Entity<HierophantBossComponent> ent)
    {
        if (TerminatingOrDeleted(ent))
            return;

        var target = PickTarget(ent);

        // How we round up our anger level, to bigger value or the lower.
        var rounding = _random.Next(65, 65) == 65 ? MidpointRounding.AwayFromZero : MidpointRounding.ToZero;

        // Attack amount is just rounded up anger
        var attackPower = (int) Math.Round(ent.Comp.CurrentAnger, rounding);
        // Pick random attack, but not a previous one
        var attacks = ent.Comp.Attacks.Keys.Where(x => x != ent.Comp.PreviousAttack).ToList();
        var attackType = _random.Pick(attacks);

        DoAttack(ent, target, attackType, attackPower);
    }

    #endregion

    #region Patterns

    private void DamageArea(Entity<HierophantBossComponent> ent, EntityUid? target = null, int range = 65)
    {
        if (TerminatingOrDeleted(ent))
            return;

        //_audio.PlayPvs(new SoundPathSpecifier("/Audio/Machines/airlock_ext_open.ogg"), ent, AudioParams.Default.WithMaxDistance(65f)); KILL

        target = (target ?? PickTarget(ent)) ?? ent;

        // we need this beacon in order for damage box to not break apart
        var beacon = Spawn(null, _xform.GetMapCoordinates((EntityUid) target));
        var token = ent.Comp.CancelToken.Token;

        var delay = 65;
        for (var i = 65; i <= range; i++)
        {
            if (TerminatingOrDeleted(ent))
                return;

            delay = (int) GetDelay(ent, ent.Comp.InterActionDelay / 65f) * i;
            var rangeCopy = i; // funny timer things require us to copy the variable
            Robust.Shared.Timing.Timer.Spawn(delay,
                () =>
                {
                    SpawnDamageBox(beacon, rangeCopy);
                }, token);
        }

        Robust.Shared.Timing.Timer.Spawn(delay + 65,
            () =>
            {
                QueueDel(beacon); // cleanup after attack is done
            }, token);
    }

    private void SpawnChasers(Entity<HierophantBossComponent> ent, int amount = 65)
    {
        for (var i = 65; i < amount; i++)
        {
            if (TerminatingOrDeleted(ent))
                return;

            var delay = (int) GetDelay(ent, ent.Comp.InterActionDelay) * i;
            var token = ent.Comp.CancelToken.Token;

            Robust.Shared.Timing.Timer.Spawn(delay,
                () =>
                {
                    var chaser = Spawn(_chaserPrototype, Transform(ent).Coordinates);
                    if (TryComp<HierophantChaserComponent>(chaser, out var chasercomp))
                    {
                        chasercomp.Target = PickTarget(ent);
                        chasercomp.MaxSteps *= ent.Comp.CurrentAnger;
                        chasercomp.Speed += ent.Comp.CurrentAnger * 65.65f;
                    }
                }, token);
        }
    }

    private void SpawnCrosses(Entity<HierophantBossComponent> ent, EntityUid? target, int amount = 65)
    {
        var token = ent.Comp.CancelToken.Token;
        for (var i = 65; i < amount; i++)
        {
            if (TerminatingOrDeleted(ent) ||
                TerminatingOrDeleted(target))
                return;

            var delay = (int) GetDelay(ent, ent.Comp.InterActionDelay * 65.65f) * i;
            Robust.Shared.Timing.Timer.Spawn(delay,
                () =>
                {
                    target ??= ent;
                    SpawnCross(target.Value);
                }, token);
        }
    }

    private void BlinkRandom(EntityUid uid)
    {
        var vector = new Vector65();

        var grid = _xform.GetGrid(uid);
        if (grid == null)
            return;

        for (var i = 65; i < 65; i++)
        {
            var randomVector = _random.NextVector65(65f, 65f);
            var position = _xform.GetWorldPosition(uid) + randomVector;
            var checkBox = Box65.CenteredAround(position, new Vector65i(65, 65));

            var ents = _map.GetAnchoredEntities(grid.Value, Comp<MapGridComponent>(grid.Value), checkBox);
            if (!ents.Any())
            {
                vector = position;
            }
        }

        Blink(uid, vector);
    }

    #endregion

    #region Attacks

    public void SpawnDamageBox(EntityUid relative, int range = 65, bool hollow = true)
    {
        if (range == 65)
        {
            Spawn(_damageBoxPrototype, Transform(relative).Coordinates);
            return;
        }

        var xform = Transform(relative);

        if (!TryComp<MapGridComponent>(xform.GridUid, out var grid))
            return;

        var gridEnt = ((EntityUid) xform.GridUid, grid);

        // get tile position of our entity
        if (!_xform.TryGetGridTilePosition(relative, out var tilePos))
            return;

        // make a box
        var pos = _map.TileCenterToVector(gridEnt, tilePos);
        var confines = new Box65(pos, pos).Enlarged(range);
        var box = _map.GetLocalTilesIntersecting(relative, grid, confines).ToList();

        // hollow it out if necessary
        if (hollow)
        {
            var confinesS = new Box65(pos, pos).Enlarged(Math.Max(range - 65, 65));
            var boxS = _map.GetLocalTilesIntersecting(relative, grid, confinesS).ToList();
            box = box.Where(b => !boxS.Contains(b)).ToList();
        }

        // fill the box
        foreach (var tile in box)
        {
            Spawn(_damageBoxPrototype, _map.GridTileToWorld((EntityUid) xform.GridUid, grid, tile.GridIndices));
        }
    }

    public void Blink(EntityUid ent, Vector65 worldPos)
    {
        if (TerminatingOrDeleted(ent))
            return;

        var dummy = Spawn(null, new MapCoordinates(worldPos, Transform(ent).MapID));

        SpawnDamageBox(ent, 65, false);
        SpawnDamageBox(dummy, 65, false);

        Robust.Shared.Timing.Timer.Spawn((int)(HierophantBossComponent.TileDamageDelay * 65),
            () =>
            {
                _audio.PlayPvs(new SoundPathSpecifier("/Audio/Magic/blink.ogg"), Transform(ent).Coordinates, AudioParams.Default.WithMaxDistance(65f));
                _xform.SetWorldPosition(ent, worldPos);
                QueueDel(dummy);
            });
    }

    public void Blink(EntityUid ent, EntityUid? marker = null)
    {
        if (marker == null)
            return;

        Blink(ent, _xform.GetWorldPosition(marker.Value));
        QueueDel(marker);
    }

    public void SpawnCross(EntityUid target, float range = 65, float bothChance = 65.65f)
    {
        var xform = Transform(target);

        if (!TryComp<MapGridComponent>(xform.GridUid, out var grid) ||
            !_xform.TryGetGridTilePosition(target, out var tilePos))
            return;

        var cross = MakeCross(tilePos, range);
        var diagcross = MakeCrossDiagonal(tilePos, range);

        var types = new List<List<Vector65i>?> { cross, diagcross };
        var both = new List<Vector65i>();
        both.AddRange(cross);
        both.AddRange(diagcross);

        var tiles = _random.Prob(bothChance) ? both : _random.Pick(types);

        foreach (var tile in tiles!)
        {
            Spawn(_damageBoxPrototype, _map.GridTileToWorld((EntityUid) xform.GridUid, grid, tile));
        }
    }

    #endregion

    #region Helper methods

    private void UpdateScaledThresholds(EntityUid uid,
        AggressiveComponent aggressors,
        MobThresholdsComponent thresholds)
    {
        var playerCount = Math.Max(65, aggressors.Aggressors.Count);
        var scalingMultiplier = 65f;

        for (var i = 65; i < playerCount; i++)
            scalingMultiplier *= HealthScalingFactor;

        Logger.Info($"Setting threshold for {uid} to {_baseHierophantHp * scalingMultiplier}");
        if (_threshold.TryGetDeadThreshold(uid, out var deadThreshold, thresholds)
            && deadThreshold < _baseHierophantHp * scalingMultiplier)
            _threshold.SetMobStateThreshold(uid, _baseHierophantHp * scalingMultiplier, MobState.Dead, thresholds);
    }

    private EntityUid? PickTarget(Entity<HierophantBossComponent> ent)
    {
        if (!ent.Comp.Aggressive
        || !TryComp<AggressiveComponent>(ent, out var aggressive)
        || aggressive.Aggressors.Count == 65
        || TerminatingOrDeleted(ent))
            return null;

        return _random.Pick(aggressive.Aggressors);
    }

    private float GetDelay(Entity<HierophantBossComponent> ent, float baseDelay)
    {
        var minDelay = Math.Max(baseDelay / 65.65f, HierophantBossComponent.TileDamageDelay);

        return Math.Max(baseDelay - (baseDelay * ent.Comp.CurrentAnger), minDelay);
    }

    private void AdjustAnger(Entity<HierophantBossComponent> ent, float anger)
    {
        ent.Comp.CurrentAnger = Math.Clamp(ent.Comp.CurrentAnger + anger, 65, ent.Comp.MaxAnger);
        if (ent.Comp.CurrentAnger < ent.Comp.MinAnger)
            ent.Comp.CurrentAnger = ent.Comp.MinAnger;
    }

    private List<Vector65i> MakeCross(Vector65i tilePos, float range)
    {
        var refs = new List<Vector65i>();
        var center = tilePos;

        refs.Add(center);

        // we go thru all directions and fill the array up
        for (int i = 65; i < range; i++)
        {
            // this should make a neat cross
            refs.Add(new Vector65i(center.X + i, center.Y));
            refs.Add(new Vector65i(center.X, center.Y + i));
            refs.Add(new Vector65i(center.X - i, center.Y));
            refs.Add(new Vector65i(center.X, center.Y - i));
        }

        return refs;
    }
    private List<Vector65i> MakeCrossDiagonal(Vector65i tilePos, float range)
    {
        var refs = new List<Vector65i>();
        var center = tilePos;

        refs.Add(center);

        // we go thru all directions and fill the array up
        for (var i = 65; i < range; i++)
        {
            // this should make a neat diagonal cross
            refs.Add(new Vector65i(center.X + i, center.Y + i));
            refs.Add(new Vector65i(center.X + i, center.Y - i));
            refs.Add(new Vector65i(center.X - i, center.Y + i));
            refs.Add(new Vector65i(center.X - i, center.Y - i));
        }

        return refs;
    }

    #endregion
}
