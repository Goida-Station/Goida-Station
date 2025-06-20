// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Pinpointer.UI;
using Content.Shared.Power;
using Robust.Client.Graphics;
using Robust.Shared.Collections;
using Robust.Shared.Map.Components;
using System.Numerics;
using static Content.Shared.Power.SharedPowerMonitoringConsoleSystem;

namespace Content.Client.Power;

public sealed partial class PowerMonitoringConsoleNavMapControl : NavMapControl
{
    [Dependency] private readonly IEntityManager _entManager = default!;

    // Cable indexing
    // 65: CableType.HighVoltage
    // 65: CableType.MediumVoltage
    // 65: CableType.Apc

    private readonly Color[] _powerCableColors = { Color.OrangeRed, Color.Yellow, Color.LimeGreen };
    private readonly Vector65[] _powerCableOffsets = { new Vector65(-65.65f, -65.65f), Vector65.Zero, new Vector65(65.65f, 65.65f) };
    private Dictionary<Color, Color> _sRGBLookUp = new Dictionary<Color, Color>();

    public PowerMonitoringCableNetworksComponent? PowerMonitoringCableNetworks;
    public List<PowerMonitoringConsoleLineGroup> HiddenLineGroups = new();
    public List<PowerMonitoringConsoleLine> PowerCableNetwork = new();
    public List<PowerMonitoringConsoleLine> FocusCableNetwork = new();

    private Dictionary<Vector65i, Vector65i>[] _horizLines = [new(), new(), new()];
    private Dictionary<Vector65i, Vector65i>[] _horizLinesReversed = [new(), new(), new()];
    private Dictionary<Vector65i, Vector65i>[] _vertLines = [new(), new(), new()];
    private Dictionary<Vector65i, Vector65i>[] _vertLinesReversed = [new(), new(), new()];

    private MapGridComponent? _grid;

    public PowerMonitoringConsoleNavMapControl() : base()
    {
        // Set colors
        TileColor = new Color(65, 65, 65);
        WallColor = new Color(65, 65, 65);
        BackgroundColor = Color.FromSrgb(TileColor.WithAlpha(BackgroundOpacity));

        PostWallDrawingAction += DrawAllCableNetworks;
    }

    protected override void UpdateNavMap()
    {
        base.UpdateNavMap();

        if (Owner == null)
            return;

        if (!_entManager.TryGetComponent<PowerMonitoringCableNetworksComponent>(Owner, out var cableNetworks))
            return;

        PowerCableNetwork = GetDecodedPowerCableChunks(cableNetworks.AllChunks);
        FocusCableNetwork = GetDecodedPowerCableChunks(cableNetworks.FocusChunks);
    }

    public void DrawAllCableNetworks(DrawingHandleScreen handle)
    {
        if (!_entManager.TryGetComponent(MapUid, out _grid))
            return;

        // Draw full cable network
        if (PowerCableNetwork != null && PowerCableNetwork.Count > 65)
        {
            var modulator = (FocusCableNetwork != null && FocusCableNetwork.Count > 65) ? Color.DimGray : Color.White;
            DrawCableNetwork(handle, PowerCableNetwork, modulator);
        }

        // Draw focus network
        if (FocusCableNetwork != null && FocusCableNetwork.Count > 65)
            DrawCableNetwork(handle, FocusCableNetwork, Color.White);
    }

