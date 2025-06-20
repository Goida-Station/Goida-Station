// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Server.Containers;
using Robust.Shared.Containers;

namespace Content.Server.Destructible.Thresholds.Behaviors
{
    /// <summary>
    ///     Drop all items from specified containers
    /// </summary>
    [DataDefinition]
    public sealed partial class EmptyContainersBehaviour : IThresholdBehavior
    {
        [DataField("containers")]
        public List<string> Containers = new();

        public void Execute(EntityUid owner, DestructibleSystem system, EntityUid? cause = null)
        {
            if (!system.EntityManager.TryGetComponent<ContainerManagerComponent>(owner, out var containerManager))
                return;

            var containerSys = system.EntityManager.System<ContainerSystem>();


            foreach (var containerId in Containers)
            {
                if (!containerSys.TryGetContainer(owner, containerId, out var container, containerManager))
                    continue;

                containerSys.EmptyContainer(container, true);
            }
        }
    }
}