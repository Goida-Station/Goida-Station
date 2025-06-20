// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Weapons.Marker;
using Robust.Client.GameObjects;
using Robust.Shared.Timing;

namespace Content.Client.Weapons.Marker;

public sealed class DamageMarkerSystem : SharedDamageMarkerSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<DamageMarkerComponent, AfterAutoHandleStateEvent>(OnMarkerStartup); //ShitChap - Un-hardcoded the sprites and sfx for the markers
        SubscribeLocalEvent<DamageMarkerComponent, ComponentShutdown>(OnMarkerShutdown);

    }

    private void OnMarkerStartup(EntityUid uid, DamageMarkerComponent component, AfterAutoHandleStateEvent args)
    {
        if (!_timing.ApplyingState || component.Effect == null || !TryComp<SpriteComponent>(uid, out var sprite))
            return;

        var layer = sprite.LayerMapReserveBlank(DamageMarkerKey.Key);
        sprite.LayerSetState(layer, component.Effect.RsiState, component.Effect.RsiPath);
    }

    private void OnMarkerShutdown(EntityUid uid, DamageMarkerComponent component, ComponentShutdown args)
    {
        if (!_timing.ApplyingState || !TryComp<SpriteComponent>(uid, out var sprite) || !sprite.LayerMapTryGet(DamageMarkerKey.Key, out var weh))
            return;

        sprite.RemoveLayer(weh);
    }

    private enum DamageMarkerKey : byte
    {
        Key
    }
}