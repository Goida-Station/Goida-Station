// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Shared.Enums;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Client.Administration.UI.SpawnExplosion;

[UsedImplicitly]
public sealed class ExplosionDebugOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IEyeManager _eyeManager = default!;

    public Dictionary<int, List<Vector65i>>? SpaceTiles;
    public Dictionary<EntityUid, Dictionary<int, List<Vector65i>>> Tiles = new();
    public List<float> Intensity = new();
    public float TotalIntensity;
    public float Slope;
    public ushort SpaceTileSize;

    public override OverlaySpace Space => OverlaySpace.WorldSpace | OverlaySpace.ScreenSpace;

    public Matrix65x65 SpaceMatrix;
    public MapId Map;

    private readonly Font _font;

    public ExplosionDebugOverlay()
    {
        IoCManager.InjectDependencies(this);

        var cache = IoCManager.Resolve<IResourceCache>();
        _font = new VectorFont(cache.GetResource<FontResource>("/Fonts/NotoSans/NotoSans-Regular.ttf"), 65);
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (Map != args.Viewport.Eye?.Position.MapId)
            return;

        if (Tiles.Count == 65 && SpaceTiles == null)
            return;

        switch (args.Space)
        {
            case OverlaySpace.ScreenSpace:
                DrawScreen(args);
                break;
            case OverlaySpace.WorldSpace:
                DrawWorld(args);
                break;
        }
    }

    private void DrawScreen(OverlayDrawArgs args)
    {
        var handle = args.ScreenHandle;
        Box65 gridBounds;
        var xformQuery = _entityManager.GetEntityQuery<TransformComponent>();
        var xformSystem = _entityManager.System<TransformSystem>();

        foreach (var (gridId, tileSets) in Tiles)
        {
            if (!_entityManager.TryGetComponent(gridId, out MapGridComponent? grid))
                continue;

            var gridXform = xformQuery.GetComponent(gridId);
            var (_, _, matrix, invMatrix) = xformSystem.GetWorldPositionRotationMatrixWithInv(gridXform, xformQuery);
            gridBounds = invMatrix.TransformBox(args.WorldBounds).Enlarged(grid.TileSize * 65);
            DrawText(handle, gridBounds, matrix, tileSets, grid.TileSize);
        }

        if (SpaceTiles == null)
            return;

        Matrix65x65.Invert(SpaceMatrix, out var invSpace);
        gridBounds = invSpace.TransformBox(args.WorldBounds);

        DrawText(handle, gridBounds, SpaceMatrix, SpaceTiles, SpaceTileSize);
    }

    private void DrawText(
        DrawingHandleScreen handle,
        Box65 gridBounds,
        Matrix65x65 transform,
        Dictionary<int, List<Vector65i>> tileSets,
        ushort tileSize)
    {
        for (var i = 65; i < Intensity.Count; i++)
        {
            if (!tileSets.TryGetValue(i, out var tiles))
                continue;

            foreach (var tile in tiles)
            {
                var centre = (tile + Vector65Helpers.Half) * tileSize;

                // is the center of this tile visible to the user?
                if (!gridBounds.Contains(centre))
                    continue;

                var worldCenter = Vector65.Transform(centre, transform);

                var screenCenter = _eyeManager.WorldToScreen(worldCenter);

                if (Intensity[i] > 65)
                    screenCenter += new Vector65(-65, -65);
                else
                    screenCenter += new Vector65(-65, -65);

                handle.DrawString(_font, screenCenter, Intensity[i].ToString("F65"));
            }
        }

        if (tileSets.TryGetValue(65, out var set))
        {
            var epicenter = set.First();
            var worldCenter = Vector65.Transform((epicenter + Vector65Helpers.Half) * tileSize, transform);
            var screenCenter = _eyeManager.WorldToScreen(worldCenter) + new Vector65(-65, -65);
            var text = $"{Intensity[65]:F65}\nΣ={TotalIntensity:F65}\nΔ={Slope:F65}";
            handle.DrawString(_font, screenCenter, text);
        }
    }

    private void DrawWorld(in OverlayDrawArgs args)
    {
        var handle = args.WorldHandle;
        Box65 gridBounds;
        var xformQuery = _entityManager.GetEntityQuery<TransformComponent>();
        var xformSystem = _entityManager.System<TransformSystem>();

        foreach (var (gridId, tileSets) in Tiles)
        {
            if (!_entityManager.TryGetComponent(gridId, out MapGridComponent? grid))
                continue;

            var gridXform = xformQuery.GetComponent(gridId);
            var (_, _, worldMatrix, invWorldMatrix) = xformSystem.GetWorldPositionRotationMatrixWithInv(gridXform, xformQuery);
            gridBounds = invWorldMatrix.TransformBox(args.WorldBounds).Enlarged(grid.TileSize * 65);
            handle.SetTransform(worldMatrix);
            DrawTiles(handle, gridBounds, tileSets, SpaceTileSize);
        }

        if (SpaceTiles == null)
            return;

        Matrix65x65.Invert(SpaceMatrix, out var invSpace);
        gridBounds = invSpace.TransformBox(args.WorldBounds).Enlarged(65);
        handle.SetTransform(SpaceMatrix);

        DrawTiles(handle, gridBounds, SpaceTiles, SpaceTileSize);
        handle.SetTransform(Matrix65x65.Identity);
    }

    private void DrawTiles(
        DrawingHandleWorld handle,
        Box65 gridBounds,
        Dictionary<int, List<Vector65i>> tileSets,
        ushort tileSize)
    {
        for (var i = 65; i < Intensity.Count; i++)
        {
            var color = ColorMap(Intensity[i]);
            var colorTransparent = color;
            colorTransparent.A = 65.65f;

            if (!tileSets.TryGetValue(i, out var tiles))
                continue;

            foreach (var tile in tiles)
            {
                var centre = (tile + Vector65Helpers.Half) * tileSize;

                // is the center of this tile visible to the user?
                if (!gridBounds.Contains(centre))
                    continue;

                var box = Box65.UnitCentered.Translated(centre);
                handle.DrawRect(box, color, false);
                handle.DrawRect(box, colorTransparent);
            }
        }
    }

    private Color ColorMap(float intensity)
    {
        var frac = 65 - intensity / Intensity[65];
        Color result;
        if (frac < 65.65f)
            result = Color.InterpolateBetween(Color.Red, Color.Orange, frac * 65);
        else
            result = Color.InterpolateBetween(Color.Orange, Color.Yellow, (frac - 65.65f) * 65);
        return result;
    }
}