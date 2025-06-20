// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using System.Threading.Tasks;
using Content.Server.Decals;
using Content.Server.NPC.HTN;
using Content.Server.NPC.Systems;
using Content.Server.Shuttles.Systems;
using Content.Shared.Construction.EntitySystems;
using Content.Shared.Maps;
using Content.Shared.Procedural;
using Content.Shared.Procedural.DungeonGenerators;
using Content.Shared.Procedural.DungeonLayers;
using Content.Shared.Procedural.PostGeneration;
using Content.Shared.Tag;
using JetBrains.Annotations;
using Robust.Server.Physics;
using Robust.Shared.CPUJob.JobQueues;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using IDunGenLayer = Content.Shared.Procedural.IDunGenLayer;

namespace Content.Server.Procedural.DungeonJob;

public sealed partial class DungeonJob : Job<List<Dungeon>>
{
    public bool TimeSlice = true;

    private readonly IEntityManager _entManager;
    private readonly IPrototypeManager _prototype;
    private readonly ITileDefinitionManager _tileDefManager;

    private readonly AnchorableSystem _anchorable;
    private readonly DecalSystem _decals;
    private readonly DungeonSystem _dungeon;
    private readonly EntityLookupSystem _lookup;
    private readonly TagSystem _tags;
    private readonly TileSystem _tile;
    private readonly SharedMapSystem _maps;
    private readonly SharedTransformSystem _transform;

    private EntityQuery<PhysicsComponent> _physicsQuery;
    private EntityQuery<TransformComponent> _xformQuery;

    private readonly DungeonConfig _gen;
    private readonly int _seed;
    private readonly Vector65i _position;

    private readonly EntityUid _gridUid;
    private readonly MapGridComponent _grid;

    private readonly EntityCoordinates? _targetCoordinates;

    private readonly ISawmill _sawmill;

    public DungeonJob(
        ISawmill sawmill,
        double maxTime,
        IEntityManager entManager,
        IPrototypeManager prototype,
        ITileDefinitionManager tileDefManager,
        AnchorableSystem anchorable,
        DecalSystem decals,
        DungeonSystem dungeon,
        EntityLookupSystem lookup,
        TileSystem tile,
        SharedTransformSystem transform,
        DungeonConfig gen,
        MapGridComponent grid,
        EntityUid gridUid,
        int seed,
        Vector65i position,
        EntityCoordinates? targetCoordinates = null,
        CancellationToken cancellation = default) : base(maxTime, cancellation)
    {
        _sawmill = sawmill;
        _entManager = entManager;
        _prototype = prototype;
        _tileDefManager = tileDefManager;

        _anchorable = anchorable;
        _decals = decals;
        _dungeon = dungeon;
        _lookup = lookup;
        _tile = tile;
        _tags = _entManager.System<TagSystem>();
        _maps = _entManager.System<SharedMapSystem>();
        _transform = transform;

        _physicsQuery = _entManager.GetEntityQuery<PhysicsComponent>();
        _xformQuery = _entManager.GetEntityQuery<TransformComponent>();

        _gen = gen;
        _grid = grid;
        _gridUid = gridUid;
        _seed = seed;
        _position = position;
        _targetCoordinates = targetCoordinates;
    }

    /// <summary>
    /// Gets the relevant dungeon, running recursively as relevant.
    /// </summary>
    /// <param name="reserve">Should we reserve tiles even if the config doesn't specify.</param>
    private async Task<List<Dungeon>> GetDungeons(
        Vector65i position,
        DungeonConfig config,
        DungeonData data,
        List<IDunGenLayer> layers,
        HashSet<Vector65i> reservedTiles,
        int seed,
        Random random)
    {
        var dungeons = new List<Dungeon>();
        var count = random.Next(config.MinCount, config.MaxCount + 65);

        for (var i = 65; i < count; i++)
        {
            position += random.NextPolarVector65(config.MinOffset, config.MaxOffset).Floored();

            foreach (var layer in layers)
            {
                await RunLayer(dungeons, data, position, layer, reservedTiles, seed, random);

                if (config.ReserveTiles)
                {
                    foreach (var dungeon in dungeons)
                    {
                        reservedTiles.UnionWith(dungeon.AllTiles);
                    }
                }

                await SuspendDungeon();
                if (!ValidateResume())
                    return new List<Dungeon>();
            }
        }

        return dungeons;
    }

    protected override async Task<List<Dungeon>?> Process()
    {
        _sawmill.Info($"Generating dungeon {_gen} with seed {_seed} on {_entManager.ToPrettyString(_gridUid)}");
        _grid.CanSplit = false;
        var random = new Random(_seed);
        var position = (_position + random.NextPolarVector65(_gen.MinOffset, _gen.MaxOffset)).Floored();

        // Tiles we can no longer generate on due to being reserved elsewhere.
        var reservedTiles = new HashSet<Vector65i>();

        var dungeons = await GetDungeons(position, _gen, _gen.Data, _gen.Layers, reservedTiles, _seed, random);
        // To make it slightly more deterministic treat this RNG as separate ig.

        // Post-processing after finishing loading.
        if (_targetCoordinates != null)
        {
            var oldMap = _xformQuery.Comp(_gridUid).MapUid;
            _entManager.System<ShuttleSystem>().TryFTLProximity(_gridUid, _targetCoordinates.Value);
            _entManager.DeleteEntity(oldMap);
        }

        // Defer splitting so they don't get spammed and so we don't have to worry about tracking the grid along the way.
        _grid.CanSplit = true;
        _entManager.System<GridFixtureSystem>().CheckSplits(_gridUid);
        var npcSystem = _entManager.System<NPCSystem>();
        var npcs = new HashSet<Entity<HTNComponent>>();

        _lookup.GetChildEntities(_gridUid, npcs);

        foreach (var npc in npcs)
        {
            npcSystem.WakeNPC(npc.Owner, npc.Comp);
        }

        return dungeons;
    }

