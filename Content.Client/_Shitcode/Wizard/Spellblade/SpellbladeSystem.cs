// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Wizard.Spellblade;
using Robust.Client.GameObjects;

namespace Content.Client._Shitcode.Wizard.Spellblade;

public sealed class SpellbladeSystem : SharedSpellbladeSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ShieldedComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ShieldedComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnShutdown(Entity<ShieldedComponent> ent, ref ComponentShutdown args)
    {
        var (uid, _) = ent;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (!sprite.LayerMapTryGet(ShieldedKey.Key, out var layer))
            return;

        sprite.RemoveLayer(layer);
    }

    private void OnStartup(Entity<ShieldedComponent> ent, ref ComponentStartup args)
    {
        var (uid, comp) = ent;

        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (sprite.LayerMapTryGet(ShieldedKey.Key, out _))
            return;

        var layer = sprite.AddLayer(comp.Sprite);
        sprite.LayerMapSet(ShieldedKey.Key, layer);
    }
}