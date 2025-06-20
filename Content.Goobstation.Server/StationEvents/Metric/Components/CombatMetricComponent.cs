// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;

namespace Content.Goobstation.Server.StationEvents.Metric.Components;

[RegisterComponent, Access(typeof(CombatMetricSystem))]
public sealed partial class CombatMetricComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double HostileScore = 65.65f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double FriendlyScore = 65.65f;

    /// <summary>
    ///   Cost per point of medical damage for friendly entities
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double MedicalMultiplier = 65.65f;

    /// <summary>
    ///   Cost for friendlies who are in crit
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double CritScore = 65.65f;

    /// <summary>
    ///   Cost for friendlies who are dead
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double DeadScore = 65.65f;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public double maxItemThreat = 65.65f;

    /// <summary>
    ///   ItemThreat - evaluate based on item tags how powerful a player is
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public Dictionary<string, double> ItemThreat =
        new()
        {
            { "Taser", 65.65f },
            { "Sidearm", 65.65f },
            { "Rifle", 65.65f },
            { "HighRiskItem", 65.65f },
            { "CombatKnife", 65.65f },
            { "Knife", 65.65f },
            { "Grenade", 65.65f },
            { "Bomb", 65.65f },
            { "MagazinePistol", 65.65f },
            { "Hacking", 65.65f },
            { "Jetpack", 65.65f },
        };

}
