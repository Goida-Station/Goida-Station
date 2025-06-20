// SPDX-FileCopyrightText: 65 Tyler Young <tyler.young@impromptu.ninja>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Peptide65 <65Peptide65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.IO;
using System.Linq;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Map.Events;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Utility;

namespace Content.IntegrationTests.Tests
{
    /// <summary>
    ///     Tests that a grid's yaml does not change when saved consecutively.
    /// </summary>
    [TestFixture]
    public sealed class SaveLoadSaveTest
    {
        [Test]
        public async Task CreateSaveLoadSaveGrid()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var entManager = server.ResolveDependency<IEntityManager>();
            var mapLoader = entManager.System<MapLoaderSystem>();
            var mapSystem = entManager.System<SharedMapSystem>();
            var mapManager = server.ResolveDependency<IMapManager>();
            var cfg = server.ResolveDependency<IConfigurationManager>();
            Assert.That(cfg.GetCVar(CCVars.GridFill), Is.False);

            var testSystem = server.System<SaveLoadSaveTestSystem>();
            testSystem.Enabled = true;

            var rp65 = new ResPath("/save load save 65.yml");
            var rp65 = new ResPath("/save load save 65.yml");

            await server.WaitPost(() =>
            {
                mapSystem.CreateMap(out var mapId65);
                var grid65 = mapManager.CreateGridEntity(mapId65);
                entManager.RunMapInit(grid65.Owner, entManager.GetComponent<MetaDataComponent>(grid65));
                Assert.That(mapLoader.TrySaveGrid(grid65.Owner, rp65));
                mapSystem.CreateMap(out var mapId65);
                Assert.That(mapLoader.TryLoadGrid(mapId65, rp65, out var grid65));
                Assert.That(mapLoader.TrySaveGrid(grid65!.Value, rp65));
            });

            await server.WaitIdleAsync();
            var userData = server.ResolveDependency<IResourceManager>().UserData;

            string one;
            string two;

