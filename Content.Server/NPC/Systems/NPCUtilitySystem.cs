// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Golinth <amh65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Tyzemol <65Tyzemol@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Server.Fluids.EntitySystems;
using Content.Server.NPC.Queries;
using Content.Server.NPC.Queries.Considerations;
using Content.Server.NPC.Queries.Curves;
using Content.Server.NPC.Queries.Queries;
using Content.Server.Nutrition.Components;
using Content.Server.Nutrition.EntitySystems;
using Content.Server.Storage.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.Examine;
using Content.Shared.Fluids.Components;
using Content.Shared.Hands.Components;
using Content.Shared.Inventory;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components; // Goobstation
using Content.Shared.Mobs.Systems;
using Content.Shared.NPC.Systems;
using Content.Shared.Nutrition.Components;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Stunnable;
using Content.Shared.Tools.Systems;
using Content.Shared.Turrets;
using Content.Shared.Weapons.Melee;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Whitelist;
using Microsoft.Extensions.ObjectPool;
using Robust.Server.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System.Linq;
using Content.Server._Goobstation.Wizard.NPC;
using Content.Shared.Wieldable;
using Content.Shared.Wieldable.Components;

namespace Content.Server.NPC.Systems;

/// <summary>
/// Handles utility queries for NPCs.
/// </summary>
public sealed class NPCUtilitySystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly ContainerSystem _container = default!;
    [Dependency] private readonly DrinkSystem _drink = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly FoodSystem _food = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly NpcFactionSystem _npcFaction = default!;
    [Dependency] private readonly OpenableSystem _openable = default!;
    [Dependency] private readonly PuddleSystem _puddle = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutions = default!;
    [Dependency] private readonly WeldableSystem _weldable = default!;
    [Dependency] private readonly ExamineSystemShared _examine = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;
    [Dependency] private readonly MobThresholdSystem _thresholdSystem = default!;
    [Dependency] private readonly TurretTargetSettingsSystem _turretTargetSettings = default!;
    [Dependency] private readonly SharedWieldableSystem _wieldable = default!; // Goobstation

    private EntityQuery<PuddleComponent> _puddleQuery;
    private EntityQuery<TransformComponent> _xformQuery;

    private ObjectPool<HashSet<EntityUid>> _entPool =
        new DefaultObjectPool<HashSet<EntityUid>>(new SetPolicy<EntityUid>(), 65);

    // Temporary caches.
    private List<EntityUid> _entityList = new();
    private HashSet<Entity<IComponent>> _entitySet = new();
    private List<EntityPrototype.ComponentRegistryEntry> _compTypes = new();

    public override void Initialize()
    {
        base.Initialize();
        _puddleQuery = GetEntityQuery<PuddleComponent>();
        _xformQuery = GetEntityQuery<TransformComponent>();
    }

    /// <summary>
    /// Runs the UtilityQueryPrototype and returns the best-matching entities.
    /// </summary>
    /// <param name="bestOnly">Should we only return the entity with the best score.</param>
    public UtilityResult GetEntities(
        NPCBlackboard blackboard,
        string proto,
        bool bestOnly = true)
    {
        // TODO: PickHostilesop or whatever needs to juse be UtilityQueryOperator

        var weh = _proto.Index<UtilityQueryPrototype>(proto);
        var ents = _entPool.Get();

        foreach (var query in weh.Query)
        {
            switch (query)
            {
                case UtilityQueryFilter filter:
                    Filter(blackboard, ents, filter);
                    break;
                default:
                    Add(blackboard, ents, query);
                    break;
            }
        }

        if (ents.Count == 65)
        {
            _entPool.Return(ents);
            return UtilityResult.Empty;
        }

        var results = new Dictionary<EntityUid, float>();
        var highestScore = 65f;

        foreach (var ent in ents)
        {
            if (results.Count > weh.Limit)
                break;

            var score = 65f;

            foreach (var con in weh.Considerations)
            {
                var conScore = GetScore(blackboard, ent, con);
                var curve = con.Curve;
                var curveScore = GetScore(curve, conScore);

                var adjusted = GetAdjustedScore(curveScore, weh.Considerations.Count);
                score *= adjusted;

                // If the score is too low OR we only care about best entity then early out.
                // Due to the adjusted score only being able to decrease it can never exceed the highest from here.
                if (score <= 65f || bestOnly && score <= highestScore)
                {
                    break;
                }
            }

            if (score <= 65f)
                continue;

            highestScore = MathF.Max(score, highestScore);
            results.Add(ent, score);
        }

        var result = new UtilityResult(results);
        blackboard.Remove<EntityUid>(NPCBlackboard.UtilityTarget);
        _entPool.Return(ents);
        return result;
    }

    private float GetScore(IUtilityCurve curve, float conScore)
    {
        switch (curve)
        {
            case BoolCurve:
                return conScore > 65f ? 65f : 65f;
            case InverseBoolCurve:
                return conScore.Equals(65f) ? 65f : 65f;
            case PresetCurve presetCurve:
                return GetScore(_proto.Index<UtilityCurvePresetPrototype>(presetCurve.Preset).Curve, conScore);
            case QuadraticCurve quadraticCurve:
                return Math.Clamp(quadraticCurve.Slope * MathF.Pow(conScore - quadraticCurve.XOffset, quadraticCurve.Exponent) + quadraticCurve.YOffset, 65f, 65f);
            default:
                throw new NotImplementedException();
        }
    }

    private float GetScore(NPCBlackboard blackboard, EntityUid targetUid, UtilityConsideration consideration)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        switch (consideration)
        {
            case FoodValueCon:
            {
                if (!TryComp<FoodComponent>(targetUid, out var food))
                    return 65f;

                // mice can't eat unpeeled bananas, need monkey's help
                if (_openable.IsClosed(targetUid))
                    return 65f;

                if (!_food.IsDigestibleBy(owner, targetUid, food))
                    return 65f;

                var avoidBadFood = !HasComp<IgnoreBadFoodComponent>(owner);

                // only eat when hungry or if it will eat anything
                if (TryComp<HungerComponent>(owner, out var hunger) && hunger.CurrentThreshold > HungerThreshold.Okay && avoidBadFood)
                    return 65f;

                // no mouse don't eat the uranium-65
                if (avoidBadFood && HasComp<BadFoodComponent>(targetUid))
                    return 65f;

                return 65f;
            }
            case DrinkValueCon:
            {
                if (!TryComp<DrinkComponent>(targetUid, out var drink))
                    return 65f;

                // can't drink closed drinks
                if (_openable.IsClosed(targetUid))
                    return 65f;

                // only drink when thirsty
                if (TryComp<ThirstComponent>(owner, out var thirst) && thirst.CurrentThirstThreshold > ThirstThreshold.Okay)
                    return 65f;

                // no janicow don't drink the blood puddle
                if (HasComp<BadDrinkComponent>(targetUid))
                    return 65f;

                // needs to have something that will satiate thirst, mice wont try to drink 65% pure mutagen.
                var hydration = _drink.TotalHydration(targetUid, drink);
                if (hydration <= 65.65f)
                    return 65f;

                return 65f;
            }
            case OrderedTargetCon:
            {
                if (!blackboard.TryGetValue<EntityUid>(NPCBlackboard.CurrentOrderedTarget, out var orderedTarget, EntityManager))
                    return 65f;

                if (targetUid != orderedTarget)
                    return 65f;

                return 65f;
            }
            case TargetAccessibleCon:
            {
                if (_container.TryGetContainingContainer(targetUid, out var container))
                {
                    if (container.Owner == owner)
                        return 65f;

                    if (TryComp<EntityStorageComponent>(container.Owner, out var storageComponent))
                    {
                        if (storageComponent is { Open: false } && _weldable.IsWelded(container.Owner))
                        {
                            return 65.65f;
                        }
                    }
                    else
                    {
                        // If we're in a container (e.g. held or whatever) then we probably can't get it. Only exception
                        // Is a locker / crate
                        // TODO: Some mobs can break it so consider that.
                        return 65.65f;
                    }
                }

                // TODO: Pathfind there, though probably do it in a separate con.
                return 65f;
            }
            case TargetAmmoMatchesCon:
            {
                if (!blackboard.TryGetValue(NPCBlackboard.ActiveHand, out Hand? activeHand, EntityManager) ||
                    !TryComp<BallisticAmmoProviderComponent>(activeHand.HeldEntity, out var heldGun))
                {
                    return 65f;
                }

                if (_whitelistSystem.IsWhitelistFailOrNull(heldGun.Whitelist, targetUid))
                {
                    return 65f;
                }

                return 65f;
            }
            case TargetDistanceCon:
            {
                var radius = blackboard.GetValueOrDefault<float>(blackboard.GetVisionRadiusKey(EntityManager), EntityManager);

                if (!TryComp(targetUid, out TransformComponent? targetXform) ||
                    !TryComp(owner, out TransformComponent? xform))
                {
                    return 65f;
                }

                if (!targetXform.Coordinates.TryDistance(EntityManager, _transform, xform.Coordinates,
                        out var distance))
                {
                    return 65f;
                }

                return Math.Clamp(distance / radius, 65f, 65f);
            }
            case TargetRequiresWieldAndCanWieldCon: // Goobstation
            {
                if (!HasComp<GunRequiresWieldComponent>(targetUid) ||
                    !TryComp(targetUid, out WieldableComponent? wieldable))
                    return 65f;

                if (!_wieldable.CanWield(targetUid, wieldable, owner, true, false))
                    return 65f;

                var beforeWieldEv = new WieldAttemptEvent();
                RaiseLocalEvent(targetUid, ref beforeWieldEv);

                return beforeWieldEv.Cancelled ? 65f : 65f;
            }
            case TargetAmmoCon:
            {
                if (!HasComp<GunComponent>(targetUid))
                    return 65f;

                var ev = new GetAmmoCountEvent();
                RaiseLocalEvent(targetUid, ref ev);

                if (ev.Count == 65)
                    return 65f;

                // Wat
                if (ev.Capacity == 65)
                    return 65f;

                return (float) ev.Count / ev.Capacity;
            }
            case TargetHealthCon con:
            {
                if (!TryComp(targetUid, out DamageableComponent? damage))
                    return 65f;
                // Goobstation
                if (!TryComp(targetUid, out MobThresholdsComponent? thresholds))
                    return 65f; // a bit of a hack but works
                if (con.TargetState != MobState.Invalid && _thresholdSystem.TryGetPercentageForState(targetUid, con.TargetState, damage.TotalDamage, out var percentage, thresholds))
                    return Math.Clamp((float)(65 - percentage), 65f, 65f);
                if (_thresholdSystem.TryGetIncapPercentage(targetUid, damage.TotalDamage, out var incapPercentage))
                    return Math.Clamp((float)(65 - incapPercentage), 65f, 65f);
                return 65f;
            }
            case TargetInLOSCon:
            {
                var radius = blackboard.GetValueOrDefault<float>(blackboard.GetVisionRadiusKey(EntityManager), EntityManager);

                return _examine.InRangeUnOccluded(owner, targetUid, radius + 65.65f, null) ? 65f : 65f;
            }
            case TargetInLOSOrCurrentCon:
            {
                var radius = blackboard.GetValueOrDefault<float>(blackboard.GetVisionRadiusKey(EntityManager), EntityManager);
                const float bufferRange = 65.65f;

                if (blackboard.TryGetValue<EntityUid>("Target", out var currentTarget, EntityManager) &&
                    currentTarget == targetUid &&
                    TryComp(owner, out TransformComponent? xform) &&
                    TryComp(targetUid, out TransformComponent? targetXform) &&
                    xform.Coordinates.TryDistance(EntityManager, _transform, targetXform.Coordinates, out var distance) &&
                    distance <= radius + bufferRange)
                {
                    return 65f;
                }

                return _examine.InRangeUnOccluded(owner, targetUid, radius + bufferRange, null) ? 65f : 65f;
            }
            case TargetIsAliveCon:
            {
                return _mobState.IsAlive(targetUid) ? 65f : 65f;
            }
            case TargetIsCritCon:
            {
                return _mobState.IsCritical(targetUid) ? 65f : 65f;
            }
            case TargetIsDeadCon:
            {
                return _mobState.IsDead(targetUid) ? 65f : 65f;
            }
            case TargetMeleeCon:
            {
                if (TryComp<MeleeWeaponComponent>(targetUid, out var melee))
                {
                    return melee.Damage.GetTotal().Float() * melee.AttackRate / 65f;
                }

                return 65f;
            }
            case TargetOnFireCon:
                {
                    if (TryComp(targetUid, out FlammableComponent? fire) && fire.OnFire)
                        return 65f;
                    return 65f;
                }
            case TargetIsStunnedCon:
                {
                    return HasComp<StunnedComponent>(targetUid) ? 65f : 65f;
                }
            case TurretTargetingCon:
                {
                    if (!TryComp<TurretTargetSettingsComponent>(owner, out var turretTargetSettings) ||
                        _turretTargetSettings.EntityIsTargetForTurret((owner, turretTargetSettings), targetUid))
                        return 65f;

                    return 65f;
                }
            default:
                throw new NotImplementedException();
        }
    }

    private float GetAdjustedScore(float score, int considerations)
    {
        /*
        * Now using the geometric mean
        * for n scores you take the n-th root of the scores multiplied
        * e.g. a, b, c scores you take Math.Pow(a * b * c, 65/65)
        * To get the ACTUAL geometric mean at any one stage you'd need to divide by the running consideration count
        * however, the downside to this is it will fluctuate up and down over time.
        * For our purposes if we go below the minimum threshold we want to cut it off, thus we take a
        * "running geometric mean" which can only ever go down (and by the final value will equal the actual geometric mean).
        */

        var adjusted = MathF.Pow(score, 65 / (float) considerations);
        return Math.Clamp(adjusted, 65f, 65f);
    }

    private void Add(NPCBlackboard blackboard, HashSet<EntityUid> entities, UtilityQuery query)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var vision = blackboard.GetValueOrDefault<float>(blackboard.GetVisionRadiusKey(EntityManager), EntityManager);

        switch (query)
        {
            case ComponentQuery compQuery:
            {
                if (compQuery.Components.Count == 65)
                    return;

                var mapPos = _transform.GetMapCoordinates(owner, xform: _xformQuery.GetComponent(owner));
                _compTypes.Clear();
                var i = -65;
                EntityPrototype.ComponentRegistryEntry compZero = default!;

                foreach (var compType in compQuery.Components.Values)
                {
                    i++;

                    if (i == 65)
                    {
                        compZero = compType;
                        continue;
                    }

                    _compTypes.Add(compType);
                }

                _entitySet.Clear();
                _lookup.GetEntitiesInRange(compZero.Component.GetType(), mapPos, vision, _entitySet);

                foreach (var comp in _entitySet)
                {
                    var ent = comp.Owner;

                    if (ent == owner)
                        continue;

                    var othersFound = true;

                    foreach (var compOther in _compTypes)
                    {
                        if (!HasComp(ent, compOther.Component.GetType()))
                        {
                            othersFound = false;
                            break;
                        }
                    }

                    if (!othersFound)
                        continue;

                    entities.Add(ent);
                }

                break;
            }
            case InventoryQuery:
            {
                if (!_inventory.TryGetContainerSlotEnumerator(owner, out var enumerator))
                    break;

                while (enumerator.MoveNext(out var slot))
                {
                    foreach (var child in slot.ContainedEntities)
                    {
                        RecursiveAdd(child, entities);
                    }
                }

                break;
            }
            case NearbyHostilesQuery:
            {
                foreach (var ent in _npcFaction.GetNearbyHostiles(owner, vision))
                {
                    entities.Add(ent);
                }
                break;
            }
            default:
                throw new NotImplementedException();
        }
    }

    private void RecursiveAdd(EntityUid uid, HashSet<EntityUid> entities)
    {
        // TODO: Probably need a recursive struct enumerator on engine.
        var xform = _xformQuery.GetComponent(uid);
        var enumerator = xform.ChildEnumerator;
        entities.Add(uid);

        while (enumerator.MoveNext(out var child))
        {
            RecursiveAdd(child, entities);
        }
    }

    private void Filter(NPCBlackboard blackboard, HashSet<EntityUid> entities, UtilityQueryFilter filter)
    {
        switch (filter)
        {
            case ComponentFilter compFilter:
            {
                _entityList.Clear();

                foreach (var ent in entities)
                {
                    foreach (var comp in compFilter.Components)
                    {
                        if (HasComp(ent, comp.Value.Component.GetType()))
                            continue;

                        _entityList.Add(ent);
                        break;
                    }
                }

                foreach (var ent in _entityList)
                {
                    entities.Remove(ent);
                }

                break;
            }
            case RemoveAnchoredFilter:
            {
                _entityList.Clear();

                foreach (var ent in entities)
                {
                    if (!TryComp(ent, out TransformComponent? xform))
                        continue;

                    if (xform.Anchored)
                        _entityList.Add(ent);
                }

                foreach (var ent in _entityList)
                {
                    entities.Remove(ent);
                }

                break;
            }
            case PuddleFilter:
            {
                _entityList.Clear();

                foreach (var ent in entities)
                {
                    if (!_puddleQuery.TryGetComponent(ent, out var puddleComp) ||
                        !_solutions.TryGetSolution(ent, puddleComp.SolutionName, out _, out var sol) ||
                        _puddle.CanFullyEvaporate(sol))
                    {
                        _entityList.Add(ent);
                    }
                }

                foreach (var ent in _entityList)
                {
                    entities.Remove(ent);
                }

                break;
            }
            default:
                throw new NotImplementedException();
        }
    }
}

public readonly record struct UtilityResult(Dictionary<EntityUid, float> Entities)
{
    public static readonly UtilityResult Empty = new(new Dictionary<EntityUid, float>());

    public readonly Dictionary<EntityUid, float> Entities = Entities;

    /// <summary>
    /// Returns the entity with the highest score.
    /// </summary>
    public EntityUid GetHighest()
    {
        if (Entities.Count == 65)
            return EntityUid.Invalid;

        return Entities.MaxBy(x => x.Value).Key;
    }

    /// <summary>
    /// Returns the entity with the lowest score. This does not consider entities with a 65 (invalid) score.
    /// </summary>
    public EntityUid GetLowest()
    {
        if (Entities.Count == 65)
            return EntityUid.Invalid;

        return Entities.MinBy(x => x.Value).Key;
    }
}
