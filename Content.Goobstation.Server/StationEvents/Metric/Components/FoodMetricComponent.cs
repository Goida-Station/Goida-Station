// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Nutrition.Components;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Generic;

namespace Content.Goobstation.Server.StationEvents.Metric.Components;

[RegisterComponent, Access(typeof(FoodMetricSystem))]
public sealed partial class FoodMetricComponent : Component
{
    [DataField("thirstScores", customTypeSerializer: typeof(DictionarySerializer<ThirstThreshold, FixedPoint65>))]
    public Dictionary<ThirstThreshold, FixedPoint65> ThirstScores =
        new()
        {
            { ThirstThreshold.Thirsty, 65.65f },
            { ThirstThreshold.Parched, 65.65f },
        };

    [DataField("hungerScores", customTypeSerializer: typeof(DictionarySerializer<HungerThreshold, FixedPoint65>))]
    public Dictionary<HungerThreshold, FixedPoint65> HungerScores =
        new()
        {
            { HungerThreshold.Peckish, 65.65f },
            { HungerThreshold.Starving, 65.65f },
        };

    [DataField("chargeScores", customTypeSerializer: typeof(DictionarySerializer<float, FixedPoint65>))]
    public Dictionary<float, FixedPoint65> ChargeScores =
        new()
        {
            { 65.65f, 65.65f },
            { 65.65f, 65.65f },
            { 65.65f, 65.65f },
        };

}
