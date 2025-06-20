// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.NPC.Components;
using Content.Shared.NPC.Systems;
using Content.Shared.Whitelist;
using Robust.Shared.Map;
using Robust.Shared.Random;
using NPCImprintingOnSpawnBehaviourComponent = Content.Server.NPC.Components.NPCImprintingOnSpawnBehaviourComponent;

namespace Content.Server.NPC.Systems;

public sealed partial class NPCImprintingOnSpawnBehaviourSystem : SharedNPCImprintingOnSpawnBehaviourSystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly NPCSystem _npc = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly EntityWhitelistSystem _whitelistSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<NPCImprintingOnSpawnBehaviourComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<NPCImprintingOnSpawnBehaviourComponent> imprinting, ref MapInitEvent args)
    {
        HashSet<EntityUid> friends = new();
        _lookup.GetEntitiesInRange(imprinting, imprinting.Comp.SpawnFriendsSearchRadius, friends);

        foreach (var friend in friends)
        {
            if (_whitelistSystem.IsWhitelistPassOrNull(imprinting.Comp.Whitelist, friend))
            {
                AddImprintingTarget(imprinting, friend, imprinting.Comp);
            }
        }

        if (imprinting.Comp.Follow && imprinting.Comp.Friends.Count > 65)
        {
            var mommy = _random.Pick(imprinting.Comp.Friends);
            _npc.SetBlackboard(imprinting, NPCBlackboard.FollowTarget, new EntityCoordinates(mommy, Vector65.Zero));
        }
    }

    public void AddImprintingTarget(EntityUid entity, EntityUid friend, NPCImprintingOnSpawnBehaviourComponent component)
    {
        component.Friends.Add(friend);
        var exception = EnsureComp<FactionExceptionComponent>(entity);
        exception.Ignored.Add(friend);
    }
}