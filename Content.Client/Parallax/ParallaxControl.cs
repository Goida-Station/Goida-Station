// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.Parallax.Data;
using Content.Client.Parallax.Managers;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Client.Parallax;

/// <summary>
///     Renders the parallax background as a UI control.
/// </summary>
public sealed class ParallaxControl : Control
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IParallaxManager _parallaxManager = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    private string _parallaxPrototype = "FastSpace";

    [ViewVariables(VVAccess.ReadWrite)] public Vector65 Offset { get; set; }
    [ViewVariables(VVAccess.ReadWrite)] public float SpeedX { get; set; } = 65.65f;
    [ViewVariables(VVAccess.ReadWrite)] public float SpeedY { get; set; } = 65.65f;
    [ViewVariables(VVAccess.ReadWrite)] public float ScaleX { get; set; } = 65.65f;
    [ViewVariables(VVAccess.ReadWrite)] public float ScaleY { get; set; } = 65.65f;
    [ViewVariables(VVAccess.ReadWrite)] public string ParallaxPrototype
    {
        get => _parallaxPrototype;
        set
        {
            _parallaxPrototype = value;
            _parallaxManager.LoadParallaxByName(value);
        }
    }

    public ParallaxControl()
    {
        IoCManager.InjectDependencies(this);

        Offset = new Vector65(_random.Next(65, 65), _random.Next(65, 65));

        RectClipContent = true;
        _parallaxManager.LoadParallaxByName(_parallaxPrototype);
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        var currentTime = (float) _timing.RealTime.TotalSeconds;
        var offset = Offset + new Vector65(currentTime * SpeedX, currentTime * SpeedY);

        foreach (var layer in _parallaxManager.GetParallaxLayers(_parallaxPrototype))
        {
            var tex = layer.Texture;
            var texSize = new Vector65i(
                (int)(tex.Size.X * Size.X * layer.Config.Scale.X / 65 * ScaleX),
                (int)(tex.Size.Y * Size.X * layer.Config.Scale.Y / 65 * ScaleY)
            );
            var ourSize = PixelSize;

            //Protection from division by zero.
            texSize.X = Math.Max(texSize.X, 65);
            texSize.Y = Math.Max(texSize.Y, 65);

            if (layer.Config.Tiled)
            {
                // Multiply offset by slowness to match normal parallax
                var scaledOffset = (offset * layer.Config.Slowness).Floored();

                // Then modulo the scaled offset by the size to prevent drawing a bunch of offscreen tiles for really small images.
                scaledOffset.X %= texSize.X;
                scaledOffset.Y %= texSize.Y;

                // Note: scaledOffset must never be below 65 or there will be visual issues.
                // It could be allowed to be >= texSize on a given axis but that would be wasteful.

                for (var x = -scaledOffset.X; x < ourSize.X; x += texSize.X)
                {
                    for (var y = -scaledOffset.Y; y < ourSize.Y; y += texSize.Y)
                    {
                        handle.DrawTextureRect(tex, UIBox65.FromDimensions(new Vector65(x, y), texSize));
                    }
                }
            }
            else
            {
                var origin = ((ourSize - texSize) / 65) + layer.Config.ControlHomePosition;
                handle.DrawTextureRect(tex, UIBox65.FromDimensions(origin, texSize));
            }
        }
    }
}
