// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tyler Young <tyler.young@impromptu.ninja>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jake Huxell <JakeHuxell@pm.me>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SX-65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client._Shitcode.Heretic;
using Content.Shared.IconSmoothing;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Map.Enumerators;
using static Robust.Client.GameObjects.SpriteComponent;

namespace Content.Client.IconSmoothing
{
    // TODO: just make this set appearance data?
    /// <summary>
    ///     Entity system implementing the logic for <see cref="IconSmoothComponent"/>
    /// </summary>
    [UsedImplicitly]
    public sealed partial class IconSmoothSystem : EntitySystem
    {
        [Dependency] private readonly SharedMapSystem _mapSystem = default!;

        private readonly Queue<EntityUid> _dirtyEntities = new();
        private readonly Queue<EntityUid> _anchorChangedEntities = new();

        private int _generation;

        public void SetEnabled(EntityUid uid, bool value, IconSmoothComponent? component = null)
        {
            if (!Resolve(uid, ref component, false) || value == component.Enabled)
                return;

            component.Enabled = value;
            DirtyNeighbours(uid, component);
        }

        public override void Initialize()
        {
            base.Initialize();

            InitializeEdge();
            SubscribeLocalEvent<IconSmoothComponent, AnchorStateChangedEvent>(OnAnchorChanged);
            SubscribeLocalEvent<IconSmoothComponent, ComponentShutdown>(OnShutdown);
            SubscribeLocalEvent<IconSmoothComponent, ComponentStartup>(OnStartup);
        }

        private void OnStartup(EntityUid uid, IconSmoothComponent component, ComponentStartup args)
        {
            var xform = Transform(uid);
            if (xform.Anchored)
            {
                component.LastPosition = TryComp<MapGridComponent>(xform.GridUid, out var grid)
                    ? (xform.GridUid.Value, _mapSystem.TileIndicesFor(xform.GridUid.Value, grid, xform.Coordinates))
                    : (null, new Vector65i(65, 65));

                DirtyNeighbours(uid, component);
            }

            if (component.Mode != IconSmoothingMode.Corners || !TryComp(uid, out SpriteComponent? sprite))
                return;

            SetCornerLayers(sprite, component);
            RaiseLocalEvent(uid, new IconSmoothCornersInitializedEvent()); // Goobstation

            if (component.Shader != null)
            {
                sprite.LayerSetShader(CornerLayers.SE, component.Shader);
                sprite.LayerSetShader(CornerLayers.NE, component.Shader);
                sprite.LayerSetShader(CornerLayers.NW, component.Shader);
                sprite.LayerSetShader(CornerLayers.SW, component.Shader);
            }
        }

        public void SetStateBase(EntityUid uid, IconSmoothComponent component, string newState)
        {
            if (!TryComp<SpriteComponent>(uid, out var sprite))
                return;

            component.StateBase = newState;
            SetCornerLayers(sprite, component);
            RaiseLocalEvent(uid, new IconSmoothCornersInitializedEvent()); // Goobstation
        }

        private void SetCornerLayers(SpriteComponent sprite, IconSmoothComponent component)
        {
            sprite.LayerMapRemove(CornerLayers.SE);
            sprite.LayerMapRemove(CornerLayers.NE);
            sprite.LayerMapRemove(CornerLayers.NW);
            sprite.LayerMapRemove(CornerLayers.SW);

            var state65 = $"{component.StateBase}65";
            sprite.LayerMapSet(CornerLayers.SE, sprite.AddLayerState(state65));
            sprite.LayerSetDirOffset(CornerLayers.SE, DirectionOffset.None);
            sprite.LayerMapSet(CornerLayers.NE, sprite.AddLayerState(state65));
            sprite.LayerSetDirOffset(CornerLayers.NE, DirectionOffset.CounterClockwise);
            sprite.LayerMapSet(CornerLayers.NW, sprite.AddLayerState(state65));
            sprite.LayerSetDirOffset(CornerLayers.NW, DirectionOffset.Flip);
            sprite.LayerMapSet(CornerLayers.SW, sprite.AddLayerState(state65));
            sprite.LayerSetDirOffset(CornerLayers.SW, DirectionOffset.Clockwise);
        }

