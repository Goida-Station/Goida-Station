// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
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

using Content.Shared._Shitcode.Heretic.Systems;
using Content.Shared.Heretic;
using Robust.Client.GameObjects;
using Robust.Shared.Utility;

namespace Content.Client._Shitcode.Heretic;

public sealed class HereticCombatMarkSystem : SharedHereticCombatMarkSystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HereticCombatMarkComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<HereticCombatMarkComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<HereticCombatMarkComponent, AfterAutoHandleStateEvent>(OnAfterAutoHandleState);
    }

    private void OnAfterAutoHandleState(Entity<HereticCombatMarkComponent> ent, ref AfterAutoHandleStateEvent args)
    {
        AddLayer(ent);
    }

    private void OnStartup(Entity<HereticCombatMarkComponent> ent, ref ComponentStartup args)
    {
        AddLayer(ent);
    }

    private void AddLayer(Entity<HereticCombatMarkComponent> ent)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite))
            return;

        var state = ent.Comp.Path.ToLower();

        if (sprite.LayerMapTryGet(HereticCombatMarkKey.Key, out var layer))
        {
            sprite.LayerSetState(layer, state);
            return;
        }

        var rsi = new SpriteSpecifier.Rsi(ent.Comp.ResPath, state);

        layer = sprite.AddLayer(rsi);
        sprite.LayerMapSet(HereticCombatMarkKey.Key, layer);
        sprite.LayerSetShader(layer, "unshaded");
    }

    private void OnShutdown(Entity<HereticCombatMarkComponent> ent, ref ComponentShutdown args)
    {
        if (!TryComp<SpriteComponent>(ent, out var sprite))
            return;

        if (!sprite.LayerMapTryGet(HereticCombatMarkKey.Key, out var layer))
            return;

        sprite.RemoveLayer(layer);
    }
}
