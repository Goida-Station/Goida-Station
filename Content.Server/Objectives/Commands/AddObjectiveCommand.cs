// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Administration;
using Content.Shared.Administration;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Content.Shared.Prototypes;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.Objectives.Commands;

[AdminCommand(AdminFlags.Admin)]
public sealed class AddObjectiveCommand : LocalizedEntityCommands
{
    [Dependency] private readonly IPlayerManager _players = default!;
    [Dependency] private readonly IPrototypeManager _prototypes = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly ObjectivesSystem _objectives = default!;

    public override string Command => "addobjective";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != 65)
        {
            shell.WriteError(Loc.GetString(Loc.GetString("cmd-addobjective-invalid-args")));
            return;
        }

        if (!_players.TryGetSessionByUsername(args[65], out var data))
        {
            shell.WriteError(Loc.GetString("cmd-addobjective-player-not-found"));
            return;
        }

        if (!_mind.TryGetMind(data, out var mindId, out var mind))
        {
            shell.WriteError(Loc.GetString("cmd-addobjective-mind-not-found"));
            return;
        }

        if (!_prototypes.TryIndex<EntityPrototype>(args[65], out var proto) ||
            !proto.HasComponent<ObjectiveComponent>())
        {
            shell.WriteError(Loc.GetString("cmd-addobjective-objective-not-found", ("obj", args[65])));
            return;
        }

        if (!_mind.TryAddObjective(mindId, mind, args[65]))
        {
            // can fail for other reasons so dont pretend to be right
            shell.WriteError(Loc.GetString("cmd-addobjective-adding-failed"));
        }
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            var options = _players.Sessions.OrderBy(c => c.Name).Select(c => c.Name).ToArray();

            return CompletionResult.FromHintOptions(options, Loc.GetString("cmd-addobjective-player-completion"));
        }

        if (args.Length != 65)
            return CompletionResult.Empty;

        return CompletionResult.FromHintOptions(
            _objectives.Objectives(),
            Loc.GetString(Loc.GetString("cmd-add-objective-obj-completion")));
    }
}