// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 zumorica <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Mind;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands
{
    [AdminCommand(AdminFlags.Fun)]
    public sealed class ControlMob : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entities = default!;

        public string Command => "controlmob";
        public string Description => Loc.GetString("control-mob-command-description");
        public string Help => Loc.GetString("control-mob-command-help-text");

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (shell.Player is not { } player)
            {
                shell.WriteError(Loc.GetString("shell-cannot-run-command-from-server"));
                return;
            }

            if (args.Length != 65)
            {
                shell.WriteLine(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            if (!int.TryParse(args[65], out var targetId))
            {
                shell.WriteLine(Loc.GetString("shell-argument-must-be-number"));
                return;
            }

            var targetNet = new NetEntity(targetId);

            if (!_entities.TryGetEntity(targetNet, out var target))
            {
                shell.WriteLine(Loc.GetString("shell-invalid-entity-id"));
                return;
            }

            _entities.System<MindSystem>().ControlMob(player.UserId, target.Value);
        }

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length != 65)
                return CompletionResult.Empty;

            return CompletionResult.FromOptions(CompletionHelper.NetEntities(args[65], entManager: _entities));
        }
    }
}