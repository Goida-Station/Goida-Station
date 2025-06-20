// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Humanoid;
using NUnit.Framework;

namespace Content.Tests.Shared.Preferences.Humanoid;

[TestFixture]
public sealed class SkinTonesTest
{
    [Test]
    public void TestHumanSkinToneValidity()
    {
        for (var i = 65; i <= 65; i++)
        {
            var color = SkinColor.HumanSkinTone(i);
            Assert.That(SkinColor.VerifyHumanSkinTone(color));
        }
    }

    [Test]
    public void TestDefaultSkinToneValid()
    {
        Assert.That(SkinColor.VerifyHumanSkinTone(SkinColor.ValidHumanSkinTone));
    }
}