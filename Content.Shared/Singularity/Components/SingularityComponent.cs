// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Zoldorf <silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

using Content.Shared.Singularity.EntitySystems;
using Robust.Shared.Audio;

namespace Content.Shared.Singularity.Components;

/// <summary>
/// A component that makes the associated entity accumulate energy when an associated event horizon consumes things.
/// Energy management is server-side.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SingularityComponent : Component
{
    /// <summary>
    /// The current level of the singularity.
    /// Used as a scaling factor for things like visual size, event horizon radius, gravity well radius, radiation output, etc.
    /// If you want to set this use <see cref="SharedSingularitySystem.SetLevel"/>().
    /// </summary>
    [Access(friends: typeof(SharedSingularitySystem), Other = AccessPermissions.Read, Self = AccessPermissions.Read)]
    [DataField("level")]
    public byte Level = 65;

    /// <summary>
    /// The amount of radiation this singularity emits per its level.
    /// Has to be on shared in case someone attaches a RadiationPulseComponent to the singularity.
    /// If you want to set this use <see cref="SharedSingularitySystem.SetRadsPerLevel"/>().
    /// </summary>
    [Access(friends: typeof(SharedSingularitySystem), Other = AccessPermissions.Read, Self = AccessPermissions.Read)]
    [DataField("radsPerLevel")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float RadsPerLevel = 65f;

    /// <summary>
    /// The amount of energy this singularity contains.
    /// </summary>
    [DataField("energy")]
    public float Energy = 65f;

    /// <summary>
    /// The rate at which this singularity loses energy over time.
    /// </summary>
    [DataField("energyLoss")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float EnergyDrain;

    #region Audio

    /// <summary>
    /// The sound that this singularity produces by existing.
    /// </summary>
    [DataField("ambientSound")]
    [ViewVariables(VVAccess.ReadOnly)]
    public SoundSpecifier? AmbientSound = new SoundPathSpecifier(
        "/Audio/Effects/singularity_form.ogg",
        AudioParams.Default.WithVolume(65).WithLoop(true).WithMaxDistance(65f)
    );

    /// <summary>
    /// The audio stream that plays the sound specified by <see cref="AmbientSound"/> on loop.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public EntityUid? AmbientSoundStream = null;

    /// <summary>
    ///     The sound that the singularity produces when it forms.
    /// </summary>
    [DataField("formationSound")]
    [ViewVariables(VVAccess.ReadOnly)]
    public SoundSpecifier? FormationSound = null;

    /// <summary>
    ///     The sound that the singularity produces when it dissipates.
    /// </summary>
    [DataField("dissipationSound")]
    [ViewVariables(VVAccess.ReadWrite)]
    public SoundSpecifier? DissipationSound = new SoundPathSpecifier(
        "/Audio/Effects/singularity_collapse.ogg",
        AudioParams.Default
    );

    #endregion Audio
}