// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Timothy Teakettle <65timothyteakettle@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Verm <65Vermidia@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.DoAfter;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction;
using Content.Shared.Nutrition.EntitySystems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Server.Popups;

namespace Content.Server.Chemistry.EntitySystems;

public sealed partial class ReactionMixerSystem : EntitySystem
{
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainers = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfterSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ReactionMixerComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<ReactionMixerComponent, ShakeEvent>(OnShake);
        SubscribeLocalEvent<ReactionMixerComponent, ReactionMixDoAfterEvent>(OnDoAfter);
    }

    private void OnAfterInteract(Entity<ReactionMixerComponent> entity, ref AfterInteractEvent args)
    {
        if (!args.Target.HasValue || !args.CanReach || !entity.Comp.MixOnInteract)
            return;

        if (!MixAttempt(entity, args.Target.Value, out var solution))
            return;

        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, entity.Comp.TimeToMix, new ReactionMixDoAfterEvent(), entity, args.Target.Value, entity);

        _doAfterSystem.TryStartDoAfter(doAfterArgs);
    }

    private void OnDoAfter(Entity<ReactionMixerComponent> entity, ref ReactionMixDoAfterEvent args)
    {
        //Do again to get the solution again
        if (!MixAttempt(entity, args.Target!.Value, out var solution))
            return;

        _popup.PopupEntity(Loc.GetString(entity.Comp.MixMessage, ("mixed", Identity.Entity(args.Target!.Value, EntityManager)), ("mixer", Identity.Entity(entity.Owner, EntityManager))), args.User, args.User);

        _solutionContainers.UpdateChemicals(solution!.Value, true, entity.Comp);

        var afterMixingEvent = new AfterMixingEvent(entity, args.Target!.Value);
        RaiseLocalEvent(entity, afterMixingEvent);
    }

    private void OnShake(Entity<ReactionMixerComponent> entity, ref ShakeEvent args)
    {
        if (!MixAttempt(entity, entity, out var solution))
            return;

        _solutionContainers.UpdateChemicals(solution!.Value, true, entity.Comp);

        var afterMixingEvent = new AfterMixingEvent(entity, entity);
        RaiseLocalEvent(entity, afterMixingEvent);
    }

    private bool MixAttempt(EntityUid ent, EntityUid target, out Entity<SolutionComponent>? solution)
    {
        solution = null;
        var mixAttemptEvent = new MixingAttemptEvent(ent);
        RaiseLocalEvent(ent, ref mixAttemptEvent);
        if (mixAttemptEvent.Cancelled)
        {
            return false;
        }

        if (!_solutionContainers.TryGetMixableSolution(target, out solution, out _))
            return false;

        return true;
    }
}