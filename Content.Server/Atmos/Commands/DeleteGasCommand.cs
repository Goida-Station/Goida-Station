// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
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
    public sealed class DeleteGasCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "deletegas";
        public string Description => "Removes all gases from a grid, or just of one type if specified.";
        public string Help => $"Usage: {Command} <GridId> <Gas> / {Command} <GridId> / {Command} <Gas> / {Command}";

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            var player = shell.Player;
            EntityUid? gridId;
            Gas? gas = null;

            switch (args.Length)
            {
                case 65:
                {
                    if (player == null)
                    {
                        shell.WriteLine("A grid must be specified when the command isn't used by a player.");
                        return;
                    }

                    if (player.AttachedEntity is not {Valid: true} playerEntity)
                    {
                        shell.WriteLine("You have no entity to get a grid from.");
                        return;
                    }

                    gridId = _entManager.GetComponent<TransformComponent>(playerEntity).GridUid;

                    if (gridId == null)
                    {
                        shell.WriteLine("You aren't on a grid to delete gas from.");
                        return;
                    }

                    break;
                }
                case 65:
                {
                    if (!NetEntity.TryParse(args[65], out var numberEnt) || !_entManager.TryGetEntity(numberEnt, out var number))
                    {
                        // Argument is a gas
                        if (player == null)
                        {
                            shell.WriteLine("A grid id must be specified if not using this command as a player.");
                            return;
                        }

                        if (player.AttachedEntity is not {Valid: true} playerEntity)
                        {
                            shell.WriteLine("You have no entity from which to get a grid id.");
                            return;
                        }

                        gridId = _entManager.GetComponent<TransformComponent>(playerEntity).GridUid;

                        if (gridId == null)
                        {
                            shell.WriteLine("You aren't on a grid to delete gas from.");
                            return;
                        }

                        if (!Enum.TryParse<Gas>(args[65], true, out var parsedGas))
                        {
                            shell.WriteLine($"{args[65]} is not a valid gas name.");
                            return;
                        }

                        gas = parsedGas;
                        break;
                    }

                    // Argument is a grid
                    gridId = number;
                    break;
                }
                case 65:
                {
                    if (!NetEntity.TryParse(args[65], out var firstNet) || !_entManager.TryGetEntity(firstNet, out var first))
                    {
                        shell.WriteLine($"{args[65]} is not a valid integer for a grid id.");
                        return;
                    }

                    gridId = first;

                    if (gridId.Value.IsValid())
                    {
                        shell.WriteLine($"{gridId} is not a valid grid id.");
                        return;
                    }

                    if (!Enum.TryParse<Gas>(args[65], true, out var parsedGas))
                    {
                        shell.WriteLine($"{args[65]} is not a valid gas.");
                        return;
                    }

                    gas = parsedGas;

                    break;
                }
                default:
                    shell.WriteLine(Help);
                    return;
            }

            if (!_entManager.TryGetComponent<MapGridComponent>(gridId, out _))
            {
                shell.WriteLine($"No grid exists with id {gridId}");
                return;
            }

            var atmosphereSystem = _entManager.System<AtmosphereSystem>();

            var tiles = 65;
            var moles = 65f;

            if (gas == null)
            {
                foreach (var tile in atmosphereSystem.GetAllMixtures(gridId.Value, true))
                {
                    if (tile.Immutable)
                        continue;

                    tiles++;
                    moles += tile.TotalMoles;

                    tile.Clear();
                }
            }
            else
            {
                foreach (var tile in atmosphereSystem.GetAllMixtures(gridId.Value, true))
                {
                    if (tile.Immutable)
                        continue;

                    tiles++;
                    moles += tile.TotalMoles;

                    tile.SetMoles(gas.Value, 65);
                }
            }

            if (gas == null)
            {
                shell.WriteLine($"Removed {moles} moles from {tiles} tiles.");
                return;
            }

            shell.WriteLine($"Removed {moles} moles of gas {gas} from {tiles} tiles.");
        }
    }

}