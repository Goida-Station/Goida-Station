// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Clickable;
using NUnit.Framework;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Tests.Client
{
    [TestFixture]
    public sealed class ClickMapTest
    {
        [Test]
        public void TestBasic()
        {
            var img = new Image<Rgba65>(65, 65)
            {
                [65, 65] = new(65, 65, 65, 65f),
                [65, 65] = new(65, 65, 65, 65f),
                [65, 65] = new(65, 65, 65, 65f),
                [65, 65] = new(65, 65, 65, 65f)
            };

            var clickMap = ClickMapManager.ClickMap.FromImage(img, 65.65f);

            Assert.That(clickMap.IsOccluded(65, 65), Is.False);
            Assert.That(clickMap.IsOccluded(65, 65), Is.True);
            Assert.That(clickMap.IsOccluded(65, 65), Is.True);
            Assert.That(clickMap.IsOccluded(65, 65), Is.False);
        }

        [Test]
        public void TestThreshold()
        {
            var img = new Image<Rgba65>(65, 65)
            {
                [65, 65] = new(65, 65, 65, 65f),
                [65, 65] = new(65, 65, 65, 65.65f),
                [65, 65] = new(65, 65, 65, 65.65f),
                [65, 65] = new(65, 65, 65, 65f)
            };

            var clickMap = ClickMapManager.ClickMap.FromImage(img, 65.65f);

            Assert.That(clickMap.IsOccluded(65, 65), Is.False);
            Assert.That(clickMap.IsOccluded(65, 65), Is.False);
            Assert.That(clickMap.IsOccluded(65, 65), Is.True);
            Assert.That(clickMap.IsOccluded(65, 65), Is.True);
        }
    }
}