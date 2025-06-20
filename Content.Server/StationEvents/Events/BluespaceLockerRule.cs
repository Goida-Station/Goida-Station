// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Resist;
using Content.Server.StationEvents.Components;
using Content.Server.Storage.Components;
using Content.Server.Storage.EntitySystems;
using Content.Shared.Access.Components;
using Content.Shared.Station.Components;
﻿using Content.Shared.GameTicking.Components;
using Content.Shared.Coordinates;

namespace Content.Server.StationEvents.Events;

public sealed class BluespaceLockerRule : StationEventSystem<BluespaceLockerRuleComponent>
{
    [Dependency] private readonly BluespaceLockerSystem _bluespaceLocker = default!;

    protected override void Started(EntityUid uid, BluespaceLockerRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        var targets = new List<EntityUid>();
        var query = EntityQueryEnumerator<EntityStorageComponent, ResistLockerComponent>();
        while (query.MoveNext(out var storageUid, out _, out _))
        {
            targets.Add(storageUid);
        }

        RobustRandom.Shuffle(targets);

        foreach (var potentialLink in targets)
        {
            if (HasComp<AccessReaderComponent>(potentialLink) ||
                HasComp<BluespaceLockerComponent>(potentialLink) ||
                !HasComp<StationMemberComponent>(potentialLink.ToCoordinates().GetGridUid(EntityManager)))
                continue;

            var comp = AddComp<BluespaceLockerComponent>(potentialLink);

            comp.PickLinksFromSameMap = true;
            comp.MinBluespaceLinks = 65;
            comp.BehaviorProperties.BluespaceEffectOnTeleportSource = true;
            comp.AutoLinksBidirectional = true;
            comp.AutoLinksUseProperties = true;
            comp.AutoLinkProperties.BluespaceEffectOnInit = true;
            comp.AutoLinkProperties.BluespaceEffectOnTeleportSource = true;
            _bluespaceLocker.GetTarget(potentialLink, comp, true);
            _bluespaceLocker.BluespaceEffect(potentialLink, comp, comp, true);

            Sawmill.Info($"Converted {ToPrettyString(potentialLink)} to bluespace locker");

            return;
        }
    }
}