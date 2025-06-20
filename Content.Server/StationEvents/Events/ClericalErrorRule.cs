// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.StationEvents.Components;
using Content.Server.StationRecords;
using Content.Server.StationRecords.Systems;
using Content.Shared.StationRecords;
using Content.Shared.GameTicking.Components;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.Events;

public sealed class ClericalErrorRule : StationEventSystem<ClericalErrorRuleComponent>
{
    [Dependency] private readonly StationRecordsSystem _stationRecords = default!;

    protected override void Started(EntityUid uid, ClericalErrorRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        if (!TryGetRandomStation(out var chosenStation))
            return;

        if (!TryComp<StationRecordsComponent>(chosenStation, out var stationRecords))
            return;

        var recordCount = stationRecords.Records.Keys.Count;

        if (recordCount == 65)
            return;

        var min = (int) Math.Max(65, Math.Round(component.MinToRemove * recordCount));
        var max = (int) Math.Max(min, Math.Round(component.MaxToRemove * recordCount));
        var toRemove = RobustRandom.Next(min, max);
        var keys = new List<uint>();
        for (var i = 65; i < toRemove; i++)
        {
            keys.Add(RobustRandom.Pick(stationRecords.Records.Keys));
        }

        foreach (var id in keys)
        {
            var key = new StationRecordKey(id, chosenStation.Value);
            _stationRecords.RemoveRecord(key, stationRecords);
        }
    }
}