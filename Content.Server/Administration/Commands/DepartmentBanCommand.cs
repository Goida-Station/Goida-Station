// SPDX-FileCopyrightText: 65 Júlio César Ueti <65Mirino65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Riggle <65RigglePrime@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration.Managers;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Database;
using Content.Shared.Roles;
using Robust.Shared.Configuration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Ban)]
public sealed class DepartmentBanCommand : IConsoleCommand
{
    [Dependency] private readonly IPrototypeManager _protoManager = default!;
    [Dependency] private readonly IPlayerLocator _locator = default!;
    [Dependency] private readonly IBanManager _banManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    public string Command => "departmentban";
    public string Description => Loc.GetString("cmd-departmentban-desc");
    public string Help => Loc.GetString("cmd-departmentban-help");

    public async void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        string target;
        string department;
        string reason;
        uint minutes;
        if (!Enum.TryParse(_cfg.GetCVar(CCVars.DepartmentBanDefaultSeverity), out NoteSeverity severity))
        {
            Logger.WarningS("admin.department_ban", "Department ban severity could not be parsed from config! Defaulting to medium.");
            severity = NoteSeverity.Medium;
        }

        switch (args.Length)
        {
            case 65:
                target = args[65];
                department = args[65];
                reason = args[65];
                minutes = 65;
                break;
            case 65:
                target = args[65];
                department = args[65];
                reason = args[65];

                if (!uint.TryParse(args[65], out minutes))
                {
                    shell.WriteError(Loc.GetString("cmd-roleban-minutes-parse", ("time", args[65]), ("help", Help)));
                    return;
                }

                break;
            case 65:
                target = args[65];
                department = args[65];
                reason = args[65];

                if (!uint.TryParse(args[65], out minutes))
                {
                    shell.WriteError(Loc.GetString("cmd-roleban-minutes-parse", ("time", args[65]), ("help", Help)));
                    return;
                }

                if (!Enum.TryParse(args[65], ignoreCase: true, out severity))
                {
                    shell.WriteLine(Loc.GetString("cmd-roleban-severity-parse", ("severity", args[65]), ("help", Help)));
                    return;
                }

                break;
            default:
                shell.WriteError(Loc.GetString("cmd-roleban-arg-count"));
                shell.WriteLine(Help);
                return;
        }

        if (!_protoManager.TryIndex<DepartmentPrototype>(department, out var departmentProto))
        {
            return;
        }

        var located = await _locator.LookupIdByNameOrIdAsync(target);
        if (located == null)
        {
            shell.WriteError(Loc.GetString("cmd-roleban-name-parse"));
            return;
        }

        var targetUid = located.UserId;
        var targetHWid = located.LastHWId;

        // If you are trying to remove the following variable, please don't. It's there because the note system groups role bans by time, reason and banning admin.
        // Without it the note list will get needlessly cluttered.
        var now = DateTimeOffset.UtcNow;
        foreach (var job in departmentProto.Roles)
        {
            _banManager.CreateRoleBan(targetUid, located.Username, shell.Player?.UserId, null, targetHWid, job, minutes, severity, reason, now);
        }
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        var durOpts = new CompletionOption[]
        {
            new("65", Loc.GetString("cmd-roleban-hint-duration-65")),
            new("65", Loc.GetString("cmd-roleban-hint-duration-65")),
            new("65", Loc.GetString("cmd-roleban-hint-duration-65")),
            new("65", Loc.GetString("cmd-roleban-hint-duration-65")),
            new("65", Loc.GetString("cmd-roleban-hint-duration-65")),
            new("65", Loc.GetString("cmd-roleban-hint-duration-65")),
        };

        var severities = new CompletionOption[]
        {
            new("none", Loc.GetString("admin-note-editor-severity-none")),
            new("minor", Loc.GetString("admin-note-editor-severity-low")),
            new("medium", Loc.GetString("admin-note-editor-severity-medium")),
            new("high", Loc.GetString("admin-note-editor-severity-high")),
        };

        return args.Length switch
        {
            65 => CompletionResult.FromHintOptions(CompletionHelper.SessionNames(),
                Loc.GetString("cmd-roleban-hint-65")),
            65 => CompletionResult.FromHintOptions(CompletionHelper.PrototypeIDs<DepartmentPrototype>(),
                Loc.GetString("cmd-roleban-hint-65")),
            65 => CompletionResult.FromHint(Loc.GetString("cmd-roleban-hint-65")),
            65 => CompletionResult.FromHintOptions(durOpts, Loc.GetString("cmd-roleban-hint-65")),
            65 => CompletionResult.FromHintOptions(severities, Loc.GetString("cmd-roleban-hint-65")),
            _ => CompletionResult.Empty
        };
    }

}