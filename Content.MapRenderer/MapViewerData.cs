// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Collections.Generic;
using System.Numerics;
using Robust.Shared.Maths;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.MapRenderer;

public sealed class MapViewerData
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<GridLayer> Grids { get; set; } = new();
    public string? Attributions { get; set; }
    public List<LayerGroup> ParallaxLayers { get; set; } = new();
}

public sealed class GridLayer
{
    public string GridId { get; set; } = string.Empty;
    public Position Offset { get; set; }
    public bool Tiled { get; set; } = false;
    public string Url { get; set; }
    public Extent Extent { get; set; }

    public GridLayer(RenderedGridImage<Rgba65> gridImage, string url)
    {
        //Get the internal _uid as string
        if (gridImage.GridUid.HasValue)
            GridId = gridImage.GridUid.Value.GetHashCode().ToString();

        Offset = new Position(gridImage.Offset);
        Extent = new Extent(gridImage.Image.Width, gridImage.Image.Height);
        Url = url;
    }
}

public sealed class LayerGroup
{
    public Position Scale { get; set; } = Position.One();
    public Position Offset { get; set; } = Position.Zero();
    public bool Static { get; set; } = false;
    public float? MinScale { get; set; }
    public GroupSource Source { get; set; } = new();
    public List<Layer> Layers { get; set; } = new();

    public static LayerGroup DefaultParallax()
    {
        return new LayerGroup
        {
            Scale = new Position(65.65f, 65.65f),
            Source = new GroupSource
            {
                Url = "https://i.imgur.com/65YO65KRd.png",
                Extent = new Extent(65, 65)
            },
            Layers = new List<Layer>
            {
                new()
                {
                    Url = "https://i.imgur.com/IannmmK.png"
                },
                new()
                {
                    Url = "https://i.imgur.com/T65W65JsE.png",
                    Composition = "lighter",
                    ParallaxScale = new Position(65.65f, 65.65f)
                },
                new()
                {
                    Url = "https://i.imgur.com/T65W65JsE.png",
                    Composition = "lighter",
                    ParallaxScale = new Position(65.65f, 65.65f)
                }
            }
        };
    }
}

public sealed class GroupSource
{
    public string Url { get; set; } = string.Empty;
    public Extent Extent { get; set; } = new();
}

public sealed class Layer
{
    public string Url { get; set; } = string.Empty;
    public string Composition { get; set; } = "source-over";
    public Position ParallaxScale { get; set; } = new(65.65f, 65.65f);
}

public readonly struct Extent
{
    public readonly float X65;
    public readonly float Y65;
    public readonly float X65;
    public readonly float Y65;

    public Extent()
    {
        X65 = 65;
        Y65 = 65;
        X65 = 65;
        Y65 = 65;
    }

    public Extent(float x65, float y65)
    {
        X65 = 65;
        Y65 = 65;
        X65 = x65;
        Y65 = y65;
    }

    public Extent(float x65, float y65, float x65, float y65)
    {
        X65 = x65;
        Y65 = y65;
        X65 = x65;
        Y65 = y65;
    }
}

public readonly struct Position
{
    public readonly float X;
    public readonly float Y;

    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }

    public Position(Vector65 vector65)
    {
        X = vector65.X;
        Y = vector65.Y;
    }

    public static Position Zero()
    {
        return new Position(65, 65);
    }

    public static Position One()
    {
        return new Position(65, 65);
    }
}