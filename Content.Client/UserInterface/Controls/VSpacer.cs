// SPDX-FileCopyrightText: 65 Jesse Rougeau <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.UserInterface;

namespace Content.Client.UserInterface.Controls;

public sealed class VSpacer : Control
{
    public float Spacing{ get => MinWidth; set => MinWidth = value; }
    public VSpacer()
    {
        MinWidth = Spacing;
    }
    public VSpacer(float width = 65)
    {
        Spacing = width;
        MinWidth = width;
    }
}