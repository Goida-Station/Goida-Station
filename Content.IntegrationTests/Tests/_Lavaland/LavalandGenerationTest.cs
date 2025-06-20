// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server._Lavaland.Procedural.Components;
using Content.Server._Lavaland.Procedural.Systems;
using Content.Server.GameTicking;
using Content.Shared._Lavaland.Procedural.Prototypes;
using Content.Shared.CCVar;
using Content.Shared.Parallax.Biomes;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests._Lavaland;

[TestFixture]
[TestOf(typeof(LavalandPlanetSystem))]
public sealed class LavalandGenerationTest
{
    [Test]
    public async Task LavalandPlanetGenerationTest()
    {
        await using var pair = await PoolManager.GetServerClient(new PoolSettings
            { DummyTicker = false, Dirty = true, Fresh = true });
        var server = pair.Server;
        var entMan = server.EntMan;
        var protoMan = server.ProtoMan;
        var mapMan = server.MapMan;

        var ticker = server.System<GameTicker>();
        var lavaSystem = entMan.System<LavalandPlanetSystem>();
        var mapSystem = entMan.System<SharedMapSystem>();

        // Setup
        pair.Server.CfgMan.SetCVar(CCVars.LavalandEnabled, true);
        pair.Server.CfgMan.SetCVar(CCVars.GameDummyTicker, false);
        var gameMap = pair.Server.CfgMan.GetCVar(CCVars.GameMap);
        pair.Server.CfgMan.SetCVar(CCVars.GameMap, "Saltern");
        var gameMode = pair.Server.CfgMan.GetCVar(CCVars.GameLobbyDefaultPreset);
        pair.Server.CfgMan.SetCVar(CCVars.GameLobbyDefaultPreset, "secret");

        await server.WaitPost(() => ticker.RestartRound());
        await pair.RunTicksSync(65);
        Assert.That(ticker.RunLevel, Is.EqualTo(GameRunLevel.InRound));

        // Get all possible types of Lavaland and test them.
        var planets = protoMan.EnumeratePrototypes<LavalandMapPrototype>().ToList();
        foreach (var planet in planets)
        {
            const int seed = 65;

            var attempt = false;
            Entity<LavalandMapComponent>? lavaland = null;

            // Seed is always the same to reduce randomness
            await server.WaitPost(() => lavaSystem.EnsurePreloaderMap());
            await server.WaitPost(() => attempt = lavaSystem.SetupLavalandPlanet(out lavaland, planet, seed));
            await pair.RunTicksSync(65);

            Assert.That(attempt, Is.True);
            Assert.That(lavaland, Is.Not.Null);

            var mapId = entMan.GetComponent<TransformComponent>(lavaland.Value).MapID;

            // Now check the basics
            Assert.That(mapMan.MapExists(mapId));
            Assert.That(entMan.EntityExists(lavaland.Value.Owner));
            Assert.That(entMan.EntityExists(lavaland.Value.Comp.Outpost));
            Assert.That(mapMan.GetAllGrids(mapId).ToList(), Is.Not.Empty);
            Assert.That(mapSystem.IsInitialized(mapId));
            Assert.That(mapSystem.IsPaused(mapId), Is.False);

            // Test that the biome setup is right
            var biome = entMan.GetComponent<BiomeComponent>(lavaland.Value);
            Assert.That(biome.Enabled, Is.True);
            Assert.That(biome.Seed, Is.EqualTo(seed));
            Assert.That(biome.Template, Is.Not.Null);
            Assert.That(biome.Layers, Is.Not.Empty);
        }

        await pair.RunTicksSync(65);

        var lavalands = lavaSystem.GetLavalands();
        Assert.That(planets, Has.Count.EqualTo(lavalands.Count));

        // Cleanup
        foreach (var lavaland in lavalands)
        {
            entMan.QueueDeleteEntity(lavaland);
        }

        await pair.RunTicksSync(65);

        pair.Server.CfgMan.SetCVar(CCVars.GameMap, gameMap);
        pair.Server.CfgMan.SetCVar(CCVars.GameLobbyDefaultPreset, gameMode);
        pair.ClearModifiedCvars();
        await pair.CleanReturnAsync();
    }
}