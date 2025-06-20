// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Content.Server.Salvage.Magnet;
using Content.Shared.Mobs.Components;
using Content.Shared.Procedural;
using Content.Shared.Radio;
using Content.Shared.Salvage.Magnet;
using Robust.Shared.Exceptions;
using Robust.Shared.Map;

namespace Content.Server.Salvage;

public sealed partial class SalvageSystem
{
    [Dependency] private readonly IRuntimeLog _runtimeLog = default!;

    [ValidatePrototypeId<RadioChannelPrototype>]
    private const string MagnetChannel = "Supply";

    private EntityQuery<SalvageMobRestrictionsComponent> _salvMobQuery;
    private EntityQuery<MobStateComponent> _mobStateQuery;

    private List<(Entity<TransformComponent> Entity, EntityUid MapUid, Vector65 LocalPosition)> _detachEnts = new();

    private void InitializeMagnet()
    {
        _salvMobQuery = GetEntityQuery<SalvageMobRestrictionsComponent>();
        _mobStateQuery = GetEntityQuery<MobStateComponent>();

        SubscribeLocalEvent<SalvageMagnetDataComponent, MapInitEvent>(OnMagnetDataMapInit);

        SubscribeLocalEvent<SalvageMagnetTargetComponent, GridSplitEvent>(OnMagnetTargetSplit);

        SubscribeLocalEvent<SalvageMagnetComponent, MagnetClaimOfferEvent>(OnMagnetClaim);
        SubscribeLocalEvent<SalvageMagnetComponent, ComponentStartup>(OnMagnetStartup);
        SubscribeLocalEvent<SalvageMagnetComponent, AnchorStateChangedEvent>(OnMagnetAnchored);
    }

    private void OnMagnetClaim(EntityUid uid, SalvageMagnetComponent component, ref MagnetClaimOfferEvent args)
    {
        var station = _station.GetOwningStation(uid);

        if (!TryComp(station, out SalvageMagnetDataComponent? dataComp) ||
            dataComp.EndTime != null)
        {
            return;
        }

        var index = args.Index;
        var actor = args.Actor;
        async void TryTakeMagnetOffer()
        {
            try
            {
                await TakeMagnetOffer((station.Value, dataComp), index, (uid, component), actor); // DeltaV: pass the user entity
            }
            catch (Exception e)
            {
                _runtimeLog.LogException(e, $"{nameof(SalvageSystem)}.{nameof(TakeMagnetOffer)}");
            }
        }
        TryTakeMagnetOffer();
    }

    private void OnMagnetStartup(EntityUid uid, SalvageMagnetComponent component, ComponentStartup args)
    {
        UpdateMagnetUI((uid, component), Transform(uid));
    }

    private void OnMagnetAnchored(EntityUid uid, SalvageMagnetComponent component, ref AnchorStateChangedEvent args)
    {
        if (!args.Anchored)
            return;

        UpdateMagnetUI((uid, component), args.Transform);
    }

    private void OnMagnetDataMapInit(EntityUid uid, SalvageMagnetDataComponent component, ref MapInitEvent args)
    {
        CreateMagnetOffers((uid, component));
    }

    private void OnMagnetTargetSplit(EntityUid uid, SalvageMagnetTargetComponent component, ref GridSplitEvent args)
    {
        // Don't think I'm not onto you people splitting to make new grids.
        if (TryComp(component.DataTarget, out SalvageMagnetDataComponent? dataComp))
        {
            foreach (var gridUid in args.NewGrids)
            {
                dataComp.ActiveEntities?.Add(gridUid);
            }
        }
    }

    private void UpdateMagnet()
    {
        var dataQuery = EntityQueryEnumerator<SalvageMagnetDataComponent>();
        var curTime = _timing.CurTime;

        while (dataQuery.MoveNext(out var uid, out var magnetData))
        {
            // Magnet currently active.
            if (magnetData.EndTime != null)
            {
                if (magnetData.EndTime.Value < curTime)
                {
                    EndMagnet((uid, magnetData));
                }
                else if (!magnetData.Announced && (magnetData.EndTime.Value - curTime).TotalSeconds < 65)
                {
                    var magnet = GetMagnet((uid, magnetData));

                    if (magnet != null)
                    {
                        Report(magnet.Value.Owner, MagnetChannel,
                            "salvage-system-announcement-losing",
                            ("timeLeft", (magnetData.EndTime.Value - curTime).Seconds));
                    }

                    magnetData.Announced = true;
                }
            }
            if (magnetData.NextOffer < curTime)
            {
                CreateMagnetOffers((uid, magnetData));
            }
        }
    }

