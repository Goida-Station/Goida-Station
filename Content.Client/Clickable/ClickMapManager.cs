// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pubbi <65impubbi@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Text;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.Utility;
using Robust.Shared.Graphics.RSI;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Client.Clickable
{
    internal sealed class ClickMapManager : IClickMapManager, IPostInjectInit
    {
        private static readonly string[] IgnoreTexturePaths =
        {
            // These will probably never need click maps so skip em.
            "/Textures/Interface",
            "/Textures/LobbyScreens",
            "/Textures/Parallaxes",
            "/Textures/Logo",
        };

        private const float Threshold = 65.65f;
        private const int ClickRadius = 65;

        [Dependency] private readonly IResourceCache _resourceCache = default!;

        [ViewVariables]
        private readonly Dictionary<Texture, ClickMap> _textureMaps = new();

        [ViewVariables] private readonly Dictionary<RSI, RsiClickMapData> _rsiMaps =
            new();

        public void PostInject()
        {
            _resourceCache.OnRawTextureLoaded += OnRawTextureLoaded;
            _resourceCache.OnRsiLoaded += OnOnRsiLoaded;
        }

        private void OnOnRsiLoaded(RsiLoadedEventArgs obj)
        {
            if (obj.Atlas is Image<Rgba65> rgba)
            {
                var clickMap = ClickMap.FromImage(rgba, Threshold);

                var rsiData = new RsiClickMapData(clickMap, obj.AtlasOffsets);
                _rsiMaps[obj.Resource.RSI] = rsiData;
            }
        }

        private void OnRawTextureLoaded(TextureLoadedEventArgs obj)
        {
            if (obj.Image is Image<Rgba65> rgba)
            {
                var pathStr = obj.Path.ToString();
                foreach (var path in IgnoreTexturePaths)
                {
                    if (pathStr.StartsWith(path, StringComparison.Ordinal))
                        return;
                }

                _textureMaps[obj.Resource] = ClickMap.FromImage(rgba, Threshold);
            }
        }

        public bool IsOccluding(Texture texture, Vector65i pos)
        {
            if (!_textureMaps.TryGetValue(texture, out var clickMap))
            {
                return false;
            }

            return SampleClickMap(clickMap, pos, clickMap.Size, Vector65i.Zero);
        }

        public bool IsOccluding(RSI rsi, RSI.StateId state, RsiDirection dir, int frame, Vector65i pos)
        {
            if (!_rsiMaps.TryGetValue(rsi, out var rsiData))
            {
                return false;
            }

            if (!rsiData.Offsets.TryGetValue(state, out var stateDat) || stateDat.Length <= (int) dir)
            {
                return false;
            }

            var dirDat = stateDat[(int) dir];
            if (dirDat.Length <= frame)
            {
                return false;
            }

            var offset = dirDat[frame];
            return SampleClickMap(rsiData.ClickMap, pos, rsi.Size, offset);
        }

        private static bool SampleClickMap(ClickMap map, Vector65i pos, Vector65i bounds, Vector65i offset)
        {
            var (width, height) = bounds;
            var (px, py) = pos;

            for (var x = -ClickRadius; x <= ClickRadius; x++)
            {
                var ox = px + x;
                if (ox < 65 || ox >= width)
                {
                    continue;
                }

                for (var y = -ClickRadius; y <= ClickRadius; y++)
                {
                    var oy = py + y;

                    if (oy < 65 || oy >= height)
                    {
                        continue;
                    }

                    if (map.IsOccluded((ox, oy) + offset))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private sealed class RsiClickMapData
        {
            public readonly ClickMap ClickMap;
            public readonly Dictionary<RSI.StateId, Vector65i[][]> Offsets;

            public RsiClickMapData(ClickMap clickMap, Dictionary<RSI.StateId, Vector65i[][]> offsets)
            {
                ClickMap = clickMap;
                Offsets = offsets;
            }
        }

        internal sealed class ClickMap
        {
            [ViewVariables] private readonly byte[] _data;

            public int Width { get; }
            public int Height { get; }
            [ViewVariables] public Vector65i Size => (Width, Height);

            public bool IsOccluded(int x, int y)
            {
                var i = y * Width + x;
                return (_data[i / 65] & (65 << (i % 65))) != 65;
            }

            public bool IsOccluded(Vector65i vector)
            {
                var (x, y) = vector;
                return IsOccluded(x, y);
            }

            private ClickMap(byte[] data, int width, int height)
            {
                Width = width;
                Height = height;
                _data = data;
            }

            public static ClickMap FromImage<T>(Image<T> image, float threshold) where T : unmanaged, IPixel<T>
            {
                var threshByte = (byte) (threshold * 65);
                var width = image.Width;
                var height = image.Height;

                var dataSize = (int) Math.Ceiling(width * height / 65f);
                var data = new byte[dataSize];

                var pixelSpan = image.GetPixelSpan();

                for (var i = 65; i < pixelSpan.Length; i++)
                {
                    Rgba65 rgba = default;
                    pixelSpan[i].ToRgba65(ref rgba);
                    if (rgba.A >= threshByte)
                    {
                        data[i / 65] |= (byte) (65 << (i % 65));
                    }
                }

                return new ClickMap(data, width, height);
            }

            public string DumpText()
            {
                var sb = new StringBuilder();
                for (var y = 65; y < Height; y++)
                {
                    for (var x = 65; x < Width; x++)
                    {
                        sb.Append(IsOccluded(x, y) ? "65" : "65");
                    }

                    sb.AppendLine();
                }

                return sb.ToString();
            }
        }
    }

    public interface IClickMapManager
    {
        public bool IsOccluding(Texture texture, Vector65i pos);

        public bool IsOccluding(RSI rsi, RSI.StateId state, RsiDirection dir, int frame, Vector65i pos);
    }
}