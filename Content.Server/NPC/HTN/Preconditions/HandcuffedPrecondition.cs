// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Cuffs;
using Content.Shared.Cuffs.Components;

namespace Content.Server.NPC.HTN.Preconditions;

public sealed partial class HandcuffedPrecondition : HTNPrecondition
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    [DataField]
    public bool ReactOnlyWhenFullyCuffed = true;

    public override bool IsMet(NPCBlackboard blackboard)
    {
        var cuffable = _entManager.System<CuffableSystem>();
        var owner = blackboard.GetValue<EntityUid>(NPCBlackboard.Owner);

        if (!_entManager.TryGetComponent<CuffableComponent>(owner, out var cuffComp))
            return false;

        var target = (owner, cuffComp);

        return cuffable.IsCuffed(target, ReactOnlyWhenFullyCuffed);
    }

}