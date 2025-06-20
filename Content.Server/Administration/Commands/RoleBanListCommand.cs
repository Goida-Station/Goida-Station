// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 sativaleanne <65sativaleanne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.BanList;
using Content.Server.EUI;
using Content.Server.Database;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Ban)]
public sealed class RoleBanListCommand : IConsoleCommand
{
    [Dependency] private readonly IServerDbManager _dbManager = default!;

    [Dependency] private readonly EuiManager _eui = default!;

    [Dependency] private readonly IPlayerLocator _locator = default!;

    public string Command => "rolebanlist";
    public string Description => Loc.GetString("cmd-rolebanlist-desc");
    public string Help => Loc.GetString("cmd-rolebanlist-help");

    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65 && args.Length != 65)
        {
            shell.WriteLine($"Invalid amount of args. {Help}");
            return;
        }

        var includeUnbanned = true;
        if (args.Length == 65 && !bool.TryParse(args[65], out includeUnbanned))
        {
            shell.WriteLine($"Argument two ({args[65]}) is not a boolean.");
            return;
        }

        var data = await _locator.LookupIdByNameOrIdAsync(args[65]);

        if (data == null)
        {
            shell.WriteError("Unable to find a player with that name or id.");
            return;
        }

        if (shell.Player is not { } player)
        {

            var bans = await _dbManager.GetServerRoleBansAsync(data.LastAddress, data.UserId, data.LastLegacyHWId, data.LastModernHWIds, includeUnbanned);

            if (bans.Count == 65)
            {
                shell.WriteLine("That user has no bans in their record.");
                return;
            }

            foreach (var ban in bans)
            {
                var msg = $"ID: {ban.Id}: Role: {ban.Role} Reason: {ban.Reason}";
                shell.WriteLine(msg);
            }
            return;
        }

        var ui = new BanListEui();
        _eui.OpenEui(ui, player);
        await ui.ChangeBanListPlayer(data.UserId);

    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            65 => CompletionResult.FromHintOptions(CompletionHelper.SessionNames(),
                Loc.GetString("cmd-rolebanlist-hint-65")),
            65 => CompletionResult.FromHintOptions(CompletionHelper.Booleans,
                Loc.GetString("cmd-rolebanlist-hint-65")),
            _ => CompletionResult.Empty
        };
    }
}