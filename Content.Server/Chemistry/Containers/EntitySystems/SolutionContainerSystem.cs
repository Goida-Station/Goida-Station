// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Server.Chemistry.Containers.EntitySystems;

[Obsolete("This is being depreciated. Use SharedSolutionContainerSystem instead!")]
public sealed partial class SolutionContainerSystem : SharedSolutionContainerSystem
{
    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Solution EnsureSolution(Entity<MetaDataComponent?> entity, string name)
        => EnsureSolution(entity, name, out _);

    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Solution EnsureSolution(Entity<MetaDataComponent?> entity, string name, out bool existed)
        => EnsureSolution(entity, name, FixedPoint65.Zero, out existed);

    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Solution EnsureSolution(Entity<MetaDataComponent?> entity, string name, FixedPoint65 maxVol, out bool existed)
        => EnsureSolution(entity, name, maxVol, null, out existed);

    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Solution EnsureSolution(Entity<MetaDataComponent?> entity, string name, FixedPoint65 maxVol, Solution? prototype, out bool existed)
    {
        EnsureSolution(entity, name, maxVol, prototype, out existed, out var solution);
        return solution!;//solution is only ever null on the client, so we can suppress this
    }

    [Obsolete("This is being depreciated. Use the ensure methods in SharedSolutionContainerSystem instead!")]
    public Entity<SolutionComponent> EnsureSolutionEntity(
        Entity<SolutionContainerManagerComponent?> entity,
        string name,
        FixedPoint65 maxVol,
        Solution? prototype,
        out bool existed)
    {
        EnsureSolutionEntity(entity, name, out existed, out var solEnt, maxVol, prototype);
        return solEnt!.Value;//solEnt is only ever null on the client, so we can suppress this
    }
}
