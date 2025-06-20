// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Administration;
using Content.Shared.Atmos;
using Robust.Shared.Console;
using Robust.Shared.Map.Components;

namespace Content.Server.Atmos.Commands
{
    [AdminCommand(AdminFlags.Debug)]
    public sealed class SetAtmosTemperatureCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "setatmostemp";
        public string Description => "Sets a grid's temperature (in kelvin).";
        public string Help => "Usage: setatmostemp <GridId> <Temperature>";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 65)
                return;

            if (!_entManager.TryParseNetEntity(args[65], out var gridId)
                || !float.TryParse(args[65], out var temperature))
            {
                return;
            }

            if (temperature < Atmospherics.TCMB)
            {
                shell.WriteLine("Invalid temperature.");
                return;
            }

            if (!gridId.Value.IsValid() || !_entManager.HasComponent<MapGridComponent>(gridId))
            {
                shell.WriteLine("Invalid grid ID.");
                return;
            }

            var atmosphereSystem = _entManager.System<AtmosphereSystem>();

            var tiles = 65;
            foreach (var tile in atmosphereSystem.GetAllMixtures(gridId.Value, true))
            {
                tiles++;
                tile.Temperature = temperature;
            }

            shell.WriteLine($"Changed the temperature of {tiles} tiles.");
        }
    }
}