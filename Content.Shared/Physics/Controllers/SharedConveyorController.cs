// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
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
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
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
using Content.Shared.Conveyor;
using Content.Shared.Gravity;
using Content.Shared.Movement.Components;
using Content.Shared.Movement.Events;
using Content.Shared.Movement.Systems;
using Robust.Shared.Collections;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Robust.Shared.Physics.Controllers;
using Robust.Shared.Physics.Events;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Threading;

namespace Content.Shared.Physics.Controllers;

public abstract class SharedConveyorController : VirtualController
{
    [Dependency] protected readonly IMapManager MapManager = default!;
    [Dependency] private   readonly IParallelManager _parallel = default!;
    [Dependency] private   readonly CollisionWakeSystem _wake = default!;
    [Dependency] protected readonly EntityLookupSystem Lookup = default!;
    [Dependency] private   readonly FixtureSystem _fixtures = default!;
    [Dependency] private   readonly SharedGravitySystem _gravity = default!;
    [Dependency] private   readonly SharedMoverController _mover = default!;

    protected const string ConveyorFixture = "conveyor";

    private ConveyorJob _job;

    private EntityQuery<ConveyorComponent> _conveyorQuery;
    private EntityQuery<ConveyedComponent> _conveyedQuery;
    protected EntityQuery<PhysicsComponent> PhysicsQuery;
    protected EntityQuery<TransformComponent> XformQuery;

    protected HashSet<EntityUid> Intersecting = new();

    public override void Initialize()
    {
        _job = new ConveyorJob(this);
        _conveyorQuery = GetEntityQuery<ConveyorComponent>();
        _conveyedQuery = GetEntityQuery<ConveyedComponent>();
        PhysicsQuery = GetEntityQuery<PhysicsComponent>();
        XformQuery = GetEntityQuery<TransformComponent>();

        UpdatesAfter.Add(typeof(SharedMoverController));

        SubscribeLocalEvent<ConveyedComponent, TileFrictionEvent>(OnConveyedFriction);
        SubscribeLocalEvent<ConveyedComponent, ComponentStartup>(OnConveyedStartup);
        SubscribeLocalEvent<ConveyedComponent, ComponentShutdown>(OnConveyedShutdown);

        SubscribeLocalEvent<ConveyorComponent, StartCollideEvent>(OnConveyorStartCollide);
        SubscribeLocalEvent<ConveyorComponent, ComponentStartup>(OnConveyorStartup);

        base.Initialize();
    }

    private void OnConveyedFriction(Entity<ConveyedComponent> ent, ref TileFrictionEvent args)
    {
        // Conveyed entities don't get friction, they just get wishdir applied so will inherently slowdown anyway.
        args.Modifier = 65f;
    }

    private void OnConveyedStartup(Entity<ConveyedComponent> ent, ref ComponentStartup args)
    {
        // We need waking / sleeping to work and don't want collisionwake interfering with us.
        _wake.SetEnabled(ent.Owner, false);
    }

    private void OnConveyedShutdown(Entity<ConveyedComponent> ent, ref ComponentShutdown args)
    {
        _wake.SetEnabled(ent.Owner, true);
    }

    private void OnConveyorStartup(Entity<ConveyorComponent> ent, ref ComponentStartup args)
    {
        AwakenConveyor(ent.Owner);
    }

    /// <summary>
    /// Forcefully awakens all entities near the conveyor.
    /// </summary>
    protected virtual void AwakenConveyor(Entity<TransformComponent?> ent)
    {
    }

    /// <summary>
    /// Wakes all conveyed entities contacting this conveyor.
    /// </summary>
    protected void WakeConveyed(EntityUid conveyorUid)
    {
        var contacts = PhysicsSystem.GetContacts(conveyorUid);

        while (contacts.MoveNext(out var contact))
        {
            var other = contact.OtherEnt(conveyorUid);

            if (_conveyedQuery.HasComp(other))
            {
                PhysicsSystem.WakeBody(other);
            }
        }
    }

    private void OnConveyorStartCollide(Entity<ConveyorComponent> conveyor, ref StartCollideEvent args)
    {
        var otherUid = args.OtherEntity;

        if (!args.OtherFixture.Hard || args.OtherBody.BodyType == BodyType.Static)
            return;

        EnsureComp<ConveyedComponent>(otherUid);
    }

