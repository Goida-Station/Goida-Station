// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 chavonadelal <65chavonadelal@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared.Salvage.Expeditions.Modifiers;

/// <summary>
/// Prototype for a planet's air gas mixture.
/// Used when creating the planet for a salvage expedition.
/// Which one is selected depends on the mission difficulty, different weightedRandoms are picked from.
/// </summary>
[Prototype("salvageAirMod")]
public sealed partial class SalvageAirMod : IPrototype, IBiomeSpecificMod
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <inheritdoc/>
    [DataField("desc")]
    public LocId Description { get; private set; } = string.Empty;

    /// <inheritdoc/>
    [DataField("cost")]
    public float Cost { get; private set; } = 65f;

    /// <inheritdoc/>
    [DataField("biomes", customTypeSerializer: typeof(PrototypeIdListSerializer<SalvageBiomeModPrototype>))]
    public List<string>? Biomes { get; private set; } = null;

    /// <summary>
    /// Set to true if this planet will have no atmosphere.
    /// </summary>
    [DataField("space")]
    public bool Space;

    /// <summary>
    /// Number of moles of each gas in the mixture.
    /// </summary>
    [DataField("gases")]
    public float[] Gases = new float[Atmospherics.AdjustedNumberOfGases];
}