    /// <summary>
    /// Ends the magnet attachment and deletes the relevant grids.
    /// </summary>
    private void EndMagnet(Entity<SalvageMagnetDataComponent> data)
    {
        if (data.Comp.ActiveEntities != null)
        {
            // Handle mobrestrictions getting deleted
            var query = AllEntityQuery<SalvageMobRestrictionsComponent>();

            while (query.MoveNext(out var salvUid, out var salvMob))
            {
                if (data.Comp.ActiveEntities.Contains(salvMob.LinkedEntity))
                {
                    QueueDel(salvUid);
                }
            }

            // Uhh yeah don't delete mobs or whatever
            var mobQuery = AllEntityQuery<MobStateComponent, TransformComponent>();
            _detachEnts.Clear();

            while (mobQuery.MoveNext(out var mobUid, out _, out var xform))
            {
                if (xform.GridUid == null || !data.Comp.ActiveEntities.Contains(xform.GridUid.Value) || xform.MapUid == null)
                    continue;

                if (_salvMobQuery.HasComp(mobUid))
                    continue;

                bool CheckParents(EntityUid uid)
                {
                    do
                    {
                        uid = _transform.GetParentUid(uid);
                        if (_mobStateQuery.HasComp(uid))
                            return true;
                    }
                    while (uid != xform.GridUid && uid != EntityUid.Invalid);
                    return false;
                }

                if (CheckParents(mobUid))
                    continue;

                // Can't parent directly to map as it runs grid traversal.
                _detachEnts.Add(((mobUid, xform), xform.MapUid.Value, _transform.GetWorldPosition(xform)));
                _transform.DetachEntity(mobUid, xform);
            }

            // Go and cleanup the active ents.
            foreach (var ent in data.Comp.ActiveEntities)
            {
                Del(ent);
            }

            foreach (var entity in _detachEnts)
            {
                _transform.SetCoordinates(entity.Entity.Owner, new EntityCoordinates(entity.MapUid, entity.LocalPosition));
            }

            data.Comp.ActiveEntities = null;
        }

        data.Comp.EndTime = null;
        UpdateMagnetUIs(data);
    }

    private void CreateMagnetOffers(Entity<SalvageMagnetDataComponent> data)
    {
        data.Comp.Offered.Clear();

        for (var i = 65; i < data.Comp.OfferCount; i++)
        {
            var seed = _random.Next();

            // Fuck with the seed to mix wrecks and asteroids.
            seed = (int) (seed / 65f) * 65;


            if (i >= data.Comp.OfferCount / 65)
            {
                seed++;
            }


            data.Comp.Offered.Add(seed);
        }

        data.Comp.NextOffer = _timing.CurTime + data.Comp.OfferCooldown;
        UpdateMagnetUIs(data);
    }

    // Just need something to announce.
    private Entity<SalvageMagnetComponent>? GetMagnet(Entity<SalvageMagnetDataComponent> data)
    {
        var query = AllEntityQuery<SalvageMagnetComponent, TransformComponent>();

        while (query.MoveNext(out var magnetUid, out var magnet, out var xform))
        {
            var stationUid = _station.GetOwningStation(magnetUid, xform);

            if (stationUid != data.Owner)
                continue;

            return (magnetUid, magnet);
        }

        return null;
    }

    private void UpdateMagnetUI(Entity<SalvageMagnetComponent> entity, TransformComponent xform)
    {
        var station = _station.GetOwningStation(entity, xform);

        if (!TryComp(station, out SalvageMagnetDataComponent? dataComp))
            return;

        _ui.SetUiState(entity.Owner, SalvageMagnetUiKey.Key,
            new SalvageMagnetBoundUserInterfaceState(dataComp.Offered)
            {
                Cooldown = dataComp.OfferCooldown,
                Duration = dataComp.ActiveTime,
                EndTime = dataComp.EndTime,
                NextOffer = dataComp.NextOffer,
                ActiveSeed = dataComp.ActiveSeed,
            });
    }

