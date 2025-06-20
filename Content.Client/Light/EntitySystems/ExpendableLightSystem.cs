// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Light.Components;
using Content.Shared.Light.Components;
using Robust.Client.GameObjects;
using Robust.Shared.Audio.Systems;

namespace Content.Client.Light.EntitySystems;

public sealed class ExpendableLightSystem : VisualizerSystem<ExpendableLightComponent>
{
    [Dependency] private readonly PointLightSystem _pointLightSystem = default!;
    [Dependency] private readonly SharedAudioSystem _audioSystem = default!;
    [Dependency] private readonly LightBehaviorSystem _lightBehavior = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ExpendableLightComponent, ComponentShutdown>(OnLightShutdown);
    }

    private void OnLightShutdown(EntityUid uid, ExpendableLightComponent component, ComponentShutdown args)
    {
        component.PlayingStream = _audioSystem.Stop(component.PlayingStream);
    }

    protected override void OnAppearanceChange(EntityUid uid, ExpendableLightComponent comp, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (AppearanceSystem.TryGetData<string>(uid, ExpendableLightVisuals.Behavior, out var lightBehaviourID, args.Component)
        &&  TryComp<LightBehaviourComponent>(uid, out var lightBehaviour))
        {
            _lightBehavior.StopLightBehaviour((uid, lightBehaviour));

            if (!string.IsNullOrEmpty(lightBehaviourID))
            {
                _lightBehavior.StartLightBehaviour((uid, lightBehaviour), lightBehaviourID);
            }
            else if (TryComp<PointLightComponent>(uid, out var light))
            {
                _pointLightSystem.SetEnabled(uid, false, light);
            }
        }

        if (!AppearanceSystem.TryGetData<ExpendableLightState>(uid, ExpendableLightVisuals.State, out var state, args.Component))
            return;

        switch (state)
        {
            case ExpendableLightState.Lit:
                _audioSystem.Stop(comp.PlayingStream);
                comp.PlayingStream = _audioSystem.PlayPvs(
                    comp.LoopedSound, uid)?.Entity;

                if (args.Sprite.LayerMapTryGet(ExpendableLightVisualLayers.Overlay, out var layerIdx, true))
                {
                    if (!string.IsNullOrWhiteSpace(comp.IconStateLit))
                        args.Sprite.LayerSetState(layerIdx, comp.IconStateLit);
                    if (!string.IsNullOrWhiteSpace(comp.SpriteShaderLit))
                        args.Sprite.LayerSetShader(layerIdx, comp.SpriteShaderLit);
                    else
                        args.Sprite.LayerSetShader(layerIdx, null, null);
                    if (comp.GlowColorLit.HasValue)
                        args.Sprite.LayerSetColor(layerIdx, comp.GlowColorLit.Value);
                    args.Sprite.LayerSetVisible(layerIdx, true);
                }

                if (comp.GlowColorLit.HasValue)
                    args.Sprite.LayerSetColor(ExpendableLightVisualLayers.Glow, comp.GlowColorLit.Value);
                args.Sprite.LayerSetVisible(ExpendableLightVisualLayers.Glow, true);

                break;
            case ExpendableLightState.Dead:
                comp.PlayingStream = _audioSystem.Stop(comp.PlayingStream);
                if (args.Sprite.LayerMapTryGet(ExpendableLightVisualLayers.Overlay, out layerIdx, true))
                {
                    if (!string.IsNullOrWhiteSpace(comp.IconStateSpent))
                        args.Sprite.LayerSetState(layerIdx, comp.IconStateSpent);
                    if (!string.IsNullOrWhiteSpace(comp.SpriteShaderSpent))
                        args.Sprite.LayerSetShader(layerIdx, comp.SpriteShaderSpent);
                    else
                        args.Sprite.LayerSetShader(layerIdx, null, null);
                }

                args.Sprite.LayerSetVisible(ExpendableLightVisualLayers.Glow, false);
                break;
        }
    }
}