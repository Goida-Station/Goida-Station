// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.NPC.HTN.Preconditions;
using Content.Server.NPC.Queries;

namespace Content.Server.NPC.HTN.PrimitiveTasks;

public sealed partial class HTNPrimitiveTask : HTNTask
{
    /// <summary>
    /// Should we re-apply our blackboard state as a result of our operator during startup?
    /// This means you can re-use old data, e.g. re-using a pathfinder result, and avoid potentially expensive operations.
    /// </summary>
    [DataField("applyEffectsOnStartup")] public bool ApplyEffectsOnStartup = true;

    /// <summary>
    /// What needs to be true for this task to be able to run.
    /// The operator may also implement its own checks internally as well if every primitive task using it requires it.
    /// </summary>
    [DataField("preconditions")] public List<HTNPrecondition> Preconditions = new();

    [DataField("operator", required:true)] public HTNOperator Operator = default!;

    /// <summary>
    /// Services actively tick and can potentially update keys, such as combat target.
    /// </summary>
    [DataField("services")] public List<UtilityService> Services = new();
}