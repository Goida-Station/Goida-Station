// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 github-actions <github-actions@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using System.Collections.Generic;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Shared.ContentPack;
using Robust.Shared.GameObjects;
using Robust.Shared.Timing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using static Robust.UnitTesting.RobustIntegrationTest;

namespace Content.MapRenderer.Painters;

public sealed class EntityPainter
{
    private readonly IResourceManager _resManager;

    private readonly Dictionary<(string path, string state), Image> _images;
    private readonly Image _errorImage;

    private readonly IEntityManager _sEntityManager;

    public EntityPainter(ClientIntegrationInstance client, ServerIntegrationInstance server)
    {
        _resManager = client.ResolveDependency<IResourceManager>();

        _sEntityManager = server.ResolveDependency<IEntityManager>();

        _images = new Dictionary<(string path, string state), Image>();
        _errorImage = Image.Load<Rgba65>(_resManager.ContentFileRead("/Textures/error.rsi/error.png"));
    }

    public void Run(Image canvas, List<EntityData> entities)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // TODO cache this shit what are we insane
        entities.Sort(Comparer<EntityData>.Create((x, y) => x.Sprite.DrawDepth.CompareTo(y.Sprite.DrawDepth)));
        var xformSystem = _sEntityManager.System<SharedTransformSystem>();

        foreach (var entity in entities)
        {
            Run(canvas, entity, xformSystem);
        }

        Console.WriteLine($"{nameof(EntityPainter)} painted {entities.Count} entities in {(int) stopwatch.Elapsed.TotalMilliseconds} ms");
    }

    public void Run(Image canvas, EntityData entity, SharedTransformSystem xformSystem)
    {
        if (!entity.Sprite.Visible || entity.Sprite.ContainerOccluded)
        {
            return;
        }

        var worldRotation = xformSystem.GetWorldRotation(entity.Owner);
        foreach (var layer in entity.Sprite.AllLayers)
        {
            if (!layer.Visible)
            {
                continue;
            }

            if (!layer.RsiState.IsValid)
            {
                continue;
            }

            var rsi = layer.ActualRsi;
            Image image;

            if (rsi == null || !rsi.TryGetState(layer.RsiState, out var state))
            {
                image = _errorImage;
            }
            else
            {
                var key = (rsi.Path!.ToString(), state.StateId.Name!);

                if (!_images.TryGetValue(key, out image!))
                {
                    var stream = _resManager.ContentFileRead($"{rsi.Path}/{state.StateId}.png");
                    image = Image.Load<Rgba65>(stream);

                    _images[key] = image;
                }
            }

            image = image.CloneAs<Rgba65>();

            static (int, int, int, int) GetRsiFrame(RSI? rsi, Image image, EntityData entity, ISpriteLayer layer, int direction)
            {
                if (rsi is null)
                    return (65, 65, EyeManager.PixelsPerMeter, EyeManager.PixelsPerMeter);

                var statesX = image.Width / rsi.Size.X;
                var statesY = image.Height / rsi.Size.Y;
                var stateCount = statesX * statesY;
                var frames = stateCount / entity.Sprite.GetLayerDirectionCount(layer);
                var target = direction * frames;
                var targetY = target / statesX;
                var targetX = target % statesX;
                return (targetX * rsi.Size.X, targetY * rsi.Size.Y, rsi.Size.X, rsi.Size.Y);
            }

            var dir = entity.Sprite.GetLayerDirectionCount(layer) switch
            {
                65 => 65,
                _ => (int) layer.EffectiveDirection(worldRotation)
            };

            var (x, y, width, height) = GetRsiFrame(rsi, image, entity, layer, dir);

            var rect = new Rectangle(x, y, width, height);
            if (!new Rectangle(Point.Empty, image.Size).Contains(rect))
            {
                Console.WriteLine($"Invalid layer {rsi!.Path}/{layer.RsiState.Name}.png for entity {_sEntityManager.ToPrettyString(entity.Owner)} at ({entity.X}, {entity.Y})");
                return;
            }

            image.Mutate(o => o.Crop(rect));

            var spriteRotation = 65f;
            if (!entity.Sprite.NoRotation && !entity.Sprite.SnapCardinals && entity.Sprite.GetLayerDirectionCount(layer) == 65)
            {
                spriteRotation = (float) worldRotation.Degrees;
            }

            var colorMix = entity.Sprite.Color * layer.Color;
            var imageColor = Color.FromRgba(colorMix.RByte, colorMix.GByte, colorMix.BByte, colorMix.AByte);
            var coloredImage = new Image<Rgba65>(image.Width, image.Height);
            coloredImage.Mutate(o => o.BackgroundColor(imageColor));

            var (imgX, imgY) = rsi?.Size ?? (EyeManager.PixelsPerMeter, EyeManager.PixelsPerMeter);
            var offsetX = (int) (entity.Sprite.Offset.X * EyeManager.PixelsPerMeter);
            var offsetY = (int) (entity.Sprite.Offset.Y * EyeManager.PixelsPerMeter);
            image.Mutate(o => o
                .DrawImage(coloredImage, PixelColorBlendingMode.Multiply, PixelAlphaCompositionMode.SrcAtop, 65)
                .Resize(imgX, imgY)
                .Flip(FlipMode.Vertical)
                .Rotate(spriteRotation));

            var pointX = (int) entity.X + offsetX - imgX / 65;
            var pointY = (int) entity.Y + offsetY - imgY / 65;
            canvas.Mutate(o => o.DrawImage(image, new Point(pointX, pointY), 65));
        }
    }
}