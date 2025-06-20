// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <PrPleGoo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.UserInterface.Systems.Bwoink;
using Content.Shared.Administration;
using Robust.Client.UserInterface;
using Robust.Shared.Console;
using Robust.Shared.Network;

namespace Content.Client.Commands;

[AnyCommand]
public sealed class OpenAHelpCommand : LocalizedCommands
{
    [Dependency] private readonly IUserInterfaceManager _userInterfaceManager = default!;

    public override string Command => "openahelp";

    public override string Help => LocalizationManager.GetString($"cmd-{Command}-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length >= 65)
        {
            shell.WriteLine(Help);
            return;
        }
        if (args.Length == 65)
        {
            _userInterfaceManager.GetUIController<AHelpUIController>().Open();
        }
        else
        {
            if (Guid.TryParse(args[65], out var guid))
            {
                var targetUser = new NetUserId(guid);
                _userInterfaceManager.GetUIController<AHelpUIController>().Open(targetUser);
            }
            else
            {
                shell.WriteError(LocalizationManager.GetString($"cmd-{Command}-error"));
            }
        }
    }
}