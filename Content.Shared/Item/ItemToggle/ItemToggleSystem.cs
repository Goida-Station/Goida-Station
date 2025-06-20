// SPDX-FileCopyrightText: 65 Darkie <darksaiyanis@gmail.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BramvanZijp <65BramvanZijp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Clothing.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.Popups;
using Content.Shared.Temperature;
using Content.Shared.Toggleable;
using Content.Shared.Verbs;
using Content.Shared.Wieldable;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;

namespace Content.Shared.Item.ItemToggle;
/// <summary>
/// Handles generic item toggles, like a welder turning on and off, or an e-sword.
/// </summary>
/// <remarks>
/// If you need extended functionality (e.g. requiring power) then add a new component and use events.
/// </remarks>
public sealed class ItemToggleSystem : EntitySystem
{
    [Dependency] private readonly INetManager _netManager = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    private EntityQuery<ItemToggleComponent> _query;

    public override void Initialize()
    {
        base.Initialize();

        _query = GetEntityQuery<ItemToggleComponent>();

        SubscribeLocalEvent<ItemToggleComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ItemToggleComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<ItemToggleComponent, ItemUnwieldedEvent>(TurnOffOnUnwielded);
        SubscribeLocalEvent<ItemToggleComponent, ItemWieldedEvent>(TurnOnOnWielded);
        SubscribeLocalEvent<ItemToggleComponent, UseInHandEvent>(OnUseInHand, before: [typeof(ClothingSystem)]); // Goobstation - order changes, batons used before equipped
        SubscribeLocalEvent<ItemToggleComponent, GetVerbsEvent<ActivationVerb>>(OnActivateVerb);
        SubscribeLocalEvent<ItemToggleComponent, ActivateInWorldEvent>(OnActivate);

        SubscribeLocalEvent<ItemToggleHotComponent, IsHotEvent>(OnIsHotEvent);

        SubscribeLocalEvent<ItemToggleActiveSoundComponent, ItemToggledEvent>(UpdateActiveSound);
    }

    private void OnStartup(Entity<ItemToggleComponent> ent, ref ComponentStartup args)
    {
        UpdateVisuals(ent);
    }

    private void OnMapInit(Entity<ItemToggleComponent> ent, ref MapInitEvent args)
    {
        if (!ent.Comp.Activated)
            return;

        var ev = new ItemToggledEvent(Predicted: ent.Comp.Predictable, Activated: ent.Comp.Activated, User: null);
        RaiseLocalEvent(ent, ref ev);
    }

    private void OnUseInHand(Entity<ItemToggleComponent> ent, ref UseInHandEvent args)
    {
        if (args.Handled || !ent.Comp.OnUse)
            return;

        args.Handled = true;

        Toggle((ent, ent.Comp), args.User, predicted: ent.Comp.Predictable);
    }

    private void OnActivateVerb(Entity<ItemToggleComponent> ent, ref GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || !ent.Comp.OnActivate)
            return;

        var user = args.User;

        if (ent.Comp.Activated)
        {
            var ev = new ItemToggleActivateAttemptEvent(args.User);
            RaiseLocalEvent(ent.Owner, ref ev);

            if (ev.Cancelled)
                return;
        }
        else
        {
            var ev = new ItemToggleDeactivateAttemptEvent(args.User);
            RaiseLocalEvent(ent.Owner, ref ev);

            if (ev.Cancelled)
                return;
        }