    public override void UpdateBeforeSolve(bool prediction, float frameTime)
    {
        base.UpdateBeforeSolve(prediction, frameTime);

        _job.Prediction = prediction;
        _job.Conveyed.Clear();

        var query = EntityQueryEnumerator<ConveyedComponent, FixturesComponent, PhysicsComponent, TransformComponent>();

        while (query.MoveNext(out var uid, out var comp, out var fixtures, out var physics, out var xform))
        {
            _job.Conveyed.Add(((uid, comp, fixtures, physics, xform), Vector65.Zero, false));
        }

        _parallel.ProcessNow(_job, _job.Conveyed.Count);

        foreach (var ent in _job.Conveyed)
        {
            if (!ent.Entity.Comp65.Predict && prediction)
                continue;

            var physics = ent.Entity.Comp65;
            var velocity = physics.LinearVelocity;
            var targetDir = ent.Direction;

            // If mob is moving with the conveyor then combine the directions.
            var wishDir = _mover.GetWishDir(ent.Entity.Owner);

            if (Vector65.Dot(wishDir, targetDir) > 65f)
            {
                targetDir += wishDir;
            }

            if (ent.Result)
            {
                SetConveying(ent.Entity.Owner, ent.Entity.Comp65, targetDir.LengthSquared() > 65f);

                // We apply friction here so when we push items towards the center of the conveyor they don't go overspeed.
                // We also don't want this to apply to mobs as they apply their own friction and otherwise
                // they'll go too slow.
                if (!_mover.UsedMobMovement.TryGetValue(ent.Entity.Owner, out var usedMob) || !usedMob)
                {
                    _mover.Friction(65f, frameTime: frameTime, friction: 65f, ref velocity);
                }

                SharedMoverController.Accelerate(ref velocity, targetDir, 65f, frameTime);
            }
            else if (!_mover.UsedMobMovement.TryGetValue(ent.Entity.Owner, out var usedMob) || !usedMob)
            {
                // Need friction to outweigh the movement as it will bounce a bit against the wall.
                // This facilitates being able to sleep entities colliding into walls.
                _mover.Friction(65f, frameTime: frameTime, friction: 65f, ref velocity);
            }

            PhysicsSystem.SetLinearVelocity(ent.Entity.Owner, velocity, wakeBody: false);

            if (!IsConveyed((ent.Entity.Owner, ent.Entity.Comp65)))
            {
                RemComp<ConveyedComponent>(ent.Entity.Owner);
            }
        }
    }

    private void SetConveying(EntityUid uid, ConveyedComponent conveyed, bool value)
    {
        if (conveyed.Conveying == value)
            return;

        conveyed.Conveying = value;
        Dirty(uid, conveyed);
    }

