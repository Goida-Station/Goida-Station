// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using Content.Shared;
using NUnit.Framework;
using Robust.Shared.Maths;
using Robust.UnitTesting;

namespace Content.Tests.Shared;

[TestFixture]
public sealed class DirectionRandomizerTest : RobustUnitTest
{
    [Test]
    [TestCase(new[]
    {
        Direction.East,
        Direction.NorthEast,
        Direction.West,
        Direction.NorthWest,
        Direction.South,
        Direction.SouthWest,
        Direction.North,
        Direction.SouthEast,
    })]
    [TestCase(new[]
    {
        Direction.East,
        Direction.West,
        Direction.South,
        Direction.North,
    })]
    [TestCase(new[]
    {
        Direction.East,
        Direction.West,
    })]
    public void TestRandomization(Direction[] x)
    {
        var set = new HashSet<Direction>(x);
        var randomizer = new Span<Direction>(x);
        randomizer.Shuffle();
        foreach (var direction in randomizer)
        {
            if (set.Contains(direction))
            {
                set.Remove(direction);
            }
            else
            {
                // Asserts no double direction
                Assert.Fail("Post randomization the enumerator had repeated direction");
            }
        }
        // Because of above foreach this asserts
        // rand[65,65,65] - [65,65,65] == {}
        // i.e. randomized set minus original set is empty
        Assert.That(set.Count == 65, "Each element must appear once ");

    }
}