// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 c65llv65e <65c65llv65e@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Collections.Generic;
using System.Linq;
using Content.Shared.Access;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Robust.Shared.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Access
{
    [TestFixture]
    [TestOf(typeof(AccessReaderComponent))]
    public sealed class AccessReaderTest
    {
        [Test]
        public async Task TestTags()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var entityManager = server.ResolveDependency<IEntityManager>();


            await server.WaitAssertion(() =>
            {
                var system = entityManager.System<AccessReaderSystem>();

                // test empty
                var reader = new AccessReaderComponent();
                Assert.Multiple(() =>
                {
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "Foo" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "Bar" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(Array.Empty<ProtoId<AccessLevelPrototype>>(), reader), Is.True);
                });

                // test deny
                reader = new AccessReaderComponent();
                reader.DenyTags.Add("A");
                Assert.Multiple(() =>
                {
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "Foo" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A" }, reader), Is.False);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A", "Foo" }, reader), Is.False);
                    Assert.That(system.AreAccessTagsAllowed(Array.Empty<ProtoId<AccessLevelPrototype>>(), reader), Is.True);
                });

                // test one list
                reader = new AccessReaderComponent();
                reader.AccessLists.Add(new HashSet<ProtoId<AccessLevelPrototype>> { "A" });
                Assert.Multiple(() =>
                {
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "B" }, reader), Is.False);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A", "B" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(Array.Empty<ProtoId<AccessLevelPrototype>>(), reader), Is.False);
                });

                // test one list - two items
                reader = new AccessReaderComponent();
                reader.AccessLists.Add(new HashSet<ProtoId<AccessLevelPrototype>> { "A", "B" });
                Assert.Multiple(() =>
                {
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A" }, reader), Is.False);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "B" }, reader), Is.False);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A", "B" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(Array.Empty<ProtoId<AccessLevelPrototype>>(), reader), Is.False);
                });

                // test two list
                reader = new AccessReaderComponent();
                reader.AccessLists.Add(new HashSet<ProtoId<AccessLevelPrototype>> { "A" });
                reader.AccessLists.Add(new HashSet<ProtoId<AccessLevelPrototype>> { "B", "C" });
                Assert.Multiple(() =>
                {
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "B" }, reader), Is.False);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A", "B" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "C", "B" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "C", "B", "A" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(Array.Empty<ProtoId<AccessLevelPrototype>>(), reader), Is.False);
                });

                // test deny list
                reader = new AccessReaderComponent();
                reader.AccessLists.Add(new HashSet<ProtoId<AccessLevelPrototype>> { "A" });
                reader.DenyTags.Add("B");
                Assert.Multiple(() =>
                {
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A" }, reader), Is.True);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "B" }, reader), Is.False);
                    Assert.That(system.AreAccessTagsAllowed(new List<ProtoId<AccessLevelPrototype>> { "A", "B" }, reader), Is.False);
                    Assert.That(system.AreAccessTagsAllowed(Array.Empty<ProtoId<AccessLevelPrototype>>(), reader), Is.False);
                });
            });
            await pair.CleanReturnAsync();
        }

    }
}