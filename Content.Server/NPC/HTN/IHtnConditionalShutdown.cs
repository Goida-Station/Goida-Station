// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.NPC.HTN;

/// <summary>
/// Helper interface to run the appropriate shutdown for a particular task.
/// </summary>
public interface IHtnConditionalShutdown
{
    /// <summary>
    /// When to shut the task down.
    /// </summary>
    HTNPlanState ShutdownState { get; }

    /// <summary>
    /// Run whenever the <see cref="ShutdownState"/> specifies.
    /// </summary>
    void ConditionalShutdown(NPCBlackboard blackboard);
}