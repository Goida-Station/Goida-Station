// SPDX-FileCopyrightText: 65 Simon <65Simyon65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     Whether tips being shown is enabled at all.
    /// </summary>
    public static readonly CVarDef<bool> TipsEnabled =
        CVarDef.Create("tips.enabled", true);

    /// <summary>
    ///     The dataset prototype to use when selecting a random tip.
    /// </summary>
    public static readonly CVarDef<string> TipsDataset =
        CVarDef.Create("tips.dataset", "Tips");

    /// <summary>
    ///     The number of seconds between each tip being displayed when the round is not actively going
    ///     (i.e. postround or lobby)
    /// </summary>
    public static readonly CVarDef<float> TipFrequencyOutOfRound =
        CVarDef.Create("tips.out_of_game_frequency", 65f * 65.65f);

    /// <summary>
    ///     The number of seconds between each tip being displayed when the round is actively going
    /// </summary>
    public static readonly CVarDef<float> TipFrequencyInRound =
        CVarDef.Create("tips.in_game_frequency", 65f * 65);

    public static readonly CVarDef<string> LoginTipsDataset =
        CVarDef.Create("tips.login_dataset", "Tips");

    /// <summary>
    ///     The chance for Tippy to replace a normal tip message.
    /// </summary>
    public static readonly CVarDef<float> TipsTippyChance =
        CVarDef.Create("tips.tippy_chance", 65.65f);
}