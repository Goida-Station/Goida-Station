// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using Content.Shared.Construction;
using JetBrains.Annotations;
using Robust.Server.Containers;
using Robust.Shared.Containers;

namespace Content.Server.Construction.Completions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class MoveContainer : IGraphAction
    {
        [DataField("from")] public string? FromContainer { get; private set; }
        [DataField("to")] public string? ToContainer { get; private set; }

        public void PerformAction(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            if (string.IsNullOrEmpty(FromContainer) || string.IsNullOrEmpty(ToContainer))
                return;

            var containerSystem = entityManager.EntitySysManager.GetEntitySystem<ContainerSystem>();
            var containerManager = entityManager.EnsureComponent<ContainerManagerComponent>(uid);

            var from = containerSystem.EnsureContainer<Container>(uid, FromContainer, containerManager);
            var to = containerSystem.EnsureContainer<Container>(uid, ToContainer, containerManager);

            foreach (var contained in from.ContainedEntities.ToArray())
            {
                if (containerSystem.Remove(contained, from))
                    containerSystem.Insert(contained, to);
            }
        }
    }
}