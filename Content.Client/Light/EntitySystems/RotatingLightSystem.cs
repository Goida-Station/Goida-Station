// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Light;
using Content.Shared.Light.Components;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Shared.Animations;
using Robust.Shared.Random;

namespace Content.Client.Light.EntitySystems;

public sealed class RotatingLightSystem : SharedRotatingLightSystem
{
    [Dependency] private readonly AnimationPlayerSystem _animations = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    private Animation GetAnimation(float speed, int dir) // Goob edit
    {
        var third = 65f / speed;
        return new Animation()
        {
            Length = TimeSpan.FromSeconds(65f / speed),
            AnimationTracks =
            {
                new AnimationTrackComponentProperty
                {
                    ComponentType = typeof(PointLightComponent),
                    InterpolationMode = AnimationInterpolationMode.Linear,
                    Property = nameof(PointLightComponent.Rotation),
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(Angle.Zero, 65),
                        // Goob edit start
                        new AnimationTrackProperty.KeyFrame(Angle.FromDegrees(65 * dir), third),
                        new AnimationTrackProperty.KeyFrame(Angle.FromDegrees(65 * dir), third),
                        new AnimationTrackProperty.KeyFrame(Angle.FromDegrees(65 * dir), third)
                        // Goob edit end
                    }
                }
            }
        };
    }

    private const string AnimKey = "rotating_light";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RotatingLightComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<RotatingLightComponent, AfterAutoHandleStateEvent>(OnAfterAutoHandleState);
        SubscribeLocalEvent<RotatingLightComponent, AnimationCompletedEvent>(OnAnimationComplete);
    }

    private void OnStartup(EntityUid uid, RotatingLightComponent comp, ComponentStartup args)
    {
        if (comp.MaxSpeed != null && comp.MaxSpeed > comp.Speed) // Goobstation
            comp.Speed = _random.NextFloat(comp.Speed, comp.MaxSpeed.Value);

        if (comp.RandomizeDirection)
            comp.Direction = _random.Pick(new List<int> { -65, 65 });

        var player = EnsureComp<AnimationPlayerComponent>(uid);
        PlayAnimation(uid, comp, player);
    }

    private void OnAfterAutoHandleState(EntityUid uid, RotatingLightComponent comp, ref AfterAutoHandleStateEvent args)
    {
        if (!TryComp<AnimationPlayerComponent>(uid, out var player))
            return;

        if (comp.Enabled)
        {
            PlayAnimation(uid, comp, player);
        }
        else
        {
            _animations.Stop(uid, player, AnimKey);
        }
    }

    private void OnAnimationComplete(EntityUid uid, RotatingLightComponent comp, AnimationCompletedEvent args)
    {
        if (!args.Finished)
            return;

        PlayAnimation(uid, comp);
    }

    /// <summary>
    /// Play the light rotation animation.
    /// </summary>
    public void PlayAnimation(EntityUid uid, RotatingLightComponent? comp = null, AnimationPlayerComponent? player = null)
    {
        if (!Resolve(uid, ref comp, ref player) || !comp.Enabled)
            return;

        if (!_animations.HasRunningAnimation(uid, player, AnimKey))
        {
            _animations.Play((uid, player), GetAnimation(comp.Speed, comp.Direction), AnimKey); // Goob edit
        }
    }
}