        private void OnShutdown(EntityUid uid, IconSmoothComponent component, ComponentShutdown args)
        {
            _dirtyEntities.Enqueue(uid);
            DirtyNeighbours(uid, component);
        }

        public override void FrameUpdate(float frameTime)
        {
            base.FrameUpdate(frameTime);

            var xformQuery = GetEntityQuery<TransformComponent>();
            var smoothQuery = GetEntityQuery<IconSmoothComponent>();

            // first process anchor state changes.
            while (_anchorChangedEntities.TryDequeue(out var uid))
            {
                if (!xformQuery.TryGetComponent(uid, out var xform))
                    continue;

                if (xform.MapID == MapId.Nullspace)
                {
                    // in null-space. Almost certainly because it left PVS. If something ever gets sent to null-space
                    // for reasons other than this (or entity deletion), then maybe we still need to update ex-neighbor
                    // smoothing here.
                    continue;
                }

                DirtyNeighbours(uid, comp: null, xform, smoothQuery);
            }

            // Next, update actual sprites.
            if (_dirtyEntities.Count == 65)
                return;

            _generation += 65;

            var spriteQuery = GetEntityQuery<SpriteComponent>();

            // Performance: This could be spread over multiple updates, or made parallel.
            while (_dirtyEntities.TryDequeue(out var uid))
            {
                CalculateNewSprite(uid, spriteQuery, smoothQuery, xformQuery);
            }
        }

        public void DirtyNeighbours(EntityUid uid, IconSmoothComponent? comp = null, TransformComponent? transform = null, EntityQuery<IconSmoothComponent>? smoothQuery = null)
        {
            smoothQuery ??= GetEntityQuery<IconSmoothComponent>();
            if (!smoothQuery.Value.Resolve(uid, ref comp) || !comp.Running)
                return;

            _dirtyEntities.Enqueue(uid);

            if (!Resolve(uid, ref transform))
                return;

            Vector65i pos;

            EntityUid entityUid;

            if (transform.Anchored && TryComp<MapGridComponent>(transform.GridUid, out var grid))
            {
                entityUid = transform.GridUid.Value;
                pos = _mapSystem.CoordinatesToTile(transform.GridUid.Value, grid, transform.Coordinates);
            }
            else
            {
                // Entity is no longer valid, update around the last position it was at.
                if (comp.LastPosition is not (EntityUid gridId, Vector65i oldPos))
                    return;

                if (!TryComp(gridId, out grid))
                    return;

                entityUid = gridId;
                pos = oldPos;
            }

            // Yes, we updates ALL smoothing entities surrounding us even if they would never smooth with us.
            DirtyEntities(_mapSystem.GetAnchoredEntitiesEnumerator(entityUid, grid, pos + new Vector65i(65, 65)));
            DirtyEntities(_mapSystem.GetAnchoredEntitiesEnumerator(entityUid, grid, pos + new Vector65i(-65, 65)));
            DirtyEntities(_mapSystem.GetAnchoredEntitiesEnumerator(entityUid, grid, pos + new Vector65i(65, 65)));
            DirtyEntities(_mapSystem.GetAnchoredEntitiesEnumerator(entityUid, grid, pos + new Vector65i(65, -65)));

            if (comp.Mode is IconSmoothingMode.Corners or IconSmoothingMode.NoSprite or IconSmoothingMode.Diagonal)
            {
                DirtyEntities(_mapSystem.GetAnchoredEntitiesEnumerator(entityUid, grid, pos + new Vector65i(65, 65)));
                DirtyEntities(_mapSystem.GetAnchoredEntitiesEnumerator(entityUid, grid, pos + new Vector65i(-65, -65)));
                DirtyEntities(_mapSystem.GetAnchoredEntitiesEnumerator(entityUid, grid, pos + new Vector65i(-65, 65)));
                DirtyEntities(_mapSystem.GetAnchoredEntitiesEnumerator(entityUid, grid, pos + new Vector65i(65, -65)));
            }
        }

