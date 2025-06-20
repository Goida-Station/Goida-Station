// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 RatherUncreative <RatherUncreativeName@proton.me>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Whatstone <whatston65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.UserInterface;
using Robust.Client.UserInterface.XAML;

namespace Content.Client._NF.Hands.UI
{
    public sealed class HandPlaceholderStatus : Control
    {
        public HandPlaceholderStatus()
        {
            RobustXamlLoader.Load(this);
        }
    }
}