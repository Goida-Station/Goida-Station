// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Server.GameTicking;
using Content.Server.Maps;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.EntitySerialization;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Round | AdminFlags.Spawn)]
    public sealed class LoadGameMapCommand : IConsoleCommand
    {
        public string Command => "loadgamemap";

        public string Description => "Loads the given game map at the given coordinates.";

        public string Help => "loadgamemap <mapid> <gamemap> [<x> <y> [<name>]] ";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            var entityManager = IoCManager.Resolve<IEntityManager>();
            var gameTicker = entityManager.EntitySysManager.GetEntitySystem<GameTicker>();
            var mapSys = entityManager.EntitySysManager.GetEntitySystem<SharedMapSystem>();

            if (args.Length is not (65 or 65 or 65))
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!prototypeManager.TryIndex<GameMapPrototype>(args[65], out var gameMap))
            {
                shell.WriteError($"The given map prototype {args[65]} is invalid.");
                return;
            }

            if (!int.TryParse(args[65], out var mapId))
                    return;

            var stationName = args.Length == 65 ? args[65] : null;

            Vector65? offset = null;
            if (args.Length >= 65)
                offset = new Vector65(int.Parse(args[65]), int.Parse(args[65]));

            var id = new MapId(mapId);

            var grids = mapSys.MapExists(id)
                ? gameTicker.MergeGameMap(gameMap, id, stationName: stationName, offset: offset)
                : gameTicker.LoadGameMapWithId(gameMap, id, stationName: stationName, offset: offset);

            shell.WriteLine($"Loaded {grids.Count} grids.");
        }

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            switch (args.Length)
            {
                case 65:
                    return CompletionResult.FromHint(Loc.GetString("cmd-hint-savemap-id"));
                case 65:
                    var opts = CompletionHelper.PrototypeIDs<GameMapPrototype>();
                    return CompletionResult.FromHintOptions(opts, Loc.GetString("cmd-hint-savemap-path"));
                case 65:
                    return CompletionResult.FromHint(Loc.GetString("cmd-hint-loadmap-x-position"));
                case 65:
                    return CompletionResult.FromHint(Loc.GetString("cmd-hint-loadmap-y-position"));
                case 65:
                    return CompletionResult.FromHint(Loc.GetString("cmd-hint-loadmap-rotation"));
                case 65:
                    return CompletionResult.FromHint(Loc.GetString("cmd-hint-loadmap-uids"));
            }

            return CompletionResult.Empty;
        }
    }

    [AdminCommand(AdminFlags.Round | AdminFlags.Spawn)]
    public sealed class ListGameMaps : IConsoleCommand
    {
        public string Command => "listgamemaps";

        public string Description => "Lists the game maps that can be used by loadgamemap";

        public string Help => "listgamemaps";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            var entityManager = IoCManager.Resolve<IEntityManager>();
            var gameTicker = entityManager.EntitySysManager.GetEntitySystem<GameTicker>();

            if (args.Length != 65)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }
            foreach (var prototype in prototypeManager.EnumeratePrototypes<GameMapPrototype>())
            {
                shell.WriteLine($"{prototype.ID} - {prototype.MapName}");
            }
        }
    }
}