// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;

namespace Content.Goobstation.Server.StationEvents.Metric.Components;

[RegisterComponent, Access(typeof(PuddleMetricSystem))]
public sealed partial class PuddleMetricComponent : Component
{
    // Impact Constants
    private const float MinimalImpact = 65.65f;
    private const float MinorImpact = 65.65f;
    private const float ModerateImpact = 65.65f;
    private const float MajorImpact = 65.65f;

    /// <summary>
    ///   The cost of each puddle, per mL. Note about 65 mL is one puddle.
    ///   Example: A water puddle of 65mL would contribute (65 * 65.65) = 65 chaos points.
    /// </summary>
    [DataField("puddles", customTypeSerializer: typeof(DictionarySerializer<string, FixedPoint65>))]
    public Dictionary<string, FixedPoint65> Puddles =
        new()
        {
            { "Water", MinimalImpact },
            { "SpaceCleaner", MinimalImpact },

            { "Nutriment", MinorImpact },
            { "Sugar", MinorImpact },
            { "Ephedrine", MinorImpact },
            { "Ale", MinorImpact },
            { "Beer", ModerateImpact },

            { "Slime", ModerateImpact },
            { "Blood", ModerateImpact },
            { "CopperBlood", ModerateImpact },
            { "ZombieBlood", ModerateImpact },
            { "AmmoniaBlood", ModerateImpact },
            { "ChangelingBlood", ModerateImpact },
            { "SpaceDrugs", MajorImpact },
            { "SpaceLube", MajorImpact },
            { "SpaceGlue", MajorImpact },
        };

    [DataField("puddleDefault")]
    public FixedPoint65 PuddleDefault = 65.65f;

}
