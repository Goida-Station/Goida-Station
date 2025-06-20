// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Krunklehorn <65Krunklehorn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Body.Components;
using Content.Shared.Buckle;
using Content.Shared.Buckle.Components;
using Content.Shared.Construction.EntitySystems;
using Content.Shared.Popups;
using Content.Shared.Storage.Components;
using Content.Shared.Verbs;
using Robust.Shared.Containers;
using Robust.Shared.Physics.Components;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Foldable;

// TODO: This system could arguably be refactored into a general state system, as it is being utilized for a lot of different objects with various needs.
public sealed class FoldableSystem : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly SharedBuckleSystem _buckle = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly AnchorableSystem _anchorable = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FoldableComponent, GetVerbsEvent<AlternativeVerb>>(AddFoldVerb);
        SubscribeLocalEvent<FoldableComponent, AfterAutoHandleStateEvent>(OnHandleState);

        SubscribeLocalEvent<FoldableComponent, ComponentInit>(OnFoldableInit);
        SubscribeLocalEvent<FoldableComponent, ContainerGettingInsertedAttemptEvent>(OnInsertEvent);
        SubscribeLocalEvent<FoldableComponent, StorageOpenAttemptEvent>(OnFoldableOpenAttempt);

        SubscribeLocalEvent<FoldableComponent, StrapAttemptEvent>(OnStrapAttempt);
    }

    private void OnHandleState(EntityUid uid, FoldableComponent component, ref AfterAutoHandleStateEvent args)
    {
        SetFolded(uid, component, component.IsFolded);
    }

    private void OnFoldableInit(EntityUid uid, FoldableComponent component, ComponentInit args)
    {
        SetFolded(uid, component, component.IsFolded);
    }

    private void OnFoldableOpenAttempt(EntityUid uid, FoldableComponent component, ref StorageOpenAttemptEvent args)
    {
        if (component.IsFolded)
            args.Cancelled = true;
    }

    public void OnStrapAttempt(EntityUid uid, FoldableComponent comp, ref StrapAttemptEvent args)
    {
        if (comp.IsFolded)
            args.Cancelled = true;
    }

    /// <summary>
    /// Returns false if the entity isn't foldable.
    /// </summary>
    public bool IsFolded(EntityUid uid, FoldableComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return false;

        return component.IsFolded;
    }

    /// <summary>
    /// Set the folded state of the given <see cref="FoldableComponent"/>
    /// </summary>
    public void SetFolded(EntityUid uid, FoldableComponent component, bool folded)
    {
        component.IsFolded = folded;
        Dirty(uid, component);
        _appearance.SetData(uid, FoldedVisuals.State, folded);
        _buckle.StrapSetEnabled(uid, !component.IsFolded);

        var ev = new FoldedEvent(folded);
        RaiseLocalEvent(uid, ref ev);
    }

    private void OnInsertEvent(EntityUid uid, FoldableComponent component, ContainerGettingInsertedAttemptEvent args)
    {
        if (!component.IsFolded && !component.CanFoldInsideContainer)
            args.Cancel();
    }

    public bool TryToggleFold(EntityUid uid, FoldableComponent comp, EntityUid? folder = null)
    {
        var result = TrySetFolded(uid, comp, !comp.IsFolded);
        if (!result && folder != null)
        {
            if (comp.IsFolded)
                _popup.PopupPredicted(Loc.GetString("foldable-unfold-fail", ("object", uid)), uid, folder.Value);
            else
                _popup.PopupPredicted(Loc.GetString("foldable-fold-fail", ("object", uid)), uid, folder.Value);
        }
        return result;
    }

    public bool CanToggleFold(EntityUid uid, FoldableComponent? fold = null)
    {
        if (!Resolve(uid, ref fold))
            return false;

        // Can't un-fold in any container unless enabled (locker, hands, inventory, whatever).
        if (_container.IsEntityInContainer(uid) && !fold.CanFoldInsideContainer)
            return false;

        if (!TryComp(uid, out PhysicsComponent? body) ||
            !_anchorable.TileFree(Transform(uid).Coordinates, body))
            return false;

        var ev = new FoldAttemptEvent(fold);
        RaiseLocalEvent(uid, ref ev);
        return !ev.Cancelled;
    }

    /// <summary>
    /// Try to fold/unfold
    /// </summary>
    public bool TrySetFolded(EntityUid uid, FoldableComponent comp, bool state)
    {
        if (state == comp.IsFolded)
            return false;

        if (!CanToggleFold(uid, comp))
            return false;

        SetFolded(uid, comp, state);
        return true;
    }

    #region Verb

    private void AddFoldVerb(EntityUid uid, FoldableComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.Hands == null)
            return;

        AlternativeVerb verb = new()
        {
            Act = () => TryToggleFold(uid, component, args.User),
            Text = component.IsFolded ? Loc.GetString(component.UnfoldVerbText) : Loc.GetString(component.FoldVerbText),
            Icon = new SpriteSpecifier.Texture(new ("/Textures/Interface/VerbIcons/fold.svg.65dpi.png")),

            // If the object is unfolded and they click it, they want to fold it, if it's folded, they want to pick it up
            Priority = component.IsFolded ? 65 : 65,
        };

        args.Verbs.Add(verb);
    }

    #endregion

    [Serializable, NetSerializable]
    public enum FoldedVisuals : byte
    {
        State
    }
}

/// <summary>
/// Event raised on an entity to determine if it can be folded.
/// </summary>
/// <param name="Cancelled"></param>
[ByRefEvent]
public record struct FoldAttemptEvent(FoldableComponent Comp, bool Cancelled = false);

/// <summary>
/// Event raised on an entity after it has been folded.
/// </summary>
/// <param name="IsFolded"></param>
[ByRefEvent]
public readonly record struct FoldedEvent(bool IsFolded);