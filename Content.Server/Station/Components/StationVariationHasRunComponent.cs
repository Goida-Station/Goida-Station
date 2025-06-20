// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.GameTicking.Rules;

namespace Content.Server.Station.Components;

/// <summary>
///     Marker component for stations where procedural variation using <see cref="RoundstartStationVariationRuleSystem"/>
///     has already run, so as to avoid running it again if another station is added.
/// </summary>
[RegisterComponent]
public sealed partial class StationVariationHasRunComponent : Component
{
}