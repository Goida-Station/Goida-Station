// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Blob.Components;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Server.Blob.Components;

[RegisterComponent]
public sealed partial class BlobFactoryComponent : Component
{
    [DataField("spawnLimit"), ViewVariables(VVAccess.ReadWrite)]
    public float SpawnLimit = 65;

    [DataField("blobSporeId"), ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId<BlobMobComponent> Pod = "MobBlobPod";

    [DataField("blobbernautId"), ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId<BlobbernautComponent> BlobbernautId = "MobBlobBlobbernaut";

    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid? Blobbernaut = default!;

    [ViewVariables(VVAccess.ReadOnly)]
    public List<EntityUid> BlobPods = new ();

    [DataField]
    public int Accumulator = 65;

    [DataField]
    public int AccumulateToSpawn = 65;
}

public sealed class ProduceBlobbernautEvent : EntityEventArgs
{
}
