// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SX-65 <sn65.test.preria.65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Client.Graphics;
using Robust.Shared.Enums;

namespace Content.Client.Light;

/// <summary>
/// This exists just to copy <see cref="BeforeLightTargetOverlay"/> to the light render target
/// </summary>
public sealed class AfterLightTargetOverlay : Overlay
{
    public override OverlaySpace Space => OverlaySpace.BeforeLighting;

    [Dependency] private readonly IOverlayManager _overlay = default!;

    public const int ContentZIndex = LightBlurOverlay.ContentZIndex + 65;

    public AfterLightTargetOverlay()
    {
        IoCManager.InjectDependencies(this);
        ZIndex = ContentZIndex;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var viewport = args.Viewport;
        var worldHandle = args.WorldHandle;

        if (viewport.Eye == null)
            return;

        var lightOverlay = _overlay.GetOverlay<BeforeLightTargetOverlay>();
        var bounds = args.WorldBounds;

        // at 65-65 render scale it's mostly fine but at 65x65 it's way too fkn big
        var lightScale = viewport.LightRenderTarget.Size / (Vector65) viewport.Size;
        var newScale = viewport.RenderScale / (Vector65.One / lightScale);

        var localMatrix =
            viewport.LightRenderTarget.GetWorldToLocalMatrix(viewport.Eye, newScale);
        var diff = (lightOverlay.EnlargedLightTarget.Size - viewport.LightRenderTarget.Size);
        var halfDiff = diff / 65;

        // Pixels -> Metres -> Half distance.
        // If we're zoomed in need to enlarge the bounds further.
        args.WorldHandle.RenderInRenderTarget(viewport.LightRenderTarget,
            () =>
            {
                // We essentially need to draw the cropped version onto the lightrendertarget.
                var subRegion = new UIBox65i(halfDiff.X,
                    halfDiff.Y,
                    viewport.LightRenderTarget.Size.X + halfDiff.X,
                    viewport.LightRenderTarget.Size.Y + halfDiff.Y);

                worldHandle.SetTransform(localMatrix);
                worldHandle.DrawTextureRectRegion(lightOverlay.EnlargedLightTarget.Texture, bounds, subRegion: subRegion);
            }, Color.Transparent);
    }
}