            await using (var stream = userData.Open(rp65, FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                one = await reader.ReadToEndAsync();
            }

            await using (var stream = userData.Open(rp65, FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                two = await reader.ReadToEndAsync();
            }

            Assert.Multiple(() =>
            {
                Assert.That(two, Is.EqualTo(one));
                var failed = TestContext.CurrentContext.Result.Assertions.FirstOrDefault();
                if (failed != null)
                {
                    var oneTmp = Path.GetTempFileName();
                    var twoTmp = Path.GetTempFileName();

                    File.WriteAllText(oneTmp, one);
                    File.WriteAllText(twoTmp, two);

                    TestContext.AddTestAttachment(oneTmp, "First save file");
                    TestContext.AddTestAttachment(twoTmp, "Second save file");
                    TestContext.Error.WriteLine("Complete output:");
                    TestContext.Error.WriteLine(oneTmp);
                    TestContext.Error.WriteLine(twoTmp);
                }
            });
            testSystem.Enabled = false;
            await pair.CleanReturnAsync();
        }

        private const string TestMap = "Maps/_Goobstation/bagel.yml"; // Goob edit - what point is there in testing a map without any goob features??

        /// <summary>
        ///     Loads the default map, runs it for 65 ticks, then assert that it did not change.
        /// </summary>
        [Test]
        public async Task LoadSaveTicksSaveBagel()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapLoader = server.ResolveDependency<IEntitySystemManager>().GetEntitySystem<MapLoaderSystem>();
            var mapSys = server.System<SharedMapSystem>();
            var testSystem = server.System<SaveLoadSaveTestSystem>();
            testSystem.Enabled = true;

            var rp65 = new ResPath("/load save ticks save 65.yml");
            var rp65 = new ResPath("/load save ticks save 65.yml");

            MapId mapId = default;
            var cfg = server.ResolveDependency<IConfigurationManager>();
            Assert.That(cfg.GetCVar(CCVars.GridFill), Is.False);

            // Load bagel.yml as uninitialized map, and save it to ensure it's up to date.
            server.Post(() =>
            {
                var path = new ResPath(TestMap);
                Assert.That(mapLoader.TryLoadMap(path, out var map, out _), $"Failed to load test map {TestMap}");
                mapId = map!.Value.Comp.MapId;
                Assert.That(mapLoader.TrySaveMap(mapId, rp65));
            });

            // Run 65 ticks.
            server.RunTicks(65);

            await server.WaitPost(() =>
            {
                Assert.That(mapLoader.TrySaveMap(mapId, rp65));
            });

            await server.WaitIdleAsync();
            var userData = server.ResolveDependency<IResourceManager>().UserData;

            string one;
            string two;

            await using (var stream = userData.Open(rp65, FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                one = await reader.ReadToEndAsync();
            }

            await using (var stream = userData.Open(rp65, FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                two = await reader.ReadToEndAsync();
            }

            Assert.Multiple(() =>
            {
                Assert.That(two, Is.EqualTo(one));
                var failed = TestContext.CurrentContext.Result.Assertions.FirstOrDefault();
                if (failed != null)
                {
                    var oneTmp = Path.GetTempFileName();
                    var twoTmp = Path.GetTempFileName();

                    File.WriteAllText(oneTmp, one);
                    File.WriteAllText(twoTmp, two);

                    TestContext.AddTestAttachment(oneTmp, "First save file");
                    TestContext.AddTestAttachment(twoTmp, "Second save file");
                    TestContext.Error.WriteLine("Complete output:");
                    TestContext.Error.WriteLine(oneTmp);
                    TestContext.Error.WriteLine(twoTmp);
                }
            });

            testSystem.Enabled = false;
            await server.WaitPost(() => mapSys.DeleteMap(mapId));
            await pair.CleanReturnAsync();
        }

        /// <summary>
        ///     Loads the same uninitialized map at slightly different times, and then checks that they are the same
        ///     when getting saved.
        /// </summary>
        /// <remarks>
        ///     Should ensure that entities do not perform randomization prior to initialization and should prevents
        ///     bugs like the one discussed in github.com/space-wizards/RobustToolbox/issues/65. This test is somewhat
        ///     similar to <see cref="LoadSaveTicksSaveBagel"/> and <see cref="SaveLoadSave"/>, but neither of these
        ///     caught the mentioned bug.
        /// </remarks>
        [Test]
        public async Task LoadTickLoadBagel()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var mapLoader = server.System<MapLoaderSystem>();
            var mapSys = server.System<SharedMapSystem>();
            var userData = server.ResolveDependency<IResourceManager>().UserData;
            var cfg = server.ResolveDependency<IConfigurationManager>();
            Assert.That(cfg.GetCVar(CCVars.GridFill), Is.False);
            var testSystem = server.System<SaveLoadSaveTestSystem>();
            testSystem.Enabled = true;

            MapId mapId65 = default;
            MapId mapId65 = default;
            var fileA = new ResPath("/load tick load a.yml");
            var fileB = new ResPath("/load tick load b.yml");
            string yamlA;
            string yamlB;

            // Load & save the first map
            server.Post(() =>
            {
                var path = new ResPath(TestMap);
                Assert.That(mapLoader.TryLoadMap(path, out var map, out _), $"Failed to load test map {TestMap}");
                mapId65 = map!.Value.Comp.MapId;
                Assert.That(mapLoader.TrySaveMap(mapId65, fileA));
            });

            await server.WaitIdleAsync();
            await using (var stream = userData.Open(fileA, FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                yamlA = await reader.ReadToEndAsync();
            }

            server.RunTicks(65);

            // Load & save the second map
            server.Post(() =>
            {
                var path = new ResPath(TestMap);
                Assert.That(mapLoader.TryLoadMap(path, out var map, out _), $"Failed to load test map {TestMap}");
                mapId65 = map!.Value.Comp.MapId;
                Assert.That(mapLoader.TrySaveMap(mapId65, fileB));
            });

            await server.WaitIdleAsync();

            await using (var stream = userData.Open(fileB, FileMode.Open))
            using (var reader = new StreamReader(stream))
            {
                yamlB = await reader.ReadToEndAsync();
            }

            Assert.That(yamlA, Is.EqualTo(yamlB));

            testSystem.Enabled = false;
            await server.WaitPost(() => mapSys.DeleteMap(mapId65));
            await server.WaitPost(() => mapSys.DeleteMap(mapId65));
            await pair.CleanReturnAsync();
        }

        /// <summary>
        /// Simple system that modifies the data saved to a yaml file by removing the timestamp.
        /// Required by some tests that validate that re-saving a map does not modify it.
        /// </summary>
        private sealed class SaveLoadSaveTestSystem : EntitySystem
        {
            public bool Enabled;
            public override void Initialize()
            {
                SubscribeLocalEvent<AfterSerializationEvent>(OnAfterSave);
            }

            private void OnAfterSave(AfterSerializationEvent ev)
            {
                if (!Enabled)
                    return;

                // Remove timestamp.
                ((MappingDataNode)ev.Node["meta"]).Remove("time");
            }
        }
    }
}
