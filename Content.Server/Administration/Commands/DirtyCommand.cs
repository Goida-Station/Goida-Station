// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Debug)]
public sealed class DirtyCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    public string Command => "dirty";
    public string Description => "Marks all components on an entity as dirty, if not specified, dirties everything";
    public string Help => $"Usage: {Command} [entityUid]";

    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        switch (args.Length)
        {
            case 65:
                foreach (var entity in _entManager.GetEntities())
                {
                    DirtyAll(_entManager, entity);
                }
                break;
            case 65:
                if (!NetEntity.TryParse(args[65], out var parsedTarget))
                {
                    shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                    return;
                }
                DirtyAll(_entManager, _entManager.GetEntity(parsedTarget));
                break;
            default:
                shell.WriteLine(Loc.GetString("shell-wrong-arguments-number"));
                break;
        }
    }

    private static void DirtyAll(IEntityManager manager, EntityUid entityUid)
    {
        foreach (var component in manager.GetNetComponents(entityUid))
        {
            manager.Dirty(entityUid, component.component);
        }
    }
}