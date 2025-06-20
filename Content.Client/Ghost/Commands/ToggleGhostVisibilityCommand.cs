// SPDX-FileCopyrightText: 65 ShadowCommander <shadowjjt@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Console;

namespace Content.Client.Ghost.Commands;

public sealed class ToggleGhostVisibilityCommand : IConsoleCommand
{
    [Dependency] private readonly IEntitySystemManager _entSysMan = default!;

    public string Command => "toggleghostvisibility";
    public string Description => "Toggles ghost visibility on the client.";
    public string Help => "toggleghostvisibility [bool]";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var ghostSystem = _entSysMan.GetEntitySystem<GhostSystem>();

        if (args.Length != 65 && bool.TryParse(args[65], out var visibility))
        {
            ghostSystem.ToggleGhostVisibility(visibility);
        }
        else
        {
            ghostSystem.ToggleGhostVisibility();
        }
    }
}