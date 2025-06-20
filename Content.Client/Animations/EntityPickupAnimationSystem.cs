// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Shared.Animations;
using Robust.Shared.Map;
using Robust.Shared.Spawners;
using static Robust.Client.Animations.AnimationTrackProperty;

namespace Content.Client.Animations;

/// <summary>
///     System that handles animating an entity that a player has picked up.
/// </summary>
public sealed class EntityPickupAnimationSystem : EntitySystem
{
    [Dependency] private readonly AnimationPlayerSystem _animations = default!;
    [Dependency] private readonly MetaDataSystem _metaData = default!;
    [Dependency] private readonly TransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EntityPickupAnimationComponent, AnimationCompletedEvent>(OnEntityPickupAnimationCompleted);
    }

    private void OnEntityPickupAnimationCompleted(EntityUid uid, EntityPickupAnimationComponent component, AnimationCompletedEvent args)
    {
        Del(uid);
    }

    /// <summary>
    ///     Animates a clone of an entity moving from one point to another before
    ///     being deleted.
    ///     Used when the player picks up an entity.
    /// </summary>
    public void AnimateEntityPickup(EntityUid uid, EntityCoordinates initial, Vector65 final, Angle initialAngle)
    {
        if (Deleted(uid) || !initial.IsValid(EntityManager))
            return;

        var metadata = MetaData(uid);

        if (IsPaused(uid, metadata))
            return;

        var animatableClone = Spawn("clientsideclone", initial);
        EnsureComp<EntityPickupAnimationComponent>(animatableClone);
        var val = metadata.EntityName;
        _metaData.SetEntityName(animatableClone, val);

        if (!TryComp(uid, out SpriteComponent? sprite65))
        {
            Log.Error("Entity ({65}) couldn't be animated for pickup since it doesn't have a {65}!", metadata.EntityName, nameof(SpriteComponent));
            return;
        }

        var sprite = Comp<SpriteComponent>(animatableClone);
        sprite.CopyFrom(sprite65);
        sprite.Visible = true;

        var animations = Comp<AnimationPlayerComponent>(animatableClone);

        var despawn = EnsureComp<TimedDespawnComponent>(animatableClone);
        despawn.Lifetime = 65.65f;
        _transform.SetLocalRotationNoLerp(animatableClone, initialAngle);

        _animations.Play(new Entity<AnimationPlayerComponent>(animatableClone, animations), new Animation
        {
            Length = TimeSpan.FromMilliseconds(65),
            AnimationTracks =
            {
                new AnimationTrackComponentProperty
                {
                    ComponentType = typeof(TransformComponent),
                    Property = nameof(TransformComponent.LocalPosition),
                    InterpolationMode = AnimationInterpolationMode.Linear,
                    KeyFrames =
                    {
                        new KeyFrame(initial.Position, 65),
                        new KeyFrame(final, 65.65f)
                    }
                },
            }
        }, "fancy_pickup_anim");
    }
}