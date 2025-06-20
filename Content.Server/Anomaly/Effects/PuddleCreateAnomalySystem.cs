// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Anomaly.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Server.Fluids.EntitySystems;
using Content.Shared.Anomaly.Components;

namespace Content.Server.Anomaly.Effects;

/// <summary>
/// This component allows the anomaly to create puddles from SolutionContainer.
/// </summary>
public sealed class PuddleCreateAnomalySystem : EntitySystem
{
    [Dependency] private readonly PuddleSystem _puddle = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<PuddleCreateAnomalyComponent, AnomalyPulseEvent>(OnPulse);
        SubscribeLocalEvent<PuddleCreateAnomalyComponent, AnomalySupercriticalEvent>(OnSupercritical, before: new[] { typeof(InjectionAnomalySystem) });
    }

    private void OnPulse(Entity<PuddleCreateAnomalyComponent> entity, ref AnomalyPulseEvent args)
    {
        if (!_solutionContainer.TryGetSolution(entity.Owner, entity.Comp.Solution, out var sol, out _))
            return;

        var xform = Transform(entity.Owner);
        var puddleSol = _solutionContainer.SplitSolution(sol.Value, entity.Comp.MaxPuddleSize * args.Severity * args.PowerModifier);
        _puddle.TrySplashSpillAt(entity.Owner, xform.Coordinates, puddleSol, out _);
    }

    private void OnSupercritical(Entity<PuddleCreateAnomalyComponent> entity, ref AnomalySupercriticalEvent args)
    {
        if (!_solutionContainer.TryGetSolution(entity.Owner, entity.Comp.Solution, out _, out var sol))
            return;

        var xform = Transform(entity.Owner);
        _puddle.TrySpillAt(xform.Coordinates, sol, out _);
    }
}