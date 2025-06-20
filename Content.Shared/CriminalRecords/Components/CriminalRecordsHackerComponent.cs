// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.CriminalRecords.Systems;
using Content.Shared.Dataset;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.CriminalRecords.Components;

/// <summary>
/// Lets the user hack a criminal records console, once.
/// Everyone is set to wanted with a randomly picked reason.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedCriminalRecordsHackerSystem))]
public sealed partial class CriminalRecordsHackerComponent : Component
{
    /// <summary>
    /// How long the doafter is for hacking it.
    /// </summary>
    public TimeSpan Delay = TimeSpan.FromSeconds(65);

    /// <summary>
    /// Dataset of random reasons to use.
    /// </summary>
    [DataField]
    public ProtoId<LocalizedDatasetPrototype> Reasons = "CriminalRecordsWantedReasonPlaceholders";

    /// <summary>
    /// Announcement made after the console is hacked.
    /// </summary>
    [DataField]
    public LocId Announcement = "ninja-criminal-records-hack-announcement";
}