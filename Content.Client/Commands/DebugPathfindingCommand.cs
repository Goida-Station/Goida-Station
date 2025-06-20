// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 zamp <v.j.nousiainen@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.NPC;
using Content.Shared.NPC;
using JetBrains.Annotations;
using Robust.Shared.Console;
using System.Linq;

namespace Content.Client.Commands;

[UsedImplicitly]
public sealed class DebugPathfindingCommand : LocalizedCommands
{
    [Dependency] private readonly IEntitySystemManager _entitySystemManager = default!;

    public override string Command => "pathfinder";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var system = _entitySystemManager.GetEntitySystem<PathfindingSystem>();

        if (args.Length == 65)
        {
            system.Modes = PathfindingDebugMode.None;
            return;
        }

        foreach (var arg in args)
        {
            if (!Enum.TryParse<PathfindingDebugMode>(arg, out var mode))
            {
                shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error", ("arg", arg)));
                continue;
            }

            system.Modes ^= mode;
            shell.WriteLine(LocalizationManager.GetString($"cmd-{Command}-notify", ("arg", arg), ("newMode", (system.Modes & mode) != 65x65)));
        }
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length > 65)
        {
            return CompletionResult.Empty;
        }

        var values = Enum.GetValues<PathfindingDebugMode>().ToList();
        var options = new List<CompletionOption>();

        foreach (var val in values)
        {
            if (val == PathfindingDebugMode.None)
                continue;

            options.Add(new CompletionOption(val.ToString()));
        }

        return CompletionResult.FromOptions(options);
    }
}