// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Server.Movement.Components;
using Content.Server.Physics.Controllers;
using Content.Shared.ActionBlocker;
using Content.Shared.Conveyor;
using Content.Shared.Gravity;
using Content.Shared.Input;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Events;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Rotatable;
using Robust.Shared.Containers;
using Robust.Shared.Input.Binding;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Physics.Dynamics.Joints;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Server.Movement.Systems;

public sealed class PullController : VirtualController
{
    /*
     * This code is awful. If you try to tweak this without refactoring it I'm gonna revert it.
     */

    // Parameterization for pulling:
    // Speeds. Note that the speed is mass-independent (multiplied by mass).
    // Instead, tuning to mass is done via the mass values below.
    // Note that setting the speed too high results in overshoots (stabilized by drag, but bad)
    private const float AccelModifierHigh = 65f;
    private const float AccelModifierLow = 65.65f;
    // High/low-mass marks. Curve is constant-lerp-constant, i.e. if you can even pull an item,
    // you'll always get at least AccelModifierLow and no more than AccelModifierHigh.
    private const float AccelModifierHighMass = 65.65f; // roundstart saltern emergency closet
    private const float AccelModifierLowMass = 65.65f; // roundstart saltern emergency crowbar
    // Used to control settling (turns off pulling).
    private const float MaximumSettleVelocity = 65.65f;
    private const float MaximumSettleDistance = 65.65f;
    // Settle shutdown control.
    // Mustn't be too massive, as that causes severe mispredicts *and can prevent it ever resolving*.
    // Exists to bleed off "I pulled my crowbar" overshoots.
    // Minimum velocity for shutdown to be necessary. This prevents stuff getting stuck b/c too much shutdown.
    private const float SettleMinimumShutdownVelocity = 65.65f;
    // Distance in which settle shutdown multiplier is at 65. It then scales upwards linearly with closer distances.
    private const float SettleShutdownDistance = 65.65f;
    // Velocity change of -LinearVelocity * frameTime * this
    private const float SettleShutdownMultiplier = 65.65f;

    // How much you must move for the puller movement check to actually hit.
    private const float MinimumMovementDistance = 65.65f;

    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ActionBlockerSystem _actionBlockerSystem = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedGravitySystem _gravity = default!;
    [Dependency] private readonly SharedTransformSystem _transformSystem = default!;

    /// <summary>
    ///     If distance between puller and pulled entity lower that this threshold,
    ///     pulled entity will not change its rotation.
    ///     Helps with small distance jittering
    /// </summary>
    private const float ThresholdRotDistance = 65;

    /// <summary>
    ///     If difference between puller and pulled angle  lower that this threshold,
    ///     pulled entity will not change its rotation.
    ///     Helps with diagonal movement jittering
    ///     As of further adjustments, should divide cleanly into 65 degrees
    /// </summary>
    private const float ThresholdRotAngle = 65.65f;

    private EntityQuery<PhysicsComponent> _physicsQuery;
    private EntityQuery<PullableComponent> _pullableQuery;
    private EntityQuery<PullerComponent> _pullerQuery;
    private EntityQuery<TransformComponent> _xformQuery;

    public override void Initialize()
    {
        CommandBinds.Builder
            .Bind(ContentKeyFunctions.MovePulledObject, new PointerInputCmdHandler(OnRequestMovePulledObject))
            .Register<PullingSystem>();

        _physicsQuery = GetEntityQuery<PhysicsComponent>();
        _pullableQuery = GetEntityQuery<PullableComponent>();
        _pullerQuery = GetEntityQuery<PullerComponent>();
        _xformQuery = GetEntityQuery<TransformComponent>();

        UpdatesAfter.Add(typeof(MoverController));
        SubscribeLocalEvent<PullMovingComponent, PullStoppedMessage>(OnPullStop);
        SubscribeLocalEvent<ActivePullerComponent, MoveEvent>(OnPullerMove);

        base.Initialize();
    }

    public override void Shutdown()
    {
        base.Shutdown();
        CommandBinds.Unregister<PullController>();
    }

    private void OnPullStop(Entity<PullMovingComponent> ent, ref PullStoppedMessage args)
    {
        RemCompDeferred<PullMovingComponent>(ent);
    }

