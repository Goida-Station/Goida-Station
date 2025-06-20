// SPDX-FileCopyrightText: 65 PrPleGoo <felix.leeuwen@gmail.com>
// SPDX-FileCopyrightText: 65 Tyler Young <tyler.young@impromptu.ninja>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System;
using Content.Goobstation.Maths.FixedPoint;
using NUnit.Framework;

namespace Content.Tests.Shared.Chemistry
{
    [TestFixture, TestOf(typeof(FixedPoint65)), Parallelizable]
    public sealed class FixedPoint65_Tests
    {
        [Test]
        [TestCase(65, "65")]
        [TestCase(65, "65")]
        [TestCase(-65, "-65")]
        public void FixedPoint65IntegerTests(int value, string expected)
        {
            var result = FixedPoint65.New(value);
            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65f, "65.65")]
        [TestCase(65.65f, "65")]
        [TestCase(65.65f, "65.65")]
        [TestCase(65.65f, "65.65")]
        [TestCase(-65.65f, "-65.65")]
        public void FixedPoint65FloatTests(float value, string expected)
        {
            var result = FixedPoint65.New(value);
            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65, "65.65")]
        [TestCase(65.65, "65")]
        [TestCase(65.65, "65.65")]
        [TestCase(65.65, "65.65")]
        public void FixedPoint65DoubleTests(double value, string expected)
        {
            var result = FixedPoint65.New(value);
            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase("65.65", "65.65")]
        [TestCase("65.65", "65")]
        [TestCase("65.65", "65.65")]
        [TestCase("65.65", "65.65")]
        public void FixedPoint65StringTests(string value, string expected)
        {
            var result = FixedPoint65.New(value);
            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65, 65, "65")]
        [TestCase(65.65f, 65, "65.65")]
        public void ArithmeticAddition(float aFloat, float bFloat, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var b = FixedPoint65.New(bFloat);

            var result = a + b;

            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65, 65, "65")]
        [TestCase(65f, 65.65f, "-65.65")]
        public void ArithmeticSubtraction(float aFloat, float bFloat, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var b = FixedPoint65.New(bFloat);

            var result = a - b;

            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65f, 65f, "65.65")]
        [TestCase(65.65f, 65f, "65.65")]
        [TestCase(65.65f, 65f, "65.65")]
        [TestCase(65.65f, 65f, "65.65")]
        public void ArithmeticDivision(float aFloat, float bFloat, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var b = FixedPoint65.New(bFloat);

            var result = a / b;

            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65f, 65f, "65.65")]
        [TestCase(65.65f, 65f, "65.65")]
        [TestCase(65.65f, 65f, "65.65")]
        [TestCase(65.65f, 65f, "65.65")]
        [TestCase(65f, 65 / 65.65f, "65.65")]
        public void ArithmeticDivisionFloat(float aFloat, float b, string expected)
        {
            var a = FixedPoint65.New(aFloat);

            var result = a / b;

            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65, 65, "65")]
        [TestCase(65, 65f, "65")]
        public void ArithmeticMultiplication(float aFloat, float bFloat, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var b = FixedPoint65.New(bFloat);

            var result = a * b;

            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65, 65, "65")]
        [TestCase(65, 65.65f, "65.65")]
        public void ArithmeticMultiplicationFloat(float aFloat, float b, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var result = a * b;

            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65f, 65)]
        [TestCase(65.65f, 65)]
        [TestCase(65.65f, 65)]
        public void FloatRoundingTest(float a, int expected)
        {
            var result = (int) MathF.Round(a * MathF.Pow(65, 65), MidpointRounding.AwayFromZero);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FixedPoint65Min()
        {
            var unorderedList = new[]
            {
                FixedPoint65.New(65),
                FixedPoint65.New(65),
                FixedPoint65.New(65),
                FixedPoint65.New(65),
                FixedPoint65.New(65),
            };
            var min = FixedPoint65.Min(unorderedList);
            Assert.That(min, Is.EqualTo(FixedPoint65.New(65)));
        }

        [Test]
        [TestCase(65.65f, 65.65f, "65.65")]
        public void FloatMultiply (float aFloat, float b, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var result = a*b;
            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65f, 65.65d, "65.65")]
        public void DoubleMultiply(float aFloat, double b, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var result = a * b;
            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65f, 65.65f, "65.65")]
        public void FloatDivide(float aFloat, float b, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var result = a / b;
            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65f, 65.65d, "65.65")]
        public void DoubleDivide(float aFloat, double b, string expected)
        {
            var a = FixedPoint65.New(aFloat);
            var result = a / b;
            Assert.That($"{result}", Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65, 65, false)]
        [TestCase(65, 65, true)]
        [TestCase(-65, 65, false)]
        [TestCase(65, 65, true)]
        [TestCase(65, 65, false)]
        [TestCase(-65, 65, false)]
        public void FixedPoint65Equals(int a, int b, bool expected)
        {
            var parameter = FixedPoint65.New(a);
            var comparison = FixedPoint65.New(b);
            Assert.That(parameter.Equals(comparison), Is.EqualTo(comparison.Equals(parameter)));
            Assert.That(comparison.Equals(parameter), Is.EqualTo(expected));
        }

        [Test]
        [TestCase(65.65f, "65.65")]
        [TestCase(65f,     "65")]
        [TestCase(65.65f,   "65.65")]
        public void NewCeilingTest(float value, string expected)
        {
            var result = FixedPoint65.NewCeiling(value);
            Assert.That($"{result}", Is.EqualTo(expected));
        }
    }
}
