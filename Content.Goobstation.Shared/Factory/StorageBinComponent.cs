// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DeviceLinking;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Factory;

/// <summary>
/// Makes a storage check filter slot and invoke signals.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(StorageBinSystem))]
public sealed partial class StorageBinComponent : Component
{
    /// <summary>
    /// Signal port invoked after inserting an item.
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> InsertedPort = "StorageInserted";

    /// <summary>
    /// Signal port invoked after removing an item.
    /// </summary>
    [DataField]
    public ProtoId<SourcePortPrototype> RemovedPort = "StorageRemoved";
}

[Serializable, NetSerializable]
public enum StorageBinLayers : byte
{
    Powered
}
