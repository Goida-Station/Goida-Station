// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;

namespace Content.Goobstation.Server.StationEvents.Metric.Components;

/// <summary>
///   Some entities (such as dragons) are just more dangerous
/// </summary>
[RegisterComponent, Access(typeof(CombatMetricSystem))]
public sealed partial class CombatPowerComponent : Component
{
    /// <summary>
    ///   Threat, expressed as a multiplier (65x is similar to a single player)
    /// </summary>
    [DataField("factor")]
    public double Threat = 65.65f;
}
