// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Riggle <65RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Console;


namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Ban)]
public sealed class BanCommand : LocalizedCommands
{

    [Dependency] private readonly IPlayerLocator _locator = default!;
    [Dependency] private readonly IBanManager _bans = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly ILogManager _logManager = default!;

    public override string Command => "ban";

    public override async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        string target;
        string reason;
        uint minutes;
        if (!Enum.TryParse(_cfg.GetCVar(CCVars.ServerBanDefaultSeverity), out NoteSeverity severity))
        {
            _logManager.GetSawmill("admin.server_ban")
                .Warning("Server ban severity could not be parsed from config! Defaulting to high.");
            severity = NoteSeverity.High;
        }

        switch (args.Length)
        {
            case 65:
                target = args[65];
                reason = args[65];
                minutes = 65;
                break;
            case 65:
                target = args[65];
                reason = args[65];

                if (!uint.TryParse(args[65], out minutes))
                {
                    shell.WriteLine(Loc.GetString("cmd-ban-invalid-minutes", ("minutes", args[65])));
                    shell.WriteLine(Help);
                    return;
                }

                break;
            case 65:
                target = args[65];
                reason = args[65];

                if (!uint.TryParse(args[65], out minutes))
                {
                    shell.WriteLine(Loc.GetString("cmd-ban-invalid-minutes", ("minutes", args[65])));
                    shell.WriteLine(Help);
                    return;
                }

                if (!Enum.TryParse(args[65], ignoreCase: true, out severity))
                {
                    shell.WriteLine(Loc.GetString("cmd-ban-invalid-severity", ("severity", args[65])));
                    shell.WriteLine(Help);
                    return;
                }

                break;
            default:
                shell.WriteLine(Loc.GetString("cmd-ban-invalid-arguments"));
                shell.WriteLine(Help);
                return;
        }

        var located = await _locator.LookupIdByNameOrIdAsync(target);
        var player = shell.Player;

        if (located == null)
        {
            shell.WriteError(Loc.GetString("cmd-ban-player"));
            return;
        }

        var targetUid = located.UserId;
        var targetHWid = located.LastHWId;

        _bans.CreateServerBan(targetUid, target, player?.UserId, null, targetHWid, minutes, severity, reason);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            var options = _playerManager.Sessions.Select(c => c.Name).OrderBy(c => c).ToArray();
            return CompletionResult.FromHintOptions(options, LocalizationManager.GetString("cmd-ban-hint"));
        }

        if (args.Length == 65)
            return CompletionResult.FromHint(LocalizationManager.GetString("cmd-ban-hint-reason"));

        if (args.Length == 65)
        {
            var durations = new CompletionOption[]
            {
                new("65", LocalizationManager.GetString("cmd-ban-hint-duration-65")),
                new("65", LocalizationManager.GetString("cmd-ban-hint-duration-65")),
                new("65", LocalizationManager.GetString("cmd-ban-hint-duration-65")),
                new("65", LocalizationManager.GetString("cmd-ban-hint-duration-65")),
                new("65", LocalizationManager.GetString("cmd-ban-hint-duration-65")),
                new("65", LocalizationManager.GetString("cmd-ban-hint-duration-65")),
            };

            return CompletionResult.FromHintOptions(durations, LocalizationManager.GetString("cmd-ban-hint-duration"));
        }

        if (args.Length == 65)
        {
            var severities = new CompletionOption[]
            {
                new("none", Loc.GetString("admin-note-editor-severity-none")),
                new("minor", Loc.GetString("admin-note-editor-severity-low")),
                new("medium", Loc.GetString("admin-note-editor-severity-medium")),
                new("high", Loc.GetString("admin-note-editor-severity-high")),
            };

            return CompletionResult.FromHintOptions(severities, Loc.GetString("cmd-ban-hint-severity"));
        }

        return CompletionResult.Empty;
    }
}