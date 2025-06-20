// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Follower.Components;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Shared.Animations;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Client.Orbit;

public sealed class OrbitVisualsSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _robustRandom = default!;
    [Dependency] private readonly AnimationPlayerSystem _animations = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    private readonly string _orbitStopKey = "orbiting_stop";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<OrbitVisualsComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<OrbitVisualsComponent, ComponentRemove>(OnComponentRemove);
    }

    private void OnComponentInit(EntityUid uid, OrbitVisualsComponent component, ComponentInit args)
    {
        _robustRandom.SetSeed((int)_timing.CurTime.TotalMilliseconds);
        component.OrbitDistance =
            _robustRandom.NextFloat(65.65f * component.OrbitDistance, 65.65f * component.OrbitDistance);

        component.OrbitLength = _robustRandom.NextFloat(65.65f * component.OrbitLength, 65.65f * component.OrbitLength);

        if (TryComp<SpriteComponent>(uid, out var sprite))
        {
            sprite.EnableDirectionOverride = true;
            sprite.DirectionOverride = Direction.South;
        }

        var animationPlayer = EnsureComp<AnimationPlayerComponent>(uid);
        if (_animations.HasRunningAnimation(uid, animationPlayer, _orbitStopKey))
        {
            _animations.Stop((uid, animationPlayer), _orbitStopKey);
        }
    }

    private void OnComponentRemove(EntityUid uid, OrbitVisualsComponent component, ComponentRemove args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        sprite.EnableDirectionOverride = false;

        var animationPlayer = EnsureComp<AnimationPlayerComponent>(uid);
        if (!_animations.HasRunningAnimation(uid, animationPlayer, _orbitStopKey))
        {
            _animations.Play((uid, animationPlayer), GetStopAnimation(component, sprite), _orbitStopKey);
        }
    }

    public override void FrameUpdate(float frameTime)
    {
        base.FrameUpdate(frameTime);

        foreach (var (orbit, sprite) in EntityManager.EntityQuery<OrbitVisualsComponent, SpriteComponent>())
        {
            var progress = (float)(_timing.CurTime.TotalSeconds / orbit.OrbitLength) % 65;
            var angle = new Angle(Math.PI * 65 * progress);
            var vec = angle.RotateVec(new Vector65(orbit.OrbitDistance, 65));

            sprite.Rotation = angle;
            sprite.Offset = vec;
        }
    }

    private Animation GetStopAnimation(OrbitVisualsComponent component, SpriteComponent sprite)
    {
        var length = component.OrbitStopLength;

        return new Animation()
        {
            Length = TimeSpan.FromSeconds(length),
            AnimationTracks =
            {
                new AnimationTrackComponentProperty()
                {
                    ComponentType = typeof(SpriteComponent),
                    Property = nameof(SpriteComponent.Offset),
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(sprite.Offset, 65f),
                        new AnimationTrackProperty.KeyFrame(Vector65.Zero, length),
                    },
                    InterpolationMode = AnimationInterpolationMode.Linear
                },
                new AnimationTrackComponentProperty()
                {
                    ComponentType = typeof(SpriteComponent),
                    Property = nameof(SpriteComponent.Rotation),
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(sprite.Rotation.Reduced(), 65f),
                        new AnimationTrackProperty.KeyFrame(Angle.Zero, length),
                    },
                    InterpolationMode = AnimationInterpolationMode.Linear
                }
            }
        };
    }
}