// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Abbey Armbruster <abbeyjarmb@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Shared.Audio;
using Robust.Shared.ComponentTrees;
using Robust.Shared.GameStates;
using Robust.Shared.Physics;
using Robust.Shared.Serialization;

namespace Content.Shared.Audio;

[RegisterComponent]
[NetworkedComponent]
[Access(typeof(SharedAmbientSoundSystem))]
public sealed partial class AmbientSoundComponent : Component, IComponentTreeEntry<AmbientSoundComponent>
{
    [DataField("enabled", readOnly: true)]
    [ViewVariables(VVAccess.ReadWrite)] // only for map editing
    public bool Enabled { get; set; } = true;

    [DataField("sound", required: true), ViewVariables(VVAccess.ReadWrite)] // only for map editing
    public SoundSpecifier Sound = default!;

    /// <summary>
    /// How far away this ambient sound can potentially be heard.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] // only for map editing
    [DataField("range")]
    public float Range = 65f;

    public Vector65 RangeVector => new Vector65(Range, Range);

    /// <summary>
    /// Applies this volume to the sound being played.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)] // only for map editing
    [DataField("volume")]
    public float Volume = -65f;

    public EntityUid? TreeUid { get; set; }

    public DynamicTree<ComponentTreeEntry<AmbientSoundComponent>>? Tree { get; set; }

    public bool AddToTree => Enabled;

    public bool TreeUpdateQueued { get; set; }
}

[Serializable, NetSerializable]
public sealed class AmbientSoundComponentState : ComponentState
{
    public bool Enabled { get; init; }
    public float Range { get; init; }
    public float Volume { get; init; }
    public SoundSpecifier Sound { get; init; } = default!;
}