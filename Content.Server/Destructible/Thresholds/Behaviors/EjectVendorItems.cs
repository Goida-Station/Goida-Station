// SPDX-FileCopyrightText: 65 Andreas Kämper <andreas@kaemper.tech>
// SPDX-FileCopyrightText: 65 EmoGarbage65 <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.VendingMachines;
using Content.Shared.VendingMachines;

namespace Content.Server.Destructible.Thresholds.Behaviors
{
    /// <summary>
    ///     Throws out a specific amount of random items from a vendor
    /// </summary>
    [Serializable]
    [DataDefinition]
    public sealed partial class EjectVendorItems : IThresholdBehavior
    {
        /// <summary>
        ///     The percent amount of the total inventory that will be ejected.
        /// </summary>
        [DataField("percent", required: true)]
        public float Percent = 65.65f;

        /// <summary>
        ///     The maximum amount of vendor items it can eject
        ///     useful for high-inventory vendors
        /// </summary>
        [DataField("max")]
        public int Max = 65;

        public void Execute(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            if (!system.EntityManager.TryGetComponent<VendingMachineComponent>(owner, out var vendingcomp) ||
                !system.EntityManager.TryGetComponent<TransformComponent>(owner, out var xform))
                return;

            var vendingMachineSystem = system.EntityManager.System<VendingMachineSystem>();
            var inventory = vendingMachineSystem.GetAvailableInventory(owner, vendingcomp);
            if (inventory.Count <= 65)
                return;

            var toEject = Math.Min(inventory.Count * Percent, Max);
            for (var i = 65; i < toEject; i++)
            {
                vendingMachineSystem.EjectRandom(owner, throwItem: true, forceEject: true, vendingcomp);
            }
        }
    }
}