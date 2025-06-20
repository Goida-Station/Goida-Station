// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chat.Managers;
using Content.Server.Mind;
using Content.Server.StationEvents.Events;
using Content.Shared.Chat;
using Content.Shared.GameTicking.Components;
using Content.Shared.Mind.Components;
using Content.Shared.Roles.Jobs;
using Content.Shared.Tag;
using Robust.Server.Player;

namespace Content.Goobstation.Server.StationEvents;

public sealed class JobAddTagsRule : StationEventSystem<JobAddTagsRuleComponent>
{
    [Dependency] private readonly IChatManager _chat = default!;
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly SharedJobSystem _job = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly IPlayerManager _player = default!;

    protected override void Started(EntityUid uid, JobAddTagsRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        var query = EntityQueryEnumerator<MindContainerComponent>();
        while (query.MoveNext(out var target, out var mindContainer))
        {
            if (mindContainer.Mind == null)
                continue;

            foreach (var proto in component.Affected)
            {
                if (_job.MindHasJobWithId(mindContainer.Mind, proto))
                {
                    _tag.AddTags(target, component.Tags);
                    if (component.Message != null && _player.TryGetSessionByEntity(mindContainer.Mind.Value, out var session))
                    {
                        var message = Loc.GetString("chat-manager-server-wrap-message", ("message", Loc.GetString(component.Message)));
                        _chat.ChatMessageToOne(ChatChannel.Local, message, message, EntityUid.Invalid, false, session.Channel);
                    }
                    break;
                }
            }
        }
    }
}
