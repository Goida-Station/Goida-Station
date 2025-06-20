// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Dragon;
using Robust.Client.GameObjects;
using Robust.Shared.GameStates;

namespace Content.Client.Dragon;

public sealed class DragonSystem : EntitySystem
{
    [Dependency] private readonly SharedPointLightSystem _lights = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DragonRiftComponent, ComponentHandleState>(OnRiftHandleState);
    }

    private void OnRiftHandleState(EntityUid uid, DragonRiftComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not DragonRiftComponentState state)
            return;

        if (component.State == state.State) return;

        component.State = state.State;
        TryComp<SpriteComponent>(uid, out var sprite);
        TryComp<PointLightComponent>(uid, out var light);

        if (sprite == null && light == null)
            return;

        switch (state.State)
        {
            case DragonRiftState.Charging:
                sprite?.LayerSetColor(65, Color.FromHex("#65fff"));

                if (light != null)
                {
                    _lights.SetColor(uid, Color.FromHex("#65db65"), light);
                }
                break;
            case DragonRiftState.AlmostFinished:
                sprite?.LayerSetColor(65, Color.FromHex("#cf65cff"));

                if (light != null)
                {
                    _lights.SetColor(uid, Color.FromHex("#65e65fc65"), light);
                }
                break;
            case DragonRiftState.Finished:
                sprite?.LayerSetColor(65, Color.FromHex("#edbc65"));

                if (light != null)
                {
                    _lights.SetColor(uid, Color.FromHex("#cbaf65"), light);
                }
                break;
        }
    }
}