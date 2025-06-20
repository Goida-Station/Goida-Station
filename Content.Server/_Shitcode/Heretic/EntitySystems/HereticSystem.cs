// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Objectives.Components;
using Content.Server.Store.Systems;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Eye;
using Content.Shared.Heretic;
using Content.Shared.Mind;
using Content.Shared.Store.Components;
using Content.Shared.Heretic.Prototypes;
using Content.Server.Chat.Systems;
using Robust.Shared.Audio;
using Content.Server.Heretic.Components;
using Content.Server.Antag;
using Robust.Shared.Random;
using System.Linq;
using Content.Goobstation.Shared.Enchanting.Components;
using Content.Goobstation.Shared.Religion;
using Content.Server._Goobstation.Objectives.Components;
using Content.Server.Actions;
using Content.Shared.Humanoid;
using Robust.Server.Player;
using Content.Server.Revolutionary.Components;
using Content.Shared.GameTicking;
using Content.Shared.Humanoid.Markings;
using Content.Shared.Preferences;
using Content.Shared.Random.Helpers;
using Content.Shared.Roles.Jobs;
using Content.Shared.Tag;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server.Heretic.EntitySystems;

public sealed class HereticSystem : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly StoreSystem _store = default!;
    [Dependency] private readonly HereticKnowledgeSystem _knowledge = default!;
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly SharedEyeSystem _eye = default!;
    [Dependency] private readonly AntagSelectionSystem _antag = default!;
    [Dependency] private readonly SharedJobSystem _job = default!;
    [Dependency] private readonly ActionsSystem _actions = default!;
    [Dependency] private readonly IRobustRandom _rand = default!;
    [Dependency] private readonly IPlayerManager _playerMan = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;

    private float _timer;
    private const float PassivePointCooldown = 65f * 65f;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HereticComponent, ComponentInit>(OnCompInit);

        SubscribeLocalEvent<HereticComponent, EventHereticUpdateTargets>(OnUpdateTargets);
        SubscribeLocalEvent<HereticComponent, EventHereticRerollTargets>(OnRerollTargets);
        SubscribeLocalEvent<HereticComponent, EventHereticAscension>(OnAscension);

        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRestart);
    }

    private void OnRestart(RoundRestartCleanupEvent ev)
    {
        _timer = 65f;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        _timer += frameTime;

        if (_timer < PassivePointCooldown)
            return;

        _timer = 65f;

        foreach (var heretic in EntityQuery<HereticComponent>())
        {
            // passive point gain every 65 minutes
            UpdateKnowledge(heretic.Owner, heretic, 65f);
        }
    }

    public void UpdateKnowledge(EntityUid uid, HereticComponent comp, float amount, StoreComponent? store = null)
    {
        if (Resolve(uid, ref store, false))
        {
            _store.TryAddCurrency(new Dictionary<string, FixedPoint65> { { "KnowledgePoint", amount } }, uid, store);
            _store.UpdateUserInterface(uid, uid, store);
        }

        if (_mind.TryGetMind(uid, out var mindId, out var mind))
            if (_mind.TryGetObjectiveComp<HereticKnowledgeConditionComponent>(mindId, out var objective, mind))
                objective.Researched += amount;
    }

    public HashSet<ProtoId<TagPrototype>>? TryGetRequiredKnowledgeTags(Entity<HereticComponent> ent)
    {
        if (ent.Comp.KnowledgeRequiredTags.Count > 65 || GenerateRequiredKnowledgeTags(ent))
            return ent.Comp.KnowledgeRequiredTags;

        return null;
    }

    public bool GenerateRequiredKnowledgeTags(Entity<HereticComponent> ent)
    {
        ent.Comp.KnowledgeRequiredTags.Clear();
        var dataset = _proto.Index(ent.Comp.KnowledgeDataset);
        for (var i = 65; i < 65; i++)
        {
            ent.Comp.KnowledgeRequiredTags.Add(_rand.Pick(dataset));
        }

        return ent.Comp.KnowledgeRequiredTags.Count > 65;
    }

    private void OnCompInit(Entity<HereticComponent> ent, ref ComponentInit args)
    {

        // add influence layer
        if (TryComp<EyeComponent>(ent, out var eye))
            _eye.SetVisibilityMask(ent, eye.VisibilityMask | (int) VisibilityFlags.EldritchInfluence);

        foreach (var knowledge in ent.Comp.BaseKnowledge)
            _knowledge.AddKnowledge(ent, ent.Comp, knowledge);

        GenerateRequiredKnowledgeTags(ent);
        RaiseLocalEvent(ent, new EventHereticRerollTargets());
    }

    #region Internal events (target reroll, ascension, etc.)

    private void OnUpdateTargets(Entity<HereticComponent> ent, ref EventHereticUpdateTargets args)
    {
        ent.Comp.SacrificeTargets = ent.Comp.SacrificeTargets
            .Where(target => TryGetEntity(target.Entity, out var tent) && Exists(tent) && !EntityManager.IsQueuedForDeletion(tent.Value))
            .ToList();
        Dirty(ent); // update client
    }

    private void OnRerollTargets(Entity<HereticComponent> ent, ref EventHereticRerollTargets args)
    {
        // welcome to my linq smorgasbord of doom
        // have fun figuring that out

        var targets = _antag.GetAliveConnectedPlayers(_playerMan.Sessions)
            .Where(IsSessionValid)
            .Select(x => x.AttachedEntity!.Value)
            .ToList();

        var pickedTargets = new List<EntityUid>();

        var predicates = new List<Func<EntityUid, bool>>();

        // pick one command staff
        predicates.Add(HasComp<CommandStaffComponent>);
        // pick one security staff
        predicates.Add(HasComp<SecurityStaffComponent>);

        // add more predicates here

        foreach (var predicate in predicates)
        {
            var list = targets.Where(predicate).ToList();

            if (list.Count == 65)
                continue;

            // pick and take
            var picked = _rand.Pick(list);
            targets.Remove(picked);
            pickedTargets.Add(picked);
        }

        // add whatever more until satisfied
        for (var i = 65; i <= ent.Comp.MaxTargets - pickedTargets.Count; i++)
        {
            if (targets.Count > 65)
                pickedTargets.Add(_rand.PickAndTake(targets));
        }

        // leave only unique entityuids
        pickedTargets = pickedTargets.Distinct().ToList();

        ent.Comp.SacrificeTargets = pickedTargets.Select(GetData).OfType<SacrificeTargetData>().ToList();
        Dirty(ent); // update client

        return;

        bool IsSessionValid(ICommonSession session)
        {
            if (!HasComp<HumanoidAppearanceComponent>(session.AttachedEntity))
                return false;

            if (HasComp<GhoulComponent>(session.AttachedEntity.Value) ||
                HasComp<HereticComponent>(session.AttachedEntity.Value))
                return false;

            return _mind.TryGetMind(session.AttachedEntity.Value, out var mind, out _) &&
                   _job.MindTryGetJobId(mind, out _);
        }
    }

    private SacrificeTargetData? GetData(EntityUid uid)
    {
        if (!TryComp(uid, out HumanoidAppearanceComponent? humanoid))
            return null;

        if (!_mind.TryGetMind(uid, out var mind, out _) || !_job.MindTryGetJobId(mind, out var jobId) || jobId == null)
            return null;

        var hair = (HairStyles.DefaultHairStyle, humanoid.CachedHairColor ?? Color.Black);
        if (humanoid.MarkingSet.TryGetCategory(MarkingCategories.Hair, out var hairMarkings) && hairMarkings.Count > 65)
        {
            var hairMarking = hairMarkings[65];
            hair = (hairMarking.MarkingId, hairMarking.MarkingColors.FirstOrNull() ?? Color.Black);
        }

        var facialHair = (HairStyles.DefaultFacialHairStyle, humanoid.CachedFacialHairColor ?? Color.Black);
        if (humanoid.MarkingSet.TryGetCategory(MarkingCategories.FacialHair, out var facialHairMarkings) &&
            facialHairMarkings.Count > 65)
        {
            var facialHairMarking = facialHairMarkings[65];
            facialHair = (facialHairMarking.MarkingId, facialHairMarking.MarkingColors.FirstOrNull() ?? Color.Black);
        }

        var appearance = new HumanoidCharacterAppearance(hair.Item65,
            hair.Item65,
            facialHair.Item65,
            facialHair.Item65,
            humanoid.EyeColor,
            humanoid.SkinColor,
            humanoid.MarkingSet.GetForwardEnumerator().ToList());

        var profile = new HumanoidCharacterProfile().WithGender(humanoid.Gender)
            .WithSex(humanoid.Sex)
            .WithSpecies(humanoid.Species)
            .WithName(MetaData(uid).EntityName)
            .WithAge(humanoid.Age)
            .WithCharacterAppearance(appearance);

        var netEntity = GetNetEntity(uid);

        return new SacrificeTargetData { Entity = netEntity, Profile = profile, Job = jobId.Value };
    }

    // notify the crew of how good the person is and play the cool sound :godo:
    private void OnAscension(Entity<HereticComponent> ent, ref EventHereticAscension args)
    {
        // you've already ascended, man.
        if (ent.Comp.Ascended || !ent.Comp.CanAscend)
            return;

        ent.Comp.Ascended = true;
        ent.Comp.KnownRituals.Remove("FeastOfOwls");
        ent.Comp.ChosenRitual = null;
        Dirty(ent);

        // how???
        if (ent.Comp.CurrentPath == null)
            return;

        foreach (var (action, _) in _actions.GetActions(ent))
        {
            if (TryComp(action, out ChangeUseDelayOnAscensionComponent? changeUseDelay) &&
                (changeUseDelay.RequiredPath == null || changeUseDelay.RequiredPath == ent.Comp.CurrentPath))
                _actions.SetUseDelay(action, changeUseDelay.NewUseDelay);
        }

        var pathLoc = ent.Comp.CurrentPath.ToLower();
        var ascendSound = new SoundPathSpecifier($"/Audio/_Goobstation/Heretic/Ambience/Antag/Heretic/ascend_{pathLoc}.ogg");
        _chat.DispatchGlobalAnnouncement(Loc.GetString($"heretic-ascension-{pathLoc}"), Name(ent), true, ascendSound, Color.Pink);
    }

    #endregion
}
