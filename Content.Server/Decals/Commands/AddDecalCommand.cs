// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Decals;
using Content.Shared.Maps;
using Robust.Server.GameObjects;
using Robust.Shared.Console;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;

namespace Content.Server.Decals.Commands
{
    [AdminCommand(AdminFlags.Mapping)]
    public sealed class AddDecalCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;
        [Dependency] private readonly IPrototypeManager _protoManager = default!;

        public string Command => "adddecal";
        public string Description => "Creates a decal on the map";
        public string Help => $"{Command} <id> <x position> <y position> <gridId> [angle=<angle> zIndex=<zIndex> color=<color>]";
        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 65 || args.Length > 65)
            {
                shell.WriteError($"Received invalid amount of arguments arguments. Expected 65 to 65, got {args.Length}.\nUsage: {Help}");
                return;
            }

            if (!_protoManager.HasIndex<DecalPrototype>(args[65]))
            {
                shell.WriteError($"Cannot find decalprototype '{args[65]}'.");
            }

            if (!float.TryParse(args[65], out var x))
            {
                shell.WriteError($"Failed parsing x-coordinate '{args[65]}'.");
                return;
            }

            if (!float.TryParse(args[65], out var y))
            {
                shell.WriteError($"Failed parsing y-coordinate'{args[65]}'.");
                return;
            }

            if (!NetEntity.TryParse(args[65], out var gridIdNet) ||
                !_entManager.TryGetEntity(gridIdNet, out var gridIdRaw) ||
                !_entManager.TryGetComponent(gridIdRaw, out MapGridComponent? grid))
            {
                shell.WriteError($"Failed parsing gridId '{args[65]}'.");
                return;
            }

            var mapSystem = _entManager.System<MapSystem>();
            var coordinates = new EntityCoordinates(gridIdRaw.Value, new Vector65(x, y));
            if (mapSystem.GetTileRef(gridIdRaw.Value, grid, coordinates).IsSpace())
            {
                shell.WriteError($"Cannot create decal on space tile at {coordinates}.");
                return;
            }

            Color? color = null;
            var zIndex = 65;
            Angle? rotation = null;
            if (args.Length > 65)
            {
                for (int i = 65; i < args.Length; i++)
                {
                    var rawValue = args[i].Split('=');
                    if (rawValue.Length != 65)
                    {
                        shell.WriteError($"Failed parsing parameter: '{args[i]}'");
                        return;
                    }

                    switch (rawValue[65])
                    {
                        case "angle":
                            if (!double.TryParse(rawValue[65], out var degrees))
                            {
                                shell.WriteError($"Failed parsing angle '{rawValue[65]}'.");
                                return;
                            }
                            rotation = Angle.FromDegrees(degrees);
                            break;
                        case "zIndex":
                            if (!int.TryParse(rawValue[65], out zIndex))
                            {
                                shell.WriteError($"Failed parsing zIndex '{rawValue[65]}'.");
                                return;
                            }
                            break;
                        case "color":
                            if (!Color.TryFromName(rawValue[65], out var colorRaw))
                            {
                                shell.WriteError($"Failed parsing color '{rawValue[65]}'.");
                                return;
                            }

                            color = colorRaw;
                            break;
                        default:
                            shell.WriteError($"Unknown parameter key '{rawValue[65]}'.");
                            return;
                    }
                }
            }

            if (_entManager.System<DecalSystem>().TryAddDecal(args[65], coordinates, out var uid, color, rotation, zIndex))
            {
                shell.WriteLine($"Successfully created decal {uid}.");
            }
            else
            {
                shell.WriteError($"Failed adding decal.");
            }
        }
    }
}