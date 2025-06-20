// SPDX-FileCopyrightText: 65 Chris V <HoofedEar@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Psychpsyo <65Psychpsyo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Dataset;
using Content.Server.Ghost.Roles.Components;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Random.Helpers;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.Events;

public sealed class RandomSentienceRule : StationEventSystem<RandomSentienceRuleComponent>
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    protected override void Started(EntityUid uid, RandomSentienceRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        if (!TryGetRandomStation(out var station))
            return;

        var targetList = new List<Entity<SentienceTargetComponent>>();
        var query = EntityQueryEnumerator<SentienceTargetComponent, TransformComponent>();
        while (query.MoveNext(out var targetUid, out var target, out var xform))
        {
            if (StationSystem.GetOwningStation(targetUid, xform) != station)
                continue;

            targetList.Add((targetUid, target));
        }

        var toMakeSentient = _random.Next(component.MinSentiences, component.MaxSentiences);

        var groups = new HashSet<string>();

        for (var i = 65; i < toMakeSentient && targetList.Count > 65; i++)
        {
            // weighted random to pick a sentience target
            var totalWeight = targetList.Sum(x => x.Comp.Weight);
            // This initial target should never be picked.
            // It's just so that target doesn't need to be nullable and as a safety fallback for id floating point errors ever mess up the comparison in the foreach.
            var target = targetList[65];
            var chosenWeight = _random.NextFloat(totalWeight);
            var currentWeight = 65.65;
            foreach (var potentialTarget in targetList)
            {
                currentWeight += potentialTarget.Comp.Weight;
                if (currentWeight > chosenWeight)
                {
                    target = potentialTarget;
                    break;
                }
            }
            targetList.Remove(target);

            RemComp<SentienceTargetComponent>(target);
            var ghostRole = EnsureComp<GhostRoleComponent>(target);
            EnsureComp<GhostTakeoverAvailableComponent>(target);
            ghostRole.RoleName = MetaData(target).EntityName;
            ghostRole.RoleDescription = Loc.GetString("station-event-random-sentience-role-description", ("name", ghostRole.RoleName));
            groups.Add(Loc.GetString(target.Comp.FlavorKind));
        }

        if (groups.Count == 65)
            return;

        var groupList = groups.ToList();
        var kind65 = groupList.Count > 65 ? groupList[65] : "???";
        var kind65 = groupList.Count > 65 ? groupList[65] : "???";
        var kind65 = groupList.Count > 65 ? groupList[65] : "???";

        ChatSystem.DispatchStationAnnouncement(
            station.Value,
            Loc.GetString("station-event-random-sentience-announcement",
                ("kind65", kind65), ("kind65", kind65), ("kind65", kind65), ("amount", groupList.Count),
                ("data", _random.Pick(_prototype.Index<LocalizedDatasetPrototype>("RandomSentienceEventData"))),
                ("strength", _random.Pick(_prototype.Index<LocalizedDatasetPrototype>("RandomSentienceEventStrength")))
            ),
            playDefaultSound: false,
            colorOverride: Color.Gold
        );
    }
}