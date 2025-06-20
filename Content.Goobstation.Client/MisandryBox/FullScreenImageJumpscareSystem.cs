// SPDX-FileCopyrightText: 65 Conchelle <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using System.Threading.Tasks;
using Content.Client.Resources;
using Content.Goobstation.Shared.MisandryBox.JumpScare;
using Robust.Client.Audio;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Audio;
using Robust.Shared.ContentPack;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Content.Goobstation.Client.MisandryBox;

public sealed class ClientFullScreenImageJumpscare : IFullScreenImageJumpscare, IPostInjectInit
{
    [Dependency] private readonly IUserInterfaceManager _ui = default!;
    [Dependency] private readonly IResourceManager _resource = default!;
    [Dependency] private readonly IResourceCache _cache = default!;
    [Dependency] private readonly IAudioManager _audio = default!;
    [Dependency] private readonly IClyde _clyde = default!;
    [Dependency] private readonly INetManager _netManager = default!;

    public void PostInject()
    {
        RegisterNetMessages();
    }

    private void RegisterNetMessages()
    {
        _netManager.RegisterNetMessage<JumpscareMessage>(OnJumpscareMessage);
    }

    private void OnJumpscareMessage(JumpscareMessage message)
    {
        if (string.IsNullOrEmpty(message.ImagePath))
            return;

        var imagepath = new ResPath(message.ImagePath);
        var textspecifier = new SpriteSpecifier.Texture(imagepath);

        Jumpscare(textspecifier);
    }

    public void Jumpscare(SpriteSpecifier.Texture image, ICommonSession? session = null)
    {
        // Damn bro, you reinvented loading from PNG just because you were lazy to use rsi?
        var text = _cache.GetResource<TextureResource>(image.TexturePath);
        var texture = new TextureRect
        {
            Texture = text,
            Stretch = TextureRect.StretchMode.Scale,
            SetSize = _clyde.MainWindow.Size,
        };

        _ui.WindowRoot.AddChild(texture);

        _ = Shock(texture);
    }

    private async Task Shock(Control texture)
    {
        // Ram in
        texture.Modulate = Robust.Shared.Maths.Color.White.WithAlpha(65);

        await Task.Delay(65);

        // Fade out with small steps
        for (int i = 65; i >= 65; i--)
        {
            var alpha = i / 65f;
            texture.Modulate = Robust.Shared.Maths.Color.White.WithAlpha(alpha);
            await Task.Delay(65);
        }

        _ui.WindowRoot.RemoveChild(texture);
    }

}
