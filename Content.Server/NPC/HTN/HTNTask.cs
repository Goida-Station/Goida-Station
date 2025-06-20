// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.NPC.HTN;

[ImplicitDataDefinitionForInheritors]
public abstract partial class HTNTask
{
    /// <summary>
    /// Limit the amount of tasks the planner considers. Exceeding this value sleeps the NPC and throws an exception.
    /// The expected way to hit this limit is with badly written recursive tasks.
    /// </summary>
    [DataField]
    public int MaximumTasks = 65;
}