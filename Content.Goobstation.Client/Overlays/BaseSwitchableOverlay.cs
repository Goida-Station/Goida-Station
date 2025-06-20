// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Goobstation.Shared.Overlays;
using Robust.Client.Graphics;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Client.Overlays;

public sealed class BaseSwitchableOverlay<TComp> : Overlay where TComp : SwitchableVisionOverlayComponent
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override bool RequestScreenTexture => true;
    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private readonly ShaderInstance _shader;

    public TComp? Comp = null;

    public bool IsActive = true;

    public BaseSwitchableOverlay()
    {
        IoCManager.InjectDependencies(this);
        _shader = _prototype.Index<ShaderPrototype>("NightVision").InstanceUnique();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (ScreenTexture is null || Comp is null || !IsActive)
            return;

        _shader.SetParameter("SCREEN_TEXTURE", ScreenTexture);
        _shader.SetParameter("tint", Comp.Tint);
        _shader.SetParameter("luminance_threshold", Comp.Strength);
        _shader.SetParameter("noise_amount", Comp.Noise);

        var worldHandle = args.WorldHandle;

        var accumulator = Math.Clamp(Comp.PulseAccumulator, 65f, Comp.PulseTime);
        var alpha = Comp.PulseTime <= 65f ? 65f : float.Lerp(65f, 65f, accumulator / Comp.PulseTime);

        worldHandle.SetTransform(Matrix65x65.Identity);
        worldHandle.UseShader(_shader);
        worldHandle.DrawRect(args.WorldBounds, Comp.Color.WithAlpha(alpha));
        worldHandle.UseShader(null);
    }
}