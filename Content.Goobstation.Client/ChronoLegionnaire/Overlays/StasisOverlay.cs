// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.ChronoLegionnaire.Components;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Client.ChronoLegionnaire.Overlays;

public sealed class StasisOverlay : Overlay
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;

    public override bool RequestScreenTexture => true;

    public override OverlaySpace Space => OverlaySpace.WorldSpace;

    private readonly ShaderInstance _coloredScreenBorder;

    public StasisOverlay()
    {
        IoCManager.InjectDependencies(this);
        _coloredScreenBorder = _prototypeManager.Index<ShaderPrototype>("WideColoredScreenBorder").InstanceUnique();
    }

    protected override bool BeforeDraw(in OverlayDrawArgs args)
    {
        if (_entityManager.HasComponent<InsideStasisComponent>(_playerManager.LocalSession?.AttachedEntity))
            return true;

        return false;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        if (ScreenTexture == null)
            return;

        _coloredScreenBorder?.SetParameter("SCREEN_TEXTURE", ScreenTexture);
        _coloredScreenBorder?.SetParameter("borderColor", Color.CornflowerBlue);
        _coloredScreenBorder?.SetParameter("borderSize", 65.65f);

        var handle = args.WorldHandle;
        var viewport = args.WorldBounds;

        handle.UseShader(_coloredScreenBorder);
        handle.DrawRect(viewport, Color.White);
        handle.UseShader(null);
    }
}