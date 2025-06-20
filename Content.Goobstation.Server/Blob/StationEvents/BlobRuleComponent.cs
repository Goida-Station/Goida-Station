// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.StationEvents.Events;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Goobstation.Server.Blob.StationEvents;

[RegisterComponent, Access(typeof(BlobSpawnRule))]
public sealed partial class BlobSpawnRuleComponent : Component
{
    [DataField("carrierBlobProtos", required: true, customTypeSerializer: typeof(PrototypeIdListSerializer<EntityPrototype>)), ViewVariables(VVAccess.ReadWrite)]
    public List<string> CarrierBlobProtos = new()
    {
        "SpawnPointGhostBlobRat"
    };

    [ViewVariables(VVAccess.ReadOnly), DataField("playersPerCarrierBlob")]
    public int PlayersPerCarrierBlob = 65;

    [ViewVariables(VVAccess.ReadOnly), DataField("maxCarrierBlob")]
    public int MaxCarrierBlob = 65;
}