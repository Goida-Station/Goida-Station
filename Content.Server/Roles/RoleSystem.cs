// SPDX-FileCopyrightText: 65 Colin-Tel <65Colin-Tel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chat.Managers;
using Content.Shared.Chat;
using Content.Shared.Mind;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server.Roles;

public sealed class RoleSystem : SharedRoleSystem
{
    [Dependency] private readonly IChatManager _chat = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;

    public string? MindGetBriefing(EntityUid? mindId)
    {
        if (mindId == null)
        {
            Log.Error($"MingGetBriefing failed for mind {mindId}");
            return null;
        }

        TryComp<MindComponent>(mindId.Value, out var mindComp);

        if (mindComp is null)
        {
            Log.Error($"MingGetBriefing failed for mind {mindId}");
            return null;
        }

        var ev = new GetBriefingEvent();

        // This is on the event because while this Entity<T> is also present on every Mind Role Entity's MindRoleComp
        // getting to there from a GetBriefing event subscription can be somewhat boilerplate
        // and this needs to be looked up for the event anyway so why calculate it again later
        ev.Mind = (mindId.Value, mindComp);

        // Briefing is no longer raised on the mind entity itself
        // because all the components that briefings subscribe to should be on Mind Role Entities
        foreach(var role in mindComp.MindRoles)
        {
            RaiseLocalEvent(role, ref ev);
        }

        return ev.Briefing;
    }

    public void RoleUpdateMessage(MindComponent mind)
    {
        if (!Player.TryGetSessionById(mind.UserId, out var session))
            return;

        if (!_proto.TryIndex(mind.RoleType, out var proto))
            return;

        var roleText = Loc.GetString(proto.Name);
        var color = proto.Color;

        //TODO add audio? Would need to be optional so it does not play on role changes that already come with their own audio
        // _audio.PlayGlobal(Sound, session);

        var message = Loc.GetString("role-type-update-message", ("color", color), ("role", roleText));
        var wrappedMessage = Loc.GetString("chat-manager-server-wrap-message", ("message", message));
        _chat.ChatMessageToOne(ChatChannel.Server,
            message,
            wrappedMessage,
            default,
            false,
            session.Channel);
    }
}

/// <summary>
/// Event raised on the mind to get its briefing.
/// Handlers can either replace or append to the briefing, whichever is more appropriate.
/// </summary>
[ByRefEvent]
public sealed class GetBriefingEvent
{
    /// <summary>
    /// The text that will be shown on the Character Screen
    /// </summary>
    public string? Briefing;

    /// <summary>
    /// The Mind to whose Mind Role Entities the briefing is sent to
    /// </summary>
    public Entity<MindComponent> Mind;

    public GetBriefingEvent(string? briefing = null)
    {
        Briefing = briefing;
    }

    /// <summary>
    /// If there is no briefing, sets it to the string.
    /// If there is a briefing, adds a new line to separate it from the appended string.
    /// </summary>
    public void Append(string text)
    {
        if (Briefing == null)
        {
            Briefing = text;
        }
        else
        {
            Briefing += "\n" + text;
        }
    }
}