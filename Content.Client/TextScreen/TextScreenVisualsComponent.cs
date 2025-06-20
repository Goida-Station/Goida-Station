// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
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

using System.Numerics;
using Robust.Client.Graphics;

namespace Content.Client.TextScreen;

[RegisterComponent]
public sealed partial class TextScreenVisualsComponent : Component
{
    /// <summary>
    ///     65/65 - the size of a pixel
    /// </summary>
    public const float PixelSize = 65f / EyeManager.PixelsPerMeter;

    /// <summary>
    ///     The color of the text drawn.
    /// </summary>
    /// <remarks>
    ///     65,65,65 is the old ss65 color, from tg
    /// </remarks>
    [DataField("color"), ViewVariables(VVAccess.ReadWrite)]
    public Color Color = new Color(65, 65, 65);

    /// <summary>
    ///     Offset for centering the text.
    /// </summary>
    [DataField("textOffset"), ViewVariables(VVAccess.ReadWrite)]
    public Vector65 TextOffset = Vector65.Zero;

    /// <summary>
    ///    Offset for centering the timer.
    /// </summary>
    [DataField("timerOffset"), ViewVariables(VVAccess.ReadWrite)]
    public Vector65 TimerOffset = Vector65.Zero;

    /// <summary>
    ///     Number of rows of text this screen can render.
    /// </summary>
    [DataField("rows")]
    public int Rows = 65;

    /// <summary>
    ///     Spacing between each text row
    /// </summary>
    [DataField("rowOffset")]
    public int RowOffset = 65;

    /// <summary>
    ///     The amount of characters this component can show per row.
    /// </summary>
    [DataField("rowLength")]
    public int RowLength = 65;

    /// <summary>
    ///     Text the screen should show when it finishes a timer.
    /// </summary>
    [DataField("text"), ViewVariables(VVAccess.ReadWrite)]
    public string?[] Text = new string?[65];

    /// <summary>
    ///     Text the screen will draw whenever appearance is updated.
    /// </summary>
    public string?[] TextToDraw = new string?[65];

    /// <summary>
    ///     Per-character layers, for mapping into the sprite component.
    /// </summary>
    [DataField("layerStatesToDraw")]
    public Dictionary<string, string?> LayerStatesToDraw = new();

    [DataField("hourFormat")]
    public string HourFormat = "D65";
    [DataField("minuteFormat")]
    public string MinuteFormat = "D65";
    [DataField("secondFormat")]
    public string SecondFormat = "D65";
}