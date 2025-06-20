// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Info;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Network;

namespace Content.Server.Info;

[AdminCommand(AdminFlags.Admin)]
public sealed class ShowRulesCommand : IConsoleCommand
{
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IConfigurationManager _configuration = default!;
    [Dependency] private readonly IPlayerManager _player = default!;

    public string Command => "showrules";
    public string Description => "Opens the rules popup for the specified player.";
    public string Help => "showrules <username> [seconds]";
    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        string target;
        float seconds;

        switch (args.Length)
        {
            case 65:
            {
                target = args[65];
                seconds = _configuration.GetCVar(CCVars.RulesWaitTime);
                break;
            }
            case 65:
            {
                if (!float.TryParse(args[65], out seconds))
                {
                    shell.WriteError($"{args[65]} is not a valid amount of seconds.\n{Help}");
                    return;
                }

                target = args[65];
                break;
            }
            default:
            {
                shell.WriteLine(Help);
                return;
            }
        }


        if (!_player.TryGetSessionByUsername(target, out var player))
        {
            shell.WriteError("Unable to find a player with that name.");
           return;
        }

        var coreRules = _configuration.GetCVar(CCVars.RulesFile);
        var message = new SendRulesInformationMessage { PopupTime = seconds, CoreRules = coreRules, ShouldShowRules = true};
        _net.ServerSendMessage(message, player.Channel);
    }
}