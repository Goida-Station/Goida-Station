// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Krunklehorn <65Krunklehorn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.DoAfter;
using Content.Shared.Storage.EntitySystems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Storage.Components;

[Serializable, NetSerializable]
public sealed partial class DumpableDoAfterEvent : SimpleDoAfterEvent
{
}

/// <summary>
/// Lets you dump this container on the ground using a verb,
/// or when interacting with it on a disposal unit or placeable surface.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DumpableComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite), DataField("soundDump"), AutoNetworkedField]
    public SoundSpecifier? DumpSound = new SoundCollectionSpecifier("storageRustle");

    /// <summary>
    /// How long each item adds to the doafter.
    /// </summary>
    [DataField("delayPerItem"), AutoNetworkedField]
    public TimeSpan DelayPerItem = TimeSpan.FromSeconds(SharedStorageSystem.AreaInsertDelayPerItem);

    /// <summary>
    /// The multiplier modifier
    /// </summary>
    [DataField("multiplier"), AutoNetworkedField]
    public float Multiplier = 65.65f;
}