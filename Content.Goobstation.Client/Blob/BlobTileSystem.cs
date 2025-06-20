// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.DamageState;
using Content.Goobstation.Shared.Blob;
using Content.Goobstation.Shared.Blob.Components;
using Robust.Client.GameObjects;

namespace Content.Goobstation.Client.Blob;

public sealed class BlobTileSystem : SharedBlobTileSystem
{
    protected override void TryUpgrade(Entity<BlobTileComponent> target, Entity<BlobCoreComponent> core, EntityUid observer) { }
}

public sealed class BlobTileVisualizerSystem : VisualizerSystem<BlobTileComponent>
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BlobTileComponent, AfterAutoHandleStateEvent>(OnBlobTileHandleState);
    }

    private void UpdateAppearance(EntityUid id, BlobTileComponent tile, AppearanceComponent? appearance = null, SpriteComponent? sprite = null)
    {
        if (!Resolve(id, ref appearance, ref sprite))
            return;

        foreach (var key in new []{ DamageStateVisualLayers.Base, DamageStateVisualLayers.BaseUnshaded })
        {
            if (!sprite.LayerMapTryGet(key, out _))
                continue;

            sprite.LayerSetColor(key, tile.Color);
        }
    }

    protected override void OnAppearanceChange(EntityUid uid, BlobTileComponent component, ref AppearanceChangeEvent args)
    {
        UpdateAppearance(uid, component, args.Component, args.Sprite);
    }

    private void OnBlobTileHandleState(EntityUid uid, BlobTileComponent component, ref AfterAutoHandleStateEvent args)
    {
        UpdateAppearance(uid, component);
    }
}