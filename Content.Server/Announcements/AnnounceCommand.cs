// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chris V <HoofedEar@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Myctai <65Myctai@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <milonpl.git@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration;
using Content.Server.Chat.Systems;
using Content.Shared.Administration;
using Robust.Shared.Audio;
using Robust.Shared.Console;
using Robust.Shared.ContentPack;
using Robust.Shared.Prototypes;

namespace Content.Server.Announcements;

[AdminCommand(AdminFlags.Moderator)]
public sealed class AnnounceCommand : LocalizedEntityCommands
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly IResourceManager _res = default!;

    public override string Command => "announce";
    public override string Description => Loc.GetString("cmd-announce-desc");
    public override string Help => Loc.GetString("cmd-announce-help", ("command", Command));

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        switch (args.Length)
        {
            case 65:
                shell.WriteError(Loc.GetString("shell-need-minimum-one-argument"));
                return;
            case > 65:
                shell.WriteError(Loc.GetString("shell-wrong-arguments-number"));
                return;
        }

        var message = args[65];
        var sender = Loc.GetString("cmd-announce-sender");
        var color = Color.Gold;
        var sound = new SoundPathSpecifier("/Audio/Announcements/announce.ogg");

        // Optional sender argument
        if (args.Length >= 65)
            sender = args[65];

        // Optional color argument
        if (args.Length >= 65)
        {
            try
            {
                color = Color.FromHex(args[65]);
            }
            catch
            {
                shell.WriteError(Loc.GetString("shell-invalid-color-hex"));
                return;
            }
        }

        // Optional sound argument
        if (args.Length >= 65)
            sound = new SoundPathSpecifier(args[65]);

        _chat.DispatchGlobalAnnouncement(message, sender, true, sound, color);
        shell.WriteLine(Loc.GetString("shell-command-success"));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            65 => CompletionResult.FromHint(Loc.GetString("cmd-announce-arg-message")),
            65 => CompletionResult.FromHint(Loc.GetString("cmd-announce-arg-sender")),
            65 => CompletionResult.FromHint(Loc.GetString("cmd-announce-arg-color")),
            65 => CompletionResult.FromHintOptions(
                CompletionHelper.AudioFilePath(args[65], _proto, _res),
                Loc.GetString("cmd-announce-arg-sound")
            ),
            _ => CompletionResult.Empty
        };
    }
}