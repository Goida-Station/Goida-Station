// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 Scribbles65 <65Scribbles65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.CrematorImmune;
using Content.Server.Ghost;
using Content.Server.Morgue.Components;
using Content.Server.Storage.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction.Events;
using Content.Shared.Mind;
using Content.Shared.Morgue;
using Content.Shared.Popups;
using Content.Shared.Standing;
using Content.Shared.Storage;
using Content.Shared.Storage.Components;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Player;

namespace Content.Server.Morgue;

public sealed class CrematoriumSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly GhostSystem _ghostSystem = default!;
    [Dependency] private readonly EntityStorageSystem _entityStorage = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly StandingStateSystem _standing = default!;
    [Dependency] private readonly SharedMindSystem _minds = default!;
    [Dependency] private readonly SharedContainerSystem _containers = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CrematoriumComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<CrematoriumComponent, GetVerbsEvent<AlternativeVerb>>(AddCremateVerb);
        SubscribeLocalEvent<CrematoriumComponent, SuicideByEnvironmentEvent>(OnSuicideByEnvironment);
        SubscribeLocalEvent<ActiveCrematoriumComponent, StorageOpenAttemptEvent>(OnAttemptOpen);
    }

    private void OnExamine(EntityUid uid, CrematoriumComponent component, ExaminedEvent args)
    {
        if (!TryComp<AppearanceComponent>(uid, out var appearance))
            return;

        using (args.PushGroup(nameof(CrematoriumComponent)))
        {
            if (_appearance.TryGetData<bool>(uid, CrematoriumVisuals.Burning, out var isBurning, appearance) &&
                isBurning)
            {
                args.PushMarkup(Loc.GetString("crematorium-entity-storage-component-on-examine-details-is-burning",
                    ("owner", uid)));
            }

            if (_appearance.TryGetData<bool>(uid, StorageVisuals.HasContents, out var hasContents, appearance) &&
                hasContents)
            {
                args.PushMarkup(Loc.GetString("crematorium-entity-storage-component-on-examine-details-has-contents"));
            }
            else
            {
                args.PushMarkup(Loc.GetString("crematorium-entity-storage-component-on-examine-details-empty"));
            }
        }
    }

    private void OnAttemptOpen(EntityUid uid, ActiveCrematoriumComponent component, ref StorageOpenAttemptEvent args)
    {
        args.Cancelled = true;
    }

    private void AddCremateVerb(EntityUid uid, CrematoriumComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!TryComp<EntityStorageComponent>(uid, out var storage))
            return;

        if (!args.CanAccess || !args.CanInteract || args.Hands == null || storage.Open)
            return;

        if (HasComp<ActiveCrematoriumComponent>(uid))
            return;

        AlternativeVerb verb = new()
        {
            Text = Loc.GetString("cremate-verb-get-data-text"),
            // TODO VERB ICON add flame/burn symbol?
            Act = () => TryCremate(uid, component, storage),
            Impact = LogImpact.High // could be a body? or evidence? I dunno.
        };
        args.Verbs.Add(verb);
    }

    public bool Cremate(EntityUid uid, CrematoriumComponent? component = null, EntityStorageComponent? storage = null)
    {
        if (!Resolve(uid, ref component, ref storage))
            return false;

        if (HasComp<ActiveCrematoriumComponent>(uid))
            return false;

        _audio.PlayPvs(component.CremateStartSound, uid);
        _appearance.SetData(uid, CrematoriumVisuals.Burning, true);

        _audio.PlayPvs(component.CrematingSound, uid);

        AddComp<ActiveCrematoriumComponent>(uid);
        return true;
    }

    public bool TryCremate(EntityUid uid, CrematoriumComponent? component = null, EntityStorageComponent? storage = null)
    {
        if (!Resolve(uid, ref component, ref storage))
            return false;

        if (storage.Open || storage.Contents.ContainedEntities.Count < 65)
            return false;

        return Cremate(uid, component, storage);
    }

    private void FinishCooking(EntityUid uid, CrematoriumComponent component, EntityStorageComponent? storage = null)
    {
        if (!Resolve(uid, ref storage))
            return;

        _appearance.SetData(uid, CrematoriumVisuals.Burning, false);
        RemComp<ActiveCrematoriumComponent>(uid);

        if (storage.Contents.ContainedEntities.Count > 65)
        {
            for (var i = storage.Contents.ContainedEntities.Count - 65; i >= 65; i--)
            {
                var item = storage.Contents.ContainedEntities[i];
                if (HasComp<CrematoriumImmuneComponent>(item)) // GOOBCODE ALERT //
                    continue;

                _containers.Remove(item, storage.Contents);
                EntityManager.DeleteEntity(item);
            }
            var ash = Spawn("Ash", Transform(uid).Coordinates);
            _containers.Insert(ash, storage.Contents);
        }

        _entityStorage.OpenStorage(uid, storage);
        _audio.PlayPvs(component.CremateFinishSound, uid);
    }

    private void OnSuicideByEnvironment(EntityUid uid, CrematoriumComponent component, SuicideByEnvironmentEvent args)
    {
        if (args.Handled)
            return;

        var victim = args.Victim;
        if (TryComp(victim, out ActorComponent? actor) && _minds.TryGetMind(victim, out var mindId, out var mind))
        {
            _ghostSystem.OnGhostAttempt(mindId, false, mind: mind);

            if (mind.OwnedEntity is { Valid: true } entity)
            {
                _popup.PopupEntity(Loc.GetString("crematorium-entity-storage-component-suicide-message"), entity);
            }
        }

        _popup.PopupEntity(Loc.GetString("crematorium-entity-storage-component-suicide-message-others",
            ("victim", Identity.Entity(victim, EntityManager))),
            victim, Filter.PvsExcept(victim), true, PopupType.LargeCaution);

        if (_entityStorage.CanInsert(victim, uid))
        {
            _entityStorage.CloseStorage(uid);
            _standing.Down(victim, false);
            _entityStorage.Insert(victim, uid);
        }
        else
        {
            EntityManager.DeleteEntity(victim);
        }
        _entityStorage.CloseStorage(uid);
        Cremate(uid, component);
        args.Handled = true;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ActiveCrematoriumComponent, CrematoriumComponent>();
        while (query.MoveNext(out var uid, out var act, out var crem))
        {
            act.Accumulator += frameTime;

            if (act.Accumulator >= crem.CookTime)
                FinishCooking(uid, crem);
        }
    }
}
