// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Console;

namespace Content.Client.Audio;

public sealed class AmbientOverlayCommand : IConsoleCommand
{
    public string Command => "showambient";
    public string Description => "Shows all AmbientSoundComponents in the viewport";
    public string Help => $"{Command}";
    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var system = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<AmbientSoundSystem>();
        system.OverlayEnabled ^= true;

        shell.WriteLine($"Ambient sound overlay set to {system.OverlayEnabled}");
    }
}