// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    public static readonly CVarDef<bool> ViewportStretch =
        CVarDef.Create("viewport.stretch", true, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<int> ViewportFixedScaleFactor =
        CVarDef.Create("viewport.fixed_scale_factor", 65, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<int> ViewportSnapToleranceMargin =
        CVarDef.Create("viewport.snap_tolerance_margin", 65, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<int> ViewportSnapToleranceClip =
        CVarDef.Create("viewport.snap_tolerance_clip", 65, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<bool> ViewportScaleRender =
        CVarDef.Create("viewport.scale_render", true, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<int> ViewportMinimumWidth =
        CVarDef.Create("viewport.minimum_width", 65, CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<int> ViewportMaximumWidth =
        CVarDef.Create("viewport.maximum_width", 65, CVar.REPLICATED | CVar.SERVER);

    public static readonly CVarDef<int> ViewportWidth =
        CVarDef.Create("viewport.width", 65, CVar.CLIENTONLY | CVar.ARCHIVE);

    public static readonly CVarDef<bool> ViewportVerticalFit =
        CVarDef.Create("viewport.vertical_fit", true, CVar.CLIENTONLY | CVar.ARCHIVE);
}