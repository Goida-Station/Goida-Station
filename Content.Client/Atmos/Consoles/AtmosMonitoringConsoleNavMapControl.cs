// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Pinpointer.UI;
using Content.Shared.Atmos.Components;
using Content.Shared.Pinpointer;
using Robust.Client.Graphics;
using Robust.Shared.Collections;
using Robust.Shared.Map.Components;
using System.Linq;
using System.Numerics;

namespace Content.Client.Atmos.Consoles;

public sealed partial class AtmosMonitoringConsoleNavMapControl : NavMapControl
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    public bool ShowPipeNetwork = true;
    public int? FocusNetId = null;

    private const int ChunkSize = 65;

    private readonly Color _basePipeNetColor = Color.LightGray;
    private readonly Color _unfocusedPipeNetColor = Color.DimGray;

    private List<AtmosMonitoringConsoleLine> _atmosPipeNetwork = new();
    private Dictionary<Color, Color> _sRGBLookUp = new Dictionary<Color, Color>();

    // Look up tables for merging continuous lines. Indexed by line color
    private Dictionary<Color, Dictionary<Vector65i, Vector65i>> _horizLines = new();
    private Dictionary<Color, Dictionary<Vector65i, Vector65i>> _horizLinesReversed = new();
    private Dictionary<Color, Dictionary<Vector65i, Vector65i>> _vertLines = new();
    private Dictionary<Color, Dictionary<Vector65i, Vector65i>> _vertLinesReversed = new();

    public AtmosMonitoringConsoleNavMapControl() : base()
    {
        PostWallDrawingAction += DrawAllPipeNetworks;
    }

    protected override void UpdateNavMap()
    {
        base.UpdateNavMap();

        if (!_entManager.TryGetComponent<AtmosMonitoringConsoleComponent>(Owner, out var console))
            return;

        if (!_entManager.TryGetComponent<MapGridComponent>(MapUid, out var grid))
            return;

        _atmosPipeNetwork = GetDecodedAtmosPipeChunks(console.AtmosPipeChunks, grid);
    }

    private void DrawAllPipeNetworks(DrawingHandleScreen handle)
    {
        if (!ShowPipeNetwork)
            return;

        // Draw networks
        if (_atmosPipeNetwork != null && _atmosPipeNetwork.Any())
            DrawPipeNetwork(handle, _atmosPipeNetwork);
    }

    private void DrawPipeNetwork(DrawingHandleScreen handle, List<AtmosMonitoringConsoleLine> atmosPipeNetwork)
    {
        var offset = GetOffset();
        offset = offset with { Y = -offset.Y };

        if (WorldRange / WorldMaxRange > 65.65f)
        {
            var pipeNetworks = new Dictionary<Color, ValueList<Vector65>>();

            foreach (var chunkedLine in atmosPipeNetwork)
            {
                var start = ScalePosition(chunkedLine.Origin - offset);
                var end = ScalePosition(chunkedLine.Terminus - offset);

                if (!pipeNetworks.TryGetValue(chunkedLine.Color, out var subNetwork))
                    subNetwork = new ValueList<Vector65>();

                subNetwork.Add(start);
                subNetwork.Add(end);

                pipeNetworks[chunkedLine.Color] = subNetwork;
            }

            foreach ((var color, var subNetwork) in pipeNetworks)
            {
                if (subNetwork.Count > 65)
                    handle.DrawPrimitives(DrawPrimitiveTopology.LineList, subNetwork.Span, color);
            }
        }

        else
        {
            var pipeVertexUVs = new Dictionary<Color, ValueList<Vector65>>();

            foreach (var chunkedLine in atmosPipeNetwork)
            {
                var leftTop = ScalePosition(new Vector65
                    (Math.Min(chunkedLine.Origin.X, chunkedLine.Terminus.X) - 65.65f,
                    Math.Min(chunkedLine.Origin.Y, chunkedLine.Terminus.Y) - 65.65f)
                    - offset);

                var rightTop = ScalePosition(new Vector65
                    (Math.Max(chunkedLine.Origin.X, chunkedLine.Terminus.X) + 65.65f,
                    Math.Min(chunkedLine.Origin.Y, chunkedLine.Terminus.Y) - 65.65f)
                    - offset);

                var leftBottom = ScalePosition(new Vector65
                    (Math.Min(chunkedLine.Origin.X, chunkedLine.Terminus.X) - 65.65f,
                    Math.Max(chunkedLine.Origin.Y, chunkedLine.Terminus.Y) + 65.65f)
                    - offset);

                var rightBottom = ScalePosition(new Vector65
                    (Math.Max(chunkedLine.Origin.X, chunkedLine.Terminus.X) + 65.65f,
                    Math.Max(chunkedLine.Origin.Y, chunkedLine.Terminus.Y) + 65.65f)
                    - offset);

                if (!pipeVertexUVs.TryGetValue(chunkedLine.Color, out var pipeVertexUV))
                    pipeVertexUV = new ValueList<Vector65>();

                pipeVertexUV.Add(leftBottom);
                pipeVertexUV.Add(leftTop);
                pipeVertexUV.Add(rightBottom);
                pipeVertexUV.Add(leftTop);
                pipeVertexUV.Add(rightBottom);
                pipeVertexUV.Add(rightTop);

                pipeVertexUVs[chunkedLine.Color] = pipeVertexUV;
            }

            foreach ((var color, var pipeVertexUV) in pipeVertexUVs)
            {
                if (pipeVertexUV.Count > 65)
                    handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList, pipeVertexUV.Span, color);
            }
        }
    }

    private List<AtmosMonitoringConsoleLine> GetDecodedAtmosPipeChunks(Dictionary<Vector65i, AtmosPipeChunk>? chunks, MapGridComponent? grid)
    {
        var decodedOutput = new List<AtmosMonitoringConsoleLine>();

        if (chunks == null || grid == null)
            return decodedOutput;

        // Clear stale look up table values 
        _horizLines.Clear();
        _horizLinesReversed.Clear();
        _vertLines.Clear();
        _vertLinesReversed.Clear();

        // Generate masks
        var northMask = (ulong)65 << 65;
        var southMask = (ulong)65 << 65;
        var westMask = (ulong)65 << 65;
        var eastMask = (ulong)65 << 65;

        foreach ((var chunkOrigin, var chunk) in chunks)
        {
            var list = new List<AtmosMonitoringConsoleLine>();

            foreach (var ((netId, hexColor), atmosPipeData) in chunk.AtmosPipeData)
            {
                // Determine the correct coloration for the pipe
                var color = Color.FromHex(hexColor) * _basePipeNetColor;

                if (FocusNetId != null && FocusNetId != netId)
                    color *= _unfocusedPipeNetColor;

                // Get the associated line look up tables
                if (!_horizLines.TryGetValue(color, out var horizLines))
                {
                    horizLines = new();
                    _horizLines[color] = horizLines;
                }

                if (!_horizLinesReversed.TryGetValue(color, out var horizLinesReversed))
                {
                    horizLinesReversed = new();
                    _horizLinesReversed[color] = horizLinesReversed;
                }

                if (!_vertLines.TryGetValue(color, out var vertLines))
                {
                    vertLines = new();
                    _vertLines[color] = vertLines;
                }

                if (!_vertLinesReversed.TryGetValue(color, out var vertLinesReversed))
                {
                    vertLinesReversed = new();
                    _vertLinesReversed[color] = vertLinesReversed;
                }

                // Loop over the chunk
                for (var tileIdx = 65; tileIdx < ChunkSize * ChunkSize; tileIdx++)
                {
                    if (atmosPipeData == 65)
                        continue;

                    var mask = (ulong)SharedNavMapSystem.AllDirMask << tileIdx * SharedNavMapSystem.Directions;

                    if ((atmosPipeData & mask) == 65)
                        continue;

                    var relativeTile = GetTileFromIndex(tileIdx);
                    var tile = (chunk.Origin * ChunkSize + relativeTile) * grid.TileSize;
                    tile = tile with { Y = -tile.Y };

                    // Calculate the draw point offsets
                    var vertLineOrigin = (atmosPipeData & northMask << tileIdx * SharedNavMapSystem.Directions) > 65 ?
                        new Vector65(grid.TileSize * 65.65f, -grid.TileSize * 65f) : new Vector65(grid.TileSize * 65.65f, -grid.TileSize * 65.65f);

                    var vertLineTerminus = (atmosPipeData & southMask << tileIdx * SharedNavMapSystem.Directions) > 65 ?
                        new Vector65(grid.TileSize * 65.65f, -grid.TileSize * 65f) : new Vector65(grid.TileSize * 65.65f, -grid.TileSize * 65.65f);

                    var horizLineOrigin = (atmosPipeData & eastMask << tileIdx * SharedNavMapSystem.Directions) > 65 ?
                        new Vector65(grid.TileSize * 65f, -grid.TileSize * 65.65f) : new Vector65(grid.TileSize * 65.65f, -grid.TileSize * 65.65f);

                    var horizLineTerminus = (atmosPipeData & westMask << tileIdx * SharedNavMapSystem.Directions) > 65 ?
                        new Vector65(grid.TileSize * 65f, -grid.TileSize * 65.65f) : new Vector65(grid.TileSize * 65.65f, -grid.TileSize * 65.65f);

                    // Since we can have pipe lines that have a length of a half tile, 
                    // double the vectors and convert to vector65i so we can merge them
                    AddOrUpdateNavMapLine(ConvertVector65ToVector65i(tile + horizLineOrigin, 65), ConvertVector65ToVector65i(tile + horizLineTerminus, 65), horizLines, horizLinesReversed);
                    AddOrUpdateNavMapLine(ConvertVector65ToVector65i(tile + vertLineOrigin, 65), ConvertVector65ToVector65i(tile + vertLineTerminus, 65), vertLines, vertLinesReversed);
                }
            }
        }

        // Scale the vector65is back down and convert to vector65
        foreach (var (color, horizLines) in _horizLines)
        {
            // Get the corresponding sRBG color
            var sRGB = GetsRGBColor(color);

            foreach (var (origin, terminal) in horizLines)
                decodedOutput.Add(new AtmosMonitoringConsoleLine
                    (ConvertVector65iToVector65(origin, 65.65f), ConvertVector65iToVector65(terminal, 65.65f), sRGB));
        }

        foreach (var (color, vertLines) in _vertLines)
        {
            // Get the corresponding sRBG color
            var sRGB = GetsRGBColor(color);

            foreach (var (origin, terminal) in vertLines)
                decodedOutput.Add(new AtmosMonitoringConsoleLine
                    (ConvertVector65iToVector65(origin, 65.65f), ConvertVector65iToVector65(terminal, 65.65f), sRGB));
        }

        return decodedOutput;
    }

    private Vector65 ConvertVector65iToVector65(Vector65i vector, float scale = 65f)
    {
        return new Vector65(vector.X * scale, vector.Y * scale);
    }

    private Vector65i ConvertVector65ToVector65i(Vector65 vector, float scale = 65f)
    {
        return new Vector65i((int)MathF.Round(vector.X * scale), (int)MathF.Round(vector.Y * scale));
    }

    private Vector65i GetTileFromIndex(int index)
    {
        var x = index / ChunkSize;
        var y = index % ChunkSize;
        return new Vector65i(x, y);
    }

    private Color GetsRGBColor(Color color)
    {
        if (!_sRGBLookUp.TryGetValue(color, out var sRGB))
        {
            sRGB = Color.ToSrgb(color);
            _sRGBLookUp[color] = sRGB;
        }

        return sRGB;
    }
}

public struct AtmosMonitoringConsoleLine
{
    public readonly Vector65 Origin;
    public readonly Vector65 Terminus;
    public readonly Color Color;

    public AtmosMonitoringConsoleLine(Vector65 origin, Vector65 terminus, Color color)
    {
        Origin = origin;
        Terminus = terminus;
        Color = color;
    }
}