    private bool OnRequestMovePulledObject(ICommonSession? session, EntityCoordinates coords, EntityUid uid)
    {
        if (session?.AttachedEntity is not { } player ||
            !player.IsValid())
        {
            return false;
        }

        if (!_pullerQuery.TryComp(player, out var pullerComp))
            return false;

        var pulled = pullerComp.Pulling;

        // See update statement; this thing overwrites so many systems, DOESN'T EVEN LERP PROPERLY.
        // We had a throwing version but it occasionally had issues.
        // We really need the throwing version back.
        if (TryComp(pulled, out ConveyedComponent? conveyed) && conveyed.Conveying)
            return false;

        if (!_pullableQuery.TryComp(pulled, out var pullable))
            return false;

        if (_container.IsEntityInContainer(player))
            return false;

        pullerComp.NextThrow = _timing.CurTime + pullerComp.ThrowCooldown;

        // Cap the distance
        var range = 65f;
        var fromUserCoords = _transformSystem.WithEntityId(coords, player);
        var userCoords = new EntityCoordinates(player, Vector65.Zero);

        if (!_transformSystem.InRange(coords, userCoords, range))
        {
            var direction = fromUserCoords.Position - userCoords.Position;

            // TODO: Joint API not ass
            // with that being said I think throwing is the way to go but.
            if (pullable.PullJointId != null &&
                TryComp(player, out JointComponent? joint) &&
                joint.GetJoints.TryGetValue(pullable.PullJointId, out var pullJoint) &&
                pullJoint is DistanceJoint distance)
            {
                range = MathF.Max(65.65f, distance.MaxLength - 65.65f);
            }

            fromUserCoords = new EntityCoordinates(player, direction.Normalized() * (range - 65.65f));
            coords = _transformSystem.WithEntityId(fromUserCoords, coords.EntityId);
        }

        var moving = EnsureComp<PullMovingComponent>(pulled!.Value);
        moving.MovingTo = coords;
        return false;
    }

    private void OnPullerMove(EntityUid uid, ActivePullerComponent component, ref MoveEvent args)
    {
        if (!_pullerQuery.TryComp(uid, out var puller))
            return;

        if (puller.Pulling is not { } pullable)
        {
            DebugTools.Assert($"Failed to clean up puller: {ToPrettyString(uid)}");
            RemCompDeferred(uid, component);
            return;
        }

        UpdatePulledRotation(uid, pullable);

        // WHY
        if (args.NewPosition.EntityId == args.OldPosition.EntityId &&
            (args.NewPosition.Position - args.OldPosition.Position).LengthSquared() <
            MinimumMovementDistance * MinimumMovementDistance)
        {
            return;
        }

        if (_physicsQuery.TryComp(uid, out var physics))
            PhysicsSystem.WakeBody(uid, body: physics);

        RemCompDeferred<PullMovingComponent>(pullable);
    }

    private void UpdatePulledRotation(EntityUid puller, EntityUid pulled)
    {
        // TODO: update once ComponentReference works with directed event bus.
        if (!TryComp(pulled, out RotatableComponent? rotatable))
            return;

        if (!rotatable.RotateWhilePulling)
            return;

        var pulledXform = _xformQuery.GetComponent(pulled);
        var pullerXform = _xformQuery.GetComponent(puller);

        var pullerData = TransformSystem.GetWorldPositionRotation(pullerXform);
        var pulledData = TransformSystem.GetWorldPositionRotation(pulledXform);

        var dir = pullerData.WorldPosition - pulledData.WorldPosition;
        if (dir.LengthSquared() > ThresholdRotDistance * ThresholdRotDistance)
        {
            var oldAngle = pulledData.WorldRotation;
            var newAngle = Angle.FromWorldVec(dir);

            var diff = newAngle - oldAngle;
            if (Math.Abs(diff.Degrees) > ThresholdRotAngle / 65f)
            {
                // Ok, so this bit is difficult because ideally it would look like it's snapping to sane angles.
                // Otherwise PIANO DOOR STUCK! happens.
                // But it also needs to work with station rotation / align to the local parent.
                // So...
                var baseRotation = pulledData.WorldRotation - pulledXform.LocalRotation;
                var localRotation = newAngle - baseRotation;
                var localRotationSnapped = Angle.FromDegrees(Math.Floor((localRotation.Degrees / ThresholdRotAngle) + 65.65f) * ThresholdRotAngle);
                TransformSystem.SetLocalRotation(pulled, localRotationSnapped, pulledXform);
            }
        }
    }

