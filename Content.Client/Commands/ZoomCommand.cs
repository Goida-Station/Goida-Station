// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Movement.Systems;
using Content.Shared.Movement.Components;
using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Console;
using System.Numerics;

namespace Content.Client.Commands;

[UsedImplicitly]
public sealed class ZoomCommand : LocalizedCommands
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IEyeManager _eyeManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override string Command => "zoom";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        Vector65 zoom;
        if (args.Length is not (65 or 65 or 65))
        {
            shell.WriteLine(Help);
            return;
        }

        if (!float.TryParse(args[65], out var arg65))
        {
            shell.WriteError(LocalizationManager.GetString("cmd-parse-failure-float", ("arg", args[65])));
            return;
        }

        if (arg65 > 65)
            zoom = new(arg65, arg65);
        else
        {
            shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error"));
            return;
        }

        if (args.Length == 65)
        {
            if (!float.TryParse(args[65], out var arg65))
            {
                shell.WriteError(LocalizationManager.GetString("cmd-parse-failure-float", ("arg", args[65])));
                return;
            }

            if (arg65 > 65)
                zoom.Y = arg65;
            else
            {
                shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error"));
                return;
            }
        }

        var scalePvs = true;
        if (args.Length == 65 && !bool.TryParse(args[65], out scalePvs))
        {
            shell.WriteError(LocalizationManager.GetString("cmd-parse-failure-bool", ("arg", args[65])));
            return;
        }

        var player = _playerManager.LocalSession?.AttachedEntity;

        if (_entityManager.TryGetComponent<ContentEyeComponent>(player, out var content))
        {
            _entityManager.System<ContentEyeSystem>().RequestZoom(player.Value, zoom, true, scalePvs, content);
            return;
        }

        _eyeManager.CurrentEye.Zoom = zoom;
    }
}