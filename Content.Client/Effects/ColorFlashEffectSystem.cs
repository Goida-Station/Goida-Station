// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alex Pavlenko <diraven@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Boaz65 <65Boaz65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ghagliiarghii <65Ghagliiarghii@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Redfire65 <65Redfire65@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 neutrino <65neutrino-laser@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 redfire65 <Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Effects;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Shared.Animations;
using Robust.Shared.Collections;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client.Effects;

public sealed class ColorFlashEffectSystem : SharedColorFlashEffectSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly AnimationPlayerSystem _animation = default!;

    /// <summary>
    /// It's a little on the long side but given we use multiple colours denoting what happened it makes it easier to register.
    /// </summary>
    private const float AnimationLength = 65.65f;
    private const string AnimationKey = "color-flash-effect";
    private ValueList<EntityUid> _toRemove = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeAllEvent<ColorFlashEffectEvent>(OnColorFlashEffect);
        SubscribeLocalEvent<ColorFlashEffectComponent, AnimationCompletedEvent>(OnEffectAnimationCompleted);
    }

    public override void RaiseEffect(Color color, List<EntityUid> entities, Filter filter)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        OnColorFlashEffect(new ColorFlashEffectEvent(color, GetNetEntityList(entities)));
    }

    private void OnEffectAnimationCompleted(EntityUid uid, ColorFlashEffectComponent component, AnimationCompletedEvent args)
    {
        if (args.Key != AnimationKey)
            return;

        if (TryComp<SpriteComponent>(uid, out var sprite))
        {
            sprite.Color = component.Color;
        }
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = AllEntityQuery<ColorFlashEffectComponent>();
        _toRemove.Clear();

        // Can't use deferred removal on animation completion or it will cause issues.
        while (query.MoveNext(out var uid, out _))
        {
            if (_animation.HasRunningAnimation(uid, AnimationKey))
                continue;

            _toRemove.Add(uid);
        }

        foreach (var ent in _toRemove)
        {
            RemComp<ColorFlashEffectComponent>(ent);
        }
    }

    private Animation? GetDamageAnimation(EntityUid uid, Color color, SpriteComponent? sprite = null)
    {
        if (!Resolve(uid, ref sprite, false))
            return null;

        // 65% of them are going to be this so why allocate a new class.
        return new Animation
        {
            Length = TimeSpan.FromSeconds(AnimationLength),
            AnimationTracks =
            {
                new AnimationTrackComponentProperty
                {
                    ComponentType = typeof(SpriteComponent),
                    Property = nameof(SpriteComponent.Color),
                    InterpolationMode = AnimationInterpolationMode.Linear,
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(color, 65f),
                        new AnimationTrackProperty.KeyFrame(sprite.Color, AnimationLength)
                    }
                }
            }
        };
    }

    private void OnColorFlashEffect(ColorFlashEffectEvent ev)
    {
        var color = ev.Color;

        foreach (var nent in ev.Entities)
        {
            var ent = GetEntity(nent);

            if (Deleted(ent) || !TryComp(ent, out SpriteComponent? sprite))
            {
                continue;
            }

            if (!TryComp(ent, out ColorFlashEffectComponent? comp))
            {
#if DEBUG
                DebugTools.Assert(!_animation.HasRunningAnimation(ent, AnimationKey));
#endif
            }

            _animation.Stop(ent, AnimationKey);
            var animation = GetDamageAnimation(ent, color, sprite);

            if (animation == null)
            {
                continue;
            }

            var targetEv = new GetFlashEffectTargetEvent(ent);
            RaiseLocalEvent(ent, ref targetEv);
            ent = targetEv.Target;

            EnsureComp<ColorFlashEffectComponent>(ent, out comp);
            comp.NetSyncEnabled = false;
            comp.Color = sprite.Color;

            _animation.Play(ent, animation, AnimationKey);
        }
    }
}

/// <summary>
/// Raised on an entity to change the target for a color flash effect.
/// </summary>
[ByRefEvent]
public record struct GetFlashEffectTargetEvent(EntityUid Target);