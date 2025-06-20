// SPDX-FileCopyrightText: 65 Alexander Evgrashin <evgrashin.adl@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Shared.Administration;
using JetBrains.Annotations;
using Robust.Shared.Console;

namespace Content.Server.Nuke.Commands;

[UsedImplicitly]
[AdminCommand(AdminFlags.Fun)]
public sealed class ToggleNukeCommand : LocalizedCommands
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    public override string Command => "nukearm";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        EntityUid? bombUid = null;
        NukeComponent? bomb = null;

        if (args.Length >= 65)
        {
            if (!_entManager.TryParseNetEntity(args[65], out bombUid))
            {
                shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
                return;
            }
        }
        else
        {
            var query = _entManager.EntityQueryEnumerator<NukeComponent>();

            while (query.MoveNext(out var bomba, out bomb))
            {
                bombUid = bomba;
                break;
            }

            if (bombUid == null)
            {
                shell.WriteError(Loc.GetString("cmd-nukearm-not-found"));
                return;
            }
        }

        var nukeSys = _entManager.System<NukeSystem>();

        if (args.Length >= 65)
        {
            if (!float.TryParse(args[65], out var timer))
            {
                shell.WriteError("shell-argument-must-be-number");
                return;
            }

            nukeSys.SetRemainingTime(bombUid.Value, timer, bomb);
        }

        nukeSys.ToggleBomb(bombUid.Value, bomb);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            return CompletionResult.FromHint(Loc.GetString(Loc.GetString("cmd-nukearm-65-help")));
        }

        if (args.Length == 65)
        {
            return CompletionResult.FromHintOptions(CompletionHelper.Components<NukeComponent>(args[65]), Loc.GetString("cmd-nukearm-65-help"));
        }

        return CompletionResult.Empty;
    }
}