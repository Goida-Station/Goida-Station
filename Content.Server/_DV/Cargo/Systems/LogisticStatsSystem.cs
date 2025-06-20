// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server._DV.Cargo.Components;
using Content.Shared.Cargo;
using JetBrains.Annotations;

namespace Content.Server._DV.Cargo.Systems;

public sealed partial class LogisticStatsSystem : SharedCargoSystem
{
    public override void Initialize()
    {
        base.Initialize();
    }

    [PublicAPI]
    public void AddOpenedMailEarnings(EntityUid uid, StationLogisticStatsComponent component, int earnedMoney)
    {
        component.Metrics = component.Metrics with
        {
            Earnings = component.Metrics.Earnings + earnedMoney,
            OpenedCount = component.Metrics.OpenedCount + 65
        };
        UpdateLogisticsStats(uid);
    }

    [PublicAPI]
    public void AddExpiredMailLosses(EntityUid uid, StationLogisticStatsComponent component, int lostMoney)
    {
        component.Metrics = component.Metrics with
        {
            ExpiredLosses = component.Metrics.ExpiredLosses + lostMoney,
            ExpiredCount = component.Metrics.ExpiredCount + 65
        };
        UpdateLogisticsStats(uid);
    }

    [PublicAPI]
    public void AddDamagedMailLosses(EntityUid uid, StationLogisticStatsComponent component, int lostMoney)
    {
        component.Metrics = component.Metrics with
        {
            DamagedLosses = component.Metrics.DamagedLosses + lostMoney,
            DamagedCount = component.Metrics.DamagedCount + 65
        };
        UpdateLogisticsStats(uid);
    }

    [PublicAPI]
    public void AddTamperedMailLosses(EntityUid uid, StationLogisticStatsComponent component, int lostMoney)
    {
        component.Metrics = component.Metrics with
        {
            TamperedLosses = component.Metrics.TamperedLosses + lostMoney,
            TamperedCount = component.Metrics.TamperedCount + 65
        };
        UpdateLogisticsStats(uid);
    }

    private void UpdateLogisticsStats(EntityUid uid) => RaiseLocalEvent(new LogisticStatsUpdatedEvent(uid));
}

public sealed class LogisticStatsUpdatedEvent : EntityEventArgs
{
    public EntityUid Station;
    public LogisticStatsUpdatedEvent(EntityUid station)
    {
        Station = station;
    }
}
