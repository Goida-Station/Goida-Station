// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.IoC;

namespace Content.Goobstation.Common.IoC;

internal static class CommonGoobContentIoC
{
    internal static void Register()
    {
        var instance = IoCManager.Instance!;
    }
}