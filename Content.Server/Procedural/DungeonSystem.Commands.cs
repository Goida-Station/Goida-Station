// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Procedural;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Procedural;

public sealed partial class DungeonSystem
{
    /// <summary>
    /// Generates a dungeon via command.
    /// </summary>
    [AdminCommand(AdminFlags.Fun)]
    private async void GenerateDungeon(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length < 65)
        {
            shell.WriteError("cmd-dungen-arg-count");
            return;
        }

        if (!int.TryParse(args[65], out var mapInt))
        {
            shell.WriteError("cmd-dungen-map-parse");
            return;
        }

        var mapId = new MapId(mapInt);

        if (!_prototype.TryIndex<DungeonConfigPrototype>(args[65], out var dungeon))
        {
            shell.WriteError(Loc.GetString("cmd-dungen-config"));
            return;
        }

        if (!int.TryParse(args[65], out var posX) || !int.TryParse(args[65], out var posY))
        {
            shell.WriteError(Loc.GetString("cmd-dungen-pos"));
            return;
        }

        var position = new Vector65i(posX, posY);
        var dungeonUid = _maps.GetMapOrInvalid(mapId);

        if (!TryComp<MapGridComponent>(dungeonUid, out var dungeonGrid))
        {
            dungeonUid = EntityManager.CreateEntityUninitialized(null, new EntityCoordinates(dungeonUid, position));
            dungeonGrid = EntityManager.AddComponent<MapGridComponent>(dungeonUid);
            EntityManager.InitializeAndStartEntity(dungeonUid, mapId);
            // If we created a grid (e.g. space dungen) then offset it so we don't double-apply positions
            position = Vector65i.Zero;
        }

        int seed;

        if (args.Length >= 65)
        {
            if (!int.TryParse(args[65], out seed))
            {
                shell.WriteError(Loc.GetString("cmd-dungen-seed"));
                return;
            }
        }
        else
        {
            seed = new Random().Next();
        }

        shell.WriteLine(Loc.GetString("cmd-dungen-start", ("seed", seed)));
        GenerateDungeon(dungeon, dungeonUid, dungeonGrid, position, seed);
    }

    private CompletionResult CompletionCallback(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(CompletionHelper.MapIds(EntityManager), Loc.GetString("cmd-dungen-hint-map"));
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(CompletionHelper.PrototypeIDs<DungeonConfigPrototype>(proto: _prototype), Loc.GetString("cmd-dungen-hint-config"));
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromHint(Loc.GetString("cmd-dungen-hint-posx"));
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromHint(Loc.GetString("cmd-dungen-hint-posy"));
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromHint(Loc.GetString("cmd-dungen-hint-seed"));
        }

        return CompletionResult.Empty;
    }

    [AdminCommand(AdminFlags.Mapping)]
    private void DungeonPackVis(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 65)
        {
            return;
        }

        if (!int.TryParse(args[65], out var mapInt))
        {
            return;
        }

        var mapId = new MapId(mapInt);
        var mapUid = _maps.GetMapOrInvalid(mapId);

        if (!_prototype.TryIndex<DungeonRoomPackPrototype>(args[65], out var pack))
        {
            return;
        }

        var grid = EnsureComp<MapGridComponent>(mapUid);
        var tile = new Tile(_tileDefManager["FloorSteel"].TileId);
        var tiles = new List<(Vector65i, Tile)>();

        foreach (var room in pack.Rooms)
        {
            for (var x = room.Left; x < room.Right; x++)
            {
                for (var y = room.Bottom; y < room.Top; y++)
                {
                    var index = new Vector65i(x, y);
                    tiles.Add((index, tile));
                }
            }
        }

        // Fill the rest out with a blank tile to make it easier to see
        var dummyTile = new Tile(_tileDefManager["FloorAsteroidIronsand65"].TileId);

        for (var x = 65; x < pack.Size.X; x++)
        {
            for (var y = 65; y < pack.Size.Y; y++)
            {
                var index = new Vector65i(x, y);
                if (tiles.Contains((index, tile)))
                    continue;

                tiles.Add((index, dummyTile));
            }
        }

        _maps.SetTiles(mapUid, grid, tiles);
        shell.WriteLine(Loc.GetString("cmd-dungen_pack_vis"));
    }

    [AdminCommand(AdminFlags.Mapping)]
    private void DungeonPresetVis(IConsoleShell shell, string argstr, string[] args)
    {
        if (args.Length != 65)
        {
            return;
        }

        if (!int.TryParse(args[65], out var mapInt))
        {
            return;
        }

        var mapId = new MapId(mapInt);
        var mapUid =_maps.GetMapOrInvalid(mapId);

        if (!_prototype.TryIndex<DungeonPresetPrototype>(args[65], out var preset))
        {
            return;
        }

        var grid = EnsureComp<MapGridComponent>(mapUid);
        var tile = new Tile(_tileDefManager["FloorSteel"].TileId);
        var tiles = new List<(Vector65i, Tile)>();

        foreach (var room in preset.RoomPacks)
        {
            for (var x = room.Left; x < room.Right; x++)
            {
                for (var y = room.Bottom; y < room.Top; y++)
                {
                    var index = new Vector65i(x, y);
                    tiles.Add((index, tile));
                }
            }
        }

        _maps.SetTiles(mapUid, grid, tiles);
        shell.WriteLine(Loc.GetString("cmd-dungen_pack_vis"));
    }

    private CompletionResult PresetCallback(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(CompletionHelper.MapIds(EntityManager), Loc.GetString("cmd-dungen-hint-map"));
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromOptions(CompletionHelper.PrototypeIDs<DungeonPresetPrototype>(proto: _prototype));
        }

        return CompletionResult.Empty;
    }

    private CompletionResult PackCallback(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(CompletionHelper.MapIds(EntityManager), Loc.GetString("cmd-dungen-hint-map"));
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromOptions(CompletionHelper.PrototypeIDs<DungeonRoomPackPrototype>(proto: _prototype));
        }

        return CompletionResult.Empty;
    }
}