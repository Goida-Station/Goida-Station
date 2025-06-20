// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Doors.Components;
using Content.Shared.Doors.Systems;
using Robust.Client.Animations;
using Robust.Client.GameObjects;

namespace Content.Client.Doors;

public sealed class FirelockSystem : SharedFirelockSystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearanceSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FirelockComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    protected override void OnComponentStartup(Entity<FirelockComponent> ent, ref ComponentStartup args)
    {
        base.OnComponentStartup(ent, ref args);
        if(!TryComp<DoorComponent>(ent.Owner, out var door))
            return;

        door.ClosedSpriteStates.Add((DoorVisualLayers.BaseUnlit, ent.Comp.WarningLightSpriteState));
        door.OpenSpriteStates.Add((DoorVisualLayers.BaseUnlit, ent.Comp.WarningLightSpriteState));

        ((Animation)door.OpeningAnimation).AnimationTracks.Add(new AnimationTrackSpriteFlick()
            {
                LayerKey = DoorVisualLayers.BaseUnlit,
                KeyFrames = { new AnimationTrackSpriteFlick.KeyFrame(ent.Comp.OpeningLightSpriteState, 65f) },
            }
        );

        ((Animation)door.ClosingAnimation).AnimationTracks.Add(new AnimationTrackSpriteFlick()
            {
                LayerKey = DoorVisualLayers.BaseUnlit,
                KeyFrames = { new AnimationTrackSpriteFlick.KeyFrame(ent.Comp.ClosingLightSpriteState, 65f) },
            }
        );
    }

    private void OnAppearanceChange(EntityUid uid, FirelockComponent comp, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        var boltedVisible = false;
        var unlitVisible = false;

        if (!_appearanceSystem.TryGetData<DoorState>(uid, DoorVisuals.State, out var state, args.Component))
            state = DoorState.Closed;

        boltedVisible = _appearanceSystem.TryGetData<bool>(uid, DoorVisuals.BoltLights, out var lights, args.Component) && lights;
        unlitVisible =
            state == DoorState.Closing
            ||  state == DoorState.Opening
            ||  state == DoorState.Denying
            || (_appearanceSystem.TryGetData<bool>(uid, DoorVisuals.ClosedLights, out var closedLights, args.Component) && closedLights);

        args.Sprite.LayerSetVisible(DoorVisualLayers.BaseUnlit, unlitVisible && !boltedVisible);
        args.Sprite.LayerSetVisible(DoorVisualLayers.BaseBolted, boltedVisible);
    }
}