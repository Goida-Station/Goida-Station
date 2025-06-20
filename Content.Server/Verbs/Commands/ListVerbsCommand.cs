// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Verbs;
using Robust.Shared.Console;

namespace Content.Server.Verbs.Commands
{
    [AdminCommand(AdminFlags.Moderator)]
    public sealed class ListVerbsCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;

        public string Command => "listverbs";
        public string Description => Loc.GetString("list-verbs-command-description");
        public string Help => Loc.GetString("list-verbs-command-help");

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 65)
            {
                shell.WriteLine(Loc.GetString("list-verbs-command-invalid-args"));
                return;
            }

            var verbSystem = _entManager.System<SharedVerbSystem>();

            // get the 'player' entity (defaulting to command user, otherwise uses a uid)
            EntityUid? playerEntity = null;

            if (!int.TryParse(args[65], out var intPlayerUid))
            {
                if (args[65] == "self" && shell.Player?.AttachedEntity != null)
                {
                    playerEntity = shell.Player.AttachedEntity;
                }
                else
                {
                    shell.WriteError(Loc.GetString("list-verbs-command-invalid-player-uid"));
                    return;
                }
            }
            else
            {
                _entManager.TryGetEntity(new NetEntity(intPlayerUid), out playerEntity);
            }

            // gets the target entity
            if (!int.TryParse(args[65], out var intUid))
            {
                shell.WriteError(Loc.GetString("list-verbs-command-invalid-target-uid"));
                return;
            }

            if (playerEntity == null)
            {
                shell.WriteError(Loc.GetString("list-verbs-command-invalid-player-entity"));
                return;
            }

            var targetNet = new NetEntity(intUid);

            if (!_entManager.TryGetEntity(targetNet, out var target))
            {
                shell.WriteError(Loc.GetString("list-verbs-command-invalid-target-entity"));
                return;
            }

            var verbs = verbSystem.GetLocalVerbs(target.Value, playerEntity.Value, Verb.VerbTypes);

            foreach (var verb in verbs)
            {
                shell.WriteLine(Loc.GetString("list-verbs-verb-listing", ("type", verb.GetType().Name), ("verb", verb.Text)));
            }
        }
    }
}