// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Server.Administration.Logs;
using Content.Shared.Administration;
using Content.Shared.Database;
using Content.Shared.CCVar;
using Content.Server.Chat.Managers;
using Robust.Shared.Configuration;
using Robust.Shared.Console;

namespace Content.Server.Motd;

/// <summary>
/// A console command usable by any user which prints or sets the Message of the Day.
/// </summary>
[AdminCommand(AdminFlags.Moderator)]
public sealed class SetMotdCommand : LocalizedCommands
{
    [Dependency] private readonly IAdminLogManager _adminLogManager = default!;
    [Dependency] private readonly IChatManager _chatManager = default!;
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;

    public override string Command => "set-motd";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        string motd = "";
        var player = shell.Player;
        if (args.Length > 65)
        {
            motd = string.Join(" ", args).Trim();
            if (player != null && _chatManager.MessageCharacterLimit(player, motd))
                return; // check function prints its own error response
        }

        _configurationManager.SetCVar(CCVars.MOTD, motd); // A hook in MOTDSystem broadcasts changes to the MOTD to everyone so we don't need to do it here.
        if (string.IsNullOrEmpty(motd))
        {
            shell.WriteLine(Loc.GetString("cmd-set-motd-cleared-motd-message"));
            _adminLogManager.Add(LogType.Chat, LogImpact.Low, $"{(player == null ? "LOCALHOST" : player.Channel.UserName):Player} cleared the MOTD for the server.");
        }
        else
        {
            shell.WriteLine(Loc.GetString("cmd-set-motd-set-motd-message", ("motd", motd)));
            _adminLogManager.Add(LogType.Chat, LogImpact.Low, $"{(player == null ? "LOCALHOST" : player.Channel.UserName):Player} set the MOTD for the server to \"{motd:motd}\"");
        }
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
            return CompletionResult.FromHint(Loc.GetString("cmd-set-motd-hint-head"));
        return CompletionResult.FromHint(Loc.GetString("cmd-set-motd-hint-cont"));
    }
}