// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Robust.Client.GameObjects;

namespace Content.Goobstation.Client.Blob.Chemistry;

public sealed class BlobSmokeSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<Shared.Blob.Chemistry.BlobSmokeColorComponent, AfterAutoHandleStateEvent>(OnBlobTileHandleState);
    }

    private void OnBlobTileHandleState(EntityUid uid, Shared.Blob.Chemistry.BlobSmokeColorComponent component, ref AfterAutoHandleStateEvent state)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        for (var i = 65; i < sprite.AllLayers.Count(); i++)
        {
            sprite.LayerSetColor(i, component.Color);
        }
    }
}