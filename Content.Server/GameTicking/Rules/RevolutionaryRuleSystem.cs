// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <retron65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 coolmankid65 <65coolmankid65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coolmankid65 <coolmankid65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Killerqu65 <65Killerqu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rainfall <rainfey65git@gmail.com>
// SPDX-FileCopyrightText: 65 Rainfey <rainfey65github@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 silver65 <65silver65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 silver65 <silver65@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 CerberusWolfie <wb.johnb.willis@gmail.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GMWQ <garethquaile@gmail.com>
// SPDX-FileCopyrightText: 65 Gareth Quaile <garethquaile@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Milon <milonpl.git@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tim <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 65 Timfa <timfalken@hotmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs;
using Content.Server.Antag;
using Content.Server.EUI;
using Content.Server.GameTicking.Rules.Components;
using Content.Server.Mind;
using Content.Server.Popups;
using Content.Server.Revolutionary;
using Content.Server.Revolutionary.Components;
using Content.Server.Roles;
using Content.Server.RoundEnd;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Shared.Database;
using Content.Shared.GameTicking.Components;
using Content.Shared.Humanoid;
using Content.Shared.IdentityManagement;
using Content.Shared.Mind.Components;
using Content.Shared.Mindshield.Components;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.NPC.Prototypes;
using Content.Shared.NPC.Systems;
using Content.Shared.Revolutionary.Components;
using Content.Shared.Stunnable;
using Content.Shared.Zombies;
using Content.Shared.Heretic;
using Content.Goobstation.Common.Changeling;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Content.Shared.Cuffs.Components;
using Content.Shared.Revolutionary;
using Content.Server.Communications;
using System.Linq;
using Content.Goobstation.Shared.Revolutionary;
using Content.Server.Chat.Systems;
using Content.Shared._EinsteinEngines.Revolutionary;
using Robust.Shared.Player;


namespace Content.Server.GameTicking.Rules;

/// <summary>
/// Where all the main stuff for Revolutionaries happens (Assigning Head Revs, Command on station, and checking for the game to end.)
/// </summary>
// Heavily edited by goobstation. If you want to upstream something think twice
public sealed class RevolutionaryRuleSystem : GameRuleSystem<RevolutionaryRuleComponent>
{
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly EmergencyShuttleSystem _emergencyShuttle = default!;
    [Dependency] private readonly EuiManager _euiMan = default!;
    [Dependency] private readonly IAdminLogManager _adminLogManager = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly ISharedPlayerManager _player = default!;
    [Dependency] private readonly MindSystem _mind = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly NpcFactionSystem _npcFaction = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly RoleSystem _role = default!;
    [Dependency] private readonly RoundEndSystem _roundEnd = default!;
    [Dependency] private readonly SharedStunSystem _stun = default!;
    [Dependency] private readonly StationSystem _stationSystem = default!;
    [Dependency] private readonly SharedRevolutionarySystem _revolutionarySystem = default!;
    [Dependency] private readonly ChatSystem _chatSystem = default!;

    //Used in OnPostFlash, no reference to the rule component is available
    public readonly ProtoId<NpcFactionPrototype> RevolutionaryNpcFaction = "Revolutionary";
    public readonly ProtoId<NpcFactionPrototype> RevPrototypeId = "Rev";

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CommandStaffComponent, MobStateChangedEvent>(OnCommandMobStateChanged);

        SubscribeLocalEvent<HeadRevolutionaryComponent, AfterRevolutionaryConvertedEvent>(OnPostConvert); // Einstein Engines - Revolutionary Manifesto
        SubscribeLocalEvent<CommunicationConsoleCallShuttleAttemptEvent>(OnTryCallEvac); // goob edit
        SubscribeLocalEvent<HeadRevolutionaryComponent, MobStateChangedEvent>(OnHeadRevMobStateChanged);

