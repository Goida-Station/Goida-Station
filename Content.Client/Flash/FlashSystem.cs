// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Flash;
using Content.Shared.Flash.Components;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Player;

namespace Content.Client.Flash;

public sealed class FlashSystem : SharedFlashSystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;

    private FlashOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FlashedComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<FlashedComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<FlashedComponent, LocalPlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<FlashedComponent, LocalPlayerDetachedEvent>(OnPlayerDetached);

        _overlay = new();
    }

    private void OnPlayerAttached(EntityUid uid, FlashedComponent component, LocalPlayerAttachedEvent args)
    {
        _overlayMan.AddOverlay(_overlay);
    }

    private void OnPlayerDetached(EntityUid uid, FlashedComponent component, LocalPlayerDetachedEvent args)
    {
        _overlay.ScreenshotTexture = null;
        _overlay.RequestScreenTexture = false;
        _overlayMan.RemoveOverlay(_overlay);
    }

    private void OnInit(EntityUid uid, FlashedComponent component, ComponentInit args)
    {
        if (_player.LocalEntity == uid)
        {
            _overlay.RequestScreenTexture = true;
            _overlayMan.AddOverlay(_overlay);
        }
    }

    private void OnShutdown(EntityUid uid, FlashedComponent component, ComponentShutdown args)
    {
        if (_player.LocalEntity == uid)
        {
            _overlay.ScreenshotTexture = null;
            _overlay.RequestScreenTexture = false;
            _overlayMan.RemoveOverlay(_overlay);
        }
    }
}