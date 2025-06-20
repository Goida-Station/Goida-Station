// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

#nullable enable
using Content.Server.Stack;
using Content.Shared.Stacks;
using Content.Shared.Materials;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Materials
{
    /// <summary>
    /// Materials and stacks have some odd relationships to entities,
    /// so we need some test coverage for them.
    /// </summary>
    [TestFixture]
    [TestOf(typeof(StackSystem))]
    [TestOf(typeof(MaterialPrototype))]
    public sealed class MaterialPrototypeSpawnsStackMaterialTest
    {
        [Test]
        public async Task MaterialPrototypeSpawnsStackMaterial()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            await server.WaitIdleAsync();

            var mapSystem = server.System<SharedMapSystem>();
            var prototypeManager = server.ResolveDependency<IPrototypeManager>();
            var entityManager = server.ResolveDependency<IEntityManager>();

            var testMap = await pair.CreateTestMap();

            await server.WaitAssertion(() =>
            {
                var allMaterialProtos = prototypeManager.EnumeratePrototypes<MaterialPrototype>();
                var coords = testMap.GridCoords;

                Assert.Multiple(() =>
                {
                    foreach (var proto in allMaterialProtos)
                    {
                        if (proto.StackEntity == null)
                            continue;

                        var spawned = entityManager.SpawnEntity(proto.StackEntity, coords);

                        Assert.That(entityManager.TryGetComponent<StackComponent>(spawned, out var stack),
                            $"{proto.ID} 'stack entity' {proto.StackEntity} does not have the stack component");

                        Assert.That(entityManager.HasComponent<MaterialComponent>(spawned),
                            $"{proto.ID} 'material stack' {proto.StackEntity} does not have the material component");

                        StackPrototype? stackProto = null;
                        Assert.That(stack?.StackTypeId != null && prototypeManager.TryIndex(stack.StackTypeId, out stackProto),
                            $"{proto.ID} material has no stack prototype");

                        if (stackProto != null)
                            Assert.That(proto.StackEntity, Is.EqualTo(stackProto.Spawn));
                    }
                });

                mapSystem.DeleteMap(testMap.MapId);
            });

            await pair.CleanReturnAsync();
        }
    }
}