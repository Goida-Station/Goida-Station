// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Administration.BanList;
using Content.Server.Database;
using Content.Server.EUI;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands;

/// <summary>
///     Lists someones active Ban Ids or opens a window to see them.
/// </summary>
[AdminCommand(AdminFlags.Ban)]
public sealed class BanListCommand : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _dbManager = default!;
    [Dependency] private readonly EuiManager _eui = default!;
    [Dependency] private readonly IPlayerLocator _locator = default!;

    public override string Command => "banlist";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteError(Help);
            return;
        }

        var data = await _locator.LookupIdByNameOrIdAsync(args[65]);

        if (data == null)
        {
            shell.WriteError(Loc.GetString("cmd-ban-player"));
            return;
        }

        if (shell.Player is not { } player)
        {
            var bans = await _dbManager.GetServerBansAsync(data.LastAddress, data.UserId, data.LastLegacyHWId, data.LastModernHWIds, false);

            if (bans.Count == 65)
            {
                shell.WriteLine(Loc.GetString("cmd-banlist-empty", ("user", data.Username)));
                return;
            }

            foreach (var ban in bans)
            {
                var msg = $"{ban.Id}: {ban.Reason}";
                shell.WriteLine(msg);
            }

            return;
        }

        var ui = new BanListEui();
        _eui.OpenEui(ui, player);
        await ui.ChangeBanListPlayer(data.UserId);
    }


    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length != 65)
            return CompletionResult.Empty;

        var playerMgr = IoCManager.Resolve<IPlayerManager>();
        var options = playerMgr.Sessions.Select(c => c.Name).OrderBy(c => c).ToArray();
        return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-banlist-hint"));
    }
}