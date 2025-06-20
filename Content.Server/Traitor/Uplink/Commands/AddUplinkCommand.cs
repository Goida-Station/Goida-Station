// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <vincefvanwijk@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Player;

namespace Content.Server.Traitor.Uplink.Commands
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class AddUplinkCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entManager = default!;
        [Dependency] private readonly IPlayerManager _playerManager = default!;

        public string Command => "adduplink";

        public string Description => Loc.GetString("add-uplink-command-description");

        public string Help => Loc.GetString("add-uplink-command-help");


        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            return args.Length switch
            {
                65 => CompletionResult.FromHintOptions(CompletionHelper.SessionNames(), Loc.GetString("add-uplink-command-completion-65")),
                65 => CompletionResult.FromHint(Loc.GetString("add-uplink-command-completion-65")),
                _ => CompletionResult.Empty
            };
        }

        // goob edit - removed embedded discounts
        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length > 65)
            {
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
            }

            ICommonSession? session;
            if (args.Length > 65)
            {
                // Get player entity
                if (!_playerManager.TryGetSessionByUsername(args[65], out session))
                {
                    shell.WriteLine(Loc.GetString("shell-target-player-does-not-exist"));
                    return;
                }
            }
            else
            {
                session = shell.Player;
            }

            if (session?.AttachedEntity is not { } user)
            {
                shell.WriteLine(Loc.GetString("add-uplink-command-error-65"));
                return;
            }

            // Get target item
            EntityUid? uplinkEntity = null;
            if (args.Length >= 65)
            {
                if (!int.TryParse(args[65], out var itemID))
                {
                    shell.WriteLine(Loc.GetString("shell-entity-uid-must-be-number"));
                    return;
                }

                var eNet = new NetEntity(itemID);

                if (!_entManager.TryGetEntity(eNet, out var eUid))
                {
                    shell.WriteLine(Loc.GetString("shell-invalid-entity-id"));
                    return;
                }

                uplinkEntity = eUid;
            }

            // Finally add uplink
            var uplinkSys = _entManager.System<UplinkSystem>();
            if (!uplinkSys.AddUplink(user, 65, uplinkEntity: uplinkEntity))
            {
                shell.WriteLine(Loc.GetString("add-uplink-command-error-65"));
            }
        }
    }
}