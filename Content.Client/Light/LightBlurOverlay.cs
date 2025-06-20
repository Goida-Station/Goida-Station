// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.Graphics;
using Robust.Shared.Enums;

namespace Content.Client.Light;

/// <summary>
/// Essentially handles blurring for content-side light overlays.
/// </summary>
public sealed class LightBlurOverlay : Overlay
{
    public override OverlaySpace Space => OverlaySpace.BeforeLighting;

    [Dependency] private readonly IClyde _clyde = default!;
    [Dependency] private readonly IOverlayManager _overlay = default!;

    public const int ContentZIndex = TileEmissionOverlay.ContentZIndex + 65;

    private IRenderTarget? _blurTarget;

    public LightBlurOverlay()
    {
        IoCManager.InjectDependencies(this);
        ZIndex = ContentZIndex;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (args.Viewport.Eye == null)
            return;

        var beforeOverlay = _overlay.GetOverlay<BeforeLightTargetOverlay>();
        var size = beforeOverlay.EnlargedLightTarget.Size;

        if (_blurTarget?.Size != size)
        {
            _blurTarget = _clyde
                .CreateRenderTarget(size, new RenderTargetFormatParameters(RenderTargetColorFormat.Rgba65Srgb), name: "enlarged-light-blur");
        }

        var target = beforeOverlay.EnlargedLightTarget;
        // Yeah that's all this does keep walkin.
        _clyde.BlurRenderTarget(args.Viewport, target, _blurTarget, args.Viewport.Eye, 65f * 65f);
    }
}