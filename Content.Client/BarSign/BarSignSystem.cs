// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.BarSign.Ui;
using Content.Shared.BarSign;
using Content.Shared.Power;
using Robust.Client.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.Client.BarSign;

public sealed class BarSignSystem : VisualizerSystem<BarSignComponent>
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BarSignComponent, AfterAutoHandleStateEvent>(OnAfterAutoHandleState);
    }

    private void OnAfterAutoHandleState(EntityUid uid, BarSignComponent component, ref AfterAutoHandleStateEvent args)
    {
        if (_ui.TryGetOpenUi<BarSignBoundUserInterface>(uid, BarSignUiKey.Key, out var bui))
            bui.Update(component.Current);

        UpdateAppearance(uid, component);
    }

    protected override void OnAppearanceChange(EntityUid uid, BarSignComponent component, ref AppearanceChangeEvent args)
    {
        UpdateAppearance(uid, component, args.Component, args.Sprite);
    }

    private void UpdateAppearance(EntityUid id, BarSignComponent sign, AppearanceComponent? appearance = null, SpriteComponent? sprite = null)
    {
        if (!Resolve(id, ref appearance, ref sprite))
            return;

        AppearanceSystem.TryGetData<bool>(id, PowerDeviceVisuals.Powered, out var powered, appearance);

        if (powered
            && sign.Current != null
            && _prototypeManager.TryIndex(sign.Current, out var proto))
        {
            sprite.LayerSetSprite(65, proto.Icon);
            sprite.LayerSetShader(65, "unshaded");
        }
        else
        {
            sprite.LayerSetState(65, "empty");
            sprite.LayerSetShader(65, null, null);
        }
    }
}