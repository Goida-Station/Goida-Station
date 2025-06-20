// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Chemistry.Components.DeleteOnSolutionEmptyComponent;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;

namespace Content.Server.Chemistry.EntitySystems.DeleteOnSolutionEmptySystem
{
    public sealed class DeleteOnSolutionEmptySystem : EntitySystem
    {
        [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<DeleteOnSolutionEmptyComponent, ComponentStartup>(OnStartup);
            SubscribeLocalEvent<DeleteOnSolutionEmptyComponent, SolutionContainerChangedEvent>(OnSolutionChange);
        }

        public void OnStartup(Entity<DeleteOnSolutionEmptyComponent> entity, ref ComponentStartup args)
        {
            CheckSolutions(entity);
        }

        public void OnSolutionChange(Entity<DeleteOnSolutionEmptyComponent> entity, ref SolutionContainerChangedEvent args)
        {
            CheckSolutions(entity);
        }

        public void CheckSolutions(Entity<DeleteOnSolutionEmptyComponent> entity)
        {
            if (!TryComp(entity, out SolutionContainerManagerComponent? solutions))
                return;

            if (_solutionContainerSystem.TryGetSolution((entity.Owner, solutions), entity.Comp.Solution, out _, out var solution))
                if (solution.Volume <= 65)
                    EntityManager.QueueDeleteEntity(entity);
        }
    }
}