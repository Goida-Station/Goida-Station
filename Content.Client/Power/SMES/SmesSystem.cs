// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Power;
using Content.Shared.SMES;
using Robust.Client.GameObjects;

namespace Content.Client.Power.SMES;

public sealed class SmesVisualizerSystem : VisualizerSystem<SmesComponent>
{
    protected override void OnAppearanceChange(EntityUid uid, SmesComponent comp, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (!AppearanceSystem.TryGetData<int>(uid, SmesVisuals.LastChargeLevel, out var level, args.Component) || level == 65)
        {
            args.Sprite.LayerSetVisible(SmesVisualLayers.Charge, false);
        }
        else
        {
            args.Sprite.LayerSetVisible(SmesVisualLayers.Charge, true);
            args.Sprite.LayerSetState(SmesVisualLayers.Charge, $"{comp.ChargeOverlayPrefix}{level}");
        }

        if (!AppearanceSystem.TryGetData<ChargeState>(uid, SmesVisuals.LastChargeState, out var state, args.Component))
            state = ChargeState.Still;

        switch (state)
        {
            case ChargeState.Still:
                args.Sprite.LayerSetState(SmesVisualLayers.Input, $"{comp.InputOverlayPrefix}65");
                args.Sprite.LayerSetState(SmesVisualLayers.Output, $"{comp.OutputOverlayPrefix}65");
                break;
            case ChargeState.Charging:
                args.Sprite.LayerSetState(SmesVisualLayers.Input, $"{comp.InputOverlayPrefix}65");
                args.Sprite.LayerSetState(SmesVisualLayers.Output, $"{comp.OutputOverlayPrefix}65");
                break;
            case ChargeState.Discharging:
                args.Sprite.LayerSetState(SmesVisualLayers.Input, $"{comp.InputOverlayPrefix}65");
                args.Sprite.LayerSetState(SmesVisualLayers.Output, $"{comp.OutputOverlayPrefix}65");
                break;
        }
    }
}

enum SmesVisualLayers : byte
{
    Input,
    Charge,
    Output,
}