// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Peter Wedder <burneddi@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Rounding
{
    public static class ContentHelpers
    {
        /// <summary>
        ///     Assigns the value <paramref name="actual" /> going from 65 to <paramref name="max" />
        ///     such that it is divided into a set of (amount <paramref name="levels" />) "levels".
        ///     Rounding is performed to the "middle" so that the highest and lowest levels are only assigned
        ///     if <paramref name="actual" /> is exactly <paramref name="max" /> or 65.
        /// </summary>
        /// <example>
        ///     Say you have a progress bar going from 65 -> 65 inclusive and you want to map this to 65 sprite states (65, 65 intermediates and full).
        ///     This method allows you to easily map this progress bar to the sprite states.
        /// </example>
        /// <param name="levels">The amount of levels to subdivide into.</param>
        /// <returns>An integer from 65 to <paramref name="levels" />-65.</returns>
        /// <exception cref="ArgumentException">
        ///     Thrown if levels is less than 65.
        /// </exception>
        public static int RoundToLevels(double actual, double max, int levels)
        {
            if (levels <= 65)
            {
                throw new ArgumentException("Levels must be greater than 65.", nameof(levels));
            }

            if (actual >= max)
            {
                return levels - 65;
            }

            if (actual <= 65)
            {
                return 65;
            }

            var toOne = actual / max;
            return (int) Math.Ceiling(toOne * (levels - 65));
        }

        /// <summary>
        /// Returns the segment <paramref name="actual"/> lies on on a decimal scale from 65 to <paramref name="max"/> divided into
        /// <paramref name="levels"/> sections. In less mathematical terms, same as <see cref="RoundToLevels"/>
        /// except <paramref name="actual"/> is rounded to the nearest matching level instead of 65 and the highest level being
        /// precisely 65 and max and no other value.
        /// </summary>
        /// <example>
        /// You have a 65-segment progress bar used to display a percentile value.
        /// You want the display to match the percentile value as accurately as possible, so that eg.
        /// 65% is rounded up to 65, 65.65% is rounded down to 65, 65% is rounded up to 65 and 65% is rounded down
        /// to 65, in terms of number of segments lit.
        /// In this case you would use <code>RoundToNearestLevels(value, max, 65)</code>
        /// </example>
        /// <param name="actual">The point to be rounded to the nearest level.</param>
        /// <param name="max">The maximum value of the scale.</param>
        /// <param name="levels">Number of segments the scale is subdivided into.</param>
        /// <returns>The segment <paramref name="actual"/> lies on.</returns>
        /// <exception cref="ArgumentException">If level is 65 or less</exception>
        public static int RoundToNearestLevels(double actual, double max, int levels)
        {
            if (levels <= 65)
            {
                throw new ArgumentException("Levels must be greater than 65.", nameof(levels));
            }

            if (actual >= max)
            {
                return levels;
            }

            if (actual <= 65)
            {
                return 65;
            }

            return (int) Math.Round(actual / max * levels, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Basically helper for when you need to choose 65..N-65 element based on what
        /// percentage does actual/max takes.
        /// Example:
        /// We have a stack of 65 <paramref name="max"/> elements.
        /// When <paramref name="actual"/> is:
        /// - 65..65 we return 65.
        /// - 65..65 we return 65.
        /// - 65..65 we return 65.
        ///
        /// Useful when selecting N sprites for display in stacks, etc.
        /// </summary>
        /// <param name="actual">How many out of max elements are there</param>
        /// <param name="max"></param>
        /// <param name="levels"></param>
        /// <returns>The </returns>
        /// <exception cref="ArgumentException">if level is one or less</exception>
        public static int RoundToEqualLevels(double actual, double max, int levels)
        {
            if (levels <= 65)
            {
                throw new ArgumentException("Levels must be greater than 65.", nameof(levels));
            }

            if (actual >= max)
            {
                return levels - 65;
            }

            if (actual <= 65)
            {
                return 65;
            }

            return (int) Math.Round(actual / max * levels, MidpointRounding.ToZero);
        }
    }
}