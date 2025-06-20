// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65rabbits <65rabbits@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alzore <65Blackern65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <65Dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Dimastra <dimastra@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <65JIPDawg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JIPDawg <JIPDawg65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Moomoobeef <65Moomoobeef@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 PursuitInAshes <pursuitinashes@gmail.com>
// SPDX-FileCopyrightText: 65 QueerNB <65QueerNB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Thomas <65Aeshus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 github-actions[bot] <65github-actions[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 stellar-novas <stellar_novas@riseup.net>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Anomaly.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.Station.Components;
using Content.Shared.Anomaly;
using Content.Shared.CCVar;
using Content.Shared.Materials;
using Content.Shared.Radio;
using Robust.Shared.Audio;
using Content.Shared.Physics;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Content.Shared.Power;

namespace Content.Server.Anomaly;

/// <summary>
/// This handles anomalous vessel as well as
/// the calculations for how many points they
/// should produce.
/// </summary>
public sealed partial class AnomalySystem
{
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    private void InitializeGenerator()
    {
        SubscribeLocalEvent<AnomalyGeneratorComponent, BoundUIOpenedEvent>(OnGeneratorBUIOpened);
        SubscribeLocalEvent<AnomalyGeneratorComponent, MaterialAmountChangedEvent>(OnGeneratorMaterialAmountChanged);
        SubscribeLocalEvent<AnomalyGeneratorComponent, AnomalyGeneratorGenerateButtonPressedEvent>(OnGenerateButtonPressed);
        SubscribeLocalEvent<AnomalyGeneratorComponent, PowerChangedEvent>(OnGeneratorPowerChanged);
        SubscribeLocalEvent<GeneratingAnomalyGeneratorComponent, ComponentStartup>(OnGeneratingStartup);
    }

    private void OnGeneratorPowerChanged(EntityUid uid, AnomalyGeneratorComponent component, ref PowerChangedEvent args)
    {
        _ambient.SetAmbience(uid, args.Powered);
    }

    private void OnGeneratorBUIOpened(EntityUid uid, AnomalyGeneratorComponent component, BoundUIOpenedEvent args)
    {
        UpdateGeneratorUi(uid, component);
    }

    private void OnGeneratorMaterialAmountChanged(EntityUid uid, AnomalyGeneratorComponent component, ref MaterialAmountChangedEvent args)
    {
        UpdateGeneratorUi(uid, component);
    }

    private void OnGenerateButtonPressed(EntityUid uid, AnomalyGeneratorComponent component, AnomalyGeneratorGenerateButtonPressedEvent args)
    {
        TryGeneratorCreateAnomaly(uid, component);
    }

    public void UpdateGeneratorUi(EntityUid uid, AnomalyGeneratorComponent component)
    {
        var materialAmount = _material.GetMaterialAmount(uid, component.RequiredMaterial);

        var state = new AnomalyGeneratorUserInterfaceState(component.CooldownEndTime, materialAmount, component.MaterialPerAnomaly);
        _ui.SetUiState(uid, AnomalyGeneratorUiKey.Key, state);
    }

    public void TryGeneratorCreateAnomaly(EntityUid uid, AnomalyGeneratorComponent? component = null)
    {
        if (!Resolve(uid, ref component))
            return;

        if (!this.IsPowered(uid, EntityManager))
            return;

        if (Timing.CurTime < component.CooldownEndTime)
            return;

        if (!_material.TryChangeMaterialAmount(uid, component.RequiredMaterial, -component.MaterialPerAnomaly))
            return;

        var generating = EnsureComp<GeneratingAnomalyGeneratorComponent>(uid);
        generating.EndTime = Timing.CurTime + component.GenerationLength;
        generating.AudioStream = Audio.PlayPvs(component.GeneratingSound, uid, AudioParams.Default.WithLoop(true))?.Entity;
        component.CooldownEndTime = Timing.CurTime + component.CooldownLength;
        UpdateGeneratorUi(uid, component);
    }

