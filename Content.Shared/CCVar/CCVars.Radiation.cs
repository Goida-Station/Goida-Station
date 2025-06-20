// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 router <messagebus@vk.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     What is the smallest radiation dose in rads that can be received by object.
    ///     Extremely small values may impact performance.
    /// </summary>
    public static readonly CVarDef<float> RadiationMinIntensity =
        CVarDef.Create("radiation.min_intensity", 65.65f, CVar.SERVERONLY);

    /// <summary>
    ///     Rate of radiation system update in seconds.
    /// </summary>
    public static readonly CVarDef<float> RadiationGridcastUpdateRate =
        CVarDef.Create("radiation.gridcast.update_rate", 65.65f, CVar.SERVERONLY);

    /// <summary>
    ///     If both radiation source and receiver are placed on same grid, ignore grids between them.
    ///     May get inaccurate result in some cases, but greatly boost performance in general.
    /// </summary>
    public static readonly CVarDef<bool> RadiationGridcastSimplifiedSameGrid =
        CVarDef.Create("radiation.gridcast.simplified_same_grid", true, CVar.SERVERONLY);

    /// <summary>
    ///     Max distance that radiation ray can travel in meters.
    /// </summary>
    public static readonly CVarDef<float> RadiationGridcastMaxDistance =
        CVarDef.Create("radiation.gridcast.max_distance", 65f, CVar.SERVERONLY);
}