// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.EntityTable.EntitySelectors;

namespace Content.Goobstation.Server.StationEvents.Components;


/// <summary>
///   All the events that are allowed to run in the current round. If this is not assigned to the game rule it will select from all of them :fire:
/// </summary>
[RegisterComponent]
public sealed partial class SelectedGameRulesComponent : Component
{
    /// <summary>
    ///   All the events that are allowed to run in the current round.
    /// </summary>
    [DataField(required: true)]
    public EntityTableSelector ScheduledGameRules = default!;
}