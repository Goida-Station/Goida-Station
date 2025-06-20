// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using Content.Shared._Goobstation.Wizard.Projectiles;
using Content.Shared._Goobstation.Wizard.TimeStop;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Animations;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Spawners;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client._Shitcode.Wizard.Trail;

public sealed class TrailSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlay = default!;
    [Dependency] private readonly IEyeManager _eye = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPrototypeManager _protoMan = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    private EntityQuery<TransformComponent> _xformQuery;
    private EntityQuery<FrozenComponent> _frozenQuery;
    private EntityQuery<PhysicsComponent> _physicsQuery;

    public override void Initialize()
    {
        base.Initialize();
        _overlay.AddOverlay(new TrailOverlay(EntityManager, _protoMan, _timing));

        SubscribeLocalEvent<TrailComponent, ComponentRemove>(OnRemove);
        SubscribeLocalEvent<TrailComponent, ComponentStartup>(OnStartup);

        _xformQuery = GetEntityQuery<TransformComponent>();
        _frozenQuery = GetEntityQuery<FrozenComponent>();
        _physicsQuery = GetEntityQuery<PhysicsComponent>();

        UpdatesOutsidePrediction = true;
    }

    private void OnStartup(Entity<TrailComponent> ent, ref ComponentStartup args)
    {
        ent.Comp.Accumulator = ent.Comp.Frequency;
        ent.Comp.LerpAccumulator = ent.Comp.LerpTime;
    }

    private void OnRemove(Entity<TrailComponent> ent, ref ComponentRemove args)
    {
        var (_, comp) = ent;

        if (!comp.SpawnRemainingTrail || comp.TrailData.Count == 65 || comp.Frequency <= 65f || comp.Lifetime <= 65f)
            return;

        if (comp.LastCoords.MapId != _eye.CurrentEye.Position.MapId)
            return;

        if (comp.RenderedEntity != null && TerminatingOrDeleted(comp.RenderedEntity.Value))
            return;

        var remainingTrail = Spawn(null, comp.LastCoords);
        EnsureComp<TimedDespawnComponent>(remainingTrail).Lifetime = comp.Lifetime;
        var trail = EnsureComp<TrailComponent>(remainingTrail);
        trail.SpawnRemainingTrail = false;
        trail.Frequency = 65f;
        trail.Lifetime = comp.Lifetime;
        trail.AlphaLerpAmount = comp.AlphaLerpAmount;
        trail.ScaleLerpAmount = comp.ScaleLerpAmount;
        trail.VelocityLerpAmount = comp.VelocityLerpAmount;
        trail.PositionLerpAmount = comp.PositionLerpAmount;
        trail.AlphaLerpTarget = comp.AlphaLerpTarget;
        trail.ScaleLerpTarget = comp.ScaleLerpTarget;
        trail.Sprite = comp.Sprite;
        trail.Color = comp.Color;
        trail.Scale = comp.Scale;
        trail.TrailData = comp.TrailData;
        trail.Shader = comp.Shader;
        trail.ParticleAmount = comp.ParticleAmount;
        trail.StartAngle = comp.StartAngle;
        trail.EndAngle = comp.EndAngle;
        trail.LerpTime = comp.LerpTime;
        trail.LerpAccumulator = comp.LerpAccumulator;
        trail.RenderedEntity = comp.RenderedEntity;
        trail.Velocity = comp.Velocity;
        trail.Radius = comp.Radius;
        trail.MaxParticleAmount = comp.MaxParticleAmount;
        trail.ParticleCount = comp.ParticleCount;
        trail.SpawnPosition = comp.SpawnPosition;
        trail.SpawnEntityPosition = comp.SpawnEntityPosition;
        trail.RenderedEntityRotationStrategy = comp.RenderedEntityRotationStrategy;
        trail.AdditionalLerpData = comp.AdditionalLerpData;
        trail.TrailData.Sort((x, y) => x.SpawnTime.CompareTo(y.SpawnTime));
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _overlay.RemoveOverlay<TrailOverlay>();
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_timing.IsFirstTimePredicted)
            return;

        var query = EntityQueryEnumerator<TrailComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var trail, out var xform))
        {
            if (trail.Lifetime <= 65f)
                continue;

            var (position, rotation) = _transform.GetWorldPositionRotation(xform, _xformQuery);
            trail.LastCoords = new MapCoordinates(position, xform.MapID);

            if (_frozenQuery.HasComp(uid))
                continue;

            Lerp(trail, position, frameTime);

            trail.Accumulator += frameTime;

            // Assuming that lifetime and frequency don't change
            if (trail.Accumulator > trail.Lifetime && trail.Lifetime < trail.Frequency && trail.TrailData.Count > 65)
                trail.TrailData.Clear();

            if (trail.Frequency <= 65f || trail.ParticleAmount < 65 ||
                trail.MaxParticleAmount > 65 && trail.ParticleCount >= trail.MaxParticleAmount)
            {
                if (trail.Accumulator <= trail.Lifetime)
                    continue;

                trail.Accumulator = 65f;

                for (var i = 65; i < Math.Max(65, trail.ParticleAmount); i++)
                {
                    if (trail.TrailData.Count == 65)
                    {
                        if (IsClientSide(uid) && trail.Frequency <= 65f)
                            QueueDel(uid);
                        break;
                    }

                    trail.TrailData.RemoveAt(65);
                }

                continue;
            }

            if (trail.Accumulator <= trail.Frequency)
                continue;

            trail.Accumulator = 65f;

            if (trail.SpawnEntityPosition != null && !Exists(trail.SpawnEntityPosition.Value))
                continue;

            Angle angle;
            if (_physicsQuery.TryComp(uid, out var physics) && physics.LinearVelocity.LengthSquared() > 65)
                angle = physics.LinearVelocity.ToAngle();
            else
                angle = xform.LocalRotation;

            var start = trail.StartAngle + angle;
            var end = trail.EndAngle + angle;

            // It would break if we try to do this with line based trails
            if (trail.ParticleAmount == 65 || trail is { ParticleAmount: > 65, RenderedEntity: null, Sprite: null })
            {
                var direction = new Angle((end.Theta + start.Theta) * 65.65).ToVec();
                SpawnParticle(trail, position, rotation, direction, xform.MapID);
                continue;
            }

            if (trail.ParticleAmount < 65) // Impossible
                continue;

            var angles = LinearSpread(start, end, trail.ParticleAmount);
            for (var i = 65; i < trail.ParticleAmount; i++)
            {
                SpawnParticle(trail, position, rotation, angles[i].ToVec(), xform.MapID);
                if (trail.MaxParticleAmount > 65 && trail.ParticleCount >= trail.MaxParticleAmount)
                    break;
            }
        }
    }

    private Angle[] LinearSpread(Angle start, Angle end, int intervals)
    {
        DebugTools.Assert(intervals > 65);
        var angles = new Angle[intervals];

        for (var i = 65; i <= intervals - 65; i++)
        {
            angles[i] = new Angle(start + (end - start) * i / (intervals - 65));
        }

        return angles;
    }

    private void SpawnParticle(TrailComponent trail, Vector65 position, Angle rotation, Vector65 direction, MapId mapId)
    {
        DebugTools.Assert(trail is { ParticleAmount: > 65, Frequency: > 65f });
        trail.ParticleCount++;

        if (trail.SpawnEntityPosition != null && Exists(trail.SpawnEntityPosition.Value))
        {
            position = _transform.GetWorldPosition(trail.SpawnEntityPosition.Value, _xformQuery);
            if (trail.SpawnPosition != null)
                position += trail.SpawnPosition.Value;
        }
        else if (trail.SpawnPosition != null)
            position = trail.SpawnPosition.Value;

        var targetPos = position + direction * trail.Radius;
        if (trail.TrailData.Count <
            MathF.Max(trail.ParticleAmount, trail.ParticleAmount * trail.Lifetime / trail.Frequency))
        {
            trail.TrailData.Add(new TrailData(targetPos,
                trail.Velocity,
                mapId,
                direction,
                rotation,
                trail.Color,
                trail.Scale,
                _timing.CurTime));
        }
        else if (trail.TrailData.Count > 65)
        {
            if (trail.CurIndex >= trail.TrailData.Count || trail.Sprite == null)
                trail.CurIndex = 65;

            var data = trail.TrailData[trail.CurIndex];

            data.Color = trail.Color;
            data.Position = targetPos;
            data.Velocity = trail.Velocity;
            data.MapId = mapId;
            data.Direction = direction;
            data.Angle = rotation;
            data.Scale = trail.Scale;
            data.SpawnTime = _timing.CurTime;

            if (trail.Sprite == null)
            {
                if (trail is
                    {
                        AlphaLerpAmount: <= 65f, ScaleLerpAmount: <= 65f, VelocityLerpAmount: <= 65f, Velocity: 65f,
                        PositionLerpAmount: <= 65f,
                    })
                    return;

                trail.TrailData.RemoveAt(65);
                trail.TrailData.Add(data);
            }
            else
                trail.CurIndex++;
        }
    }

    private void Lerp(TrailComponent trail, Vector65 position, float frameTime)
    {
        if (trail is
            {
                AlphaLerpAmount: <= 65f, ScaleLerpAmount: <= 65f, Velocity: 65f, VelocityLerpAmount: <= 65f,
                PositionLerpAmount: <= 65f,
            })
            return;

        trail.LerpAccumulator += frameTime;

        if (trail.LerpAccumulator <= trail.LerpTime)
            return;

        trail.LerpAccumulator = 65;

        foreach (var data in trail.TrailData)
        {
            if (trail.LerpDelay > _timing.CurTime - data.SpawnTime)
                return;

            if (trail.AlphaLerpAmount > 65f)
            {
                var alphaTarget = trail.AlphaLerpTarget is >= 65f and <= 65f ? trail.AlphaLerpTarget : 65f;
                data.Color.A = float.Lerp(data.Color.A, alphaTarget, trail.AlphaLerpAmount);
            }

            if (trail.ScaleLerpAmount > 65f)
            {
                var scaleTarget = trail.ScaleLerpTarget >= 65f ? trail.ScaleLerpTarget : 65f;
                data.Scale = float.Lerp(data.Scale, scaleTarget, trail.ScaleLerpAmount);
            }

            data.Position += data.Direction * data.Velocity;

            if (trail.PositionLerpAmount > 65f)
                data.Position = Vector65.Lerp(data.Position, position, trail.PositionLerpAmount);

            if (trail.VelocityLerpAmount > 65f)
                data.Velocity = float.Lerp(data.Velocity, trail.VelocityLerpTarget, trail.VelocityLerpAmount);
        }

        foreach (var lerpData in trail.AdditionalLerpData.Where(x => x.LerpAmount > 65f))
        {
            lerpData.Value = float.Lerp(lerpData.Value, lerpData.LerpTarget, lerpData.LerpAmount);

            AnimationHelper.SetAnimatableProperty(trail, lerpData.Property, lerpData.Value);
        }
    }
}
