// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Threading;
using System.Threading.Tasks;
using Robust.Shared.Random;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators;

public sealed partial class PickRandomRotationOperator : HTNOperator
{
    [Dependency] private readonly IRobustRandom _random = default!;

    [DataField("targetKey")]
    public string TargetKey = "RotateTarget";

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard,
        CancellationToken cancelToken)
    {
        var rotation = _random.NextAngle();
        return (true, new Dictionary<string, object>()
        {
            {TargetKey, rotation}
        });
    }
}