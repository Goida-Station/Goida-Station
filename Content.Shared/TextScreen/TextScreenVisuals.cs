// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 avery <65graevy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.TextScreen;

[Serializable, NetSerializable]
public enum TextScreenVisuals : byte
{
    // TODO: support for a small image, I think. Probably want to rename textscreen to just screen then.
    /// <summary>
    ///     What text to default to after timer completion?
    ///     Expects a <see cref="string"/>.
    /// </summary>
    DefaultText,
    /// <summary>
    ///     What text to render? <br/>
    ///     Expects a <see cref="string"/>.
    /// </summary>
    ScreenText,

    /// <summary>
    ///     What is the target time? <br/>
    ///     Expects a <see cref="TimeSpan"/>.
    /// </summary>
    TargetTime,

    /// <summary>
    ///     Change text color on the entire screen
    ///     Expects a <see cref="Color"/>.
    /// </summary>
    Color
}