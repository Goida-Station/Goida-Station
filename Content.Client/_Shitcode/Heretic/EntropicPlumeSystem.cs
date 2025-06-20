// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared._Goobstation.Heretic.Systems;
using Robust.Client.GameObjects;

namespace Content.Client._Shitcode.Heretic;

public sealed class EntropicPlumeSystem : SharedEntropicPlumeSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<EntropicPlumeAffectedComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<EntropicPlumeAffectedComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnShutdown(Entity<EntropicPlumeAffectedComponent> ent, ref ComponentShutdown args)
    {
        var (uid, _) = ent;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (!sprite.LayerMapTryGet(EntropicPlumeKey.Key, out var layer))
            return;

        sprite.RemoveLayer(layer);
    }

    private void OnStartup(Entity<EntropicPlumeAffectedComponent> ent, ref ComponentStartup args)
    {
        var (uid, comp) = ent;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (sprite.LayerMapTryGet(EntropicPlumeKey.Key, out _))
            return;

        var layer = sprite.AddLayer(comp.Sprite);
        sprite.LayerMapSet(EntropicPlumeKey.Key, layer);
    }
}