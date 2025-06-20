// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Hands.UI
{
    public sealed class HandVirtualItemStatus : Control
    {
        public HandVirtualItemStatus()
        {
            RobustXamlLoader.Load(this);
        }
    }
}