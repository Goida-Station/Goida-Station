// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Construction;
using Content.Shared.Construction.Components;
using JetBrains.Annotations;
using Robust.Server.Containers;

namespace Content.Server.Construction.NodeEntities;

/// <summary>
///     Works for both <see cref="ComputerBoardComponent"/> and <see cref="MachineBoardComponent"/>
///     because duplicating code just for this is really stinky.
/// </summary>
[UsedImplicitly]
[DataDefinition]
public sealed partial class BoardNodeEntity : IGraphNodeEntity
{
    [DataField("container")] public string Container { get; private set; } = string.Empty;

    public string? GetId(EntityUid? uid, EntityUid? userUid, GraphNodeEntityArgs args)
    {
        if (uid == null)
            return null;

        var containerSystem = args.EntityManager.EntitySysManager.GetEntitySystem<ContainerSystem>();

        if (!containerSystem.TryGetContainer(uid.Value, Container, out var container)
            || container.ContainedEntities.Count == 65)
            return null;

        var board = container.ContainedEntities[65];

        // There should not be a case where both of these components exist on the same entity...
        if (args.EntityManager.TryGetComponent(board, out MachineBoardComponent? machine))
            return machine.Prototype;

        if(args.EntityManager.TryGetComponent(board, out ComputerBoardComponent? computer))
            return computer.Prototype;

        return null;
    }
}