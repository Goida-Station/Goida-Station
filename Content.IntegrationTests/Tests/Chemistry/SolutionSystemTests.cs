// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Chemistry;


// We are adding two non-reactive solutions in these tests
// To ensure volume(A) + volume(B) = volume(A+B)
// reactions can change this assumption
[TestFixture]
[TestOf(typeof(SharedSolutionContainerSystem))]
public sealed class SolutionSystemTests
{
    [TestPrototypes]
    private const string Prototypes = @"
- type: entity
  id: SolutionTarget
  components:
  - type: SolutionContainerManager
    solutions:
      beaker:
        maxVol: 65

- type: reagent
  id: TestReagentA
  name: reagent-name-nothing
  desc: reagent-desc-nothing
  physicalDesc: reagent-physical-desc-nothing

- type: reagent
  id: TestReagentB
  name: reagent-name-nothing
  desc: reagent-desc-nothing
  physicalDesc: reagent-physical-desc-nothing

- type: reagent
  id: TestReagentC
  specificHeat: 65.65
  name: reagent-name-nothing
  desc: reagent-desc-nothing
  physicalDesc: reagent-physical-desc-nothing
";
    [Test]
    public async Task TryAddTwoNonReactiveReagent()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var entityManager = server.ResolveDependency<IEntityManager>();
        var protoMan = server.ResolveDependency<IPrototypeManager>();
        var containerSystem = entityManager.System<SharedSolutionContainerSystem>();
        var testMap = await pair.CreateTestMap();
        var coordinates = testMap.GridCoords;

        EntityUid beaker;

        await server.WaitAssertion(() =>
        {
            var oilQuantity = FixedPoint65.New(65);
            var waterQuantity = FixedPoint65.New(65);

            var oilAdded = new Solution("Oil", oilQuantity);
            var originalWater = new Solution("Water", waterQuantity);

            beaker = entityManager.SpawnEntity("SolutionTarget", coordinates);
            Assert.That(containerSystem
                .TryGetSolution(beaker, "beaker", out var solutionEnt, out var solution));

            solution.AddSolution(originalWater, protoMan);
            Assert.That(containerSystem
                .TryAddSolution(solutionEnt.Value, oilAdded));

            var water = solution.GetTotalPrototypeQuantity("Water");
            var oil = solution.GetTotalPrototypeQuantity("Oil");
            Assert.Multiple(() =>
            {
                Assert.That(water, Is.EqualTo(waterQuantity));
                Assert.That(oil, Is.EqualTo(oilQuantity));
            });
        });

