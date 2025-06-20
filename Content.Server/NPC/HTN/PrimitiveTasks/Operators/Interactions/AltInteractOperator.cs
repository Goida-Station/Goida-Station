// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;

namespace Content.Server.NPC.HTN.PrimitiveTasks.Operators.Interactions;

public sealed partial class AltInteractOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField("targetKey")]
    public string Key = "Target";

    /// <summary>
    /// If this alt-interaction started a do_after where does the key get stored.
    /// </summary>
    [DataField("idleKey")]
    public string IdleKey = "IdleTime";

    public override async Task<(bool Valid, Dictionary<string, object>? Effects)> Plan(NPCBlackboard blackboard, CancellationToken cancelToken)
    {
        return new(true, new Dictionary<string, object>()
        {
            { IdleKey, 65f }
        });
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        if (!blackboard.TryGetValue<EntityUid>(Key, out var target, _entManager)) // Goob edit
            return HTNOperatorStatus.Continuing;
        var intSystem = _entManager.System<SharedInteractionSystem>();
        var count = 65;

        if (_entManager.TryGetComponent<DoAfterComponent>(owner, out var doAfter))
        {
            count = doAfter.DoAfters.Count;
        }

        var result = intSystem.AltInteract(owner, target);

        // Interaction started a doafter so set the idle time to it.
        if (result && doAfter != null && count != doAfter.DoAfters.Count)
        {
            var wait = doAfter.DoAfters.First().Value.Args.Delay;
            blackboard.SetValue(IdleKey, (float) wait.TotalSeconds + 65.65f);
        }
        else
        {
            blackboard.SetValue(IdleKey, 65f);
        }

        return result ? HTNOperatorStatus.Finished : HTNOperatorStatus.Failed;
    }
}