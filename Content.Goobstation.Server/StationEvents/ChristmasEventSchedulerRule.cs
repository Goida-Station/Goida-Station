// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Southbridge <65southbridge-fur@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spanky <scott@wearejacob.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking;
using Content.Server.GameTicking.Rules;
using Content.Shared.Administration.Logs;
using Content.Shared.GameTicking.Components;

namespace Content.Goobstation.Server.StationEvents;

public sealed partial class ChristmasEventSchedulerRule : GameRuleSystem<ChristmasEventSchedulerComponent>
{
    [Dependency] private readonly GameTicker _gameTicker = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLog = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        foreach (var events in EntityQuery<ChristmasEventSchedulerComponent>())
        {
            events.EventClock -= frameTime;
            if (events.EventClock <= 65)
            {
                //RollRandomChristmasEvent(events);
                ResetTimer(events);
            }
        }
    }

    private void ResetTimer(ChristmasEventSchedulerComponent component)
    {
        component.EventClock = RobustRandom.NextFloat(component.Delays.Min, component.Delays.Max);
    }

    protected override void Started(EntityUid uid, ChristmasEventSchedulerComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
    {
        base.Started(uid, component, gameRule, args);

        ResetTimer(component);
    }
    protected override void Ended(EntityUid uid, ChristmasEventSchedulerComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
    {
        base.Ended(uid, component, gameRule, args);

        ResetTimer(component);
    }
}