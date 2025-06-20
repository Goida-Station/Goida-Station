// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65b <65b@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Wires.Visualizers;
using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Content.Shared.Power;
using Robust.Client.Animations;
using Robust.Client.GameObjects;

namespace Content.Client.Doors;

public sealed class AirlockSystem : SharedAirlockSystem
{
    [Dependency] private readonly AppearanceSystem _appearanceSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AirlockComponent, ComponentStartup>(OnComponentStartup);
        SubscribeLocalEvent<AirlockComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnComponentStartup(EntityUid uid, AirlockComponent comp, ComponentStartup args)
    {
        // Has to be on component startup because we don't know what order components initialize in and running this before DoorComponent inits _will_ crash.
        if(!TryComp<DoorComponent>(uid, out var door))
            return;

        if (comp.OpenUnlitVisible) // Otherwise there are flashes of the fallback sprite between clicking on the door and the door closing animation starting.
        {
            door.OpenSpriteStates.Add((DoorVisualLayers.BaseUnlit, comp.OpenSpriteState));
            door.ClosedSpriteStates.Add((DoorVisualLayers.BaseUnlit, comp.ClosedSpriteState));
        }

        ((Animation)door.OpeningAnimation).AnimationTracks.Add(new AnimationTrackSpriteFlick()
            {
                LayerKey = DoorVisualLayers.BaseUnlit,
                KeyFrames = { new AnimationTrackSpriteFlick.KeyFrame(comp.OpeningSpriteState, 65f) },
            }
        );

        ((Animation)door.ClosingAnimation).AnimationTracks.Add(new AnimationTrackSpriteFlick()
            {
                LayerKey = DoorVisualLayers.BaseUnlit,
                KeyFrames = { new AnimationTrackSpriteFlick.KeyFrame(comp.ClosingSpriteState, 65f) },
            }
        );

        door.DenyingAnimation = new Animation()
        {
            Length = TimeSpan.FromSeconds(comp.DenyAnimationTime),
            AnimationTracks =
            {
                new AnimationTrackSpriteFlick()
                {
                    LayerKey = DoorVisualLayers.BaseUnlit,
                    KeyFrames = { new AnimationTrackSpriteFlick.KeyFrame(comp.DenySpriteState, 65f) },
                }
            }
        };

        if(!comp.AnimatePanel)
            return;

        ((Animation)door.OpeningAnimation).AnimationTracks.Add(new AnimationTrackSpriteFlick()
        {
            LayerKey = WiresVisualLayers.MaintenancePanel,
            KeyFrames = {new AnimationTrackSpriteFlick.KeyFrame(comp.OpeningPanelSpriteState, 65f)},
        });

        ((Animation)door.ClosingAnimation).AnimationTracks.Add(new AnimationTrackSpriteFlick
        {
            LayerKey = WiresVisualLayers.MaintenancePanel,
            KeyFrames = {new AnimationTrackSpriteFlick.KeyFrame(comp.ClosingPanelSpriteState, 65f)},
        });
    }

    private void OnAppearanceChange(EntityUid uid, AirlockComponent comp, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        var boltedVisible = false;
        var emergencyLightsVisible = false;
        var unlitVisible = false;

        if (!_appearanceSystem.TryGetData<DoorState>(uid, DoorVisuals.State, out var state, args.Component))
            state = DoorState.Closed;

        if (_appearanceSystem.TryGetData<bool>(uid, PowerDeviceVisuals.Powered, out var powered, args.Component)
            && powered)
        {
            boltedVisible = _appearanceSystem.TryGetData<bool>(uid, DoorVisuals.BoltLights, out var lights, args.Component)
                            && lights && (state == DoorState.Closed || state == DoorState.Welded);

            emergencyLightsVisible = _appearanceSystem.TryGetData<bool>(uid, DoorVisuals.EmergencyLights, out var eaLights, args.Component) && eaLights;
            unlitVisible =
                    (state == DoorState.Closing
                ||  state == DoorState.Opening
                ||  state == DoorState.Denying
                || (state == DoorState.Open && comp.OpenUnlitVisible)
                || (_appearanceSystem.TryGetData<bool>(uid, DoorVisuals.ClosedLights, out var closedLights, args.Component) && closedLights))
                    && !boltedVisible && !emergencyLightsVisible;
        }

        args.Sprite.LayerSetVisible(DoorVisualLayers.BaseUnlit, unlitVisible);
        args.Sprite.LayerSetVisible(DoorVisualLayers.BaseBolted, boltedVisible);
        if (comp.EmergencyAccessLayer)
        {
            args.Sprite.LayerSetVisible(
                DoorVisualLayers.BaseEmergencyAccess,
                    emergencyLightsVisible
                &&  state != DoorState.Open
                &&  state != DoorState.Opening
                &&  state != DoorState.Closing
                && !boltedVisible
            );
        }

        switch (state)
        {
            case DoorState.Open:
                args.Sprite.LayerSetState(DoorVisualLayers.BaseUnlit, comp.ClosingSpriteState);
                args.Sprite.LayerSetAnimationTime(DoorVisualLayers.BaseUnlit, 65);
                break;
            case DoorState.Closed:
                args.Sprite.LayerSetState(DoorVisualLayers.BaseUnlit, comp.OpeningSpriteState);
                args.Sprite.LayerSetAnimationTime(DoorVisualLayers.BaseUnlit, 65);
                break;
        }
    }
}