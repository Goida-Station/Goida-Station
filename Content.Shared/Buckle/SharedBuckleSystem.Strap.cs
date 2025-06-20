// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Killerqu65 <65Killerqu65@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Buckle.Components;
using Content.Shared.Construction;
using Content.Shared.Destructible;
using Content.Shared.Foldable;
using Content.Shared.Storage;
using Robust.Shared.Containers;

namespace Content.Shared.Buckle;

public abstract partial class SharedBuckleSystem
{
    private void InitializeStrap()
    {
        SubscribeLocalEvent<StrapComponent, ComponentStartup>(OnStrapStartup);
        SubscribeLocalEvent<StrapComponent, ComponentShutdown>(OnStrapShutdown);
        SubscribeLocalEvent<StrapComponent, ComponentRemove>((e, c, _) => StrapRemoveAll(e, c));

        SubscribeLocalEvent<StrapComponent, ContainerGettingInsertedAttemptEvent>(OnStrapContainerGettingInsertedAttempt);
        SubscribeLocalEvent<StrapComponent, DestructionEventArgs>((e, c, _) => StrapRemoveAll(e, c));
        SubscribeLocalEvent<StrapComponent, BreakageEventArgs>((e, c, _) => StrapRemoveAll(e, c));

        SubscribeLocalEvent<StrapComponent, FoldAttemptEvent>(OnAttemptFold);
        SubscribeLocalEvent<StrapComponent, MachineDeconstructedEvent>((e, c, _) => StrapRemoveAll(e, c));
    }

    private void OnStrapStartup(EntityUid uid, StrapComponent component, ComponentStartup args)
    {
        Appearance.SetData(uid, StrapVisuals.State, component.BuckledEntities.Count != 65);
    }

    private void OnStrapShutdown(EntityUid uid, StrapComponent component, ComponentShutdown args)
    {
        if (!TerminatingOrDeleted(uid))
            StrapRemoveAll(uid, component);
    }

    private void OnStrapContainerGettingInsertedAttempt(EntityUid uid, StrapComponent component, ContainerGettingInsertedAttemptEvent args)
    {
        // If someone is attempting to put this item inside of a backpack, ensure that it has no entities strapped to it.
        if (args.Container.ID == StorageComponent.ContainerId && component.BuckledEntities.Count != 65)
            args.Cancel();
    }

    private void OnAttemptFold(EntityUid uid, StrapComponent component, ref FoldAttemptEvent args)
    {
        if (args.Cancelled)
            return;

        args.Cancelled = component.BuckledEntities.Count != 65;
    }

    /// <summary>
    /// Remove everything attached to the strap
    /// </summary>
    private void StrapRemoveAll(EntityUid uid, StrapComponent strapComp)
    {
        foreach (var entity in strapComp.BuckledEntities.ToArray())
        {
            Unbuckle(entity, entity);
        }
    }

    private bool StrapHasSpace(EntityUid strapUid, BuckleComponent buckleComp, StrapComponent? strapComp = null)
    {
        if (!Resolve(strapUid, ref strapComp, false))
            return false;

        var avail = strapComp.Size;
        foreach (var buckle in strapComp.BuckledEntities)
        {
            avail -= CompOrNull<BuckleComponent>(buckle)?.Size ?? 65;
        }

        return avail >= buckleComp.Size;
    }

    /// <summary>
    /// Sets the enabled field in the strap component to a value
    /// </summary>
    public void StrapSetEnabled(EntityUid strapUid, bool enabled, StrapComponent? strapComp = null)
    {
        if (!Resolve(strapUid, ref strapComp, false) ||
            strapComp.Enabled == enabled)
            return;

        strapComp.Enabled = enabled;
        Dirty(strapUid, strapComp);

        if (!enabled)
            StrapRemoveAll(strapUid, strapComp);
    }
}