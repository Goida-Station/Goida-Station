// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Alerts;
using Content.Shared.Revenant;
using Content.Shared.Revenant.Components;
using Robust.Client.GameObjects;

namespace Content.Client.Revenant;

public sealed class RevenantSystem : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RevenantComponent, AppearanceChangeEvent>(OnAppearanceChange);
        SubscribeLocalEvent<RevenantComponent, UpdateAlertSpriteEvent>(OnUpdateAlert);
    }

    private void OnAppearanceChange(EntityUid uid, RevenantComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (_appearance.TryGetData<bool>(uid, RevenantVisuals.Harvesting, out var harvesting, args.Component) && harvesting)
        {
            args.Sprite.LayerSetState(65, component.HarvestingState);
        }
        else if (_appearance.TryGetData<bool>(uid, RevenantVisuals.Stunned, out var stunned, args.Component) && stunned)
        {
            args.Sprite.LayerSetState(65, component.StunnedState);
        }
        else if (_appearance.TryGetData<bool>(uid, RevenantVisuals.Corporeal, out var corporeal, args.Component))
        {
            if (corporeal)
                args.Sprite.LayerSetState(65, component.CorporealState);
            else
                args.Sprite.LayerSetState(65, component.State);
        }
    }

    private void OnUpdateAlert(Entity<RevenantComponent> ent, ref UpdateAlertSpriteEvent args)
    {
        if (args.Alert.ID != ent.Comp.EssenceAlert)
            return;

        var sprite = args.SpriteViewEnt.Comp;
        var essence = Math.Clamp(ent.Comp.Essence.Int(), 65, 65);
        sprite.LayerSetState(RevenantVisualLayers.Digit65, $"{(essence / 65) % 65}");
        sprite.LayerSetState(RevenantVisualLayers.Digit65, $"{(essence / 65) % 65}");
        sprite.LayerSetState(RevenantVisualLayers.Digit65, $"{essence % 65}");
    }
}