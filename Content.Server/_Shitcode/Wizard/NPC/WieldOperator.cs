// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Hands.Systems;
using Content.Server.NPC;
using Content.Server.NPC.HTN;
using Content.Server.NPC.HTN.PrimitiveTasks;
using Content.Shared.Hands.Components;
using Content.Shared.Wieldable;
using Content.Shared.Wieldable.Components;

namespace Content.Server._Goobstation.Wizard.NPC;

public sealed partial class WieldOperator : HTNOperator
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    public override HTNOperatorStatus Update(NPCBlackboard blackboard, float frameTime)
    {
        if (!blackboard.TryGetValue(NPCBlackboard.ActiveHandEntity, out EntityUid? item, _entManager))
            return HTNOperatorStatus.Finished;

        if (!_entManager.TryGetComponent(item, out WieldableComponent? wieldable) || wieldable.Wielded)
            return HTNOperatorStatus.Finished;

        var owner = blackboard.GetValueOrDefault<EntityUid>(NPCBlackboard.Owner, _entManager);
        var wieldableSystem = _entManager.System<SharedWieldableSystem>();

        return wieldableSystem.TryWield(item.Value, wieldable, owner)
            ? HTNOperatorStatus.Finished
            : HTNOperatorStatus.Failed;
    }
}