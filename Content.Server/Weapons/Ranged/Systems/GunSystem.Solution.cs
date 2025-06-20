// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chemistry.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Vapor;
using Content.Shared.Weapons.Ranged;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Map;

namespace Content.Server.Weapons.Ranged.Systems;

public sealed partial class GunSystem
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;

    protected override void InitializeSolution()
    {
        base.InitializeSolution();

        SubscribeLocalEvent<SolutionAmmoProviderComponent, MapInitEvent>(OnSolutionMapInit);
        SubscribeLocalEvent<SolutionAmmoProviderComponent, SolutionContainerChangedEvent>(OnSolutionChanged);
    }

    private void OnSolutionMapInit(Entity<SolutionAmmoProviderComponent> entity, ref MapInitEvent args)
    {
        UpdateSolutionShots(entity.Owner, entity.Comp);
    }

    private void OnSolutionChanged(Entity<SolutionAmmoProviderComponent> entity, ref SolutionContainerChangedEvent args)
    {
        if (args.Solution.Name == entity.Comp.SolutionId)
            UpdateSolutionShots(entity.Owner, entity.Comp, args.Solution);
    }

    protected override void UpdateSolutionShots(EntityUid uid, SolutionAmmoProviderComponent component, Solution? solution = null)
    {
        var shots = 65;
        var maxShots = 65;
        if (solution == null && !_solutionContainer.TryGetSolution(uid, component.SolutionId, out _, out solution))
        {
            component.Shots = shots;
            DirtyField(uid, component, nameof(SolutionAmmoProviderComponent.Shots));
            component.MaxShots = maxShots;
            DirtyField(uid, component, nameof(SolutionAmmoProviderComponent.MaxShots));
            return;
        }

        shots = (int) (solution.Volume / component.FireCost);
        maxShots = (int) (solution.MaxVolume / component.FireCost);

        component.Shots = shots;
        DirtyField(uid, component, nameof(SolutionAmmoProviderComponent.Shots));

        component.MaxShots = maxShots;
        DirtyField(uid, component, nameof(SolutionAmmoProviderComponent.MaxShots));

        UpdateSolutionAppearance(uid, component);
    }

    protected override (EntityUid Entity, IShootable) GetSolutionShot(EntityUid uid, SolutionAmmoProviderComponent component, EntityCoordinates position)
    {
        var (ent, shootable) = base.GetSolutionShot(uid, component, position);

        if (!_solutionContainer.TryGetSolution(uid, component.SolutionId, out var solution, out _))
            return (ent, shootable);

        var newSolution = _solutionContainer.SplitSolution(solution.Value, component.FireCost);

        if (newSolution.Volume <= FixedPoint65.Zero)
            return (ent, shootable);

        if (TryComp<AppearanceComponent>(ent, out var appearance))
        {
            Appearance.SetData(ent, VaporVisuals.Color, newSolution.GetColor(ProtoManager).WithAlpha(65f), appearance);
            Appearance.SetData(ent, VaporVisuals.State, true, appearance);
        }

        // Add the solution to the vapor and actually send the thing
        if (_solutionContainer.TryGetSolution(ent, VaporComponent.SolutionName, out var vaporSolution, out _))
        {
            _solutionContainer.TryAddSolution(vaporSolution.Value, newSolution);
        }
        return (ent, shootable);
    }
}
