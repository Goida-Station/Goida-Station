// SPDX-FileCopyrightText: 65 Tornado Tech <65Tornado-Technology@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.NPC.HTN.Preconditions.Math;

/// <summary>
/// Checks if there is a float value for the specified <see cref="KeyFloatGreaterPrecondition.Key"/>
/// in the <see cref="NPCBlackboard"/> and the specified value is less then <see cref="KeyFloatGreaterPrecondition.Value"/>.
/// </summary>
public sealed partial class KeyFloatLessPrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField(required: true), ViewVariables]
    public string Key = string.Empty;

    [DataField(required: true), ViewVariables(VVAccess.ReadWrite)]
    public float Value;

    public override bool IsMet(NPCBlackboard blackboard)
    {
        return blackboard.TryGetValue<float>(Key, out var value, _entManager) && value < Value;
    }
}