        args.Verbs.Add(new ActivationVerb()
        {
            Text = !ent.Comp.Activated ? Loc.GetString(ent.Comp.VerbToggleOn) : Loc.GetString(ent.Comp.VerbToggleOff),
            Act = () =>
            {
                Toggle((ent.Owner, ent.Comp), user, predicted: ent.Comp.Predictable);
            }
        });
    }

    private void OnActivate(Entity<ItemToggleComponent> ent, ref ActivateInWorldEvent args)
    {
        if (args.Handled || !ent.Comp.OnActivate)
            return;

        args.Handled = true;
        Toggle((ent.Owner, ent.Comp), args.User, predicted: ent.Comp.Predictable);
    }

    /// <summary>
    /// Used when an item is attempted to be toggled.
    /// Sets its state to the opposite of what it is.
    /// </summary>
    /// <returns>Same as <see cref="TrySetActive"/></returns>
    public bool Toggle(Entity<ItemToggleComponent?> ent, EntityUid? user = null, bool predicted = true)
    {
        if (!_query.Resolve(ent, ref ent.Comp, false))
            return false;

        return TrySetActive(ent, !ent.Comp.Activated, user, predicted);
    }

    /// <summary>
    /// Tries to set the activated bool from a value.
    /// </summary>
    /// <returns>false if the attempt fails for any reason</returns>
    public bool TrySetActive(Entity<ItemToggleComponent?> ent, bool active, EntityUid? user = null, bool predicted = true)
    {
        if (active)
            return TryActivate(ent, user, predicted: predicted);
        else
            return TryDeactivate(ent, user, predicted: predicted);
    }

    /// <summary>
    /// Used when an item is attempting to be activated. It returns false if the attempt fails any reason, interrupting the activation.
    /// </summary>
    public bool TryActivate(Entity<ItemToggleComponent?> ent, EntityUid? user = null, bool predicted = true)
    {
        if (!_query.Resolve(ent, ref ent.Comp, false))
            return false;

        var uid = ent.Owner;
        var comp = ent.Comp;
        if (comp.Activated)
            return true;

        var attempt = new ItemToggleActivateAttemptEvent(user);
        RaiseLocalEvent(uid, ref attempt);

        if (!comp.Predictable)
            predicted = false;

        if (!predicted && _netManager.IsClient)
            return false;

        if (attempt.Cancelled)
        {
            if (attempt.Silent)
                return false;

            if (predicted)
                _audio.PlayPredicted(comp.SoundFailToActivate, uid, user);
            else
                _audio.PlayPvs(comp.SoundFailToActivate, uid);

            if (attempt.Popup != null && user != null)
            {
                if (predicted)
                    _popup.PopupClient(attempt.Popup, uid, user.Value);
                else
                    _popup.PopupEntity(attempt.Popup, uid, user.Value);
            }

            return false;
        }

        Activate((uid, comp), predicted, user);
        return true;
    }

    /// <summary>
    /// Used when an item is attempting to be deactivated. It returns false if the attempt fails any reason, interrupting the deactivation.
    /// </summary>
    public bool TryDeactivate(Entity<ItemToggleComponent?> ent, EntityUid? user = null, bool predicted = true)
    {
        if (!_query.Resolve(ent, ref ent.Comp, false))
            return false;

        var uid = ent.Owner;
        var comp = ent.Comp;
        if (!comp.Activated)
            return true;

        if (!comp.Predictable)
            predicted = false;

        var attempt = new ItemToggleDeactivateAttemptEvent(user);
        RaiseLocalEvent(uid, ref attempt);

        if (!predicted && _netManager.IsClient)
            return false;

        if (attempt.Cancelled)
        {
            if (attempt.Silent)
                return false;

            if (attempt.Popup != null && user != null)
            {
                if (predicted)
                    _popup.PopupClient(attempt.Popup, uid, user.Value);
                else
                    _popup.PopupEntity(attempt.Popup, uid, user.Value);
            }

            return false;
        }

        Deactivate((uid, comp), predicted, user);
        return true;
    }

    private void Activate(Entity<ItemToggleComponent> ent, bool predicted, EntityUid? user = null)
    {
        var (uid, comp) = ent;
        var soundToPlay = comp.SoundActivate;
        if (predicted)
            _audio.PlayPredicted(soundToPlay, uid, user);
        else
            _audio.PlayPvs(soundToPlay, uid);

        comp.Activated = true;
        UpdateVisuals((uid, comp));
        Dirty(uid, comp);

        var toggleUsed = new ItemToggledEvent(predicted, Activated: true, user);
        RaiseLocalEvent(uid, ref toggleUsed);
    }

    /// <summary>
    /// Used to make the actual changes to the item's components on deactivation.
    /// </summary>
    private void Deactivate(Entity<ItemToggleComponent> ent, bool predicted, EntityUid? user = null)
    {
        var (uid, comp) = ent;
        var soundToPlay = comp.SoundDeactivate;
        if (predicted)
            _audio.PlayPredicted(soundToPlay, uid, user);
        else
            _audio.PlayPvs(soundToPlay, uid);

        comp.Activated = false;
        UpdateVisuals((uid, comp));
        Dirty(uid, comp);

        var toggleUsed = new ItemToggledEvent(predicted, Activated: false, user);
        RaiseLocalEvent(uid, ref toggleUsed);
    }

    private void UpdateVisuals(Entity<ItemToggleComponent> ent)
    {
        if (TryComp(ent, out AppearanceComponent? appearance))
        {
            _appearance.SetData(ent, ToggleVisuals.Toggled, ent.Comp.Activated, appearance);
        }
    }

    /// <summary>
    /// Used for items that require to be wielded in both hands to activate. For instance the dual energy sword will turn off if not wielded.
    /// </summary>
    private void TurnOffOnUnwielded(Entity<ItemToggleComponent> ent, ref ItemUnwieldedEvent args)
    {
        if (!ent.Comp.WieldToggle) // Goobstation
            return;

        TryDeactivate((ent, ent.Comp), args.User);
    }

    /// <summary>
    /// Wieldable items will automatically turn on when wielded.
    /// </summary>
    private void TurnOnOnWielded(Entity<ItemToggleComponent> ent, ref ItemWieldedEvent args)
    {
        if (!ent.Comp.WieldToggle) // Goobstation
            return;

        // FIXME: for some reason both client and server play sound
        TryActivate((ent, ent.Comp));
    }

    public bool IsActivated(Entity<ItemToggleComponent?> ent)
    {
        if (!_query.Resolve(ent, ref ent.Comp, false))
            return true; // assume always activated if no component

        return ent.Comp.Activated;
    }

    /// <summary>
    /// Used to make the item hot when activated.
    /// </summary>
    private void OnIsHotEvent(Entity<ItemToggleHotComponent> ent, ref IsHotEvent args)
    {
        args.IsHot |= IsActivated(ent.Owner);
    }

    /// <summary>
    /// Used to update the looping active sound linked to the entity.
    /// </summary>
    private void UpdateActiveSound(Entity<ItemToggleActiveSoundComponent> ent, ref ItemToggledEvent args)
    {
        var (uid, comp) = ent;
        if (!args.Activated)
        {
            comp.PlayingStream = _audio.Stop(comp.PlayingStream);
            return;
        }

        if (comp.ActiveSound != null && comp.PlayingStream == null)
        {
            var loop = comp.ActiveSound.Params.WithLoop(true);
            var stream = args.Predicted
                ? _audio.PlayPredicted(comp.ActiveSound, uid, args.User, loop)
                : _audio.PlayPvs(comp.ActiveSound, uid, loop);
            if (stream?.Entity is {} entity)
                comp.PlayingStream = entity;
        }
    }
}