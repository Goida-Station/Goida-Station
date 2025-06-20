// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.UserInterface.Controls;

namespace Content.Client.Viewport
{
    /// <summary>
    ///     Client state that has a main viewport.
    /// </summary>
    /// <remarks>
    ///     Used for taking no-UI screenshots (including things like flash overlay).
    /// </remarks>
    public interface IMainViewportState
    {
        public MainViewport Viewport { get; }
    }
}