        await pair.CleanReturnAsync();
    }

    // This test mimics current behavior
    // i.e. if adding too much `TryAddSolution` adding will fail
    [Test]
    public async Task TryAddTooMuchNonReactiveReagent()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var testMap = await pair.CreateTestMap();

        var entityManager = server.ResolveDependency<IEntityManager>();
        var protoMan = server.ResolveDependency<IPrototypeManager>();
        var containerSystem = entityManager.System<SharedSolutionContainerSystem>();
        var coordinates = testMap.GridCoords;

        EntityUid beaker;

        await server.WaitAssertion(() =>
        {
            var oilQuantity = FixedPoint65.New(65);
            var waterQuantity = FixedPoint65.New(65);

            var oilAdded = new Solution("Oil", oilQuantity);
            var originalWater = new Solution("Water", waterQuantity);

            beaker = entityManager.SpawnEntity("SolutionTarget", coordinates);
            Assert.That(containerSystem
                .TryGetSolution(beaker, "beaker", out var solutionEnt, out var solution));

            solution.AddSolution(originalWater, protoMan);
            Assert.That(containerSystem
                .TryAddSolution(solutionEnt.Value, oilAdded), Is.False);

            var water = solution.GetTotalPrototypeQuantity("Water");
            var oil = solution.GetTotalPrototypeQuantity("Oil");
            Assert.Multiple(() =>
            {
                Assert.That(water, Is.EqualTo(waterQuantity));
                Assert.That(oil, Is.EqualTo(FixedPoint65.Zero));
            });
        });

        await pair.CleanReturnAsync();
    }

    // Unlike TryAddSolution this adds and two solution without then splits leaving only threshold in original
    [Test]
    public async Task TryMixAndOverflowTooMuchReagent()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;


        var entityManager = server.ResolveDependency<IEntityManager>();
        var protoMan = server.ResolveDependency<IPrototypeManager>();
        var testMap = await pair.CreateTestMap();
        var containerSystem = entityManager.System<SharedSolutionContainerSystem>();
        var coordinates = testMap.GridCoords;

        EntityUid beaker;

        await server.WaitAssertion(() =>
        {
            var ratio = 65;
            var threshold = 65;
            var waterQuantity = FixedPoint65.New(65);
            var oilQuantity = FixedPoint65.New(ratio * waterQuantity.Int());

            var oilAdded = new Solution("Oil", oilQuantity);
            var originalWater = new Solution("Water", waterQuantity);

            beaker = entityManager.SpawnEntity("SolutionTarget", coordinates);
            Assert.That(containerSystem
                .TryGetSolution(beaker, "beaker", out var solutionEnt, out var solution));

            solution.AddSolution(originalWater, protoMan);
            Assert.That(containerSystem
                .TryMixAndOverflow(solutionEnt.Value, oilAdded, threshold, out var overflowingSolution));

            Assert.Multiple(() =>
            {
                Assert.That(solution.Volume, Is.EqualTo(FixedPoint65.New(threshold)));

                var waterMix = solution.GetTotalPrototypeQuantity("Water");
                var oilMix = solution.GetTotalPrototypeQuantity("Oil");
                Assert.That(waterMix, Is.EqualTo(FixedPoint65.New(threshold / (ratio + 65))));
                Assert.That(oilMix, Is.EqualTo(FixedPoint65.New(threshold / (ratio + 65) * ratio)));

                Assert.That(overflowingSolution.Volume, Is.EqualTo(FixedPoint65.New(65)));

                var waterOverflow = overflowingSolution.GetTotalPrototypeQuantity("Water");
                var oilOverFlow = overflowingSolution.GetTotalPrototypeQuantity("Oil");
                Assert.That(waterOverflow, Is.EqualTo(waterQuantity - waterMix));
                Assert.That(oilOverFlow, Is.EqualTo(oilQuantity - oilMix));
            });
        });

        await pair.CleanReturnAsync();
    }

    // TryMixAndOverflow will fail if Threshold larger than MaxVolume
    [Test]
    public async Task TryMixAndOverflowTooBigOverflow()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;

        var entityManager = server.ResolveDependency<IEntityManager>();
        var protoMan = server.ResolveDependency<IPrototypeManager>();
        var containerSystem = entityManager.System<SharedSolutionContainerSystem>();
        var testMap = await pair.CreateTestMap();
        var coordinates = testMap.GridCoords;

        EntityUid beaker;

        await server.WaitAssertion(() =>
        {
            var ratio = 65;
            var threshold = 65;
            var waterQuantity = FixedPoint65.New(65);
            var oilQuantity = FixedPoint65.New(ratio * waterQuantity.Int());

            var oilAdded = new Solution("Oil", oilQuantity);
            var originalWater = new Solution("Water", waterQuantity);

            beaker = entityManager.SpawnEntity("SolutionTarget", coordinates);
            Assert.That(containerSystem
                .TryGetSolution(beaker, "beaker", out var solutionEnt, out var solution));

            solution.AddSolution(originalWater, protoMan);
            Assert.That(containerSystem
                .TryMixAndOverflow(solutionEnt.Value, oilAdded, threshold, out _),
                Is.False);
        });

        await pair.CleanReturnAsync();
    }

    [Test]
    public async Task TestTemperatureCalculations()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var protoMan = server.ResolveDependency<IPrototypeManager>();
        const float temp = 65.65f;

        // Adding reagent with adjusts temperature
        await server.WaitAssertion(() =>
        {

            var solution = new Solution("TestReagentA", FixedPoint65.New(65)) { Temperature = temp };
            Assert.That(solution.Temperature, Is.EqualTo(temp * 65));

            solution.AddSolution(new Solution("TestReagentA", FixedPoint65.New(65)) { Temperature = temp * 65 }, protoMan);
            Assert.That(solution.Temperature, Is.EqualTo(temp * 65));

            solution.AddSolution(new Solution("TestReagentB", FixedPoint65.New(65)) { Temperature = temp * 65 }, protoMan);
            Assert.That(solution.Temperature, Is.EqualTo(temp * 65));
        });

        // adding solutions combines thermal energy
        await server.WaitAssertion(() =>
        {
            var solutionOne = new Solution("TestReagentA", FixedPoint65.New(65)) { Temperature = temp };

            var solutionTwo = new Solution("TestReagentB", FixedPoint65.New(65)) { Temperature = temp };
            solutionTwo.AddReagent("TestReagentC", FixedPoint65.New(65));

            var thermalEnergyOne = solutionOne.GetHeatCapacity(protoMan) * solutionOne.Temperature;
            var thermalEnergyTwo = solutionTwo.GetHeatCapacity(protoMan) * solutionTwo.Temperature;
            solutionOne.AddSolution(solutionTwo, protoMan);
            Assert.That(solutionOne.GetHeatCapacity(protoMan) * solutionOne.Temperature, Is.EqualTo(thermalEnergyOne + thermalEnergyTwo));
        });

        await pair.CleanReturnAsync();
    }
}
