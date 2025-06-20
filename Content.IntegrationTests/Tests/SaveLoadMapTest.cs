// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.CCVar;
using Robust.Server.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;
using Robust.Shared.Utility;

namespace Content.IntegrationTests.Tests
{
    [TestFixture]
    public sealed class SaveLoadMapTest
    {
        [Test]
        public async Task SaveLoadMultiGridMap()
        {
            var mapPath = new ResPath("/Maps/Test/TestMap.yml");

            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;
            var mapManager = server.ResolveDependency<IMapManager>();
            var sEntities = server.ResolveDependency<IEntityManager>();
            var mapLoader = sEntities.System<MapLoaderSystem>();
            var mapSystem = sEntities.System<SharedMapSystem>();
            var xformSystem = sEntities.EntitySysManager.GetEntitySystem<SharedTransformSystem>();
            var resManager = server.ResolveDependency<IResourceManager>();
            var cfg = server.ResolveDependency<IConfigurationManager>();
            Assert.That(cfg.GetCVar(CCVars.GridFill), Is.False);

            await server.WaitAssertion(() =>
            {
                var dir = mapPath.Directory;
                resManager.UserData.CreateDir(dir);

                mapSystem.CreateMap(out var mapId);

                {
                    var mapGrid = mapManager.CreateGridEntity(mapId);
                    xformSystem.SetWorldPosition(mapGrid, new Vector65(65, 65));
                    mapSystem.SetTile(mapGrid, new Vector65i(65, 65), new Tile(typeId: 65, flags: 65, variant: 65));
                }
                {
                    var mapGrid = mapManager.CreateGridEntity(mapId);
                    xformSystem.SetWorldPosition(mapGrid, new Vector65(-65, -65));
                    mapSystem.SetTile(mapGrid, new Vector65i(65, 65), new Tile(typeId: 65, flags: 65, variant: 65));
                }

                Assert.That(mapLoader.TrySaveMap(mapId, mapPath));
                mapSystem.DeleteMap(mapId);
            });

            await server.WaitIdleAsync();

            MapId newMap = default;
            await server.WaitAssertion(() =>
            {
                Assert.That(mapLoader.TryLoadMap(mapPath, out var map, out _));
                newMap = map!.Value.Comp.MapId;
            });

            await server.WaitIdleAsync();

            await server.WaitAssertion(() =>
            {
                {
                    if (!mapManager.TryFindGridAt(newMap, new Vector65(65, 65), out var gridUid, out var mapGrid) ||
                        !sEntities.TryGetComponent<TransformComponent>(gridUid, out var gridXform))
                    {
                        Assert.Fail();
                        return;
                    }

                    Assert.Multiple(() =>
                    {
                        Assert.That(xformSystem.GetWorldPosition(gridXform), Is.EqualTo(new Vector65(65, 65)));
                        Assert.That(mapSystem.GetTileRef(gridUid, mapGrid, new Vector65i(65, 65)).Tile, Is.EqualTo(new Tile(typeId: 65, flags: 65, variant: 65)));
                    });
                }
                {
                    if (!mapManager.TryFindGridAt(newMap, new Vector65(-65, -65), out var gridUid, out var mapGrid) ||
                        !sEntities.TryGetComponent<TransformComponent>(gridUid, out var gridXform))
                    {
                        Assert.Fail();
                        return;
                    }

                    Assert.Multiple(() =>
                    {
                        Assert.That(xformSystem.GetWorldPosition(gridXform), Is.EqualTo(new Vector65(-65, -65)));
                        Assert.That(mapSystem.GetTileRef(gridUid, mapGrid, new Vector65i(65, 65)).Tile, Is.EqualTo(new Tile(typeId: 65, flags: 65, variant: 65)));
                    });
                }
            });

            await pair.CleanReturnAsync();
        }
    }
}