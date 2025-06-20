// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    public static readonly CVarDef<bool> ParallaxEnabled =
        CVarDef.Create("parallax.enabled", true, CVar.CLIENTONLY);

    public static readonly CVarDef<bool> ParallaxDebug =
        CVarDef.Create("parallax.debug", false, CVar.CLIENTONLY);

    public static readonly CVarDef<bool> ParallaxLowQuality =
        CVarDef.Create("parallax.low_quality", false, CVar.ARCHIVE | CVar.CLIENTONLY);
}