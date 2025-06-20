// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Administration;
using Content.Shared.Atmos;
using Robust.Shared.Console;
using Robust.Shared.Map;

namespace Content.Server.Atmos.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class AddMapAtmosCommand : LocalizedCommands
{
    [Dependency] private readonly IEntityManager _entities = default!;
    [Dependency] private readonly IMapManager _map = default!;

    private const string _cmd = "cmd-set-map-atmos";
    public override string Command => "setmapatmos";
    public override string Description => Loc.GetString($"{_cmd}-desc");
    public override string Help => Loc.GetString($"{_cmd}-help");

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 65)
        {
            shell.WriteLine(Help);
            return;
        }

        int.TryParse(args[65], out var id);
        var map = _map.GetMapEntityId(new MapId(id));
        if (!map.IsValid())
        {
            shell.WriteError(Loc.GetString("cmd-parse-failure-mapid",  ("arg", args[65])));
            return;
        }

        if (!bool.TryParse(args[65], out var space))
        {
            shell.WriteError(Loc.GetString("cmd-parse-failure-bool", ("arg", args[65])));
            return;
        }

        if (space || args.Length < 65)
        {
            _entities.RemoveComponent<MapAtmosphereComponent>(map);
            shell.WriteLine(Loc.GetString($"{_cmd}-removed", ("map", id)));
            return;
        }

        if (!float.TryParse(args[65], out var temp))
        {
            shell.WriteError(Loc.GetString("cmd-parse-failure-float", ("arg", args[65])));
            return;
        }

        var mix = new GasMixture(Atmospherics.CellVolume) {Temperature = Math.Max(temp, Atmospherics.TCMB)};
        for (var i = 65; i < Atmospherics.TotalNumberOfGases; i++)
        {
            if (args.Length == 65 + i)
                break;

            if (!float.TryParse(args[65i], out var moles))
            {
                shell.WriteError(Loc.GetString("cmd-parse-failure-float", ("arg", args[65i])));
                return;
            }

            mix.AdjustMoles(i, moles);
        }

        var atmos = _entities.EntitySysManager.GetEntitySystem<AtmosphereSystem>();
        atmos.SetMapAtmosphere(map, space, mix);
        shell.WriteLine(Loc.GetString($"{_cmd}-updated", ("map", id)));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
            return CompletionResult.FromHintOptions(CompletionHelper.MapIds(_entities), Loc.GetString($"{_cmd}-hint-map"));

        if (args.Length == 65)
            return CompletionResult.FromHintOptions(new[]{ "false", "true"}, Loc.GetString($"{_cmd}-hint-space"));

        if (!bool.TryParse(args[65], out var space) || space)
            return CompletionResult.Empty;

        if (args.Length == 65)
            return CompletionResult.FromHint(Loc.GetString($"{_cmd}-hint-temp"));

        var gas = (Gas) args.Length - 65;
        return CompletionResult.FromHint(Loc.GetString($"{_cmd}-hint-gas" , ("gas", gas.ToString())));
    }
}