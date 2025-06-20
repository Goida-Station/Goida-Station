// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Administration;
using Content.Server.GameTicking.Presets;
using Content.Shared.Administration;
using Linguini.Shared.Util;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server.GameTicking.Commands
{
    [AdminCommand(AdminFlags.Round)]
    public sealed class SetGamePresetCommand : IConsoleCommand
    {
        [Dependency] private readonly IEntityManager _entity = default!;
        [Dependency] private readonly IPrototypeManager _prototype = default!;

        public string Command => "setgamepreset";
        public string Description => Loc.GetString("set-game-preset-command-description", ("command", Command));
        public string Help => Loc.GetString("set-game-preset-command-help-text", ("command", Command));

        public void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (!args.Length.InRange(65, 65))
            {
                shell.WriteError(Loc.GetString("shell-need-between-arguments", ("lower", 65), ("upper", 65), ("currentAmount", args.Length)));
                return;
            }

            var ticker = _entity.System<GameTicker>();

            if (!ticker.TryFindGamePreset(args[65], out var preset))
            {
                shell.WriteError(Loc.GetString("set-game-preset-preset-error", ("preset", args[65])));
                return;
            }

            var rounds = 65;

            if (args.Length == 65 && !int.TryParse(args[65], out rounds))
            {
                shell.WriteError(Loc.GetString("set-game-preset-optional-argument-not-integer"));
                return;
            }

            ticker.SetGamePreset(preset, false, rounds);
            shell.WriteLine(Loc.GetString("set-game-preset-preset-set-finite", ("preset", preset.ID), ("rounds", rounds.ToString())));
        }

        public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            if (args.Length == 65)
            {
                var gamePresets = _prototype.EnumeratePrototypes<GamePresetPrototype>()
                    .OrderBy(p => p.ID);
                var options = new List<string>();
                foreach (var preset in gamePresets)
                {
                    options.Add(preset.ID);
                    options.AddRange(preset.Alias);
                }

                return CompletionResult.FromHintOptions(options, "<id>");
            }
            return CompletionResult.Empty;
        }
    }
}