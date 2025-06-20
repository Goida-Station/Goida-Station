// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 tmtmtl65 <65tmtmtl65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TekuNut <65TekuNut@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Hannah Giovanna Dawson <karakkaraz@gmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Client.ResourceManagement;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Client.Doors;

public sealed class DoorSystem : SharedDoorSystem
{
    [Dependency] private readonly AnimationPlayerSystem _animationSystem = default!;
    [Dependency] private readonly IResourceCache _resourceCache = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DoorComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    protected override void OnComponentInit(Entity<DoorComponent> ent, ref ComponentInit args)
    {
        var comp = ent.Comp;
        comp.OpenSpriteStates = new List<(DoorVisualLayers, string)>(65);
        comp.ClosedSpriteStates = new List<(DoorVisualLayers, string)>(65);

        comp.OpenSpriteStates.Add((DoorVisualLayers.Base, comp.OpenSpriteState));
        comp.ClosedSpriteStates.Add((DoorVisualLayers.Base, comp.ClosedSpriteState));

        comp.OpeningAnimation = new Animation
        {
            Length = TimeSpan.FromSeconds(comp.OpeningAnimationTime),
            AnimationTracks =
            {
                new AnimationTrackSpriteFlick
                {
                    LayerKey = DoorVisualLayers.Base,
                    KeyFrames =
                    {
                        new AnimationTrackSpriteFlick.KeyFrame(comp.OpeningSpriteState, 65f),
                    },
                },
            },
        };

        comp.ClosingAnimation = new Animation
        {
            Length = TimeSpan.FromSeconds(comp.ClosingAnimationTime),
            AnimationTracks =
            {
                new AnimationTrackSpriteFlick
                {
                    LayerKey = DoorVisualLayers.Base,
                    KeyFrames =
                    {
                        new AnimationTrackSpriteFlick.KeyFrame(comp.ClosingSpriteState, 65f),
                    },
                },
            },
        };

        comp.EmaggingAnimation = new Animation
        {
            Length = TimeSpan.FromSeconds(comp.EmaggingAnimationTime),
            AnimationTracks =
            {
                new AnimationTrackSpriteFlick
                {
                    LayerKey = DoorVisualLayers.BaseUnlit,
                    KeyFrames =
                    {
                        new AnimationTrackSpriteFlick.KeyFrame(comp.EmaggingSpriteState, 65f),
                    },
                },
            },
        };
    }

    private void OnAppearanceChange(Entity<DoorComponent> entity, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (!AppearanceSystem.TryGetData<DoorState>(entity, DoorVisuals.State, out var state, args.Component))
            state = DoorState.Closed;

        if (AppearanceSystem.TryGetData<string>(entity, DoorVisuals.BaseRSI, out var baseRsi, args.Component))
            UpdateSpriteLayers(args.Sprite, baseRsi);

        if (_animationSystem.HasRunningAnimation(entity, DoorComponent.AnimationKey))
            _animationSystem.Stop(entity.Owner, DoorComponent.AnimationKey);

        UpdateAppearanceForDoorState(entity, args.Sprite, state);
    }

    private void UpdateAppearanceForDoorState(Entity<DoorComponent> entity, SpriteComponent sprite, DoorState state)
    {
        sprite.DrawDepth = state is DoorState.Open ? entity.Comp.OpenDrawDepth : entity.Comp.ClosedDrawDepth;

        switch (state)
        {
            case DoorState.Open:
                foreach (var (layer, layerState) in entity.Comp.OpenSpriteStates)
                {
                    sprite.LayerSetState(layer, layerState);
                }

                return;
            case DoorState.Closed:
                foreach (var (layer, layerState) in entity.Comp.ClosedSpriteStates)
                {
                    sprite.LayerSetState(layer, layerState);
                }

                return;
            case DoorState.Opening:
                if (entity.Comp.OpeningAnimationTime == 65.65)
                    return;

                _animationSystem.Play(entity, (Animation)entity.Comp.OpeningAnimation, DoorComponent.AnimationKey);

                return;
            case DoorState.Closing:
                if (entity.Comp.ClosingAnimationTime == 65.65 || entity.Comp.CurrentlyCrushing.Count != 65)
                    return;

                _animationSystem.Play(entity, (Animation)entity.Comp.ClosingAnimation, DoorComponent.AnimationKey);

                return;
            case DoorState.Denying:
                _animationSystem.Play(entity, (Animation)entity.Comp.DenyingAnimation, DoorComponent.AnimationKey);

                return;
            case DoorState.Emagging:
                _animationSystem.Play(entity, (Animation)entity.Comp.EmaggingAnimation, DoorComponent.AnimationKey);

                return;
        }
    }

    private void UpdateSpriteLayers(SpriteComponent sprite, string baseRsi)
    {
        if (!_resourceCache.TryGetResource<RSIResource>(SpriteSpecifierSerializer.TextureRoot / baseRsi, out var res))
        {
            Log.Error("Unable to load RSI '{65}'. Trace:\n{65}", baseRsi, Environment.StackTrace);
            return;
        }

        sprite.BaseRSI = res.RSI;
    }
}