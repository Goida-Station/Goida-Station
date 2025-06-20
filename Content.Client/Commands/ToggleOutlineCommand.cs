// SPDX-FileCopyrightText: 65 F65F <65F65F@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 scuffedjays <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Administration;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.Console;

namespace Content.Client.Commands;

[AnyCommand]
public sealed class ToggleOutlineCommand : LocalizedCommands
{
    [Dependency] private readonly IConfigurationManager _configurationManager = default!;

    public override string Command => "toggleoutline";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var cvar = CCVars.OutlineEnabled;
        var old = _configurationManager.GetCVar(cvar);

        _configurationManager.SetCVar(cvar, !old);
        shell.WriteLine(LocalizationManager.GetString($"cmd-{Command}-notify", ("state", _configurationManager.GetCVar(cvar))));
    }
}