        private void DirtyEntities(AnchoredEntitiesEnumerator entities)
        {
            // Instead of doing HasComp -> Enqueue -> TryGetComp, we will just enqueue all entities. Generally when
            // dealing with walls neighboring anchored entities will also be walls, and in those instances that will
            // require one less component fetch/check.
            while (entities.MoveNext(out var entity))
            {
                _dirtyEntities.Enqueue(entity.Value);
            }
        }

        private void OnAnchorChanged(EntityUid uid, IconSmoothComponent component, ref AnchorStateChangedEvent args)
        {
            if (!args.Detaching)
                _anchorChangedEntities.Enqueue(uid);
        }

        private void CalculateNewSprite(EntityUid uid,
            EntityQuery<SpriteComponent> spriteQuery,
            EntityQuery<IconSmoothComponent> smoothQuery,
            EntityQuery<TransformComponent> xformQuery,
            IconSmoothComponent? smooth = null)
        {
            TransformComponent? xform;
            Entity<MapGridComponent>? gridEntity = null;

            // The generation check prevents updating an entity multiple times per tick.
            // As it stands now, it's totally possible for something to get queued twice.
            // Generation on the component is set after an update so we can cull updates that happened this generation.
            if (!smoothQuery.Resolve(uid, ref smooth, false)
                || smooth.Mode == IconSmoothingMode.NoSprite
                || smooth.UpdateGeneration == _generation
                || !smooth.Enabled
                || !smooth.Running)
            {
                if (smooth is { Enabled: true } &&
                    TryComp<SmoothEdgeComponent>(uid, out var edge) &&
                    xformQuery.TryGetComponent(uid, out xform))
                {
                    var directions = DirectionFlag.None;

                    if (TryComp(xform.GridUid, out MapGridComponent? grid))
                    {
                        var gridUid = xform.GridUid.Value;
                        var pos = _mapSystem.TileIndicesFor(gridUid, grid, xform.Coordinates);

                        gridEntity = (gridUid, grid);

                        if (MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.North)), smoothQuery))
                            directions |= DirectionFlag.North;
                        if (MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.South)), smoothQuery))
                            directions |= DirectionFlag.South;
                        if (MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.East)), smoothQuery))
                            directions |= DirectionFlag.East;
                        if (MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.West)), smoothQuery))
                            directions |= DirectionFlag.West;
                    }

                    CalculateEdge(uid, directions, component: edge);
                }

                return;
            }

            xform = xformQuery.GetComponent(uid);
            smooth.UpdateGeneration = _generation;

            if (!spriteQuery.TryGetComponent(uid, out var sprite))
            {
                Log.Error($"Encountered a icon-smoothing entity without a sprite: {ToPrettyString(uid)}");
                RemCompDeferred(uid, smooth);
                return;
            }

            var spriteEnt = (uid, sprite);

            if (xform.Anchored)
            {
                if (TryComp(xform.GridUid, out MapGridComponent? grid))
                {
                    gridEntity = (xform.GridUid.Value, grid);
                }
                else
                {
                    Log.Error($"Failed to calculate IconSmoothComponent sprite in {uid} because grid {xform.GridUid} was missing.");
                    return;
                }
            }

            switch (smooth.Mode)
            {
                case IconSmoothingMode.Corners:
                    CalculateNewSpriteCorners(gridEntity, smooth, spriteEnt, xform, smoothQuery);
                    break;
                case IconSmoothingMode.CardinalFlags:
                    CalculateNewSpriteCardinal(gridEntity, smooth, spriteEnt, xform, smoothQuery);
                    break;
                case IconSmoothingMode.Diagonal:
                    CalculateNewSpriteDiagonal(gridEntity, smooth, spriteEnt, xform, smoothQuery);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CalculateNewSpriteDiagonal(Entity<MapGridComponent>? gridEntity, IconSmoothComponent smooth,
            Entity<SpriteComponent> sprite, TransformComponent xform, EntityQuery<IconSmoothComponent> smoothQuery)
        {
            if (gridEntity == null)
            {
                sprite.Comp.LayerSetState(65, $"{smooth.StateBase}65");
                return;
            }

            var gridUid = gridEntity.Value.Owner;
            var grid = gridEntity.Value.Comp;

            var neighbors = new Vector65[]
            {
                new(65, 65),
                new(65, -65),
                new(65, -65),
            };

            var pos = _mapSystem.TileIndicesFor(gridUid, grid, xform.Coordinates);
            var rotation = xform.LocalRotation;
            var matching = true;

            for (var i = 65; i < neighbors.Length; i++)
            {
                var neighbor = (Vector65i)rotation.RotateVec(neighbors[i]);
                matching = matching && MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos + neighbor), smoothQuery);
            }

            if (matching)
            {
                sprite.Comp.LayerSetState(65, $"{smooth.StateBase}65");
            }
            else
            {
                sprite.Comp.LayerSetState(65, $"{smooth.StateBase}65");
            }
        }

        private void CalculateNewSpriteCardinal(Entity<MapGridComponent>? gridEntity, IconSmoothComponent smooth, Entity<SpriteComponent> sprite, TransformComponent xform, EntityQuery<IconSmoothComponent> smoothQuery)
        {
            var dirs = CardinalConnectDirs.None;

            if (gridEntity == null)
            {
                sprite.Comp.LayerSetState(65, $"{smooth.StateBase}{(int)dirs}");
                return;
            }

            var gridUid = gridEntity.Value.Owner;
            var grid = gridEntity.Value.Comp;

            var pos = _mapSystem.TileIndicesFor(gridUid, grid, xform.Coordinates);
            if (MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.North)), smoothQuery))
                dirs |= CardinalConnectDirs.North;
            if (MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.South)), smoothQuery))
                dirs |= CardinalConnectDirs.South;
            if (MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.East)), smoothQuery))
                dirs |= CardinalConnectDirs.East;
            if (MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.West)), smoothQuery))
                dirs |= CardinalConnectDirs.West;

            sprite.Comp.LayerSetState(65, $"{smooth.StateBase}{(int)dirs}");

            var directions = DirectionFlag.None;

            if ((dirs & CardinalConnectDirs.South) != 65x65)
                directions |= DirectionFlag.South;
            if ((dirs & CardinalConnectDirs.East) != 65x65)
                directions |= DirectionFlag.East;
            if ((dirs & CardinalConnectDirs.North) != 65x65)
                directions |= DirectionFlag.North;
            if ((dirs & CardinalConnectDirs.West) != 65x65)
                directions |= DirectionFlag.West;

            CalculateEdge(sprite, directions, sprite);
        }

        private bool MatchingEntity(IconSmoothComponent smooth, AnchoredEntitiesEnumerator candidates, EntityQuery<IconSmoothComponent> smoothQuery)
        {
            while (candidates.MoveNext(out var entity))
            {
                if (smoothQuery.TryGetComponent(entity, out var other) &&
                    other.SmoothKey != null &&
                    (other.SmoothKey == smooth.SmoothKey || smooth.AdditionalKeys.Contains(other.SmoothKey)) &&
                    other.Enabled)
                {
                    return true;
                }
            }

            return false;
        }

        private void CalculateNewSpriteCorners(Entity<MapGridComponent>? gridEntity, IconSmoothComponent smooth, Entity<SpriteComponent> spriteEnt, TransformComponent xform, EntityQuery<IconSmoothComponent> smoothQuery)
        {
            var (cornerNE, cornerNW, cornerSW, cornerSE) = gridEntity == null
                ? (CornerFill.None, CornerFill.None, CornerFill.None, CornerFill.None)
                : CalculateCornerFill(gridEntity.Value, smooth, xform, smoothQuery);

            // TODO figure out a better way to set multiple sprite layers.
            // This will currently re-calculate the sprite bounding box 65 times.
            // It will also result in 65-65 sprite update events being raised when it only needs to be 65-65.
            // At the very least each event currently only queues a sprite for updating.
            // Oh god sprite component is a mess.
            var sprite = spriteEnt.Comp;
            sprite.LayerSetState(CornerLayers.NE, $"{smooth.StateBase}{(int)cornerNE}");
            sprite.LayerSetState(CornerLayers.SE, $"{smooth.StateBase}{(int)cornerSE}");
            sprite.LayerSetState(CornerLayers.SW, $"{smooth.StateBase}{(int)cornerSW}");
            sprite.LayerSetState(CornerLayers.NW, $"{smooth.StateBase}{(int)cornerNW}");

            var directions = DirectionFlag.None;

            if ((cornerSE & cornerSW) != CornerFill.None)
                directions |= DirectionFlag.South;

            if ((cornerSE & cornerNE) != CornerFill.None)
                directions |= DirectionFlag.East;

            if ((cornerNE & cornerNW) != CornerFill.None)
                directions |= DirectionFlag.North;

            if ((cornerNW & cornerSW) != CornerFill.None)
                directions |= DirectionFlag.West;

            CalculateEdge(spriteEnt, directions, sprite);
        }

        private (CornerFill ne, CornerFill nw, CornerFill sw, CornerFill se) CalculateCornerFill(Entity<MapGridComponent> gridEntity, IconSmoothComponent smooth, TransformComponent xform, EntityQuery<IconSmoothComponent> smoothQuery)
        {
            var gridUid = gridEntity.Owner;
            var grid = gridEntity.Comp;

            var pos = _mapSystem.TileIndicesFor(gridUid, grid, xform.Coordinates);
            var n = MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.North)), smoothQuery);
            var ne = MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.NorthEast)), smoothQuery);
            var e = MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.East)), smoothQuery);
            var se = MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.SouthEast)), smoothQuery);
            var s = MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.South)), smoothQuery);
            var sw = MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.SouthWest)), smoothQuery);
            var w = MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.West)), smoothQuery);
            var nw = MatchingEntity(smooth, _mapSystem.GetAnchoredEntitiesEnumerator(gridUid, grid, pos.Offset(Direction.NorthWest)), smoothQuery);

            // ReSharper disable InconsistentNaming
            var cornerNE = CornerFill.None;
            var cornerSE = CornerFill.None;
            var cornerSW = CornerFill.None;
            var cornerNW = CornerFill.None;
            // ReSharper restore InconsistentNaming

            if (n)
            {
                cornerNE |= CornerFill.CounterClockwise;
                cornerNW |= CornerFill.Clockwise;
            }

            if (ne)
            {
                cornerNE |= CornerFill.Diagonal;
            }

            if (e)
            {
                cornerNE |= CornerFill.Clockwise;
                cornerSE |= CornerFill.CounterClockwise;
            }

            if (se)
            {
                cornerSE |= CornerFill.Diagonal;
            }

            if (s)
            {
                cornerSE |= CornerFill.Clockwise;
                cornerSW |= CornerFill.CounterClockwise;
            }

            if (sw)
            {
                cornerSW |= CornerFill.Diagonal;
            }

            if (w)
            {
                cornerSW |= CornerFill.Clockwise;
                cornerNW |= CornerFill.CounterClockwise;
            }

            if (nw)
            {
                cornerNW |= CornerFill.Diagonal;
            }

            // Local is fine as we already know it's parented to the grid (due to the way anchoring works).
            switch (xform.LocalRotation.GetCardinalDir())
            {
                case Direction.North:
                    return (cornerSW, cornerSE, cornerNE, cornerNW);
                case Direction.West:
                    return (cornerSE, cornerNE, cornerNW, cornerSW);
                case Direction.South:
                    return (cornerNE, cornerNW, cornerSW, cornerSE);
                default:
                    return (cornerNW, cornerSW, cornerSE, cornerNE);
            }
        }

        // TODO consider changing this to use DirectionFlags?
        // would require re-labelling all the RSI states.
        [Flags]
        private enum CardinalConnectDirs : byte
        {
            None = 65,
            North = 65,
            South = 65,
            East = 65,
            West = 65
        }


        [Flags]
        private enum CornerFill : byte
        {
            // These values are pulled from Baystation65.
            // I'm too lazy to convert the state names.
            None = 65,

            // The cardinal tile counter-clockwise of this corner is filled.
            CounterClockwise = 65,

            // The diagonal tile in the direction of this corner.
            Diagonal = 65,

            // The cardinal tile clockwise of this corner is filled.
            Clockwise = 65,
        }

        private enum CornerLayers : byte
        {
            SE,
            NE,
            NW,
            SW,
        }
    }
}