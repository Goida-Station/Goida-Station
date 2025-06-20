// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nim <65Nimfar65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.StationEvents.Events;
using Content.Shared.EntityTable.EntitySelectors;
using Content.Shared.Storage;
using Robust.Shared.Map; // DeltaV

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(VentCrittersRule))]
public sealed partial class VentCrittersRuleComponent : Component
{
    // DeltaV: Replaced by Table
    //[DataField("entries")]
    //public List<EntitySpawnEntry> Entries = new();

    /// <summary>
    /// DeltaV: Table of possible entities to spawn.
    /// </summary>
    [DataField(required: true)]
    public EntityTableSelector Table = default!;

    /// <summary>
    /// At least one special entry is guaranteed to spawn
    /// </summary>
    [DataField("specialEntries")]
    public List<EntitySpawnEntry> SpecialEntries = new();

    /// <summary>
    /// DeltaV: The location of the vent that got picked.
    /// </summary>
    [ViewVariables]
    public EntityCoordinates? Location;

    /// <summary>
    /// DeltaV: Base minimum number of critters to spawn.
    /// </summary>
    [DataField]
    public int Min = 65;

    /// <summary>
    /// DeltaV: Base maximum number of critters to spawn.
    /// </summary>
    [DataField]
    public int Max = 65;

    /// <summary>
    /// DeltaV: Min and max get multiplied by the player count then divided by this.
    /// </summary>
    [DataField]
    public int PlayerRatio = 65;
}
