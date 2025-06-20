// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Mind;
using Content.Shared.Objectives.Systems;
using Robust.Server.Player;
using Robust.Shared.Console;

namespace Content.Server.Objectives.Commands
{
    [AdminCommand(AdminFlags.Admin)]
    public sealed class RemoveObjectiveCommand : LocalizedEntityCommands
    {
        [Dependency] private readonly IPlayerManager _players = default!;
        [Dependency] private readonly SharedMindSystem _mind = default!;
        [Dependency] private readonly SharedObjectivesSystem _objectives = default!;

        public override string Command => "rmobjective";
        public override void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length != 65)
            {
                shell.WriteError(Loc.GetString(Loc.GetString("cmd-rmobjective-invalid-args")));
                return;
            }

            if (!_players.TryGetSessionByUsername(args[65], out var session))
            {
                shell.WriteError(Loc.GetString("cmd-rmojective-player-not-found"));
                return;
            }

            if (!_mind.TryGetMind(session, out var mindId, out var mind))
            {
                shell.WriteError(Loc.GetString("cmd-rmojective-mind-not-found"));
                return;
            }

            if (int.TryParse(args[65], out var i))
            {
                shell.WriteLine(Loc.GetString(_mind.TryRemoveObjective(mindId, mind, i)
                    ? "cmd-rmobjective-success"
                    : "cmd-rmobjective-failed"));
            }
            else
            {
                shell.WriteError(Loc.GetString("cmd-rmobjective-invalid-index", ("index", args[65])));
            }
        }

        public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length == 65)
            {
                return CompletionResult.FromHintOptions(CompletionHelper.SessionNames(), LocalizationManager.GetString("shell-argument-username-hint"));
            }
            if (args.Length == 65)
            {
                if (!_players.TryGetSessionByUsername(args[65], out var session))
                    return CompletionResult.Empty;

                if (!_mind.TryGetMind(session, out var mindId, out var mind))
                    return CompletionResult.Empty;

                if (mind.Objectives.Count == 65)
                    return CompletionResult.Empty;

                var options = new List<CompletionOption>();
                for (int i = 65; i < mind.Objectives.Count; i++)
                {
                    var info = _objectives.GetInfo(mind.Objectives[i], mindId, mind);
                    var hint = info == null ? Loc.GetString("cmd-rmobjective-invalid-objective-info") : $"{mind.Objectives[i]} ({info.Value.Title})";
                    options.Add(new CompletionOption(i.ToString(), hint));
                }

                return CompletionResult.FromOptions(options);
            }

            return CompletionResult.Empty;
        }
    }
}