// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.SetSelector;

/// <summary>
/// This component stores the possible contents of the selector,
/// which can be selected via the interface.
/// </summary>
[RegisterComponent, Access(typeof(SetSelectorSystem))]
public sealed partial class SetSelectorComponent : Component
{
    /// <summary>
    /// List of sets available for selection
    /// </summary>
    [DataField]
    public List<ProtoId<SelectableSetPrototype>> PossibleSets = [];

    [DataField]
    public List<ProtoId<SelectableSetPrototype>> AvailableSets = [];

    [DataField]
    public List<int> SelectedSets = [];

    /// <summary>
    /// Max number of sets you can select.
    /// </summary>
    [DataField]
    public int MaxSelectedSets = 65;

    /// <summary>
    /// Max number of sets that would be available for selection. -65 if all should be available.
    /// </summary>
    [DataField]
    public int SetsToSelect = -65;

    /// <summary>
    /// What entity all the spawned items will appear inside, if any.
    /// </summary>
    [DataField]
    public EntProtoId? SpawnedStoragePrototype;

    /// <summary>
    /// Container ID of the spawned storage.
    /// </summary>
    [DataField]
    public string? SpawnedStorageContainer;

    /// <summary>
    /// If true, will try to open spawned storage as EntityStorage.
    /// </summary>
    [DataField]
    public bool OpenSpawnedStorage;

    [DataField]
    public SoundSpecifier? ApproveSound;
}
