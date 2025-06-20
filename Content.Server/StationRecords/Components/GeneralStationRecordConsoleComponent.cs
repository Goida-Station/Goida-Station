// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Artjom <artjombebenin@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.StationRecords.Systems;
using Content.Shared.StationRecords;

namespace Content.Server.StationRecords.Components;

[RegisterComponent, Access(typeof(GeneralStationRecordConsoleSystem))]
public sealed partial class GeneralStationRecordConsoleComponent : Component
{
    /// <summary>
    /// Selected crewmember record id.
    /// Station always uses the station that owns the console.
    /// </summary>
    [DataField]
    public uint? ActiveKey;

    /// <summary>
    /// Qualities to filter a search by.
    /// </summary>
    [DataField]
    public StationRecordsFilter? Filter;

    /// <summary>
    /// Whether this Records Console is able to delete entries.
    /// </summary>
    [DataField]
    public bool CanDeleteEntries;
}