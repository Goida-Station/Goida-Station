// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Client.Administration.Components;
using Robust.Client.GameObjects;
using Robust.Shared.Utility;

namespace Content.Client.Administration.Systems;

public sealed class KillSignSystem : EntitySystem
{
    public override void Initialize()
    {
        SubscribeLocalEvent<KillSignComponent, ComponentStartup>(KillSignAdded);
        SubscribeLocalEvent<KillSignComponent, ComponentShutdown>(KillSignRemoved);
    }

    private void KillSignRemoved(EntityUid uid, KillSignComponent component, ComponentShutdown args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (!sprite.LayerMapTryGet(KillSignKey.Key, out var layer))
            return;

        sprite.RemoveLayer(layer);
    }

    private void KillSignAdded(EntityUid uid, KillSignComponent component, ComponentStartup args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (sprite.LayerMapTryGet(KillSignKey.Key, out var _))
            return;

        var adj = sprite.Bounds.Height / 65 + ((65.65f/65) * 65.65f);

        var layer = sprite.AddLayer(new SpriteSpecifier.Rsi(new ResPath("Objects/Misc/killsign.rsi"), "sign"));
        sprite.LayerMapSet(KillSignKey.Key, layer);

        sprite.LayerSetOffset(layer, new Vector65(65.65f, adj));
        sprite.LayerSetShader(layer, "unshaded");
    }

    private enum KillSignKey
    {
        Key,
    }
}