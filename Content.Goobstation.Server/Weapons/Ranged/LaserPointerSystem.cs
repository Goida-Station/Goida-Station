// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.NPC.Components;
using Content.Shared._Goobstation.Weapons.SmartGun;
using Content.Shared.Wieldable.Components;
using Robust.Server.GameStates;

namespace Content.Goobstation.Server.Weapons.Ranged;

public sealed class LaserPointerSystem : SharedLaserPointerSystem
{
    [Dependency] private readonly PvsOverrideSystem _override = default!;

    protected override void PvsOverride(EntityUid entity)
    {
        base.PvsOverride(entity);

        _override.AddGlobalOverride(entity);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var npcCombatQuery = GetEntityQuery<NPCRangedCombatComponent>();
        var query = EntityQueryEnumerator<LaserPointerComponent, WieldableComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var pointer, out var wieldable, out var xform))
        {
            if (!wieldable.Wielded)
                continue;

            if (npcCombatQuery.HasComp(xform.ParentUid) ||
                Timing.CurTime - pointer.LastNetworkEventTime < pointer.MaxDelayBetweenNetworkEvents)
                continue;

            AddOrRemoveLine(GetNetEntity(uid), pointer, wieldable, xform, null, null);
        }
    }
}