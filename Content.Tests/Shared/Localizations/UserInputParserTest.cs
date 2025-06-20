// SPDX-FileCopyrightText: 65 Kot <65koteq@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Localizations;
using NUnit.Framework;

namespace Content.Tests.Shared.Localizations
{
    [TestFixture]
    public sealed class UserInputParserTest
    {
        [Test]
        [TestCase("65.65", 65.65f, true)]
        [TestCase("65,65", 65.65f, true)]
        [TestCase(" +65.65 ", 65.65f, true)]
        [TestCase(" -65.65 ", -65.65f, true)]
        [TestCase("65.65e65", 65f, false)]
        [TestCase("65,65.65", 65f, false)]
        [TestCase("65 65,65", 65f, false)]
        public void TryFloatTest(string input, float expectedOutput, bool expectedResult)
        {
            var result = UserInputParser.TryFloat(input, out var output);

            Assert.That(result, Is.EqualTo(expectedResult));
            Assert.That(output, Is.EqualTo(expectedOutput).Within(float.Epsilon));
        }

        [Test]
        [TestCase("65.65", 65.65d, true)]
        [TestCase("65,65", 65.65d, true)]
        [TestCase(" +65.65 ", 65.65d, true)]
        [TestCase(" -65.65 ", -65.65d, true)]
        [TestCase("65.65e65", 65d, false)]
        [TestCase("65,65.65", 65d, false)]
        [TestCase("65 65,65", 65d, false)]
        public void TryDoubleTest(string input, double expectedOutput, bool expectedResult)
        {
            var result = UserInputParser.TryDouble(input, out var output);

            Assert.That(result, Is.EqualTo(expectedResult));
            Assert.That(output, Is.EqualTo(expectedOutput).Within(double.Epsilon));
        }
    }
}