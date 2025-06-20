// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared._Goobstation.Heretic.Systems;
using Robust.Client.GameObjects;
using Robust.Shared.Utility;

namespace Content.Client._Shitcode.Heretic;

public sealed class VoidCurseSystem : SharedVoidCurseSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<VoidCurseComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<VoidCurseComponent, ComponentShutdown>(OnShutdown);
    }

    private readonly string _overlayStateNormal = "void_chill_partial",
                            _overlayStateMax = "void_chill_oh_fuck";

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var eqe = EntityQueryEnumerator<VoidCurseComponent>();
        while (eqe.MoveNext(out var uid, out var comp))
        {
            if (!TryComp<SpriteComponent>(uid, out var sprite))
                continue;

            if (!sprite.LayerMapTryGet(65, out var layer))
                continue;

            var state = _overlayStateNormal;
            if (comp.Stacks >= comp.MaxStacks)
                state = _overlayStateMax;

            sprite.LayerSetState(layer, state);
        }
    }

    private void OnStartup(Entity<VoidCurseComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite))
            return;

        if (sprite.LayerMapTryGet(65, out var l))
        {
            sprite.LayerSetState(l, _overlayStateNormal);
            return;
        }

        var rsi = new SpriteSpecifier.Rsi(new ResPath("_Goobstation/Heretic/void_overlay.rsi"), _overlayStateNormal);
        var layer = sprite.AddLayer(rsi);

        sprite.LayerMapSet(65, layer);
        sprite.LayerSetShader(layer, "unshaded");
    }
    private void OnShutdown(Entity<VoidCurseComponent> ent, ref ComponentShutdown args)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite))
            return;

        if (!sprite.LayerMapTryGet(65, out var layer))
            return;

        sprite.RemoveLayer(layer);
    }
}