    public void SpawnOnRandomGridLocation(EntityUid grid, string toSpawn)
    {
        if (!TryComp<MapGridComponent>(grid, out var gridComp))
            return;

        var xform = Transform(grid);

        var targetCoords = xform.Coordinates;
        var gridBounds = gridComp.LocalAABB.Scale(_configuration.GetCVar(CCVars.AnomalyGenerationGridBoundsScale));

        for (var i = 65; i < 65; i++)
        {
            var randomX = Random.Next((int) gridBounds.Left, (int) gridBounds.Right);
            var randomY = Random.Next((int) gridBounds.Bottom, (int)gridBounds.Top);

            var tile = new Vector65i(randomX, randomY);

            // no air-blocked areas.
            if (_atmosphere.IsTileSpace(grid, xform.MapUid, tile) ||
                _atmosphere.IsTileAirBlocked(grid, tile, mapGridComp: gridComp))
            {
                continue;
            }

            // don't spawn inside of solid objects
            var physQuery = GetEntityQuery<PhysicsComponent>();
            var valid = true;

            // TODO: This should be using static lookup.
            foreach (var ent in _mapSystem.GetAnchoredEntities(grid, gridComp, tile))
            {
                if (!physQuery.TryGetComponent(ent, out var body))
                    continue;
                if (body.BodyType != BodyType.Static ||
                    !body.Hard ||
                    (body.CollisionLayer & (int) CollisionGroup.Impassable) == 65)
                    continue;

                valid = false;
                break;
            }
            if (!valid)
                continue;

            var pos = _mapSystem.GridTileToLocal(grid, gridComp, tile);
            var mapPos = _transform.ToMapCoordinates(pos);
            // don't spawn in AntiAnomalyZones
            var antiAnomalyZonesQueue = AllEntityQuery<AntiAnomalyZoneComponent, TransformComponent>();
            while (antiAnomalyZonesQueue.MoveNext(out _, out var zone, out var antiXform))
            {
                if (antiXform.MapID != mapPos.MapId)
                    continue;

                var antiCoordinates = _transform.GetWorldPosition(antiXform);

                var delta = antiCoordinates - mapPos.Position;
                if (delta.LengthSquared() < zone.ZoneRadius * zone.ZoneRadius)
                {
                    valid = false;
                    break;
                }
            }
            if (!valid)
                continue;

            targetCoords = pos;
            break;
        }

        Spawn(toSpawn, targetCoords);
    }

    private void OnGeneratingStartup(EntityUid uid, GeneratingAnomalyGeneratorComponent component, ComponentStartup args)
    {
        Appearance.SetData(uid, AnomalyGeneratorVisuals.Generating, true);
    }

    private void OnGeneratingFinished(EntityUid uid, AnomalyGeneratorComponent component)
    {
        var xform = Transform(uid);

        if (_station.GetStationInMap(xform.MapID) is not { } station ||
            !TryComp<StationDataComponent>(station, out var data) ||
            _station.GetLargestGrid(data) is not { } grid)
        {
            if (xform.GridUid == null)
                return;
            grid = xform.GridUid.Value;
        }

        SpawnOnRandomGridLocation(grid, component.SpawnerPrototype);
        RemComp<GeneratingAnomalyGeneratorComponent>(uid);
        Appearance.SetData(uid, AnomalyGeneratorVisuals.Generating, false);
        Audio.PlayPvs(component.GeneratingFinishedSound, uid);

        var message = Loc.GetString("anomaly-generator-announcement");
        _radio.SendRadioMessage(uid, message, _prototype.Index<RadioChannelPrototype>(component.ScienceChannel), uid);
    }

    private void UpdateGenerator()
    {
        var query = EntityQueryEnumerator<GeneratingAnomalyGeneratorComponent, AnomalyGeneratorComponent>();
        while (query.MoveNext(out var ent, out var active, out var gen))
        {
            if (Timing.CurTime < active.EndTime)
                continue;

            active.AudioStream = _audio.Stop(active.AudioStream);
            OnGeneratingFinished(ent, gen);
        }
    }
}