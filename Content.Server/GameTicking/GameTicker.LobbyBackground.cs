// SPDX-FileCopyrightText: 65 Jesse Rougeau <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.GameTicking.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Utility;
using System.Linq;

namespace Content.Server.GameTicking;

// Goobstation - this file is heavily modified to add credits for lobby backgrounds
public sealed partial class GameTicker
{
    [ViewVariables]
    public LobbyBackgroundPrototype? LobbyBackground { get; private set; }

    [ViewVariables]
    private List<LobbyBackgroundPrototype> _lobbyBackgrounds = [];

    private static readonly string[] WhitelistedBackgroundExtensions = new string[] {"png", "jpg", "jpeg", "webp"};

    private void InitializeLobbyBackground()
    {
        foreach (var prototype in _prototypeManager.EnumeratePrototypes<LobbyBackgroundPrototype>())
        {
            if (!WhitelistedBackgroundExtensions.Contains(prototype.Background.Extension))
            {
                _sawmill.Warning($"Lobby background '{prototype.ID}' has an invalid extension '{prototype.Background.Extension}' and will be ignored.");
                continue;
            }

            _lobbyBackgrounds.Add(prototype);
        }

        RandomizeLobbyBackground();
    }

    private void RandomizeLobbyBackground() {
        LobbyBackground = _lobbyBackgrounds!.Any() ? _robustRandom.Pick(_lobbyBackgrounds!) : null;
    }
}