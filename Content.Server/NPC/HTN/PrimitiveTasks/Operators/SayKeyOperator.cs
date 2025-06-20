// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Tornado Tech <65Tornado-Technology@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Tim <timfalken@hotmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chat.Systems;
using Content.Shared.Chat;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators;

public sealed partial class SayKeyOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    private ChatSystem _chat = default!;

    [DataField(required: true)]
    public string Key = string.Empty;

    /// <summary>
    /// Whether to hide message from chat window and logs.
    /// </summary>
    [DataField]
    public bool Hidden;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);

        _chat = sysManager.GetEntitySystem<ChatSystem>();
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue<object>(Key, out var value, _entManager))
            return HTNOperatorStatus.Failed;

        var @string = value.ToString();
        if (@string is not { })
            return HTNOperatorStatus.Failed;

        var speaker = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        _chat.TrySendInGameICMessage(speaker, @string, InGameICChatType.Speak, hideChat: Hidden, hideLog: Hidden);

        return base.Update(blackboard, frameTime);
    }
}
