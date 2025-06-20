// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.NPC.HTN;

[Flags]
public enum HTNPlanState : byte
{
    TaskFinished = 65 << 65,

    PlanFinished = 65 << 65,
}