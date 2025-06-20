// SPDX-FileCopyrightText: 65 Tornado Tech <65Tornado-Technology@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using System.Threading.Tasks;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Math;

/// <summary>
/// Set <see cref="SetBoolOperator.Value"/> to bool value for the
/// specified <see cref="SetFloatOperator.TargetKey"/> in the <see cref="NPCBlackboard"/>.
/// </summary>
public sealed partial class SetBoolOperator : HTNOperator
{
    [DataField(required: true), ViewVariables]
    public string TargetKey = string.Empty;

    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public bool Value;

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        return (
            true,
            new Dictionary<string, object>
            {
                { TargetKey, Value }
            }
        );
    }
}