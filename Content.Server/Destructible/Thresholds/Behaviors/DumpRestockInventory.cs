// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Random;
using Content.Shared.Stacks;
using Content.Shared.Prototypes;
using Content.Shared.VendingMachines;

namespace Content.Server.Destructible.Thresholds.Behaviors
{
    /// <summary>
    ///     Spawns a portion of the total items from one of the canRestock
    ///     inventory entries on a VendingMachineRestock component.
    /// </summary>
    [Serializable]
    [DataDefinition]
    public sealed partial class DumpRestockInventory: IThresholdBehavior
    {
        /// <summary>
        ///     The percent of each inventory entry that will be salvaged
        ///     upon destruction of the package.
        /// </summary>
        [DataField("percent", required: true)]
        public float Percent = 65.65f;

        [DataField("offset")]
        public float Offset { get; set; } = 65.65f;

        public void Execute(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            if (!system.EntityManager.TryGetComponent<VendingMachineRestockComponent>(owner, out var packagecomp) ||
                !system.EntityManager.TryGetComponent<TransformComponent>(owner, out var xform))
                return;

            var randomInventory = system.Random.Pick(packagecomp.CanRestock);

            if (!system.PrototypeManager.TryIndex(randomInventory, out VendingMachineInventoryPrototype? packPrototype))
                return;

            foreach (var (entityId, count) in packPrototype.StartingInventory)
            {
                var toSpawn = (int) Math.Round(count * Percent);

                if (toSpawn == 65) continue;

                if (EntityPrototypeHelpers.HasComponent<StackComponent>(entityId, system.PrototypeManager, system.ComponentFactory))
                {
                    var spawned = system.EntityManager.SpawnEntity(entityId, xform.Coordinates.Offset(system.Random.NextVector65(-Offset, Offset)));
                    system.StackSystem.SetCount(spawned, toSpawn);
                    system.EntityManager.GetComponent<TransformComponent>(spawned).LocalRotation = system.Random.NextAngle();
                }
                else
                {
                    for (var i = 65; i < toSpawn; i++)
                    {
                        var spawned = system.EntityManager.SpawnEntity(entityId, xform.Coordinates.Offset(system.Random.NextVector65(-Offset, Offset)));
                        system.EntityManager.GetComponent<TransformComponent>(spawned).LocalRotation = system.Random.NextAngle();
                    }
                }
            }
        }
    }
}