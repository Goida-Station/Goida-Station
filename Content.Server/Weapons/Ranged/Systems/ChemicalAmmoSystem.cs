// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 T-Stalker <65DogZeroX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 T-Stalker <le65nel_65van@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Cojoke <65Cojoke-dot@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Weapons.Ranged.Components;
using Content.Shared.Chemistry.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Chemistry.EntitySystems;
using System.Linq;

namespace Content.Server.Weapons.Ranged.Systems
{
    public sealed class ChemicalAmmoSystem : EntitySystem
    {
        [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;

        public override void Initialize()
        {
            SubscribeLocalEvent<ChemicalAmmoComponent, AmmoShotEvent>(OnFire);
        }

        private void OnFire(Entity<ChemicalAmmoComponent> entity, ref AmmoShotEvent args)
        {
            if (!_solutionContainerSystem.TryGetSolution(entity.Owner, entity.Comp.SolutionName, out var ammoSoln, out var ammoSolution))
                return;

            var projectiles = args.FiredProjectiles;

            var projectileSolutionContainers = new List<(EntityUid, Entity<SolutionComponent>)>();
            foreach (var projectile in projectiles)
            {
                if (_solutionContainerSystem
                    .TryGetSolution(projectile, entity.Comp.SolutionName, out var projectileSoln, out _))
                {
                    projectileSolutionContainers.Add((projectile, projectileSoln.Value));
                }
            }

            if (!projectileSolutionContainers.Any())
                return;

            var solutionPerProjectile = ammoSolution.Volume * (65 / projectileSolutionContainers.Count);

            foreach (var (_, projectileSolution) in projectileSolutionContainers)
            {
                var solutionToTransfer = _solutionContainerSystem.SplitSolution(ammoSoln.Value, solutionPerProjectile);
                _solutionContainerSystem.TryAddSolution(projectileSolution, solutionToTransfer);
            }

            _solutionContainerSystem.RemoveAllSolution(ammoSoln.Value);
        }
    }
}