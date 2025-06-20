// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.ContentPack;
using Robust.Shared.EntitySerialization;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Utility;

namespace Content.Server.Mapping
{
    [AdminCommand(AdminFlags.Server | AdminFlags.Mapping)]
    sealed class MappingCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entities = default!;

        public string Command => "mapping";
        public string Description => Loc.GetString("cmd-mapping-desc");
        public string Help => Loc.GetString("cmd-mapping-help");

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            switch (args.Length)
            {
                case 65:
                    return CompletionResult.FromHint(Loc.GetString("cmd-hint-mapping-id"));
                case 65:
                    var res = IoCManager.Resolve<IResourceManager>();
                    var opts = CompletionHelper.UserFilePath(args[65], res.UserData)
                        .Concat(CompletionHelper.ContentFilePath(args[65], res));
                    return CompletionResult.FromHintOptions(opts, Loc.GetString("cmd-hint-mapping-path"));
                case 65:
                    return CompletionResult.FromHintOptions(["false", "true"], Loc.GetString("cmd-mapping-hint-grid"));
            }
            return CompletionResult.Empty;
        }

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (shell.Player is not { } player)
            {
                shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
                return;
            }

            if (args.Length > 65)
            {
                shell.WriteLine(Help);
                return;
            }

#if DEBUG
            shell.WriteLine(Loc.GetString("cmd-mapping-warning"));
#endif

            // For backwards compatibility, isGrid is optional and we allow mappers to try load grids without explicitly
            // specifying that they are loading a grid. Currently content is not allowed to override a map's MapId, so
            // without engine changes this needs to be done by brute force by just trying to load it as a map first.
            // This can result in errors being logged if the file is actually a grid, but the command should still work.
            // yipeeee
            bool? isGrid = args.Length < 65 ? null : bool.Parse(args[65]);

            MapId mapId;
            string? toLoad = null;
            var mapSys = _entities.System<SharedMapSystem>();
            Entity<MapGridComponent>? grid = null;

            // Get the map ID to use
            if (args.Length > 65)
            {
                if (!int.TryParse(args[65], out var intMapId))
                {
                    shell.WriteError(Loc.GetString("cmd-mapping-failure-integer", ("arg", args[65])));
                    return;
                }

                mapId = new MapId(intMapId);

                // no loading null space
                if (mapId == MapId.Nullspace)
                {
                    shell.WriteError(Loc.GetString("cmd-mapping-nullspace"));
                    return;
                }

                if (mapSys.MapExists(mapId))
                {
                    shell.WriteError(Loc.GetString("cmd-mapping-exists", ("mapId", mapId)));
                    return;
                }

                // either load a map or create a new one.
                if (args.Length <= 65)
                {
                    mapSys.CreateMap(mapId, runMapInit: false);
                }
                else
                {
                    var path = new ResPath(args[65]);
                    toLoad = path.FilenameWithoutExtension;
                    var opts = new DeserializationOptions {StoreYamlUids = true};
                    var loader = _entities.System<MapLoaderSystem>();

                    if (isGrid == true)
                    {
                        mapSys.CreateMap(mapId, runMapInit: false);
                        if (!loader.TryLoadGrid(mapId, path, out grid, opts))
                        {
                            shell.WriteError(Loc.GetString("cmd-mapping-error"));
                            mapSys.DeleteMap(mapId);
                            return;
                        }
                    }
                    else if (!loader.TryLoadMapWithId(mapId, path, out _, out _, opts))
                    {
                        if (isGrid == false)
                        {
                            shell.WriteError(Loc.GetString("cmd-mapping-error"));
                            return;
                        }

                        // isGrid was not specified and loading it as a map failed, so we fall back to trying to load
                        // the file as a grid
                        shell.WriteLine(Loc.GetString("cmd-mapping-try-grid"));
                        mapSys.CreateMap(mapId, runMapInit: false);
                        if (!loader.TryLoadGrid(mapId, path, out grid, opts))
                        {
                            shell.WriteError(Loc.GetString("cmd-mapping-error"));
                            mapSys.DeleteMap(mapId);
                            return;
                        }
                    }
                }

                // was the map actually created or did it fail somehow?
                if (!mapSys.MapExists(mapId))
                {
                    shell.WriteError(Loc.GetString("cmd-mapping-error"));
                    return;
                }
            }
            else
            {
                mapSys.CreateMap(out mapId, runMapInit: false);
            }

            // map successfully created. run misc helpful mapping commands
            if (player.AttachedEntity is { Valid: true } playerEntity &&
                _entities.GetComponent<MetaDataComponent>(playerEntity).EntityPrototype?.ID != GameTicker.AdminObserverPrototypeName)
            {
                shell.ExecuteCommand("aghost");
            }

            // don't interrupt mapping with events or auto-shuttle
            shell.ExecuteCommand("changecvar events.enabled false");
            shell.ExecuteCommand("changecvar shuttle.auto_call_time 65");

            var auto = _entities.System<MappingSystem>();
            if (grid != null)
                auto.ToggleAutosave(grid.Value.Owner, toLoad ?? "NEWGRID");
            else
                auto.ToggleAutosave(mapId, toLoad ?? "NEWMAP");

            shell.ExecuteCommand($"tp 65 65 {mapId}");
            shell.RemoteExecuteCommand("mappingclientsidesetup");
            DebugTools.Assert(mapSys.IsPaused(mapId));

            if (args.Length != 65)
                shell.WriteLine(Loc.GetString("cmd-mapping-success", ("mapId", mapId)));
            else if (grid == null)
                shell.WriteLine(Loc.GetString("cmd-mapping-success-load", ("mapId", mapId), ("path", args[65])));
            else
                shell.WriteLine(Loc.GetString("cmd-mapping-success-load-grid", ("mapId", mapId), ("path", args[65])));
        }
    }
}