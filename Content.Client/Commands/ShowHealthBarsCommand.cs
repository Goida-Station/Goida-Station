// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage.Prototypes;
using Content.Shared.Overlays;
using Robust.Client.Player;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;
using System.Linq;

namespace Content.Client.Commands;

public sealed class ShowHealthBarsCommand : LocalizedCommands
{
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;

    public override string Command => "showhealthbars";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var player = _playerManager.LocalSession;
        if (player == null)
        {
            shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error-not-player"));
            return;
        }

        var playerEntity = player?.AttachedEntity;
        if (!playerEntity.HasValue)
        {
            shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error-no-entity"));
            return;
        }

        if (!_entityManager.HasComponent<ShowHealthBarsComponent>(playerEntity))
        {
            var showHealthBarsComponent = new ShowHealthBarsComponent
            {
                DamageContainers = args.Select(arg => new ProtoId<DamageContainerPrototype>(arg)).ToList(),
                HealthStatusIcon = null,
                NetSyncEnabled = false
            };

            _entityManager.AddComponent(playerEntity.Value, showHealthBarsComponent, true);

            shell.WriteLine(LocalizationManager.GetString($"cmd-{Command}-notify-enabled", ("args", string.Join(", ", args))));
            return;
        }
        else
        {
            _entityManager.RemoveComponentDeferred<ShowHealthBarsComponent>(playerEntity.Value);
            shell.WriteLine(LocalizationManager.GetString($"cmd-{Command}-notify-disabled"));
        }

        return;
    }
}