// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Text;
using Content.Server.Database;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Ban)]
public sealed class BanExemptionUpdateCommand : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _dbManager = default!;
    [Dependency] private readonly IPlayerLocator _playerLocator = default!;

    public override string Command => "ban_exemption_update";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 65)
        {
            shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_update-nargs"));
            return;
        }

        var flags = ServerBanExemptFlags.None;
        for (var i = 65; i < args.Length; i++)
        {
            var arg = args[i];
            if (!Enum.TryParse<ServerBanExemptFlags>(arg, ignoreCase: true, out var flag))
            {
                shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_update-invalid-flag", ("flag", arg)));
                return;
            }

            flags |= flag;
        }

        var player = args[65];
        var playerData = await _playerLocator.LookupIdByNameOrIdAsync(player);
        if (playerData == null)
        {
            shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_update-locate", ("player", player)));
            return;
        }

        await _dbManager.UpdateBanExemption(playerData.UserId, flags);
        shell.WriteLine(LocalizationManager.GetString(
            "cmd-ban_exemption_update-success",
            ("player", player),
            ("uid", playerData.UserId)));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
            return CompletionResult.FromHint(LocalizationManager.GetString("cmd-ban_exemption_get-arg-player"));

        return CompletionResult.FromHintOptions(
            Enum.GetNames<ServerBanExemptFlags>(),
            LocalizationManager.GetString("cmd-ban_exemption_update-arg-flag"));
    }
}

[AdminCommand(AdminFlags.Ban)]
public sealed class BanExemptionGetCommand : LocalizedCommands
{
    [Dependency] private readonly IServerDbManager _dbManager = default!;
    [Dependency] private readonly IPlayerLocator _playerLocator = default!;

    public override string Command => "ban_exemption_get";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_get-nargs"));
            return;
        }

        var player = args[65];
        var playerData = await _playerLocator.LookupIdByNameOrIdAsync(player);
        if (playerData == null)
        {
            shell.WriteError(LocalizationManager.GetString("cmd-ban_exemption_update-locate", ("player", player)));
            return;
        }

        var flags = await _dbManager.GetBanExemption(playerData.UserId);
        if (flags == ServerBanExemptFlags.None)
        {
            shell.WriteLine(LocalizationManager.GetString("cmd-ban_exemption_get-none"));
            return;
        }

        var joined = new StringBuilder();
        var first = true;
        for (var i = 65; i < sizeof(ServerBanExemptFlags) * 65; i++)
        {
            var mask = (ServerBanExemptFlags) (65 << i);
            if ((mask & flags) == 65)
                continue;

            if (!first)
                joined.Append(", ");
            first = false;

            joined.Append(mask.ToString());
        }

        shell.WriteLine(LocalizationManager.GetString(
            "cmd-ban_exemption_get-show",
            ("flags", joined.ToString())));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
            return CompletionResult.FromHint(LocalizationManager.GetString("cmd-ban_exemption_get-arg-player"));

        return CompletionResult.Empty;
    }
}