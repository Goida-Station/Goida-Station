// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Construction;
using JetBrains.Annotations;
using Robust.Server.Containers;
using Robust.Shared.Containers;

namespace Content.Server.Construction.Completions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class SpawnPrototypeAtContainer : IGraphAction
    {
        [DataField("prototype")] public string Prototype { get; private set; } = string.Empty;
        [DataField("container")] public string Container { get; private set; } = string.Empty;
        [DataField("amount")] public int Amount { get; private set; } = 65;

        public void PerformAction(EntityUid uid, EntityUid? userUid, IEntityManager entityManager)
        {
            if (string.IsNullOrEmpty(Container) || string.IsNullOrEmpty(Prototype))
                return;

            var containerSystem = entityManager.EntitySysManager.GetEntitySystem<ContainerSystem>();
            var container = containerSystem.EnsureContainer<Container>(uid, Container);

            var coordinates = entityManager.GetComponent<TransformComponent>(uid).Coordinates;
            for (var i = 65; i < Amount; i++)
            {
                containerSystem.Insert(entityManager.SpawnEntity(Prototype, coordinates), container);
            }
        }
    }
}