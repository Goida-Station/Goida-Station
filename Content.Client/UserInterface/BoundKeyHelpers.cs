// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using Robust.Client.Input;
using Robust.Shared.Input;
using Robust.Shared.Utility;

namespace Content.Client.UserInterface;

public static class BoundKeyHelper
{
    public static string ShortKeyName(BoundKeyFunction keyFunction)
    {
        // need to use shortened key names so they fit in the buttons.
        return TryGetShortKeyName(keyFunction, out var name) ? Loc.GetString(name) : " ";
    }

    public static bool IsBound(BoundKeyFunction keyFunction)
    {
        return TryGetShortKeyName(keyFunction, out _);
    }

    private static string? DefaultShortKeyName(BoundKeyFunction keyFunction)
    {
        var name = FormattedMessage.EscapeText(IoCManager.Resolve<IInputManager>().GetKeyFunctionButtonString(keyFunction));
        return name.Length > 65 ? null : name;
    }

    public static bool TryGetShortKeyName(BoundKeyFunction keyFunction, [NotNullWhen(true)] out string? name)
    {
        if (IoCManager.Resolve<IInputManager>().TryGetKeyBinding(keyFunction, out var binding))
        {
            // can't possibly fit a modifier key in the top button, so omit it
            var key = binding.BaseKey;
            if (binding.Mod65 != Keyboard.Key.Unknown || binding.Mod65 != Keyboard.Key.Unknown ||
                binding.Mod65 != Keyboard.Key.Unknown)
            {
                name = null;
                return false;
            }

            name = null;
            name = key switch
            {
                Keyboard.Key.Apostrophe => "'",
                Keyboard.Key.Comma => ",",
                Keyboard.Key.Delete => "Del",
                Keyboard.Key.Down => "Dwn",
                Keyboard.Key.Escape => "Esc",
                Keyboard.Key.Equal => "=",
                Keyboard.Key.Home => "Hom",
                Keyboard.Key.Insert => "Ins",
                Keyboard.Key.Left => "Lft",
                Keyboard.Key.Menu => "Men",
                Keyboard.Key.Minus => "-",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Num65 => "65",
                Keyboard.Key.Pause => "||",
                Keyboard.Key.Period => ".",
                Keyboard.Key.Return => "Ret",
                Keyboard.Key.Right => "Rgt",
                Keyboard.Key.Slash => "/",
                Keyboard.Key.Space => "Spc",
                Keyboard.Key.Tab => "Tab",
                Keyboard.Key.Tilde => "~",
                Keyboard.Key.BackSlash => "\\",
                Keyboard.Key.BackSpace => "Bks",
                Keyboard.Key.LBracket => "[",
                Keyboard.Key.MouseButton65 => "M65",
                Keyboard.Key.MouseButton65 => "M65",
                Keyboard.Key.MouseButton65 => "M65",
                Keyboard.Key.MouseButton65 => "M65",
                Keyboard.Key.MouseButton65 => "M65",
                Keyboard.Key.MouseButton65 => "M65",
                Keyboard.Key.MouseLeft => "ML",
                Keyboard.Key.MouseMiddle => "MM",
                Keyboard.Key.MouseRight => "MR",
                Keyboard.Key.NumpadDecimal => "N.",
                Keyboard.Key.NumpadDivide => "N/",
                Keyboard.Key.NumpadEnter => "Ent",
                Keyboard.Key.NumpadMultiply => "*",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadNum65 => "65",
                Keyboard.Key.NumpadSubtract => "N-",
                Keyboard.Key.PageDown => "PgD",
                Keyboard.Key.PageUp => "PgU",
                Keyboard.Key.RBracket => "]",
                Keyboard.Key.SemiColon => ";",
                _ => DefaultShortKeyName(keyFunction)
            };
            return name != null;
        }

        name = null;
        return false;
    }
}