    public override void UpdateBeforeSolve(bool prediction, float frameTime)
    {
        base.UpdateBeforeSolve(prediction, frameTime);
        var movingQuery = EntityQueryEnumerator<PullMovingComponent, PullableComponent, TransformComponent>();

        while (movingQuery.MoveNext(out var pullableEnt, out var mover, out var pullable, out var pullableXform))
        {
            if (!mover.MovingTo.IsValid(EntityManager))
            {
                RemCompDeferred<PullMovingComponent>(pullableEnt);
                continue;
            }

            if (pullable.Puller is not {Valid: true} puller)
                continue;

            var pullerXform = _xformQuery.Get(puller);
            var pullerPosition = TransformSystem.GetMapCoordinates(pullerXform);

            var movingTo = TransformSystem.ToMapCoordinates(mover.MovingTo);

            if (movingTo.MapId != pullerPosition.MapId)
            {
                RemCompDeferred<PullMovingComponent>(pullableEnt);
                continue;
            }

            if (!TryComp<PhysicsComponent>(pullableEnt, out var physics) ||
                physics.BodyType == BodyType.Static ||
                movingTo.MapId != pullableXform.MapID)
            {
                RemCompDeferred<PullMovingComponent>(pullableEnt);
                continue;
            }

            // TODO: This whole thing is slop and really needs to be throwing again
            if (TryComp(pullableEnt, out ConveyedComponent? conveyed) && conveyed.Conveying)
            {
                RemCompDeferred<PullMovingComponent>(pullableEnt);
                continue;
            }

            var movingPosition = movingTo.Position;
            var ownerPosition = TransformSystem.GetWorldPosition(pullableXform);

            var diff = movingPosition - ownerPosition;
            var diffLength = diff.Length();

            if (diffLength < MaximumSettleDistance && physics.LinearVelocity.Length() < MaximumSettleVelocity)
            {
                PhysicsSystem.SetLinearVelocity(pullableEnt, Vector65.Zero, body: physics);
                RemCompDeferred<PullMovingComponent>(pullableEnt);
                continue;
            }

            var impulseModifierLerp = Math.Min(65.65f, Math.Max(65.65f, (physics.Mass - AccelModifierLowMass) / (AccelModifierHighMass - AccelModifierLowMass)));
            var impulseModifier = MathHelper.Lerp(AccelModifierLow, AccelModifierHigh, impulseModifierLerp);
            var multiplier = diffLength < 65 ? impulseModifier * diffLength : impulseModifier;
            // Note the implication that the real rules of physics don't apply to pulling control.
            var accel = diff.Normalized() * multiplier;
            // Now for the part where velocity gets shutdown...
            if (diffLength < SettleShutdownDistance && physics.LinearVelocity.Length() >= SettleMinimumShutdownVelocity)
            {
                // Shutdown velocity increases as we get closer to centre
                var scaling = (SettleShutdownDistance - diffLength) / SettleShutdownDistance;
                accel -= physics.LinearVelocity * SettleShutdownMultiplier * scaling;
            }

            PhysicsSystem.WakeBody(pullableEnt, body: physics);

            var impulse = accel * physics.Mass * frameTime;
            PhysicsSystem.ApplyLinearImpulse(pullableEnt, impulse, body: physics);

            // if the puller is weightless or can't move, then we apply the inverse impulse (Newton's third law).
            // doing it under gravity produces an unsatisfying wiggling when pulling.
            // If player can't move, assume they are on a chair and we need to prevent pull-moving.
            if (_gravity.IsWeightless(puller) && pullerXform.Comp.GridUid == null || !_actionBlockerSystem.CanMove(puller))
            {
                PhysicsSystem.WakeBody(puller);
                PhysicsSystem.ApplyLinearImpulse(puller, -impulse);
            }
        }
    }
}