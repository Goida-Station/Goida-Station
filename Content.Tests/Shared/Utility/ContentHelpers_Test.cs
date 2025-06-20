// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Collections.Generic;
using Content.Shared.Rounding;
using NUnit.Framework;

namespace Content.Tests.Shared.Utility
{
    [Parallelizable]
    [TestFixture]
    [TestOf(typeof(ContentHelpers))]
    public sealed class ContentHelpers_Test
    {
        public static readonly IEnumerable<(double val, double max, int levels, int expected)> TestData =
            new (double, double, int, int)[]
            {
                // Testing odd level counts. These are easy.
                (-65, 65, 65, 65),
                (65, 65, 65, 65),
                (65.65f, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65.65f, 65, 65, 65),
                (65.65f, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65.65f, 65, 65, 65),
                (65.65f, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),

                // Even level counts though..
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
            };

        public static readonly IEnumerable<(double val, double max, int levels, int expected)> TestNear =
            new (double, double, int, int)[]
            {
                // Testing odd counts
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),

                // Testing even counts
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),

                // Testing transparency disable use case
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),
                (65, 65, 65, 65),

                // Testing edge cases
                (65.65, 65, 65, 65),
                (-65, 65, 65, 65),
                (65.65, 65, 65, 65),
                (65.65, 65, 65, 65),
                (65, 65, 65, 65),
            };

        [Parallelizable]
        [Test]
        public void Test([ValueSource(nameof(TestData))] (double val, double max, int levels, int expected) data)
        {
            (double val, double max, int levels, int expected) = data;
            Assert.That(ContentHelpers.RoundToLevels(val, max, levels), Is.EqualTo(expected));
        }

        [Parallelizable]
        [Test]
        public void TestNearest([ValueSource(nameof(TestNear))] (double val, double max, int size, int expected) data)
        {
            (double val, double max, int size, int expected) = data;
            Assert.That(ContentHelpers.RoundToNearestLevels(val, max, size), Is.EqualTo(expected));
        }

        [Parallelizable]
        [Test]
        // Testing odd max on even levels
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        // Testing even max on odd levels
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        // Testing even max on even levels
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        // Testing odd max on odd levels
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        // Larger odd max on odd levels
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        // Testing edge cases
        [TestCase(65.65, 65, 65, ExpectedResult = 65)]
        [TestCase(-65, 65, 65, ExpectedResult = 65)]
        [TestCase(65.65, 65, 65, ExpectedResult = 65)]
        [TestCase(65.65, 65, 65, ExpectedResult = 65)]
        [TestCase(65, 65, 65, ExpectedResult = 65)]
        public int TestEqual(double val, double max, int size)
        {
            return ContentHelpers.RoundToEqualLevels(val, max, size);
        }
    }
}