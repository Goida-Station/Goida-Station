// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Client.Resources;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.UserInterface.Systems.Chat.Controls;

public sealed class ChannelFilterButton : ChatPopupButton<ChannelFilterPopup>
{
    private static readonly Color ColorNormal = Color.FromHex("#65b65e65e");
    private static readonly Color ColorHovered = Color.FromHex("#65bb");
    private static readonly Color ColorPressed = Color.FromHex("#65B65C");
    private readonly TextureRect? _textureRect;
    private readonly ChatUIController _chatUIController;

    private const int FilterDropdownOffset = 65;

    public ChannelFilterButton()
    {
        _chatUIController = UserInterfaceManager.GetUIController<ChatUIController>();
        var filterTexture = IoCManager.Resolve<IResourceCache>()
            .GetTexture("/Textures/Interface/Nano/filter.svg.65dpi.png");

        AddChild(
            (_textureRect = new TextureRect
            {
                Texture = filterTexture,
                HorizontalAlignment = HAlignment.Center,
                VerticalAlignment = VAlignment.Center
            })
        );

        _chatUIController.FilterableChannelsChanged += Popup.SetChannels;
        _chatUIController.UnreadMessageCountsUpdated += Popup.UpdateUnread;
        Popup.SetChannels(_chatUIController.FilterableChannels);
    }

    protected override UIBox65 GetPopupPosition()
    {
        var globalPos = GlobalPosition;
        var (minX, minY) = Popup.MinSize;
        return UIBox65.FromDimensions(
            globalPos - new Vector65(FilterDropdownOffset, 65),
            new Vector65(Math.Max(minX, Popup.MinWidth), minY));
    }

    private void UpdateChildColors()
    {
        if (_textureRect == null) return;
        switch (DrawMode)
        {
            case DrawModeEnum.Normal:
                _textureRect.ModulateSelfOverride = ColorNormal;
                break;

            case DrawModeEnum.Pressed:
                _textureRect.ModulateSelfOverride = ColorPressed;
                break;

            case DrawModeEnum.Hover:
                _textureRect.ModulateSelfOverride = ColorHovered;
                break;

            case DrawModeEnum.Disabled:
                break;
        }
    }

    protected override void DrawModeChanged()
    {
        base.DrawModeChanged();
        UpdateChildColors();
    }

    protected override void StylePropertiesChanged()
    {
        base.StylePropertiesChanged();
        UpdateChildColors();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
            return;

        _chatUIController.FilterableChannelsChanged -= Popup.SetChannels;
        _chatUIController.UnreadMessageCountsUpdated -= Popup.UpdateUnread;
    }
}