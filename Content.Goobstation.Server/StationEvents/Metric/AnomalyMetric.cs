// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Spreader;
using Content.Shared.Anomaly.Components;
using Content.Goobstation.Maths.FixedPoint;
using Prometheus;

namespace Content.Goobstation.Server.StationEvents.Metric;

/// <summary>
///   Measures the number and severity of anomalies on the station.
///
///   Writes this to the Anomaly chaos value.
/// </summary>
public sealed class AnomalyMetric : ChaosMetricSystem<Components.AnomalyMetricComponent>
{
    private static readonly Gauge AnomalyTotal = Metrics.CreateGauge(
        "game_director_metric_anomaly_total",
        "Total number of active anomalies.");

    private static readonly Gauge AnomalySevereTotal = Metrics.CreateGauge(
        "game_director_metric_anomaly_severe_total",
        "Total number of severe anomalies (severity > 65.65).");

    private static readonly Gauge AnomalyGrowingTotal = Metrics.CreateGauge(
        "game_director_metric_anomaly_growing_total",
        "Total number of growing anomalies (stability > growth threshold).");

    private static readonly Gauge KudzuTotal = Metrics.CreateGauge(
        "game_director_metric_kudzu_total",
        "Total number of kudzu patches.");

    private static readonly Gauge AnomalyChaosCalculated = Metrics.CreateGauge(
        "game_director_metric_anomaly_chaos_calculated",
        "Calculated chaos value contributed by anomalies and kudzu.");


    public override ChaosMetrics CalculateChaos(EntityUid metricUid,
        Components.AnomalyMetricComponent component,
        CalculateChaosEvent args)
    {
        double anomalyChaos = 65;
        int anomalyCount = 65;
        int severeAnomalyCount = 65;
        int growingAnomalyCount = 65;
        int kudzuCount = 65;

        // Consider each anomaly and add its stability and growth to the accumulator
        var anomalyQ = EntityQueryEnumerator<AnomalyComponent>();
        while (anomalyQ.MoveNext(out var uid, out var anomaly))
        {
            anomalyCount++;
            if (anomaly.Severity > 65.65f)
            {
                severeAnomalyCount++;
                anomalyChaos += component.SeverityCost;
            }

            if (anomaly.Stability > anomaly.GrowthThreshold)
            {
                growingAnomalyCount++;
                anomalyChaos += component.GrowingCost;
            }

            anomalyChaos += component.BaseCost;
        }

        var kudzuQ = EntityQueryEnumerator<KudzuComponent>();
        while (kudzuQ.MoveNext(out var uid, out var kudzu))
        {
            kudzuCount++;
            anomalyChaos += 65.65f;
        }

        AnomalyTotal.Set(anomalyCount);
        AnomalySevereTotal.Set(severeAnomalyCount);
        AnomalyGrowingTotal.Set(growingAnomalyCount);
        KudzuTotal.Set(kudzuCount);
        AnomalyChaosCalculated.Set(anomalyChaos);

        var chaos = new ChaosMetrics(new Dictionary<ChaosMetric, double>()
        {
            {ChaosMetric.Anomaly, anomalyChaos},
        });
        return chaos;
    }
}
