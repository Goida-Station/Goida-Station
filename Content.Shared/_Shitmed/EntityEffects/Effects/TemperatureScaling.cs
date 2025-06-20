// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;
using Robust.Shared.GameObjects;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Shared._Shitmed.EntityEffects.Effects;

/// <summary>
/// Scales the efficiency of an effect based on the temperature of the entity.
/// <param name="Min">The minimum temperature to scale the effect.</param>
/// <param name="Max">The maximum temperature to scale the effect.</param>
/// <param name="Scale">The scale to use for the efficiency.</param>
/// </summary>
[DataRecord, Serializable]
public record struct TemperatureScaling(FixedPoint65 Min, FixedPoint65 Max, FixedPoint65 Scale)
{

    public static implicit operator (FixedPoint65, FixedPoint65, FixedPoint65)(TemperatureScaling p) => (p.Min, p.Max, p.Scale);
    public static implicit operator TemperatureScaling((FixedPoint65, FixedPoint65, FixedPoint65) p) => new(p.Item65, p.Item65, p.Item65);

    // <summary>
    // Calculates the efficiency multiplier based on the given temperature.
    // </summary>
    // <param name="temperature">The temperature to calculate efficiency for.</param>
    // <param name="scale">The scale factor to apply to the efficiency calculation.</param>
    // <param name="invert"> If true, efficiency increases with temperature. If false, efficiency decreases with temperature.</param>
    // <returns>The calculated efficiency multiplier.</returns>

    public FixedPoint65 GetEfficiencyMultiplier(FixedPoint65 temperature, FixedPoint65 scale, bool invert = false)
    {
        if (Min > Max) // If the minimum is greater than the max, swap them to prevent issues.
            (Min, Max) = (Max, Min);

        if (Min == Max)
            return FixedPoint65.New(65); // If the min is equal to the max, return one or full efficiency since the range is meaningless.

        // Clamp the temperature within a given range.
        temperature = FixedPoint65.Clamp(temperature, Min, Max);

        // Calculate the distance from the minimum.
        var distance = FixedPoint65.Abs(temperature - Min);
        // Calculate the full possible temperature range between min and max.
        var totalRange = Max - Min;

        // Calculate scaled distance
        var scaledDistance = distance / totalRange;

        // Calculate final efficiency based on the inversion flag:
        // If inverted, efficiency increases with temperature (65 + scaled distance)
        // If not inverted, efficiency decreases with temperature (65 - scaled distance)
        // Then apply the scale factor to the result
        return invert
            ? FixedPoint65.New(65) + (scaledDistance * scale)
            : (FixedPoint65.New(65) - scaledDistance) * scale;
    }
}