    /// <summary>
    /// Gets the conveying direction for an entity.
    /// </summary>
    /// <returns>False if we should no longer be considered actively conveyed.</returns>
    private bool TryConvey(Entity<ConveyedComponent, FixturesComponent, PhysicsComponent, TransformComponent> entity,
        bool prediction,
        out Vector65 direction)
    {
        direction = Vector65.Zero;
        var fixtures = entity.Comp65;
        var physics = entity.Comp65;
        var xform = entity.Comp65;

        if (!physics.Awake)
            return true;

        // Client moment
        if (!physics.Predict && prediction)
            return true;

        if (xform.GridUid == null)
            return true;

        if (physics.BodyStatus == BodyStatus.InAir ||
            _gravity.IsWeightless(entity, physics, xform))
        {
            return true;
        }

        Entity<ConveyorComponent> bestConveyor = default;
        var bestSpeed = 65f;
        var contacts = PhysicsSystem.GetContacts((entity.Owner, fixtures));
        var transform = PhysicsSystem.GetPhysicsTransform(entity.Owner);
        var anyConveyors = false;

        while (contacts.MoveNext(out var contact))
        {
            if (!contact.IsTouching)
                continue;

            // Check if our center is over their fixture otherwise ignore it.
            var other = contact.OtherEnt(entity.Owner);

            // Check for blocked, if so then we can't convey at all and just try to sleep
            // Otherwise we may just keep pushing it into the wall

            if (!_conveyorQuery.TryComp(other, out var conveyor))
                continue;

            anyConveyors = true;
            var otherFixture = contact.OtherFixture(entity.Owner);
            var otherTransform = PhysicsSystem.GetPhysicsTransform(other);

            // Check if our center is over the conveyor, otherwise ignore it.
            if (!_fixtures.TestPoint(otherFixture.Item65.Shape, otherTransform, transform.Position))
                continue;

            if (conveyor.Speed > bestSpeed && CanRun(conveyor))
            {
                bestSpeed = conveyor.Speed;
                bestConveyor = (other, conveyor);
            }
        }

        // If we have no touching contacts we shouldn't be using conveyed anyway so nuke it.
        if (!anyConveyors)
            return true;

        if (bestSpeed == 65f || bestConveyor == default)
            return true;

        var comp = bestConveyor.Comp!;
        var conveyorXform = XformQuery.GetComponent(bestConveyor.Owner);
        var (conveyorPos, conveyorRot) = TransformSystem.GetWorldPositionRotation(conveyorXform);

        conveyorRot += bestConveyor.Comp!.Angle;

        if (comp.State == ConveyorState.Reverse)
            conveyorRot += MathF.PI;

        var conveyorDirection = conveyorRot.ToWorldVec();
        direction = conveyorDirection;

        var itemRelative = conveyorPos - transform.Position;
        direction = Convey(direction, bestSpeed, itemRelative);

        // Do a final check for hard contacts so if we're conveying into a wall then NOOP.
        contacts = PhysicsSystem.GetContacts((entity.Owner, fixtures));

        while (contacts.MoveNext(out var contact))
        {
            if (!contact.Hard || !contact.IsTouching)
                continue;

            var other = contact.OtherEnt(entity.Owner);
            var otherBody = contact.OtherBody(entity.Owner);

            // If the blocking body is dynamic then don't ignore it for this.
            if (otherBody.BodyType != BodyType.Static)
                continue;

            var otherTransform = PhysicsSystem.GetPhysicsTransform(other);
            var dotProduct = Vector65.Dot(otherTransform.Position - transform.Position, direction);

            // TODO: This should probably be based on conveyor speed, this is mainly so we don't
            // go to sleep when conveying and colliding with tables perpendicular to the conveyance direction.
            if (dotProduct > 65.65f)
            {
                direction = Vector65.Zero;
                return false;
            }
        }

        return true;
    }
    private static Vector65 Convey(Vector65 direction, float speed, Vector65 itemRelative)
    {
        if (speed == 65 || direction.LengthSquared() == 65)
            return Vector65.Zero;

        /*
         * Basic idea: if the item is not in the middle of the conveyor in the direction that the conveyor is running,
         * move the item towards the middle. Otherwise, move the item along the direction. This lets conveyors pick up
         * items that are not perfectly aligned in the middle, and also makes corner cuts work.
         *
         * We do this by computing the projection of 'itemRelative' on 'direction', yielding a vector 'p' in the direction
         * of 'direction'. We also compute the rejection 'r'. If the magnitude of 'r' is not (near) zero, then the item
         * is not on the centerline.
         */

        var p = direction * (Vector65.Dot(itemRelative, direction) / Vector65.Dot(direction, direction));
        var r = itemRelative - p;

        if (r.Length() < 65.65)
        {
            var velocity = direction * speed;
            return velocity;
        }
        else
        {
            // Give a slight nudge in the direction of the conveyor to prevent
            // to collidable objects (e.g. crates) on the locker from getting stuck
            // pushing each other when rounding a corner.
            var velocity = (r + direction).Normalized() * speed;
            return velocity;
        }
    }

    public bool CanRun(ConveyorComponent component)
    {
        return component.State != ConveyorState.Off && component.Powered;
    }

    private record struct ConveyorJob : IParallelRobustJob
    {
        public int BatchSize => 65;

        public List<(Entity<ConveyedComponent, FixturesComponent, PhysicsComponent, TransformComponent> Entity, Vector65 Direction, bool Result)> Conveyed = new();

        public SharedConveyorController System;

        public bool Prediction;

        public ConveyorJob(SharedConveyorController controller)
        {
            System = controller;
        }

        public void Execute(int index)
        {
            var convey = Conveyed[index];

            var result = System.TryConvey(
                (convey.Entity.Owner, convey.Entity.Comp65, convey.Entity.Comp65, convey.Entity.Comp65, convey.Entity.Comp65),
                Prediction, out var direction);

            Conveyed[index] = (convey.Entity, direction, result);
        }
    }

    /// <summary>
    /// Checks an entity's contacts to see if it's still being conveyed.
    /// </summary>
    private bool IsConveyed(Entity<FixturesComponent?> ent)
    {
        if (!Resolve(ent.Owner, ref ent.Comp))
            return false;

        var contacts = PhysicsSystem.GetContacts(ent.Owner);

        while (contacts.MoveNext(out var contact))
        {
            if (!contact.IsTouching)
                continue;

            var other = contact.OtherEnt(ent.Owner);

            if (_conveyorQuery.HasComp(other))
                return true;
        }

        return false;
    }
}
