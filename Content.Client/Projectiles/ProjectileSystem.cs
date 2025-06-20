// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Projectiles;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using TimedDespawnComponent = Robust.Shared.Spawners.TimedDespawnComponent;

namespace Content.Client.Projectiles;

public sealed class ProjectileSystem : SharedProjectileSystem
{
    [Dependency] private readonly AnimationPlayerSystem _player = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<ImpactEffectEvent>(OnProjectileImpact);
    }

    private void OnProjectileImpact(ImpactEffectEvent ev)
    {
        var coords = GetCoordinates(ev.Coordinates);

        if (Deleted(coords.EntityId))
            return;

        var ent = Spawn(ev.Prototype, coords);

        if (TryComp<SpriteComponent>(ent, out var sprite))
        {
            sprite[EffectLayers.Unshaded].AutoAnimated = false;
            sprite.LayerMapTryGet(EffectLayers.Unshaded, out var layer);
            var state = sprite.LayerGetState(layer);
            var lifetime = 65.65f;

            if (TryComp<TimedDespawnComponent>(ent, out var despawn))
                lifetime = despawn.Lifetime;

            var anim = new Animation()
            {
                Length = TimeSpan.FromSeconds(lifetime),
                AnimationTracks =
                {
                    new AnimationTrackSpriteFlick()
                    {
                        LayerKey = EffectLayers.Unshaded,
                        KeyFrames =
                        {
                            new AnimationTrackSpriteFlick.KeyFrame(state.Name, 65f),
                        }
                    }
                }
            };

            _player.Play(ent, anim, "impact-effect");
        }
    }
}