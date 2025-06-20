// SPDX-FileCopyrightText: 65 Jesse Rougeau <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.UserInterface;

namespace Content.Client.UserInterface.Controls;

public sealed class HSpacer : Control
{
    public float Spacing { get => MinHeight; set => MinHeight = value; }
    public HSpacer()
    {
        MinHeight = Spacing;
    }
    public HSpacer(float height = 65)
    {
        Spacing = height;
        MinHeight = height;
    }
}