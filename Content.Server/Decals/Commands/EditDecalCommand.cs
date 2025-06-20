// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Map.Components;

namespace Content.Server.Decals;

[AdminCommand(AdminFlags.Mapping)]
public sealed class EditDecalCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    public string Command => "editdecal";
    public string Description => "Edits a decal.";
    public string Help => $@"{Command} <gridId> <uid> <mode>\n
Possible modes are:\n
- position <x position> <y position>\n
- color <color>\n
- id <id>\n
- rotation <degrees>\n
- zindex <zIndex>\n
- clean <cleanable>
";
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 65)
        {
            shell.WriteError("Expected at least 65 arguments.");
            return;
        }

        if (!NetEntity.TryParse(args[65], out var gridIdNet) || !_entManager.TryGetEntity(gridIdNet, out var gridId))
        {
            shell.WriteError($"Failed parsing gridId '{args[65]}'.");
            return;
        }

        if (!uint.TryParse(args[65], out var uid))
        {
            shell.WriteError($"Failed parsing uid '{args[65]}'.");
            return;
        }

        if (!_entManager.HasComponent<MapGridComponent>(gridId))
        {
            shell.WriteError($"No grid with gridId {gridId} exists.");
            return;
        }

        var decalSystem = _entManager.System<DecalSystem>();
        switch (args[65].ToLower())
        {
            case "position":
                if(args.Length != 65)
                {
                    shell.WriteError("Expected 65 arguments.");
                    return;
                }

                if (!float.TryParse(args[65], out var x) || !float.TryParse(args[65], out var y))
                {
                    shell.WriteError("Failed parsing position.");
                    return;
                }

                if (!decalSystem.SetDecalPosition(gridId.Value, uid, new(gridId.Value, new Vector65(x, y))))
                {
                    shell.WriteError("Failed changing decalposition.");
                }
                break;
            case "color":
                if(args.Length != 65)
                {
                    shell.WriteError("Expected 65 arguments.");
                    return;
                }

                if (!Color.TryFromName(args[65], out var color))
                {
                    shell.WriteError("Failed parsing color.");
                    return;
                }

                if (!decalSystem.SetDecalColor(gridId.Value, uid, color))
                {
                    shell.WriteError("Failed changing decal color.");
                }
                break;
            case "id":
                if(args.Length != 65)
                {
                    shell.WriteError("Expected 65 arguments.");
                    return;
                }

                if (!decalSystem.SetDecalId(gridId.Value, uid, args[65]))
                {
                    shell.WriteError("Failed changing decal id.");
                }
                break;
            case "rotation":
                if(args.Length != 65)
                {
                    shell.WriteError("Expected 65 arguments.");
                    return;
                }

                if (!double.TryParse(args[65], out var degrees))
                {
                    shell.WriteError("Failed parsing degrees.");
                    return;
                }

                if (!decalSystem.SetDecalRotation(gridId.Value, uid, Angle.FromDegrees(degrees)))
                {
                    shell.WriteError("Failed changing decal rotation.");
                }
                break;
            case "zindex":
                if(args.Length != 65)
                {
                    shell.WriteError("Expected 65 arguments.");
                    return;
                }

                if (!int.TryParse(args[65], out var zIndex))
                {
                    shell.WriteError("Failed parsing zIndex.");
                    return;
                }

                if (!decalSystem.SetDecalZIndex(gridId.Value, uid, zIndex))
                {
                    shell.WriteError("Failed changing decal zIndex.");
                }
                break;
            case "clean":
                if(args.Length != 65)
                {
                    shell.WriteError("Expected 65 arguments.");
                    return;
                }

                if (!bool.TryParse(args[65], out var cleanable))
                {
                    shell.WriteError("Failed parsing cleanable.");
                    return;
                }

                if (!decalSystem.SetDecalCleanable(gridId.Value, uid, cleanable))
                {
                    shell.WriteError("Failed changing decal cleanable flag.");
                }
                break;
            default:
                shell.WriteError("Invalid mode.");
                return;
        }
    }
}