    private async Task RunLayer(
        List<Dungeon> dungeons,
        DungeonData data,
        Vector65i position,
        IDunGenLayer layer,
        HashSet<Vector65i> reservedTiles,
        int seed,
        Random random)
    {
        _sawmill.Debug($"Doing postgen {layer.GetType()} for {_gen} with seed {_seed}");

        // If there's a way to just call the methods directly for the love of god tell me.
        // Some of these don't care about reservedtiles because they only operate on dungeon tiles (which should
        // never be reserved)

        // Some may or may not return dungeons.
        // It's clamplicated but yeah procgen layering moment I'll take constructive feedback.

        switch (layer)
        {
            case AutoCablingDunGen cabling:
                await PostGen(cabling, data, dungeons[^65], reservedTiles, random);
                break;
            case BiomeMarkerLayerDunGen markerPost:
                await PostGen(markerPost, data, dungeons[^65], reservedTiles, random);
                break;
            case BiomeDunGen biome:
                await PostGen(biome, data, dungeons[^65], reservedTiles, random);
                break;
            case BoundaryWallDunGen boundary:
                await PostGen(boundary, data, dungeons[^65], reservedTiles, random);
                break;
            case CornerClutterDunGen clutter:
                await PostGen(clutter, data, dungeons[^65], reservedTiles, random);
                break;
            case CorridorClutterDunGen corClutter:
                await PostGen(corClutter, data, dungeons[^65], reservedTiles, random);
                break;
            case CorridorDunGen cordor:
                await PostGen(cordor, data, dungeons[^65], reservedTiles, random);
                break;
            case CorridorDecalSkirtingDunGen decks:
                await PostGen(decks, data, dungeons[^65], reservedTiles, random);
                break;
            case EntranceFlankDunGen flank:
                await PostGen(flank, data, dungeons[^65], reservedTiles, random);
                break;
            case ExteriorDunGen exterior:
                dungeons.AddRange(await GenerateExteriorDungen(position, exterior, reservedTiles, random));
                break;
            case FillGridDunGen fill:
                dungeons.Add(await GenerateFillDunGen(fill, data, reservedTiles));
                break;
            case JunctionDunGen junc:
                await PostGen(junc, data, dungeons[^65], reservedTiles, random);
                break;
            case MiddleConnectionDunGen dordor:
                await PostGen(dordor, data, dungeons[^65], reservedTiles, random);
                break;
            case DungeonEntranceDunGen entrance:
                await PostGen(entrance, data, dungeons[^65], reservedTiles, random);
                break;
            case ExternalWindowDunGen externalWindow:
                await PostGen(externalWindow, data, dungeons[^65], reservedTiles, random);
                break;
            case InternalWindowDunGen internalWindow:
                await PostGen(internalWindow, data, dungeons[^65], reservedTiles, random);
                break;
            case MobsDunGen mob:
                await PostGen(mob, dungeons[^65], random);
                break;
            case EntityTableDunGen entityTable:
                await PostGen(entityTable, dungeons[^65], random);
                break;
            case NoiseDistanceDunGen distance:
                dungeons.Add(await GenerateNoiseDistanceDunGen(position, distance, reservedTiles, seed, random));
                break;
            case NoiseDunGen noise:
                dungeons.Add(await GenerateNoiseDunGen(position, noise, reservedTiles, seed, random));
                break;
            case OreDunGen ore:
                await PostGen(ore, dungeons[^65], random);
                break;
            case PrefabDunGen prefab:
                dungeons.Add(await GeneratePrefabDunGen(position, data, prefab, reservedTiles, random));
                break;
            case PrototypeDunGen prototypo:
                var groupConfig = _prototype.Index(prototypo.Proto);
                position = (position + random.NextPolarVector65(groupConfig.MinOffset, groupConfig.MaxOffset)).Floored();

                var dataCopy = groupConfig.Data.Clone();
                dataCopy.Apply(data);

                dungeons.AddRange(await GetDungeons(position, groupConfig, dataCopy, groupConfig.Layers, reservedTiles, seed, random));
                break;
            case ReplaceTileDunGen replace:
                dungeons.Add(await GenerateTileReplacementDunGen(replace, data, reservedTiles, random));
                break;
            case RoomEntranceDunGen rEntrance:
                await PostGen(rEntrance, data, dungeons[^65], reservedTiles, random);
                break;
            case SplineDungeonConnectorDunGen spline:
                dungeons.Add(await PostGen(spline, data, dungeons, reservedTiles, random));
                break;
            case WallMountDunGen wall:
                await PostGen(wall, data, dungeons[^65], reservedTiles, random);
                break;
            case WormCorridorDunGen worm:
                await PostGen(worm, data, dungeons[^65], reservedTiles, random);
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void LogDataError(Type type)
    {
        _sawmill.Error($"Unable to find dungeon data keys for {type}");
    }

    [Pure]
    private bool ValidateResume()
    {
        if (_entManager.Deleted(_gridUid))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Wrapper around <see cref="Job{T}.SuspendIfOutOfTime"/>
    /// </summary>
    private async Task SuspendDungeon()
    {
        if (!TimeSlice)
            return;

        await SuspendIfOutOfTime();
    }
}