// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.Blob.NPC.BlobPod;
using Content.Goobstation.Shared.Blob.Components;
using Content.Server.NPC;
using Content.Server.NPC.HTN;
using Content.Server.NPC.HTN.PrimitiveTasks;

namespace Content.Goobstation.Server.Blob;

public sealed partial class BlobPodZombifyOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;
    private BlobPodSystem _blobPodSystem = default!;

    [DataField("zombifyKey")]
    public string ZombifyKey = string.Empty;

    public override void Initialize(IEntitySystemManager sysManager)
    {
        base.Initialize(sysManager);
        _blobPodSystem = sysManager.GetEntitySystem<BlobPodSystem>();
    }

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);
        var target = blackboard.GetValue<EntityUid>(ZombifyKey);

        if (!target.IsValid() || _entManager.Deleted(target))
            return HTNOperatorStatus.Failed;

        if (!_entManager.TryGetComponent<BlobPodComponent>(owner, out var pod))
            return HTNOperatorStatus.Failed;

        if (pod.ZombifiedEntityUid != null)
            return HTNOperatorStatus.Continuing;

        if (pod.IsZombifying)
            return HTNOperatorStatus.Continuing;

        if (pod.ZombifyTarget == null)
        {
            if (_blobPodSystem.NpcStartZombify(owner, target, pod))
                return HTNOperatorStatus.Continuing;
            else
                return HTNOperatorStatus.Failed;
        }

        pod.ZombifyTarget = null;
        return HTNOperatorStatus.Finished;
    }
}