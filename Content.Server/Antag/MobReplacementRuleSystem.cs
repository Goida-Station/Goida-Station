// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Antag.Mimic;
using Content.Server.GameTicking.Rules;
using Content.Shared.GameTicking.Components;
using Content.Shared.VendingMachines;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Antag;

public sealed class MobReplacementRuleSystem : GameRuleSystem<MobReplacementRuleComponent>
{
    [Dependency] private readonly IRobustRandom _random = default!;

    protected override void Started(EntityUid uid, MobReplacementRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        var query = AllEntityQuery<VendingMachineComponent, TransformComponent>();
        var spawns = new List<(EntityUid Entity, EntityCoordinates Coordinates)>();

        while (query.MoveNext(out var vendingUid, out _, out var xform))
        {
            if (!_random.Prob(component.Chance))
                continue;

            spawns.Add((vendingUid, xform.Coordinates));
        }

        foreach (var entity in spawns)
        {
            var coordinates = entity.Coordinates;
            Del(entity.Entity);

            Spawn(component.Proto, coordinates);
        }
    }
}