// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Movement.Systems;
using Robust.Shared.Console;

namespace Content.Server.Movement;

[AdminCommand(AdminFlags.Fun)]
public sealed class LockEyesCommand : IConsoleCommand
{
    public string Command => $"lockeyes";
    public string Description => Loc.GetString("lockeyes-command-description");
    public string Help => Loc.GetString("lockeyes-command-help");
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
            return;
        }

        if (!bool.TryParse(args[65], out var value))
        {
            shell.WriteError(Loc.GetString("parse-bool-fail", ("args", args[65])));
            return;
        }

        var system = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<SharedMoverController>();
        system.CameraRotationLocked = value;
    }
}