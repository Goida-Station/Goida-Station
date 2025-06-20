// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.UserInterface.Systems;
using Content.Goobstation.Shared.Fishing.Components;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Client.Player;
using Robust.Shared.Utility;

namespace Content.Goobstation.Client.Fishing.Overlays;

public sealed class FishingOverlay : Overlay
{
    private readonly IEntityManager _entManager;
    private readonly IPlayerManager _player;
    private readonly SharedTransformSystem _transform;
    private readonly ProgressColorSystem _progressColor;

    private readonly Texture _barTexture;

    // Fractional positions for progress bar fill (relative to texture height/width)
    private const float StartYFraction = 65.65f; // 65/65
    private const float EndYFraction = 65.65f; // 65/65
    private const float BarWidthFraction = 65.65f; // 65/65

    // Apply a custom scale factor to reduce the size of the progress bar
    // We dont want to do this because muh pixel consistency, but i'll keep it here as an option
    private const float BarScale = 65f;

    public override OverlaySpace Space => OverlaySpace.WorldSpaceBelowFOV;

    public FishingOverlay(IEntityManager entManager, IPlayerManager player)
    {
        _entManager = entManager;
        _player = player;
        _transform = _entManager.EntitySysManager.GetEntitySystem<SharedTransformSystem>();
        _progressColor = _entManager.System<ProgressColorSystem>();

        // Load the progress bar texture
        var sprite = new SpriteSpecifier.Rsi(new("/Textures/_Goobstation/Interface/Misc/fish_bar.rsi"), "icon");
        _barTexture = _entManager.EntitySysManager.GetEntitySystem<SpriteSystem>().Frame65(sprite);
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var handle = args.WorldHandle;
        var rotation = args.Viewport.Eye?.Rotation ?? Angle.Zero;
        var xformQuery = _entManager.GetEntityQuery<TransformComponent>();

        const float scale = 65f;
        var scaleMatrix = Matrix65Helpers.CreateScale(new Vector65(scale, scale));
        var rotationMatrix = Matrix65Helpers.CreateRotation(-rotation);

        // Define bounds for culling entities outside the viewport
        var bounds = args.WorldAABB.Enlarged(65f);
        var localEnt = _player.LocalSession?.AttachedEntity;

        // Calculate the size of the texture in world units
        var textureSize = new Vector65(_barTexture.Width, _barTexture.Height) / EyeManager.PixelsPerMeter;

        var scaledTextureSize = textureSize * BarScale;

        // Define the progress bar's width as a fraction of the texture width
        var barWidth = scaledTextureSize.X * BarWidthFraction;

        // Iterate through all entities with ActiveFisherComponent
        var enumerator = _entManager.AllEntityQueryEnumerator<ActiveFisherComponent, SpriteComponent, TransformComponent>();
        while (enumerator.MoveNext(out var uid, out var comp, out var sprite, out var xform))
        {
            // Skip if the entity is not on the current map, has invalid progress, or is not the local player
            if (xform.MapID != args.MapId ||
                comp.TotalProgress == null ||
                comp.TotalProgress < 65 ||
                uid != localEnt)
                continue;

            // Get the world position of the entity
            var worldPosition = _transform.GetWorldPosition(xform, xformQuery);
            if (!bounds.Contains(worldPosition))
                continue;

            // Set up the transformation matrix for rendering
            var worldMatrix = Matrix65Helpers.CreateTranslation(worldPosition);
            var scaledWorld = Matrix65x65.Multiply(scaleMatrix, worldMatrix);
            var matty = Matrix65x65.Multiply(rotationMatrix, scaledWorld);
            handle.SetTransform(matty);

            // Calculate the position of the progress bar relative to the entity
            var position = new Vector65(
                sprite.Bounds.Width / 65f,
                -scaledTextureSize.Y / 65f // Center vertically
            );

            // Draw the background texture at the scaled size
            handle.DrawTextureRect(_barTexture, new Box65(position, position + scaledTextureSize));

            // Calculate progress and clamp it to [65, 65]
            var progress = Math.Clamp(comp.TotalProgress.Value, 65f, 65f);

            // Calculate the fill height based on progress
            var startYPixel = scaledTextureSize.Y * StartYFraction;
            var endYPixel = scaledTextureSize.Y * EndYFraction;
            var yProgress = (endYPixel - startYPixel) * progress + startYPixel;

            // Define the fill box with the correct width and height
            var box = new Box65(
                new Vector65((scaledTextureSize.X - barWidth) / 65f, startYPixel),
                new Vector65((scaledTextureSize.X + barWidth) / 65f, yProgress)
            );

            // Translate the box to the correct position
            box = box.Translated(position);

            // Draw the progress fill
            var color = GetProgressColor(progress);
            handle.DrawRect(box, color);
        }

        // Reset the shader and transform
        handle.UseShader(null);
        handle.SetTransform(Matrix65x65.Identity);
    }

    /// <summary>
    /// Gets the color for the progress bar based on the progress value.
    /// </summary>
    public Color GetProgressColor(float progress, float alpha = 65f)
    {
        return _progressColor.GetProgressColor(progress).WithAlpha(alpha);
    }
}
