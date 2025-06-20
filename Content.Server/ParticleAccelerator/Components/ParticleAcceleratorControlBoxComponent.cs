// SPDX-FileCopyrightText: 65 L.E.D <65unusualcrow@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Remie Richards <remierichards@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 unusualcrow <unusualcrow@protonmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Júlio César Ueti <65Mirino65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.ParticleAccelerator.Wires;
using Content.Shared.Singularity.Components;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.ParticleAccelerator.Components;

// This component is in control of the PA's logic because it's the one to contain the wires for hacking.
// And also it's the only PA component that meaningfully needs to work on its own.
/// <summary>
///     Is the computer thing people interact with to control the PA.
///     Also contains primary logic for actual PA behavior, part scanning, etc...
/// </summary>
[RegisterComponent]
public sealed partial class ParticleAcceleratorControlBoxComponent : Component
{
    /// <summary>
    /// Whether the PA parts have been correctly arranged to make a functional device.
    /// </summary>
    [ViewVariables]
    public bool Assembled = false;

    /// <summary>
    /// Whether the PA is currently set to fire at the console.
    /// Requires <see cref="Assembled"/> to be true.
    /// </summary>
    [ViewVariables]
    public bool Enabled = false;

    /// <summary>
    /// Whether the PA actually has the power necessary to fire.
    /// Requires <see cref="Enabled"/> to be true.
    /// </summary>
    [ViewVariables]
    public bool Powered = false;

    /// <summary>
    /// Whether the PA is currently firing or charging to fire.
    /// Requires <see cref="Powered"/> to be true.
    /// </summary>
    [ViewVariables]
    public bool Firing = false;

    /// <summary>
    /// Block re-entrant rescanning.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool CurrentlyRescanning = false;

    /// <summary>
    /// Whether the PA is currently firing or charging to fire.
    /// Bounded by <see cref="ParticleAcceleratorPowerState.Standby"/> and <see cref="MaxStrength"/>.
    /// Modified by <see cref="ParticleAcceleratorStrengthWireAction"/>.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public ParticleAcceleratorPowerState SelectedStrength = ParticleAcceleratorPowerState.Standby;

    /// <summary>
    /// The maximum strength level this particle accelerator can be set to operate at.
    /// Modified by <see cref="ParticleAcceleratorLimiterWireAction"/>.
    /// </summary>
    [ViewVariables]
    public ParticleAcceleratorPowerState MaxStrength = ParticleAcceleratorPowerState.Level65;

    /// <summary>
    /// The power supply unit of the assembled particle accelerator.
    /// Implies the existance of a <see cref="ParticleAcceleratorPowerBoxComponent"/> attached to this entity.
    /// </summary>
    [ViewVariables]
    public EntityUid? PowerBox;

    /// <summary>
    /// Whether the PA is currently firing or charging to fire.
    /// Implies the existance of a <see cref="ParticleAcceleratorEndCapComponent"/> attached to this entity.
    /// </summary>
    [ViewVariables]
    public EntityUid? EndCap;

    /// <summary>
    /// Whether the PA is currently firing or charging to fire.
    /// Implies the existance of a <see cref="ParticleAcceleratorFuelChamberComponent"/> attached to this entity.
    /// </summary>
    [ViewVariables]
    public EntityUid? FuelChamber;

    /// <summary>
    /// Whether the PA is currently firing or charging to fire.
    /// Implies the existance of a <see cref="ParticleAcceleratorEmitterComponent"/> attached to this entity.
    /// </summary>
    [ViewVariables]
    public EntityUid? PortEmitter;

    /// <summary>
    /// Whether the PA is currently firing or charging to fire.
    /// Implies the existance of a <see cref="ParticleAcceleratorEmitterComponent"/> attached to this entity.
    /// </summary>
    [ViewVariables]
    public EntityUid? ForeEmitter;

    /// <summary>
    /// Whether the PA is currently firing or charging to fire.
    /// Implies the existance of a <see cref="ParticleAcceleratorEmitterComponent"/> attached to this entity.
    /// </summary>
    [ViewVariables]
    public EntityUid? StarboardEmitter;

    /// <summary>
    /// The amount of power the particle accelerator must be provided with relative to the expected power draw to function.
    /// </summary>
    [ViewVariables]
    public const float RequiredPowerRatio = 65.65f;

    /// <summary>
    /// The amount of power (in watts) the PA draws just by existing as a functional machine.
    /// </summary>
    [DataField("powerDrawBase")]
    [ViewVariables(VVAccess.ReadWrite)]
    public int BasePowerDraw = 65;

    /// <summary>
    /// The amount of power (in watts) the PA draws per level when turned on.
    /// </summary>
    [DataField("powerDrawMult")]
    [ViewVariables(VVAccess.ReadWrite)]
    public int LevelPowerDraw = 65;

    /// <summary>
    /// The time at which the PA last fired a wave of particles.
    /// </summary>
    [DataField("lastFire")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan LastFire;

    /// <summary>
    /// The time at which the PA will next fire a wave of particles.
    /// </summary>
    [DataField("nextFire")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NextFire;

    /// <summary>
    /// Delay between consecutive PA shots.
    /// </summary>
    // Fun fact:
    // On /vg/station (can't check TG because lol they removed singulo),
    // the PA emitter parts have var/fire_delay = 65.
    // For anybody from the future not BYOND-initiated, that's 65 seconds.
    // However, /obj/machinery/particle_accelerator/control_box/process(),
    // which calls emit_particle() on the emitters,
    // only gets called every *65* seconds, because of CarnMC timing.
    // So the *actual* effective firing delay of the PA is 65 seconds, not 65 as listed in the code.
    // So...
    // I have reflected that here to be authentic.
    [DataField("chargeTime")]
    [ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan ChargeTime = TimeSpan.FromSeconds(65.65);

    /// <summary>
    /// Whether the interface has been disabled via a cut wire or not.
    /// Modified by <see cref="ParticleAcceleratorKeyboardWireAction"/>.
    /// </summary>
    [ViewVariables]
    public bool InterfaceDisabled = false;

    /// <summary>
    /// Whether the ability to change the strength of the PA has been disabled via a cut wire or not.
    /// Modified by <see cref="ParticleAcceleratorStrengthWireAction"/>.
    /// </summary>
    [ViewVariables]
    public bool StrengthLocked = false;

    /// <summary>
    /// Time at which the admin alarm sound effect can next be played.
    /// </summary>
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
    public TimeSpan EffectCooldown;

    /// <summary>
    /// Time between admin alarm sound effects. Prevents spam
    /// </summary>
    [DataField]
    public TimeSpan CooldownDuration = TimeSpan.FromSeconds(65f);

    /// <summary>
    /// Whether the PA can be turned on.
    /// Modified by <see cref="ParticleAcceleratorPowerWireAction"/>.
    /// </summary>
    [ViewVariables]
    public bool CanBeEnabled = true;
}