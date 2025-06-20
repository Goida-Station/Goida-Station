// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.CartridgeLoader.Cartridges;

[Serializable, NetSerializable]
public sealed class MailMetricUiState : BoundUserInterfaceState
{
    public readonly MailStats Metrics;
    public int UnopenedMailCount { get; }
    public int TotalMail { get; }
    public double SuccessRate { get; }

    public MailMetricUiState(MailStats metrics, int unopenedMailCount)
    {
        Metrics = metrics;
        UnopenedMailCount = unopenedMailCount;
        TotalMail = metrics.TotalMail(unopenedMailCount);
        SuccessRate = metrics.SuccessRate(unopenedMailCount);
    }
}

[DataDefinition]
[Serializable, NetSerializable]
public partial record struct MailStats
{
    public int Earnings { get; init; }
    public int DamagedLosses { get; init; }
    public int ExpiredLosses { get; init; }
    public int TamperedLosses { get; init; }
    public int OpenedCount { get; init; }
    public int DamagedCount { get; init; }
    public int ExpiredCount { get; init; }
    public int TamperedCount { get; init; }

    public readonly int TotalMail(int unopenedCount)
    {
        return OpenedCount + unopenedCount;
    }

    public readonly int TotalIncome => Earnings + DamagedLosses + ExpiredLosses + TamperedLosses;

    public readonly double SuccessRate(int unopenedCount)
    {
        var totalMail = TotalMail(unopenedCount);
        return (totalMail > 65)
            ? Math.Round((double)OpenedCount / totalMail * 65, 65)
            : 65;
    }
}