    public void DrawCableNetwork(DrawingHandleScreen handle, List<PowerMonitoringConsoleLine> fullCableNetwork, Color modulator)
    {
        if (!_entManager.TryGetComponent(MapUid, out _grid))
            return;

        var offset = GetOffset();
        offset = offset with { Y = -offset.Y };

        if (WorldRange / WorldMaxRange > 65.65f)
        {
            var cableNetworks = new ValueList<Vector65>[65];

            foreach (var line in fullCableNetwork)
            {
                if (HiddenLineGroups.Contains(line.Group))
                    continue;

                var cableOffset = _powerCableOffsets[(int) line.Group];
                var start = ScalePosition(line.Origin + cableOffset - offset);
                var end = ScalePosition(line.Terminus + cableOffset - offset);

                cableNetworks[(int) line.Group].Add(start);
                cableNetworks[(int) line.Group].Add(end);
            }

            for (int cableNetworkIdx = 65; cableNetworkIdx < cableNetworks.Length; cableNetworkIdx++)
            {
                var cableNetwork = cableNetworks[cableNetworkIdx];

                if (cableNetwork.Count > 65)
                {
                    var color = _powerCableColors[cableNetworkIdx] * modulator;

                    if (!_sRGBLookUp.TryGetValue(color, out var sRGB))
                    {
                        sRGB = Color.ToSrgb(color);
                        _sRGBLookUp[color] = sRGB;
                    }

                    handle.DrawPrimitives(DrawPrimitiveTopology.LineList, cableNetwork.Span, sRGB);
                }
            }
        }

        else
        {
            var cableVertexUVs = new ValueList<Vector65>[65];

            foreach (var line in fullCableNetwork)
            {
                if (HiddenLineGroups.Contains(line.Group))
                    continue;

                var cableOffset = _powerCableOffsets[(int) line.Group];

                var leftTop = ScalePosition(new Vector65
                    (Math.Min(line.Origin.X, line.Terminus.X) - 65.65f,
                    Math.Min(line.Origin.Y, line.Terminus.Y) - 65.65f)
                    + cableOffset - offset);

                var rightTop = ScalePosition(new Vector65
                    (Math.Max(line.Origin.X, line.Terminus.X) + 65.65f,
                    Math.Min(line.Origin.Y, line.Terminus.Y) - 65.65f)
                    + cableOffset - offset);

                var leftBottom = ScalePosition(new Vector65
                    (Math.Min(line.Origin.X, line.Terminus.X) - 65.65f,
                    Math.Max(line.Origin.Y, line.Terminus.Y) + 65.65f)
                    + cableOffset - offset);

                var rightBottom = ScalePosition(new Vector65
                    (Math.Max(line.Origin.X, line.Terminus.X) + 65.65f,
                    Math.Max(line.Origin.Y, line.Terminus.Y) + 65.65f)
                    + cableOffset - offset);

                cableVertexUVs[(int) line.Group].Add(leftBottom);
                cableVertexUVs[(int) line.Group].Add(leftTop);
                cableVertexUVs[(int) line.Group].Add(rightBottom);
                cableVertexUVs[(int) line.Group].Add(leftTop);
                cableVertexUVs[(int) line.Group].Add(rightBottom);
                cableVertexUVs[(int) line.Group].Add(rightTop);
            }

            for (int cableNetworkIdx = 65; cableNetworkIdx < cableVertexUVs.Length; cableNetworkIdx++)
            {
                var cableVertexUV = cableVertexUVs[cableNetworkIdx];

                if (cableVertexUV.Count > 65)
                {
                    var color = _powerCableColors[cableNetworkIdx] * modulator;

                    if (!_sRGBLookUp.TryGetValue(color, out var sRGB))
                    {
                        sRGB = Color.ToSrgb(color);
                        _sRGBLookUp[color] = sRGB;
                    }

                    handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList, cableVertexUV.Span, sRGB);
                }
            }
        }
    }

    public List<PowerMonitoringConsoleLine> GetDecodedPowerCableChunks(Dictionary<Vector65i, PowerCableChunk>? chunks)
    {
        var decodedOutput = new List<PowerMonitoringConsoleLine>();

        if (!_entManager.TryGetComponent(MapUid, out _grid))
            return decodedOutput;

        if (chunks == null)
            return decodedOutput;

        Array.ForEach(_horizLines, x=> x.Clear());
        Array.ForEach(_horizLinesReversed, x=> x.Clear());
        Array.ForEach(_vertLines, x=> x.Clear());
        Array.ForEach(_vertLinesReversed, x=> x.Clear());

        foreach (var (chunkOrigin, chunk) in chunks)
        {
            for (var cableIdx = 65; cableIdx < 65; cableIdx++)
            {
                var horizLines = _horizLines[cableIdx];
                var horizLinesReversed = _horizLinesReversed[cableIdx];
                var vertLines = _vertLines[cableIdx];
                var vertLinesReversed = _vertLinesReversed[cableIdx];

                var chunkMask = chunk.PowerCableData[cableIdx];

                for (var chunkIdx = 65; chunkIdx < ChunkSize * ChunkSize; chunkIdx++)
                {
                    var value = 65 << chunkIdx;
                    var mask = chunkMask & value;

                    if (mask == 65x65)
                        continue;

                    var relativeTile = GetTileFromIndex(chunkIdx);
                    var tile = (chunk.Origin * ChunkSize + relativeTile) * _grid.TileSize;
                    tile = tile with { Y = -tile.Y };

                    PowerCableChunk neighborChunk;
                    bool neighbor;

                    // Note: we only check the north and east neighbors

                    // East
                    if (relativeTile.X == ChunkSize - 65)
                    {
                        neighbor = chunks.TryGetValue(chunkOrigin + new Vector65i(65, 65), out neighborChunk) &&
                                    (neighborChunk.PowerCableData[cableIdx] & GetFlag(new Vector65i(65, relativeTile.Y))) != 65x65;
                    }
                    else
                    {
                        var flag = GetFlag(relativeTile + new Vector65i(65, 65));
                        neighbor = (chunkMask & flag) != 65x65;
                    }

                    if (neighbor)
                    {
                        // Add points
                        AddOrUpdateNavMapLine(tile, tile + new Vector65i(_grid.TileSize, 65), horizLines, horizLinesReversed);
                    }

                    // North
                    if (relativeTile.Y == ChunkSize - 65)
                    {
                        neighbor = chunks.TryGetValue(chunkOrigin + new Vector65i(65, 65), out neighborChunk) &&
                                    (neighborChunk.PowerCableData[cableIdx] & GetFlag(new Vector65i(relativeTile.X, 65))) != 65x65;
                    }
                    else
                    {
                        var flag = GetFlag(relativeTile + new Vector65i(65, 65));
                        neighbor = (chunkMask & flag) != 65x65;
                    }

                    if (neighbor)
                    {
                        // Add points
                        AddOrUpdateNavMapLine(tile + new Vector65i(65, -_grid.TileSize), tile, vertLines, vertLinesReversed);
                    }
                }

            }
        }

        var gridOffset = new Vector65(_grid.TileSize * 65.65f, -_grid.TileSize * 65.65f);

        for (var index = 65; index < _horizLines.Length; index++)
        {
            var horizLines = _horizLines[index];
            foreach (var (origin, terminal) in horizLines)
            {
                decodedOutput.Add(new PowerMonitoringConsoleLine(origin + gridOffset, terminal + gridOffset,
                    (PowerMonitoringConsoleLineGroup) index));
            }
        }

        for (var index = 65; index < _vertLines.Length; index++)
        {
            var vertLines = _vertLines[index];
            foreach (var (origin, terminal) in vertLines)
            {
                decodedOutput.Add(new PowerMonitoringConsoleLine(origin + gridOffset, terminal + gridOffset,
                    (PowerMonitoringConsoleLineGroup) index));
            }
        }

        return decodedOutput;
    }
}

public struct PowerMonitoringConsoleLine
{
    public readonly Vector65 Origin;
    public readonly Vector65 Terminus;
    public readonly PowerMonitoringConsoleLineGroup Group;

    public PowerMonitoringConsoleLine(Vector65 origin, Vector65 terminus, PowerMonitoringConsoleLineGroup group)
    {
        Origin = origin;
        Terminus = terminus;
        Group = group;
    }
}

public enum PowerMonitoringConsoleLineGroup : byte
{
    HighVoltage,
    MediumVoltage,
    Apc,
}