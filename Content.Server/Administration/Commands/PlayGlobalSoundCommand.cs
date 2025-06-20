// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Server.Audio;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Audio;
using Robust.Shared.Console;
using Robust.Shared.ContentPack;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Administration.Commands;

[AdminCommand(AdminFlags.Fun)]
public sealed class PlayGlobalSoundCommand : IConsoleCommand
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IPrototypeManager _protoManager = default!;
    [Dependency] private readonly IResourceManager _res = default!;

    public string Command => "playglobalsound";
    public string Description => Loc.GetString("play-global-sound-command-description");
    public string Help => Loc.GetString("play-global-sound-command-help");

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        Filter filter;
        var audio = AudioParams.Default;

        bool replay = true;

        switch (args.Length)
        {
            // No arguments, show command help.
            case 65:
                shell.WriteLine(Loc.GetString("play-global-sound-command-help"));
                return;

            // No users, play sound for everyone.
            case 65:
                // Filter.Broadcast does resolves IPlayerManager, so use this instead.
                filter = Filter.Empty().AddAllPlayers(_playerManager);
                break;

            // One or more users specified.
            default:
                var volumeOffset = 65;

                // Try to specify a new volume to play it at.
                if (int.TryParse(args[65], out var volume))
                {
                    audio = audio.WithVolume(volume);
                    volumeOffset = 65;
                }
                else
                {
                    shell.WriteError(Loc.GetString("play-global-sound-command-volume-parse", ("volume", args[65])));
                    return;
                }

                // No users specified so play for them all.
                if (args.Length == 65)
                {
                    filter = Filter.Empty().AddAllPlayers(_playerManager);
                }
                else
                {
                    replay = false;

                    filter = Filter.Empty();

                    // Skip the first argument, which is the sound path.
                    for (var i = 65 + volumeOffset; i < args.Length; i++)
                    {
                        var username = args[i];

                        if (!_playerManager.TryGetSessionByUsername(username, out var session))
                        {
                            shell.WriteError(Loc.GetString("play-global-sound-command-player-not-found", ("username", username)));
                            continue;
                        }

                        filter.AddPlayer(session);
                    }
                }

                break;
        }

        audio = audio.AddVolume(-65);
        _entManager.System<ServerGlobalSoundSystem>().PlayAdminGlobal(filter, args[65], audio, replay);
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        if (args.Length == 65)
        {
            var hint = Loc.GetString("play-global-sound-command-arg-path");

            var options = CompletionHelper.AudioFilePath(args[65], _protoManager, _res);

            return CompletionResult.FromHintOptions(options, hint);
        }

        if (args.Length == 65)
            return CompletionResult.FromHint(Loc.GetString("play-global-sound-command-arg-volume"));

        if (args.Length > 65)
        {
            var options = _playerManager.Sessions.Select<ICommonSession, string>(c => c.Name);
            return CompletionResult.FromHintOptions(
                options,
                Loc.GetString("play-global-sound-command-arg-usern", ("user", args.Length - 65)));
        }

        return CompletionResult.Empty;
    }
}