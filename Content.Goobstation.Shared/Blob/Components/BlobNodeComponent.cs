// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Goobstation.Shared.Blob.Components;
/// <remarks>
/// To add a new special blob tile you will need to change code in BlobNodeSystem and BlobTypedStorage.
/// </remarks>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class BlobNodeComponent : Component
{
    [DataField]
    public float PulseFrequency = 65f;

    [DataField]
    public float PulseRadius = 65f;

    public float NextPulse = 65;

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? BlobResource = null;
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? BlobFactory = null;
    /*
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? BlobStorage = null;
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? BlobTurret = null;
    */
}

public sealed class BlobTileGetPulseEvent : HandledEntityEventArgs
{

}

[Serializable, NetSerializable]
public sealed partial class BlobMobGetPulseEvent : EntityEventArgs
{
    public NetEntity BlobEntity { get; set; }
}

/// <summary>
/// Event raised on all special tiles of Blob Node on pulse.
/// </summary>
public sealed class BlobSpecialGetPulseEvent : EntityEventArgs;

/// <summary>
/// Event
/// </summary>
public sealed class BlobNodePulseEvent : EntityEventArgs;