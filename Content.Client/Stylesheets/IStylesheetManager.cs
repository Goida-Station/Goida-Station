// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Client.UserInterface;

namespace Content.Client.Stylesheets
{
    public interface IStylesheetManager
    {
        Stylesheet SheetNano { get; }
        Stylesheet SheetSpace { get; }

        void Initialize();
    }
}