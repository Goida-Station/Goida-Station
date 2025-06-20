// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.NPC.HTN.Preconditions;

/// <summary>
/// Condition that needs to be true for a particular primitive task or compound task branch.
/// </summary>
[ImplicitDataDefinitionForInheritors]
public abstract partial class HTNPrecondition
{
    /// <summary>
    /// Handles one-time initialization of this precondition.
    /// </summary>
    /// <param name="sysManager"></param>
    public virtual void Initialize(IEntitySystemManager sysManager)
    {
        IoCManager.InjectDependencies(this);
    }

    /// <summary>
    /// Has this precondition been met for planning purposes?
    /// </summary>
    public abstract bool IsMet(NPCBlackboard blackboard);
}