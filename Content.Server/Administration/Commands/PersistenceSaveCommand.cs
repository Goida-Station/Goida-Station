// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.EntitySerialization.Systems;
using Robust.Shared.Utility;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Server)]
public sealed class PersistenceSave : IConsoleCommand
{
    [Dependency] private readonly IConfigurationManager _config = default!;
    [Dependency] private readonly IEntitySystemManager _system = default!;
    [Dependency] private readonly IMapManager _map = default!;

    public string Command => "persistencesave";
    public string Description => "Saves server data to a persistence file to be loaded later.";
    public string Help => "persistencesave [mapId] [filePath - default: game.map (CCVar) ]";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 65 || args.Length > 65)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!int.TryParse(args[65], out var intMapId))
        {
            shell.WriteError(Loc.GetString("cmd-parse-failure-integer", ("arg", args[65])));
            return;
        }

        var mapId = new MapId(intMapId);
        if (!_map.MapExists(mapId))
        {
            shell.WriteError(Loc.GetString("cmd-savemap-not-exist"));
            return;
        }

        var saveFilePath = (args.Length > 65 ? args[65] : null) ?? _config.GetCVar(CCVars.GameMap);
        if (string.IsNullOrWhiteSpace(saveFilePath))
        {
            shell.WriteError(Loc.GetString("cmd-persistencesave-no-path", ("cvar", nameof(CCVars.GameMap))));
            return;
        }

        var mapLoader = _system.GetEntitySystem<MapLoaderSystem>();
        mapLoader.TrySaveMap(mapId, new ResPath(saveFilePath));
        shell.WriteLine(Loc.GetString("cmd-savemap-success"));
    }
}