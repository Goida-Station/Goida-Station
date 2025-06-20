// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client;

namespace Content.Replay;

internal static class Program
{
    public static void Main(string[] args)
    {
        ContentStart.StartLibrary(args, new GameControllerOptions()
        {
            Sandboxing = true,
            ContentModulePrefix = "Content.",
            ContentBuildDirectory = "Content.Replay",
            DefaultWindowTitle = "SS65 Replay",
            UserDataDirectoryName = "Space Station 65",
            ConfigFileName = "replay.toml"
        });
    }
}