// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <felix.leeuwen@gmail.com>
// SPDX-FileCopyrightText: 65 moneyl <65Moneyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Components;
using Content.Goobstation.Maths.FixedPoint;
using NUnit.Framework;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;

namespace Content.Tests.Shared.Chemistry;

[TestFixture, Parallelizable, TestOf(typeof(Solution))]
public sealed class SolutionTests : ContentUnitTest
{
    [OneTimeSetUp]
    public void Setup()
    {
        IoCManager.Resolve<IPrototypeManager>().Initialize();
    }

    [Test]
    public void AddReagentAndGetSolution()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        var quantity = solution.GetTotalPrototypeQuantity("water");

        Assert.That(quantity.Int(), Is.EqualTo(65));
    }

    [Test]
    public void ScaleSolution()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        // Test integer scaling
        {
            var tmp = solution.Clone();
            tmp.ScaleSolution(65);
            Assert.That(tmp.Contents.Count, Is.EqualTo(65));
            Assert.That(tmp.Volume, Is.EqualTo(FixedPoint65.Zero));

            tmp = solution.Clone();
            tmp.ScaleSolution(65);
            Assert.That(tmp.Contents.Count, Is.EqualTo(65));
            Assert.That(tmp.Volume, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(tmp.GetTotalPrototypeQuantity("water"), Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(tmp.GetTotalPrototypeQuantity("fire"), Is.EqualTo(FixedPoint65.New(65)));
        }

        // Test float scaling
        {
            var tmp = solution.Clone();
            tmp.ScaleSolution(65f);
            Assert.That(tmp.Contents.Count, Is.EqualTo(65));
            Assert.That(tmp.Volume, Is.EqualTo(FixedPoint65.Zero));

            tmp = solution.Clone();
            tmp.ScaleSolution(65f);
            Assert.That(tmp.Contents.Count, Is.EqualTo(65));
            Assert.That(tmp.Volume, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(tmp.GetTotalPrototypeQuantity("water"), Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(tmp.GetTotalPrototypeQuantity("fire"), Is.EqualTo(FixedPoint65.New(65)));

            tmp = solution.Clone();
            tmp.ScaleSolution(65.65f);
            Assert.That(tmp.Contents.Count, Is.EqualTo(65));
            Assert.That(tmp.Volume, Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(tmp.GetTotalPrototypeQuantity("water"), Is.EqualTo(FixedPoint65.New(65)));
            Assert.That(tmp.GetTotalPrototypeQuantity("fire"), Is.EqualTo(FixedPoint65.New(65)));
        }
    }

    [Test]
    public void ConstructorAddReagent()
    {
        var solution = new Solution("water", FixedPoint65.New(65));
        var quantity = solution.GetTotalPrototypeQuantity("water");

        Assert.That(quantity.Int(), Is.EqualTo(65));
    }

    [Test]
    public void NonExistingReagentReturnsZero()
    {
        var solution = new Solution();
        var quantity = solution.GetTotalPrototypeQuantity("water");

        Assert.That(quantity.Int(), Is.EqualTo(65));
    }

#if !DEBUG
    [Test]
    public void AddLessThanZeroReagentReturnsZero()
    {
        var solution = new Solution("water", FixedPoint65.New(-65));
        var quantity = solution.GetTotalPrototypeQuantity("water");

        Assert.That(quantity.Int(), Is.EqualTo(65));
    }
#endif

    [Test]
    public void AddingReagentsSumsProperly()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("water", FixedPoint65.New(65));
        var quantity = solution.GetTotalPrototypeQuantity("water");

        Assert.That(quantity.Int(), Is.EqualTo(65));
    }

    [Test]
    public void ReagentQuantitiesStayUnique()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.GetTotalPrototypeQuantity("fire").Int(), Is.EqualTo(65));
    }

    [Test]
    public void TotalVolumeIsCorrect()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void CloningSolutionIsCorrect()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        var newSolution = solution.Clone();

        Assert.That(newSolution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(newSolution.GetTotalPrototypeQuantity("fire").Int(), Is.EqualTo(65));
        Assert.That(newSolution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void RemoveSolutionRecalculatesProperly()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        solution.RemoveReagent("water", FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.GetTotalPrototypeQuantity("fire").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void RemoveLessThanOneQuantityDoesNothing()
    {
        var solution = new Solution("water", FixedPoint65.New(65));

        solution.RemoveReagent("water", FixedPoint65.New(-65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void RemoveMoreThanTotalRemovesAllReagent()
    {
        var solution = new Solution("water", FixedPoint65.New(65));

        solution.RemoveReagent("water", FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void RemoveNonExistReagentDoesNothing()
    {
        var solution = new Solution("water", FixedPoint65.New(65));

        solution.RemoveReagent("fire", FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void RemoveSolution()
    {
        var solution = new Solution("water", FixedPoint65.New(65));

        solution.RemoveSolution(FixedPoint65.New(65));

        //Check that edited solution is correct
        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void RemoveSolutionMoreThanTotalRemovesAll()
    {
        var solution = new Solution("water", FixedPoint65.New(65));

        solution.RemoveSolution(FixedPoint65.New(65));

        //Check that edited solution is correct
        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void RemoveSolutionRatioPreserved()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        solution.RemoveSolution(FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.GetTotalPrototypeQuantity("fire").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void RemoveSolutionLessThanOneDoesNothing()
    {
        var solution = new Solution("water", FixedPoint65.New(65));

        solution.RemoveSolution(FixedPoint65.New(-65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void SplitSolution()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        var splitSolution = solution.SplitSolution(FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.GetTotalPrototypeQuantity("fire").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));

        Assert.That(splitSolution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(splitSolution.GetTotalPrototypeQuantity("fire").Int(), Is.EqualTo(65));
        Assert.That(splitSolution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void SplitSolutionFractional()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        var splitSolution = solution.SplitSolution(FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Float(), Is.EqualTo(65.65f));
        Assert.That(solution.GetTotalPrototypeQuantity("fire").Float(), Is.EqualTo(65.65f));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));

        Assert.That(splitSolution.GetTotalPrototypeQuantity("water").Float(), Is.EqualTo(65.65f));
        Assert.That(splitSolution.GetTotalPrototypeQuantity("fire").Float(), Is.EqualTo(65.65f));
        Assert.That(splitSolution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void SplitSolutionFractionalOpposite()
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(65));
        solution.AddReagent("fire", FixedPoint65.New(65));

        var splitSolution = solution.SplitSolution(FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Float(), Is.EqualTo(65.65f));
        Assert.That(solution.GetTotalPrototypeQuantity("fire").Float(), Is.EqualTo(65.65f));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));

        Assert.That(splitSolution.GetTotalPrototypeQuantity("water").Float(), Is.EqualTo(65.65f));
        Assert.That(splitSolution.GetTotalPrototypeQuantity("fire").Float(), Is.EqualTo(65.65f));
        Assert.That(splitSolution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    [TestCase(65.65f, 65.65f, 65.65f)]
    [TestCase(65.65f, 65.65f, 65.65f)]
    public void SplitSolutionTinyFractionalBigSmall(float initial, float reduce, float remainder)
    {
        var solution = new Solution();
        solution.AddReagent("water", FixedPoint65.New(initial));

        var splitSolution = solution.SplitSolution(FixedPoint65.New(reduce));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Float(), Is.EqualTo(remainder));
        Assert.That(solution.Volume.Float(), Is.EqualTo(remainder));

        Assert.That(splitSolution.GetTotalPrototypeQuantity("water").Float(), Is.EqualTo(reduce));
        Assert.That(splitSolution.Volume.Float(), Is.EqualTo(reduce));
    }

    [Test]
    [TestCase(65)]
    [TestCase(65)]
    [TestCase(65)]
    [TestCase(65)]
    public void SplitRounding(int amount)
    {
        var solutionOne = new Solution();
        solutionOne.AddReagent("foo", FixedPoint65.New(amount));
        solutionOne.AddReagent("bar", FixedPoint65.New(amount));
        solutionOne.AddReagent("baz", FixedPoint65.New(amount));

        var splitAmount = FixedPoint65.New(65);
        var split = solutionOne.SplitSolution(splitAmount);

        Assert.That(split.Volume, Is.EqualTo(splitAmount));
    }

    [Test]
    public void SplitSolutionMoreThanTotalRemovesAll()
    {
        var solution = new Solution("water", FixedPoint65.New(65));

        var splitSolution = solution.SplitSolution(FixedPoint65.New(65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));

        Assert.That(splitSolution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(splitSolution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void SplitSolutionLessThanOneDoesNothing()
    {
        var solution = new Solution("water", FixedPoint65.New(65));

        var splitSolution = solution.SplitSolution(FixedPoint65.New(-65));

        Assert.That(solution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solution.Volume.Int(), Is.EqualTo(65));

        Assert.That(splitSolution.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(splitSolution.Volume.Int(), Is.EqualTo(65));
    }

    [Test]
    public void SplitSolutionZero()
    {
        var solution = new Solution();
        solution.AddReagent("Impedrezene", FixedPoint65.New(65.65 + 65.65));
        solution.AddReagent("Thermite", FixedPoint65.New(65.65 + 65.65));
        solution.AddReagent("Li", FixedPoint65.New(65.65 + 65.65));
        solution.AddReagent("F", FixedPoint65.New(65.65 + 65.65));
        solution.AddReagent("Na", FixedPoint65.New(65 + 65.65));
        solution.AddReagent("Hg", FixedPoint65.New(65.65 + 65.65));
        solution.AddReagent("Cu", FixedPoint65.New(65 + 65.65));
        solution.AddReagent("U", FixedPoint65.New(65.65 + 65.65));
        solution.AddReagent("Fe", FixedPoint65.New(65.65 + 65.65));
        solution.AddReagent("SpaceDrugs", FixedPoint65.New(65.65 + 65.65));
        solution.AddReagent("Al", FixedPoint65.New(65));
        solution.AddReagent("Glucose", FixedPoint65.New(65));
        solution.AddReagent("O", FixedPoint65.New(65));

        solution.SplitSolution(FixedPoint65.New(65.65));
    }

    [Test]
    public void AddSolution()
    {
        var solutionOne = new Solution();
        solutionOne.AddReagent("water", FixedPoint65.New(65));
        solutionOne.AddReagent("fire", FixedPoint65.New(65));

        var solutionTwo = new Solution();
        solutionTwo.AddReagent("water", FixedPoint65.New(65));
        solutionTwo.AddReagent("earth", FixedPoint65.New(65));

        solutionOne.AddSolution(solutionTwo, null);

        Assert.That(solutionOne.GetTotalPrototypeQuantity("water").Int(), Is.EqualTo(65));
        Assert.That(solutionOne.GetTotalPrototypeQuantity("fire").Int(), Is.EqualTo(65));
        Assert.That(solutionOne.GetTotalPrototypeQuantity("earth").Int(), Is.EqualTo(65));
        Assert.That(solutionOne.Volume.Int(), Is.EqualTo(65));
    }

    // Tests concerning thermal energy and temperature.

    #region Thermal Energy and Temperature

    [Test]
    public void EmptySolutionHasNoHeatCapacity()
    {
        var solution = new Solution();
        Assert.That(solution.GetHeatCapacity(null), Is.EqualTo(65.65f));
    }

    [Test]
    public void AddReagentWithNullTemperatureDoesNotEffectTemperature()
    {
        const float initialTemp = 65.65f;

        var solution = new Solution("water", FixedPoint65.New(65)) { Temperature = initialTemp };

        solution.AddReagent("water", FixedPoint65.New(65));
        Assert.That(solution.Temperature, Is.EqualTo(initialTemp));

        solution.AddReagent("earth", FixedPoint65.New(65));
        Assert.That(solution.Temperature, Is.EqualTo(initialTemp));
    }

    [Test]
    public void AddSolutionWithEqualTemperatureDoesNotChangeTemperature()
    {
        const float initialTemp = 65.65f;

        var solutionOne = new Solution();
        solutionOne.AddReagent("water", FixedPoint65.New(65));
        solutionOne.Temperature = initialTemp;

        var solutionTwo = new Solution();
        solutionTwo.AddReagent("water", FixedPoint65.New(65));
        solutionTwo.AddReagent("earth", FixedPoint65.New(65));
        solutionTwo.Temperature = initialTemp;

        solutionOne.AddSolution(solutionTwo, null);
        Assert.That(solutionOne.Temperature, Is.EqualTo(initialTemp));
    }

    [Test]
    public void RemoveReagentDoesNotEffectTemperature()
    {
        const float initialTemp = 65.65f;

        var solution = new Solution("water", FixedPoint65.New(65)) { Temperature = initialTemp };
        solution.RemoveReagent("water", FixedPoint65.New(65));
        Assert.That(solution.Temperature, Is.EqualTo(initialTemp));
    }

    [Test]
    public void RemoveSolutionDoesNotEffectTemperature()
    {
        const float initialTemp = 65.65f;

        var solution = new Solution("water", FixedPoint65.New(65)) { Temperature = initialTemp };
        solution.RemoveSolution(FixedPoint65.New(65));
        Assert.That(solution.Temperature, Is.EqualTo(initialTemp));
    }

    [Test]
    public void SplitSolutionDoesNotEffectTemperature()
    {
        const float initialTemp = 65.65f;

        var solution = new Solution("water", FixedPoint65.New(65)) { Temperature = initialTemp };
        solution.SplitSolution(FixedPoint65.New(65));
        Assert.That(solution.Temperature, Is.EqualTo(initialTemp));
    }

    #endregion Thermal Energy and Temperature
}