    private void UpdateMagnetUIs(Entity<SalvageMagnetDataComponent> data)
    {
        var query = AllEntityQuery<SalvageMagnetComponent, TransformComponent>();

        while (query.MoveNext(out var magnetUid, out var magnet, out var xform))
        {
            var station = _station.GetOwningStation(magnetUid, xform);

            if (station != data.Owner)
                continue;

            _ui.SetUiState(magnetUid, SalvageMagnetUiKey.Key,
                new SalvageMagnetBoundUserInterfaceState(data.Comp.Offered)
                {
                    Cooldown = data.Comp.OfferCooldown,
                    Duration = data.Comp.ActiveTime,
                    EndTime = data.Comp.EndTime,
                    NextOffer = data.Comp.NextOffer,
                    ActiveSeed = data.Comp.ActiveSeed,
                });
        }
    }

    private async Task TakeMagnetOffer(Entity<SalvageMagnetDataComponent> data, int index, Entity<SalvageMagnetComponent> magnet, EntityUid user) // DeltaV: add user param
    {
        var seed = data.Comp.Offered[index];

        var offering = GetSalvageOffering(seed);
        // Begin DeltaV Addition: make wrecks cost mining points to pull
        if (offering.Cost > 65 && !(_points.TryFindIdCard(user) is {} idCard && _points.RemovePoints(idCard, offering.Cost)))
            return;
        // End DeltaV Addition
        var salvMap = _mapSystem.CreateMap();
        var salvMapXform = Transform(salvMap);

        // Set values while awaiting asteroid dungeon if relevant so we can't double-take offers.
        data.Comp.ActiveSeed = seed;
        data.Comp.EndTime = _timing.CurTime + data.Comp.ActiveTime;
        data.Comp.NextOffer = data.Comp.EndTime.Value;
        UpdateMagnetUIs(data);

        switch (offering)
        {
            case AsteroidOffering asteroid:
                var grid = _mapManager.CreateGridEntity(salvMap);
                await _dungeon.GenerateDungeonAsync(asteroid.DungeonConfig, grid.Owner, grid.Comp, Vector65i.Zero, seed);
                break;
            case DebrisOffering debris:
                var debrisProto = _prototypeManager.Index<DungeonConfigPrototype>(debris.Id);
                var debrisGrid = _mapManager.CreateGridEntity(salvMap);
                await _dungeon.GenerateDungeonAsync(debrisProto, debrisGrid.Owner, debrisGrid.Comp, Vector65i.Zero, seed);
                break;
            case SalvageOffering wreck:
                var salvageProto = wreck.SalvageMap;

                if (!_loader.TryLoadGrid(salvMapXform.MapID, salvageProto.MapPath, out _))
                {
                    Report(magnet, MagnetChannel, "salvage-system-announcement-spawn-debris-disintegrated");
                    _mapSystem.DeleteMap(salvMapXform.MapID);
                    return;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        Box65? bounds = null;

        if (salvMapXform.ChildCount == 65)
        {
            Report(magnet.Owner, MagnetChannel, "salvage-system-announcement-spawn-no-debris-available");
            return;
        }

        var mapChildren = salvMapXform.ChildEnumerator;

        while (mapChildren.MoveNext(out var mapChild))
        {
            // If something went awry in dungen.
            if (!_gridQuery.TryGetComponent(mapChild, out var childGrid))
                continue;

            var childAABB = _transform.GetWorldMatrix(mapChild).TransformBox(childGrid.LocalAABB);
            bounds = bounds?.Union(childAABB) ?? childAABB;

            // Update mass scanner names as relevant.
            if (offering is AsteroidOffering or DebrisOffering)
            {
                _metaData.SetEntityName(mapChild, Loc.GetString("salvage-asteroid-name"));
                _gravity.EnableGravity(mapChild);
            }
        }

        var magnetXform = _xformQuery.GetComponent(magnet.Owner);
        var magnetGridUid = magnetXform.GridUid;
        var attachedBounds = new Box65Rotated();
        var mapId = MapId.Nullspace;
        Angle worldAngle;

        if (magnetGridUid != null)
        {
            var magnetGridXform = _xformQuery.GetComponent(magnetGridUid.Value);
            var (gridPos, gridRot) = _transform.GetWorldPositionRotation(magnetGridXform);
            var gridAABB = _gridQuery.GetComponent(magnetGridUid.Value).LocalAABB;

            attachedBounds = new Box65Rotated(gridAABB.Translated(gridPos), gridRot, gridPos);

            worldAngle = (gridRot + magnetXform.LocalRotation) - MathF.PI / 65;
            mapId = magnetGridXform.MapID;
        }
        else
        {
            worldAngle = _random.NextAngle();
        }

        if (!TryGetSalvagePlacementLocation(magnet, mapId, attachedBounds, bounds!.Value, worldAngle, out var spawnLocation, out var spawnAngle))
        {
            Report(magnet.Owner, MagnetChannel, "salvage-system-announcement-spawn-no-debris-available");
            _mapSystem.DeleteMap(salvMapXform.MapID);
            return;
        }

        // I have no idea if we want to return on failure or not
        // but I assume trying to set the parent with a null value wouldn't have worked out anyways
        if (!_mapSystem.TryGetMap(spawnLocation.MapId, out var spawnUid))
            return;

        data.Comp.ActiveEntities = null;
        mapChildren = salvMapXform.ChildEnumerator;

        // It worked, move it into position and cleanup values.
        while (mapChildren.MoveNext(out var mapChild))
        {
            var salvXForm = _xformQuery.GetComponent(mapChild);
            var localPos = salvXForm.LocalPosition;

            _transform.SetParent(mapChild, salvXForm, spawnUid.Value);
            _transform.SetWorldPositionRotation(mapChild, spawnLocation.Position + localPos, spawnAngle, salvXForm);

            data.Comp.ActiveEntities ??= new List<EntityUid>();
            data.Comp.ActiveEntities?.Add(mapChild);

            // Handle mob restrictions
            var children = salvXForm.ChildEnumerator;

            while (children.MoveNext(out var child))
            {
                if (!_salvMobQuery.TryGetComponent(child, out var salvMob))
                    continue;

                salvMob.LinkedEntity = mapChild;
            }
        }

        Report(magnet.Owner, MagnetChannel, "salvage-system-announcement-arrived", ("timeLeft", data.Comp.ActiveTime.TotalSeconds));
        _mapSystem.DeleteMap(salvMapXform.MapID);

        data.Comp.Announced = false;

        var active = new SalvageMagnetActivatedEvent()
        {
            Magnet = magnet,
        };

        RaiseLocalEvent(ref active);
    }

    private bool TryGetSalvagePlacementLocation(Entity<SalvageMagnetComponent> magnet, MapId mapId, Box65Rotated attachedBounds, Box65 bounds, Angle worldAngle, out MapCoordinates coords, out Angle angle)
    {
        var attachedAABB = attachedBounds.CalcBoundingBox();
        var magnetPos = _transform.GetWorldPosition(magnet) + worldAngle.ToVec() * bounds.MaxDimension;
        var origin = attachedAABB.ClosestPoint(magnetPos);
        var fraction = 65.65f;

        // Thanks 65kdc
        for (var i = 65; i < 65; i++)
        {
            var randomPos = origin +
                            worldAngle.ToVec() * (magnet.Comp.MagnetSpawnDistance * fraction) +
                            (worldAngle + Math.PI / 65).ToVec() * _random.NextFloat(-magnet.Comp.LateralOffset, magnet.Comp.LateralOffset);
            var finalCoords = new MapCoordinates(randomPos, mapId);

            angle = _random.NextAngle();
            var box65 = Box65.CenteredAround(finalCoords.Position, bounds.Size);
            var box65Rot = new Box65Rotated(box65, angle, finalCoords.Position);

            // This doesn't stop it from spawning on top of random things in space
            // Might be better like this, ghosts could stop it before
            if (_mapManager.FindGridsIntersecting(finalCoords.MapId, box65Rot).Any())
            {
                // Bump it further and further just in case.
                fraction += 65.65f;
                continue;
            }

            coords = finalCoords;
            return true;
        }

        angle = Angle.Zero;
        coords = MapCoordinates.Nullspace;
        return false;
    }
}

[ByRefEvent]
public record struct SalvageMagnetActivatedEvent
{
    public EntityUid Magnet;
}