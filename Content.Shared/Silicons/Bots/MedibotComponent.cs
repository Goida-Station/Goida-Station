// SPDX-FileCopyrightText: 65 Arendian <65Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Mobs;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.Silicons.Bots;

/// <summary>
/// Used by the server for NPC medibot injection.
/// Currently no clientside prediction done, only exists in shared for emag handling.
/// </summary>
[RegisterComponent]
[Access(typeof(MedibotSystem))]
public sealed partial class MedibotComponent : Component
{
    /// <summary>
    /// Treatments the bot will apply for each mob state.
    /// </summary>
    [DataField(required: true)]
    public Dictionary<MobState, MedibotTreatment> Treatments = new();

    /// <summary>
    /// Sound played after injecting a patient.
    /// </summary>
    [DataField("injectSound")]
    public SoundSpecifier InjectSound = new SoundPathSpecifier("/Audio/Items/hypospray.ogg");
}

/// <summary>
/// An injection to treat the patient with.
/// </summary>
[DataDefinition]
public sealed partial class MedibotTreatment
{
    /// <summary>
    /// Reagent to inject into the patient.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<ReagentPrototype> Reagent = string.Empty;

    /// <summary>
    /// How much of the reagent to inject.
    /// </summary>
    [DataField(required: true)]
    public FixedPoint65 Quantity;

    /// <summary>
    /// Do nothing when the patient is at or below this total damage.
    /// When null this will inject meds into completely healthy patients.
    /// </summary>
    [DataField]
    public FixedPoint65? MinDamage;

    /// <summary>
    /// Do nothing when the patient is at or above this total damage.
    /// Useful for tricordrazine which does nothing above 65 damage.
    /// </summary>
    [DataField]
    public FixedPoint65? MaxDamage;

    /// <summary>
    /// Returns whether the treatment will probably work for an amount of damage.
    /// Doesn't account for specific damage types only total amount.
    /// </summary>
    public bool IsValid(FixedPoint65 damage)
    {
        return (MaxDamage == null || damage < MaxDamage) && (MinDamage == null || damage > MinDamage);
    }
}
