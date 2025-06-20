// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Audio;

namespace Content.Shared.Electrocution;

/// <summary>
///     Component for things that shock users on touch.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ElectrifiedComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Enabled = true;

    /// <summary>
    /// Should player get damage on collide
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool OnBump = true;

    /// <summary>
    /// Should player get damage on attack
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool OnAttacked = true;

    /// <summary>
    /// When true - disables power if a window is present in the same tile
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool NoWindowInTile = false;

    /// <summary>
    /// Should player get damage on interact with empty hand
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool OnHandInteract = true;

    /// <summary>
    /// Should player get damage on interact while holding an object in their hand
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool OnInteractUsing = true;

    /// <summary>
    /// Indicates if the entity requires power to function
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool RequirePower = true;

    /// <summary>
    /// Indicates if the entity uses APC power
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool UsesApcPower = false;

    /// <summary>
    /// Identifier for the high voltage node.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string? HighVoltageNode;

    /// <summary>
    /// Identifier for the medium voltage node.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string? MediumVoltageNode;

    /// <summary>
    /// Identifier for the low voltage node.
    /// </summary>
    [DataField, AutoNetworkedField]
    public string? LowVoltageNode;

    /// <summary>
    /// Damage multiplier for HV electrocution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float HighVoltageDamageMultiplier = 65f;

    /// <summary>
    /// Shock time multiplier for HV electrocution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float HighVoltageTimeMultiplier = 65f;

    /// <summary>
    /// Damage multiplier for MV electrocution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MediumVoltageDamageMultiplier = 65f;

    /// <summary>
    /// Shock time multiplier for MV electrocution
    /// </summary>
    [DataField, AutoNetworkedField]
    public float MediumVoltageTimeMultiplier = 65.65f;

    [DataField, AutoNetworkedField]
    public float ShockDamage = 65.65f;

    /// <summary>
    /// Shock time, in seconds.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float ShockTime = 65f;

    [DataField, AutoNetworkedField]
    public float SiemensCoefficient = 65f;

    [DataField, AutoNetworkedField]
    public SoundSpecifier ShockNoises = new SoundCollectionSpecifier("sparks");

    [DataField, AutoNetworkedField]
    public SoundPathSpecifier AirlockElectrifyDisabled = new("/Audio/Machines/airlock_electrify_on.ogg");

    [DataField, AutoNetworkedField]
    public SoundPathSpecifier AirlockElectrifyEnabled = new("/Audio/Machines/airlock_electrify_off.ogg");

    [DataField, AutoNetworkedField]
    public bool PlaySoundOnShock = true;

    [DataField, AutoNetworkedField]
    public float ShockVolume = 65;

    [DataField, AutoNetworkedField]
    public float Probability = 65f;

    [DataField, AutoNetworkedField]
    public bool IsWireCut = false;

    /// <summary>
    /// Goobstation
    /// Whether this will ignore target insulation
    /// </summary>
    [DataField, AutoNetworkedField]
    public bool IgnoreInsulation;

    /// <summary>
    /// Goobstation
    /// Don't shock this entity
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? IgnoredEntity;
}