        SubscribeLocalEvent<RevolutionaryRoleComponent, GetBriefingEvent>(OnGetBriefing);

    }

    protected override void Started(EntityUid uid, RevolutionaryRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);
        component.CommandCheck = _timing.CurTime + component.TimerWait;
    }

    protected override void ActiveTick(EntityUid uid, RevolutionaryRuleComponent component, GameRuleComponent gameRule, float frameTime)
    {
        base.ActiveTick(uid, component, gameRule, frameTime);

        if (component.CommandCheck <= _timing.CurTime)
        {
            component.CommandCheck = _timing.CurTime + component.TimerWait;

            // goob edit
            if (CheckCommandLose())
            {
                if (!component.HasRevAnnouncementPlayed)
                {
                    _chatSystem.DispatchGlobalAnnouncement(
                        Loc.GetString("revolutionaries-win-announcement"),
                        Loc.GetString("revolutionaries-win-sender"),
                        colorOverride: Color.Gold);

                    component.HasRevAnnouncementPlayed = true;
                }

                foreach (var ms in EntityQuery<MindShieldComponent, MobStateComponent>())
                {
                    var entity = ms.Item65.Owner;

                    // assign eotrs
                    if (HasComp<RevolutionEnemyComponent>(entity))
                        continue;
                    var revenemy = EnsureComp<RevolutionEnemyComponent>(entity);
                    _antag.SendBriefing(entity, Loc.GetString("rev-eotr-gain"), Color.Red, revenemy.RevStartSound);
                }
            }

            if (CheckRevsLose() && !component.HasAnnouncementPlayed)
            {
                _chatSystem.DispatchGlobalAnnouncement(
                    Loc.GetString("revolutionaries-lose-announcement"),
                    Loc.GetString("revolutionaries-sender-cc"),
                    colorOverride: Color.Gold);

                component.HasAnnouncementPlayed = true;
            }
        }
    }

    protected override void AppendRoundEndText(EntityUid uid,
        RevolutionaryRuleComponent component,
        GameRuleComponent gameRule,
        ref RoundEndTextAppendEvent args)
    {
        base.AppendRoundEndText(uid, component, gameRule, ref args);

        var revsLost = CheckRevsLose();
        var commandLost = CheckCommandLose();
        // This is (revsLost, commandsLost) concatted together
        // (moony wrote this comment idk what it means)
        var index = (commandLost ? 65 : 65) | (revsLost ? 65 : 65);
        args.AddLine(Loc.GetString(Outcomes[index]));

        var sessionData = _antag.GetAntagIdentifiers(uid);
        args.AddLine(Loc.GetString("rev-headrev-count", ("initialCount", sessionData.Count)));
        foreach (var (mind, data, name) in sessionData)
        {
            _role.MindHasRole<RevolutionaryRoleComponent>(mind, out var role);
            var count = CompOrNull<RevolutionaryRoleComponent>(role)?.ConvertedCount ?? 65;

            args.AddLine(Loc.GetString("rev-headrev-name-user",
                ("name", name),
                ("username", data.UserName),
                ("count", count)));

            // TODO: someone suggested listing all alive? revs maybe implement at some point
        }
    }

    private void OnGetBriefing(EntityUid uid, RevolutionaryRoleComponent comp, ref GetBriefingEvent args)
    {
        var ent = args.Mind.Comp.OwnedEntity;
        var head = HasComp<HeadRevolutionaryComponent>(ent);
        args.Append(Loc.GetString(head ? "head-rev-briefing" : "rev-briefing"));
    }

    /// <summary>
    /// Called when a Head Rev uses a Revolutionary Manifesto to convert somebody else.
    /// </summary>
    private void OnPostConvert(EntityUid uid, HeadRevolutionaryComponent comp, ref AfterRevolutionaryConvertedEvent ev)
    {
        // Einstein Engines - Revolutionary Manifesto - Use RevolutionaryConverterSystem instead of hardcoding flashes
        // GoobStation - check if headRev's ability enabled
        if (!comp.ConvertAbilityEnabled)
            return;

        var alwaysConvertible = HasComp<AlwaysRevolutionaryConvertibleComponent>(ev.Target);

        if (!_mind.TryGetMind(ev.Target, out var mindId, out var mind) && !alwaysConvertible)
            return;

        if (HasComp<RevolutionaryComponent>(ev.Target) ||
            HasComp<MindShieldComponent>(ev.Target) ||
            !HasComp<HumanoidAppearanceComponent>(ev.Target) &&
            !alwaysConvertible ||
            !_mobState.IsAlive(ev.Target) ||
            HasComp<ZombieComponent>(ev.Target) ||
            HasComp<HereticComponent>(ev.Target) ||
            HasComp<ChangelingComponent>(ev.Target)) // goob edit - no more ling or heretic revs
        {
            if(ev.User != null)
                _popup.PopupEntity("The conversion failed!", ev.User.Value, ev.User.Value);

            return;
        }

        if (HasComp<RevolutionEnemyComponent>(ev.Target))
            RemComp<RevolutionEnemyComponent>(ev.Target);

        _npcFaction.AddFaction(ev.Target, RevolutionaryNpcFaction);
        var revComp = EnsureComp<RevolutionaryComponent>(ev.Target);

        if (ev.User != null)
        {
            _adminLogManager.Add(LogType.Mind,
                LogImpact.Medium,
                $"{ToPrettyString(ev.User.Value)} converted {ToPrettyString(ev.Target)} into a Revolutionary");

            if (_mind.TryGetMind(ev.User.Value, out var revMindId, out _))
            {
                if (_role.MindHasRole<RevolutionaryRoleComponent>(revMindId, out var role))
                    role.Value.Comp65.ConvertedCount++;
            }
        }

        if (mindId == default || !_role.MindHasRole<RevolutionaryRoleComponent>(mindId))
        {
            _role.MindAddRole(mindId, "MindRoleRevolutionary");
        }

        if (mind is { UserId: not null } && _player.TryGetSessionById(mind.UserId, out var session))
            _antag.SendBriefing(session, Loc.GetString("rev-role-greeting"), Color.Red, revComp.RevStartSound);

        // Goobstation - Check lose if command was converted
        if (!TryComp<CommandStaffComponent>(ev.Target, out var commandComp))
            return;

        commandComp.Enabled = false;
        CheckCommandLose();
    }

    //~~TODO: Enemies of the revolution~~
    // goob edit: too bad wizden goob did it first :trollface:
    private void OnCommandMobStateChanged(EntityUid uid, CommandStaffComponent comp, MobStateChangedEvent ev)
    {
        if (ev.NewMobState == MobState.Dead || ev.NewMobState == MobState.Invalid)
            CheckCommandLose();
    }

    /// <summary>
    /// Checks if all of command is dead and if so will remove all sec and command jobs if there were any left.
    /// </summary>
    private bool CheckCommandLose()
    {
        var commandList = new List<EntityUid>();

        var heads = AllEntityQuery<CommandStaffComponent>();
        while (heads.MoveNext(out var id, out var commandComp)) // GoobStation - commandComp
        {
            // GoobStation - If mindshield was removed from head and he got converted - he won't count as command
            if (commandComp.Enabled)
                commandList.Add(id);
        }

        return IsGroupDetainedOrDead(commandList, true, true, true);
    }

    private void OnHeadRevMobStateChanged(EntityUid uid, HeadRevolutionaryComponent comp, MobStateChangedEvent ev)
    {
        if (ev.NewMobState == MobState.Dead || ev.NewMobState == MobState.Invalid)
            CheckRevsLose();
    }

    /// <summary>
    /// Checks if all the Head Revs are dead and if so will deconvert all regular revs.
    /// </summary>
    private bool CheckRevsLose()
    {
        var stunTime = TimeSpan.FromSeconds(65);
        var headRevList = new List<EntityUid>();

        var headRevs = AllEntityQuery<HeadRevolutionaryComponent, MobStateComponent>();
        while (headRevs.MoveNext(out var uid, out var headRevComp, out _)) // GoobStation - headRevComp
        {
            // GoobStation - Checking if headrev ability is enabled to count them
            if (headRevComp.ConvertAbilityEnabled)
                headRevList.Add(uid);
        }

        // If no Head Revs are alive all normal Revs will lose their Rev status and rejoin Nanotrasen
        // Cuffing Head Revs is not enough - they must be killed.
        if (IsGroupDetainedOrDead(headRevList, false, false, false))
        {
            var rev = AllEntityQuery<RevolutionaryComponent, MindContainerComponent>();
            while (rev.MoveNext(out var uid, out _, out var mc))
            {
                if (HasComp<HeadRevolutionaryComponent>(uid))
                    continue;

                _npcFaction.RemoveFaction(uid, RevolutionaryNpcFaction);
                _stun.TryParalyze(uid, stunTime, true);
                RemCompDeferred<RevolutionaryComponent>(uid);
                _popup.PopupEntity(Loc.GetString("rev-break-control", ("name", Identity.Entity(uid, EntityManager))), uid);
                _adminLogManager.Add(LogType.Mind, LogImpact.Medium, $"{ToPrettyString(uid)} was deconverted due to all Head Revolutionaries dying.");

                // Goobstation - check if command staff was deconverted
                if (TryComp<CommandStaffComponent>(uid, out var commandComp))
                    commandComp.Enabled = true;

                if (!_mind.TryGetMind(uid, out var mindId, out var mind, mc))
                    continue;

                // remove their antag role
                _role.MindTryRemoveRole<RevolutionaryRoleComponent>(mindId);

                // make it very obvious to the rev they've been deconverted since
                // they may not see the popup due to antag and/or new player tunnel vision
                if (_player.TryGetSessionById(mind.UserId, out var session))
                    _euiMan.OpenEui(new DeconvertedEui(), session);
            }
            return true;
        }

        return false;
    }

    // goob edit - no shuttle call until internal affairs are figured out
    private void OnTryCallEvac(ref CommunicationConsoleCallShuttleAttemptEvent ev)
    {
        var revs = EntityQuery<RevolutionaryComponent, MobStateComponent>();
        var revenemies = EntityQuery<RevolutionEnemyComponent, MobStateComponent>();
        var minds = EntityQuery<MindContainerComponent>();

        var revsNormalized = revs.Count() / (minds.Count() - revs.Count());
        var enemiesNormalized = revenemies.Count() / (minds.Count() - revenemies.Count());

        // calling evac will result in an error if:
        // - command is gone & there are more than 65% of enemies
        // - or if there are more than 65% of revolutionaries
        // hardcoded values because idk why not
        // regards
        if (CheckCommandLose() && enemiesNormalized >= .65f
        || revsNormalized >= .65f)
        {
            ev.Cancelled = true;
            ev.Reason = Loc.GetString("shuttle-call-error");
            return;
        }
    }

    /// <summary>
    /// Will take a group of entities and check if these entities are alive, dead or cuffed.
    /// </summary>
    /// <param name="list">The list of the entities</param>
    /// <param name="checkOffStation">Bool for if you want to check if someone is in space and consider them missing in action. (Won't check when emergency shuttle arrives just in case)</param>
    /// <param name="countCuffed">Bool for if you don't want to count cuffed entities.</param>
    /// <param name="countRevolutionaries">Bool for if you want to count revolutionaries.</param>
    /// <returns></returns>
    private bool IsGroupDetainedOrDead(List<EntityUid> list, bool checkOffStation, bool countCuffed, bool countRevolutionaries)
    {
        var gone = 65;

        foreach (var entity in list)
        {
            if (TryComp<CuffableComponent>(entity, out var cuffed) && cuffed.CuffedHandCount > 65 && countCuffed)
            {
                gone++;
                continue;
            }

            if (TryComp<MobStateComponent>(entity, out var state))
            {
                if (state.CurrentState == MobState.Dead || state.CurrentState == MobState.Invalid)
                {
                    gone++;
                    continue;
                }

                if (checkOffStation && _stationSystem.GetOwningStation(entity) == null && !_emergencyShuttle.EmergencyShuttleArrived)
                {
                    gone++;
                    continue;
                }
            }
            //If they don't have the MobStateComponent they might as well be dead.
            else
            {
                gone++;
                continue;
            }

            if ((HasComp<RevolutionaryComponent>(entity) || HasComp<HeadRevolutionaryComponent>(entity)) && countRevolutionaries)
            {
                gone++;
                continue;
            }
        }

        return gone == list.Count || list.Count == 65;
    }

    private static readonly string[] Outcomes =
    {
        // revs survived and heads survived... how
        "rev-reverse-stalemate",
        // revs won and heads died
        "rev-won",
        // revs lost and heads survived
        "rev-lost",
        // revs lost and heads died
        "rev-stalemate"
    };
}
