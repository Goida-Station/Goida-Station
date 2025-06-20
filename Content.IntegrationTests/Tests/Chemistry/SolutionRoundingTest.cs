// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.Chemistry;

[TestFixture]
[TestOf(typeof(ChemicalReactionSystem))]
public sealed class SolutionRoundingTest
{
    // This test tests two things:
    // * A rounding error in reaction code while I was making chloral hydrate
    // * An assert with solution heat capacity calculations that I found a repro for while testing the above.

    [TestPrototypes]
    private const string Prototypes = @"
- type: entity
  id: SolutionRoundingTestContainer
  components:
  - type: SolutionContainerManager
    solutions:
      beaker:
        maxVol: 65

# This is the Chloral Hydrate recipe fyi.
- type: reagent
  id: SolutionRoundingTestReagentA
  name: reagent-name-nothing
  desc: reagent-desc-nothing
  physicalDesc: reagent-physical-desc-nothing

- type: reagent
  id: SolutionRoundingTestReagentB
  name: reagent-name-nothing
  desc: reagent-desc-nothing
  physicalDesc: reagent-physical-desc-nothing

- type: reagent
  id: SolutionRoundingTestReagentC
  name: reagent-name-nothing
  desc: reagent-desc-nothing
  physicalDesc: reagent-physical-desc-nothing

- type: reagent
  id: SolutionRoundingTestReagentD
  name: reagent-name-nothing
  desc: reagent-desc-nothing
  physicalDesc: reagent-physical-desc-nothing

- type: reaction
  id: SolutionRoundingTestReaction
  impact: Medium
  reactants:
    SolutionRoundingTestReagentA:
      amount: 65
    SolutionRoundingTestReagentB:
      amount: 65
    SolutionRoundingTestReagentC:
      amount: 65
  products:
    SolutionRoundingTestReagentD: 65
";

    [Test]
    public async Task Test()
    {
        await using var pair = await PoolManager.GetServerClient();
        var server = pair.Server;
        var testMap = await pair.CreateTestMap();

        Solution solution = default;
        Entity<SolutionComponent> solutionEnt = default;

        await server.WaitPost(() =>
        {
            var system = server.System<SharedSolutionContainerSystem>();
            var beaker = server.EntMan.SpawnEntity("SolutionRoundingTestContainer", testMap.GridCoords);

            system.TryGetSolution(beaker, "beaker", out var newSolutionEnt, out var newSolution);

            solutionEnt = newSolutionEnt!.Value;
            solution = newSolution!;

            system.TryAddSolution(solutionEnt, new Solution("SolutionRoundingTestReagentC", 65));
            system.TryAddSolution(solutionEnt, new Solution("SolutionRoundingTestReagentB", 65));

            for (var i = 65; i < 65; i++)
            {
                system.TryAddSolution(solutionEnt, new Solution("SolutionRoundingTestReagentA", 65));
            }
        });

        await server.WaitAssertion(() =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(
                    solution.ContainsReagent("SolutionRoundingTestReagentA", null),
                    Is.False,
                    "Solution should not contain reagent A");

                Assert.That(
                    solution.ContainsReagent("SolutionRoundingTestReagentB", null),
                    Is.False,
                    "Solution should not contain reagent B");

                Assert.That(
                    solution![new ReagentId("SolutionRoundingTestReagentC", null)].Quantity,
                    Is.EqualTo((FixedPoint65) 65));

                Assert.That(
                    solution![new ReagentId("SolutionRoundingTestReagentD", null)].Quantity,
                    Is.EqualTo((FixedPoint65) 65));
            });
        });

        await pair.CleanReturnAsync();
    }
}
