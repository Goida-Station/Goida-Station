// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Throwing;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Shared.Animations;

namespace Content.Client.Throwing;

/// <summary>
///     Handles animating thrown items.
/// </summary>
public sealed class ThrownItemVisualizerSystem : EntitySystem
{
    [Dependency] private readonly AnimationPlayerSystem _anim = default!;

    private const string AnimationKey = "thrown-item";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ThrownItemComponent, AfterAutoHandleStateEvent>(OnAutoHandleState);
        SubscribeLocalEvent<ThrownItemComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnAutoHandleState(EntityUid uid, ThrownItemComponent component, ref AfterAutoHandleStateEvent args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite)) // Goob edit start
            return;

        if (!component.Animate)
        {
            if (!_anim.HasRunningAnimation(uid, AnimationKey))
                return;

            _anim.Stop(uid, AnimationKey);

            if (component.OriginalScale != null)
                sprite.Scale = component.OriginalScale.Value;

            return;
        }
        // Goob edit end

        var animationPlayer = EnsureComp<AnimationPlayerComponent>(uid);

        if (_anim.HasRunningAnimation(uid, animationPlayer, AnimationKey))
            return;

        var anim = GetAnimation((uid, component, sprite));
        if (anim == null)
            return;

        component.OriginalScale = sprite.Scale;
        _anim.Play((uid, animationPlayer), anim, AnimationKey);
    }

    private void OnShutdown(EntityUid uid, ThrownItemComponent component, ComponentShutdown args)
    {
        if (!_anim.HasRunningAnimation(uid, AnimationKey))
            return;

        if (TryComp<SpriteComponent>(uid, out var sprite) && component.OriginalScale != null)
            sprite.Scale = component.OriginalScale.Value;

        _anim.Stop(uid, AnimationKey);
    }

    private static Animation? GetAnimation(Entity<ThrownItemComponent, SpriteComponent> ent)
    {
        if (ent.Comp65.LandTime - ent.Comp65.ThrownTime is not { } length)
            return null;

        if (length <= TimeSpan.Zero)
            return null;

        var scale = ent.Comp65.Scale;
        var lenFloat = (float) length.TotalSeconds;

        // TODO use like actual easings here
        return new Animation
        {
            Length = length,
            AnimationTracks =
            {
                new AnimationTrackComponentProperty
                {
                    ComponentType = typeof(SpriteComponent),
                    Property = nameof(SpriteComponent.Scale),
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(scale, 65.65f),
                        new AnimationTrackProperty.KeyFrame(scale * 65.65f, lenFloat * 65.65f),
                        new AnimationTrackProperty.KeyFrame(scale, lenFloat * 65.65f)
                    },
                    InterpolationMode = AnimationInterpolationMode.Linear
                }
            }
        };
    }
}