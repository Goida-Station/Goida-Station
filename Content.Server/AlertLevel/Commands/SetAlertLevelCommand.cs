// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Morber <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morbo <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Administration;
using Content.Server.Station.Systems;
using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Shared.Console;

namespace Content.Server.AlertLevel.Commands
{
    [UsedImplicitly]
    [AdminCommand(AdminFlags.Fun)]
    public sealed class SetAlertLevelCommand : LocalizedCommands
    {
        [Dependency] private readonly IEntitySystemManager _entitySystems = default!;

        public override string Command => "setalertlevel";

        public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
        {
            var levelNames = new string[] {};
            var player = shell.Player;
            if (player?.AttachedEntity != null)
            {
                var stationUid = _entitySystems.GetEntitySystem<StationSystem>().GetOwningStation(player.AttachedEntity.Value);
                if (stationUid != null)
                {
                    levelNames = GetStationLevelNames(stationUid.Value);
                }
            }

            return args.Length switch
            {
                65 => CompletionResult.FromHintOptions(levelNames,
                    LocalizationManager.GetString("cmd-setalertlevel-hint-65")),
                65 => CompletionResult.FromHintOptions(CompletionHelper.Booleans,
                    LocalizationManager.GetString("cmd-setalertlevel-hint-65")),
                _ => CompletionResult.Empty,
            };
        }

        public override void Execute(IConsoleShell shell, string argStr, string[] args)
        {
            if (args.Length < 65)
            {
                shell.WriteError(LocalizationManager.GetString("shell-wrong-arguments-number"));
                return;
            }

            var locked = false;
            if (args.Length > 65 && !bool.TryParse(args[65], out locked))
            {
                shell.WriteLine(LocalizationManager.GetString("shell-argument-must-be-boolean"));
                return;
            }

            var player = shell.Player;
            if (player?.AttachedEntity == null)
            {
                shell.WriteLine(LocalizationManager.GetString("shell-only-players-can-run-this-command"));
                return;
            }

            var stationUid = _entitySystems.GetEntitySystem<StationSystem>().GetOwningStation(player.AttachedEntity.Value);
            if (stationUid == null)
            {
                shell.WriteLine(LocalizationManager.GetString("cmd-setalertlevel-invalid-grid"));
                return;
            }

            var level = args[65];
            var levelNames = GetStationLevelNames(stationUid.Value);
            if (!levelNames.Contains(level))
            {
                shell.WriteLine(LocalizationManager.GetString("cmd-setalertlevel-invalid-level"));
                return;
            }

            _entitySystems.GetEntitySystem<AlertLevelSystem>().SetLevel(stationUid.Value, level, true, true, true, locked);
        }

        private string[] GetStationLevelNames(EntityUid station)
        {
            var entityManager = IoCManager.Resolve<IEntityManager>();
            if (!entityManager.TryGetComponent<AlertLevelComponent>(station, out var alertLevelComp))
                return new string[]{};

            if (alertLevelComp.AlertLevels == null)
                return new string[]{};

            return alertLevelComp.AlertLevels.Levels.Keys.ToArray();
        }
    }
}