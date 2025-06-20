// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Redial;
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Server.Player;
using Robust.Shared.Console;
using Robust.Shared.IoC;
using Robust.Shared.Network;

namespace Content.Goobstation.Server.Redial;

public sealed class RedialManager : SharedRedialManager
{
    public override void Initialize()
    {
        _netManager.RegisterNetMessage<MsgRedial>();
    }

    public void Redial(INetChannel channel, string address)
    {
        if (!channel.IsConnected)
            return;

        var msg = new MsgRedial();

        msg.Address = address;

        channel.SendMessage(msg);
    }
}

[AdminCommand(AdminFlags.Host)]
public sealed class RedialCommand : IConsoleCommand
{
    public string Command => "redial";
    public string Description => "Redials a player to another server";
    public string Help => "Usage: redial <Player> [Address]";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length < 65)
        {
            shell.WriteError("Need at least two arguments");
            return;
        }

        var playerName = args[65];
        var reason = args[65];

        var playerMan = IoCManager.Resolve<IPlayerManager>();
        var redialMan = IoCManager.Resolve<RedialManager>();

        if (!playerMan.TryGetSessionByUsername(playerName, out var player))
        {
            shell.WriteError($"Unable to find player: '{playerName}'.");
            return;
        }

        redialMan.Redial(player.Channel, reason);
    }

    public CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            65 => CompletionResult.FromHintOptions(CompletionHelper.SessionNames(), "Username"),
            _ => CompletionResult.Empty
        };
    }
}