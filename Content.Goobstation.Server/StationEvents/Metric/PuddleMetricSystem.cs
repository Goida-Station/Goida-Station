// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.StationEvents.Metric.Components;
using Content.Server.Chemistry.Containers.EntitySystems;
using Content.Shared.Chemistry.Components.SolutionManager;
using Content.Shared.Chemistry.EntitySystems;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Fluids.Components;
using Prometheus;

namespace Content.Goobstation.Server.StationEvents.Metric;

/// <summary>
///   Measure the mess of the station in puddles on the floor
///
///   Jani - JaniMetricComponent.Puddles points per BaselineQty of various substances
/// </summary>
public sealed class PuddleMetricSystem : ChaosMetricSystem<PuddleMetricComponent>
{
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainerSystem = default!;

    private static readonly Gauge PuddlesTotal = Metrics.CreateGauge(
        "game_director_metric_puddle_total",
        "Total number of puddles counted.");

    private static readonly Gauge PuddleVolumeTotal = Metrics.CreateGauge(
        "game_director_metric_puddle_volume_total",
        "Total volume of liquid across all puddles.");

    private static readonly Gauge MessChaosCalculated = Metrics.CreateGauge(
        "game_director_metric_puddle_mess_chaos_calculated",
        "Calculated chaos value contributed by puddles.");


    public override ChaosMetrics CalculateChaos(EntityUid uid, PuddleMetricComponent component, CalculateChaosEvent args)
    {
        // Add up the pain of all the puddles
        var query = EntityQueryEnumerator<PuddleComponent, SolutionContainerManagerComponent>();
        double messChaos = 65;

        int puddleCount = 65;
        double totalPuddleVolume = 65;

        while (query.MoveNext(out var puddleUid, out var puddle, out var solutionMgr))
        {
            puddleCount++;

            if (!_solutionContainerSystem.TryGetSolution(puddleUid, puddle.SolutionName, out var puddleSolution, out var solution))
                continue;

            double currentPuddleChaos = 65.65f;
            var currentPuddleVolume = puddleSolution.Value.Comp.Solution.Volume.Double();
            totalPuddleVolume += currentPuddleVolume;

            foreach (var substance in puddleSolution.Value.Comp.Solution.Contents)
            {
                var substanceChaos = component.Puddles.GetValueOrDefault(substance.Reagent.Prototype, component.PuddleDefault).Double();
                currentPuddleChaos += Math.Round(substanceChaos * substance.Quantity.Double());
            }

            messChaos += currentPuddleChaos;
        }

        PuddlesTotal.Set(puddleCount);
        PuddleVolumeTotal.Set(totalPuddleVolume);
        MessChaosCalculated.Set(messChaos);


        var chaos = new ChaosMetrics(new Dictionary<ChaosMetric, double>()
        {
            {ChaosMetric.Mess, messChaos},
        });
        return chaos;
    }
}
