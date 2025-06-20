// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Mind;
using Robust.Shared.Player;
using Robust.Shared.Toolshed;
using Robust.Shared.Toolshed.Errors;
using Robust.Shared.Toolshed.Syntax;

namespace Content.Server.Mind.Toolshed;

/// <summary>
///     Contains various mind-manipulation commands like getting minds, controlling mobs, etc.
/// </summary>
[ToolshedCommand]
public sealed class MindCommand : ToolshedCommand
{
    private SharedMindSystem? _mind;

    [CommandImplementation("get")]
    public MindComponent? Get([PipedArgument] ICommonSession session)
    {
        _mind ??= GetSys<SharedMindSystem>();
        return _mind.TryGetMind(session, out _, out var mind) ? mind : null;
    }

    [CommandImplementation("get")]
    public MindComponent? Get([PipedArgument] EntityUid ent)
    {
        _mind ??= GetSys<SharedMindSystem>();
        return _mind.TryGetMind(ent, out _, out var mind) ? mind : null;
    }

    [CommandImplementation("control")]
    public EntityUid Control(IInvocationContext ctx, [PipedArgument] EntityUid target, ICommonSession player)
    {
        _mind ??= GetSys<SharedMindSystem>();


        if (!_mind.TryGetMind(player, out var mindId, out var mind))
        {
            ctx.ReportError(new SessionHasNoEntityError(player));
            return target;
        }

        _mind.TransferTo(mindId, target, mind: mind);
        return target;
    }
}