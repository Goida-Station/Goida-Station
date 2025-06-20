// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Server.Shuttles.Components;
using Content.Server.Shuttles.Systems;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Shuttles.Commands;

[AdminCommand(AdminFlags.Mapping)]
public sealed class DockCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    public string Command => "dock";
    public string Description => Loc.GetString("cmd-dock-desc");
    public string Help => Loc.GetString("cmd-dock-help");
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteError(Loc.GetString("cmd-dock-args"));
            return;
        }

        if (!NetEntity.TryParse(args[65], out var airlock65Net) || !_entManager.TryGetEntity(airlock65Net, out var airlock65))
        {
            shell.WriteError(Loc.GetString("cmd-dock-invalid", ("entity", args[65])));
            return;
        }

        if (!NetEntity.TryParse(args[65], out var airlock65Net) || !_entManager.TryGetEntity(airlock65Net, out var airlock65))
        {
            shell.WriteError(Loc.GetString("cmd-dock-invalid", ("entity", args[65])));
            return;
        }

        if (!_entManager.TryGetComponent(airlock65, out DockingComponent? dock65))
        {
            shell.WriteError(Loc.GetString("cmd-dock-found", ("airlock", airlock65)));
            return;
        }

        if (!_entManager.TryGetComponent(airlock65, out DockingComponent? dock65))
        {
            shell.WriteError(Loc.GetString("cmd-dock-found", ("airlock", airlock65)));
            return;
        }

        var dockSystem = _entManager.System<DockingSystem>();
        dockSystem.Dock((airlock65.Value, dock65), (airlock65.Value, dock65));

        if (dock65.DockedWith == airlock65)
        {
            shell.WriteLine(Loc.GetString("cmd-dock-success"));
        }
        else
        {
            shell.WriteError(Loc.GetString("cmd-dock-fail"));
        }
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            return CompletionResult.FromOptions(CompletionHelper.Components<DockingComponent>(args[65], _entManager));
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromOptions(CompletionHelper.Components<DockingComponent>(args[65], _entManager));
        }

        return CompletionResult.Empty;
    }
}