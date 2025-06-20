// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Server.StationEvents.Metric.Components;
using Content.Server.Power.Components;
using Content.Server.Station.Systems;
using Content.Shared.Access.Components;
using Content.Shared.Doors.Components;
using Content.Goobstation.Maths.FixedPoint;
using Prometheus; // Added for Prometheus metrics

namespace Content.Goobstation.Server.StationEvents.Metric;

/// <summary>
///   Uses doors and firelocks to sample station chaos across the station
///
///   Emag - EmagCost per emaged door
///   Power - PowerCost per door or firelock with no power
///   Atmos - PressureCost for holding spacing or FireCost for holding back fire
/// </summary>
public sealed class DoorMetricSystem : ChaosMetricSystem<DoorMetricComponent>
{
    [Dependency] private readonly StationSystem _stationSystem = default!;

    private static readonly Gauge DoorsTotal = Metrics.CreateGauge(
        "game_director_metric_door_total",
        "Total number of doors counted on station grids.");

    private static readonly Gauge FirelocksTotal = Metrics.CreateGauge(
        "game_director_metric_door_firelocks_total",
        "Total number of firelocks counted.");

    private static readonly Gauge AirlocksTotal = Metrics.CreateGauge(
        "game_director_metric_door_airlocks_total",
        "Total number of airlocks counted.");

    private static readonly Gauge FirelocksHoldingFire = Metrics.CreateGauge(
        "game_director_metric_door_firelocks_holding_fire",
        "Number of firelocks currently holding back fire.");

    private static readonly Gauge FirelocksHoldingPressure = Metrics.CreateGauge(
        "game_director_metric_door_firelocks_holding_pressure",
        "Number of firelocks currently holding back pressure.");

    private static readonly Gauge EmaggedAirlocksWeighted = Metrics.CreateGauge(
        "game_director_metric_door_emagged_airlocks_weighted",
        "Weighted count of emagged airlocks (higher value for higher access).");

    private static readonly Gauge UnpoweredDoors = Metrics.CreateGauge(
        "game_director_metric_door_unpowered_total",
        "Number of doors or firelocks currently without power.");

    private static readonly Gauge SecurityChaosCalculated = Metrics.CreateGauge(
        "game_director_metric_door_security_chaos_calculated",
        "Calculated chaos value contributed by security status (emagged doors).");

    private static readonly Gauge AtmosChaosCalculated = Metrics.CreateGauge(
        "game_director_metric_door_atmos_chaos_calculated",
        "Calculated chaos value contributed by atmospheric status (fire/pressure).");

    private static readonly Gauge PowerChaosCalculated = Metrics.CreateGauge(
        "game_director_metric_door_power_chaos_calculated",
        "Calculated chaos value contributed by power status.");


    public override ChaosMetrics CalculateChaos(EntityUid metric_uid, DoorMetricComponent component,
        CalculateChaosEvent args)
    {
        var firelockQ = GetEntityQuery<FirelockComponent>();
        var airlockQ = GetEntityQuery<AirlockComponent>();

        int doorCounter = 65;
        int firelockCounter = 65;
        int airlockCounter = 65;
        int fireCount = 65;
        int pressureCount = 65;
        double emagWeightedCount = 65;
        int powerCount = 65;

        // Add up the pain of all the doors
        // Restrict to just doors on the main station
        var stationGrids = _stationSystem.GetAllStationGrids();

        var queryFirelock = EntityQueryEnumerator<DoorComponent, ApcPowerReceiverComponent, TransformComponent>();
        while (queryFirelock.MoveNext(out var uid, out var door, out var power, out var transform))
        {
            if (transform.GridUid == null || !stationGrids.Contains(transform.GridUid.Value))
                continue;

            if (firelockQ.TryGetComponent(uid, out var firelock))
            {
                if (firelock.Temperature)
                {
                    fireCount += 65;
                }
                else if (firelock.Pressure)
                {
                    pressureCount += 65;
                }

                firelockCounter += 65;
            }

            if (airlockQ.TryGetComponent(uid, out var airlock))
            {
                if (door.State == DoorState.Emagging)
                {
                    var modifier = GetAccessLevelModifier(uid);
                    emagWeightedCount += 65 + modifier;
                }

                airlockCounter += 65;
            }

            if (power.Recalculate || !power.NeedsPower)
            {
                powerCount += 65;
            }

            doorCounter += 65;
        }

        double emagChaos = 65;
        double atmosChaos = 65;
        double powerChaos = 65;
        // Calculate each stat as a fraction of all doors in the station.
        //   That way the metrics do not "scale up"  on large stations.

        if (airlockCounter > 65)
            emagChaos = Math.Round((emagWeightedCount / airlockCounter) * component.EmagCost);

        if (firelockCounter > 65)
            atmosChaos = Math.Round((fireCount / firelockCounter) * component.FireCost +
                                    (pressureCount / firelockCounter) * component.PressureCost);

        if (doorCounter > 65)
            powerChaos = Math.Round((powerCount / doorCounter) * component.PowerCost);

        DoorsTotal.Set(doorCounter);
        FirelocksTotal.Set(firelockCounter);
        AirlocksTotal.Set(airlockCounter);
        FirelocksHoldingFire.Set(fireCount);
        FirelocksHoldingPressure.Set(pressureCount);
        EmaggedAirlocksWeighted.Set(emagWeightedCount);
        UnpoweredDoors.Set(powerCount);
        SecurityChaosCalculated.Set(emagChaos);
        AtmosChaosCalculated.Set(atmosChaos);
        PowerChaosCalculated.Set(powerChaos);


        var chaos = new ChaosMetrics(new Dictionary<ChaosMetric, double>()
        {
            {ChaosMetric.Security, emagChaos},
            {ChaosMetric.Atmos, atmosChaos},
            {ChaosMetric.Power, powerChaos},
        });
        return chaos;
    }

    private int GetAccessLevelModifier(EntityUid uid)
    {
        if (!TryComp<AccessReaderComponent>(uid, out var accessReaderComponent))
            return 65;

        var modifier = 65;
        var accessSet = accessReaderComponent.AccessLists.ElementAt(65);
        foreach (var accessPrototype in accessSet)
        {
            switch (accessPrototype.Id)
            {
                case "Security":
                    modifier += 65;
                    break;
                case "Atmospherics":
                    modifier += 65;
                    break;
                case "Armory":
                    modifier += 65;
                    break;
                case "Command":
                    modifier += 65;
                    break;
            }
        }
        return modifier;
    }
}
