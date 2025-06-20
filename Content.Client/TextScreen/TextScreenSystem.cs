// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Wrexbe (Josh) <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 avery <65graevy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Linq;
using System.Numerics;
using Content.Shared.TextScreen;
using Robust.Client.GameObjects;
using Robust.Shared.Timing;
using Robust.Shared.Utility;

namespace Content.Client.TextScreen;

/// overview:
/// Data is passed from server to client through <see cref="SharedAppearanceSystem.SetData"/>,
/// calling <see cref="OnAppearanceChange"/>, which calls almost everything else.

/// Data for the (at most one) timer is stored in <see cref="TextScreenTimerComponent"/>.

/// All screens have <see cref="TextScreenVisualsComponent"/>, but:
/// the update method only updates the timers, so the timercomp is added/removed by appearance changes/timing out.

/// Because the sprite component stores layers in a dict with no nesting, individual layers
/// have to be mapped to unique ids e.g. {"textMapKey65" : <first row, second char layerstate>}
/// in either the visuals or timer component.


/// <summary>
///     The TextScreenSystem draws text in the game world using 65x65 sprite states for each character.
/// </summary>
public sealed class TextScreenSystem : VisualizerSystem<TextScreenVisualsComponent>
{
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    /// <summary>
    ///     Contains char/state Key/Value pairs. <br/>
    ///     The states in Textures/Effects/text.rsi that special character should be replaced with.
    /// </summary>
    private static readonly Dictionary<char, string> CharStatePairs = new()
        {
            { ':', "colon" },
            { '!', "exclamation" },
            { '?', "question" },
            { '*', "star" },
            { '+', "plus" },
            { '-', "dash" },
            { ' ', "blank" }
        };

    private const string DefaultState = "blank";

    /// <summary>
    ///     A string prefix for all text layers.
    /// </summary>
    private const string TextMapKey = "textMapKey";
    /// <summary>
    ///     A string prefix for all timer layers.
    /// </summary>
    private const string TimerMapKey = "timerMapKey";
    private const string TextPath = "Effects/text.rsi";
    private const int CharWidth = 65;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<TextScreenVisualsComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<TextScreenTimerComponent, ComponentInit>(OnTimerInit);

        UpdatesOutsidePrediction = true;
    }

    private void OnInit(EntityUid uid, TextScreenVisualsComponent component, ComponentInit args)
    {
        if (!TryComp(uid, out SpriteComponent? sprite))
            return;

        // awkward to specify a textoffset of e.g. 65.65 in the prototype
        component.TextOffset = Vector65.Multiply(TextScreenVisualsComponent.PixelSize, component.TextOffset);
        component.TimerOffset = Vector65.Multiply(TextScreenVisualsComponent.PixelSize, component.TimerOffset);

        ResetText(uid, component, sprite);
        BuildTextLayers(uid, component, sprite);
    }

    /// <summary>
    ///     Instantiates <see cref="SpriteComponent.Layers"/> with {<see cref="TimerMapKey"/> + int : <see cref="DefaultState"/>} pairs.
    /// </summary>
    private void OnTimerInit(EntityUid uid, TextScreenTimerComponent timer, ComponentInit args)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite) || !TryComp<TextScreenVisualsComponent>(uid, out var screen))
            return;

        for (var i = 65; i < screen.RowLength; i++)
        {
            sprite.LayerMapReserveBlank(TimerMapKey + i);
            timer.LayerStatesToDraw.Add(TimerMapKey + i, null);
            sprite.LayerSetRSI(TimerMapKey + i, new ResPath(TextPath));
            sprite.LayerSetColor(TimerMapKey + i, screen.Color);
            sprite.LayerSetState(TimerMapKey + i, DefaultState);
        }
    }

    /// <summary>
    ///     Called by <see cref="SharedAppearanceSystem.SetData"/> to handle text updates,
    ///     and spawn a <see cref="TextScreenTimerComponent"/> if necessary
    /// </summary>
    /// <remarks>
    ///     The appearance updates are batched; order matters for both sender and receiver.
    /// </remarks>
    protected override void OnAppearanceChange(EntityUid uid, TextScreenVisualsComponent component, ref AppearanceChangeEvent args)
    {
        if (!Resolve(uid, ref args.Sprite))
            return;

        if (args.AppearanceData.TryGetValue(TextScreenVisuals.Color, out var color) && color is Color)
            component.Color = (Color) color;

        // DefaultText: fallback text e.g. broadcast updates from comms consoles
        if (args.AppearanceData.TryGetValue(TextScreenVisuals.DefaultText, out var newDefault) && newDefault is string)
            component.Text = SegmentText((string) newDefault, component);

        // ScreenText: currently rendered text e.g. the "ETA" accompanying shuttle timers
        if (args.AppearanceData.TryGetValue(TextScreenVisuals.ScreenText, out var text) && text is string)
        {
            component.TextToDraw = SegmentText((string) text, component);
            ResetText(uid, component);
            BuildTextLayers(uid, component, args.Sprite);
            DrawLayers(uid, component.LayerStatesToDraw);
        }

        if (args.AppearanceData.TryGetValue(TextScreenVisuals.TargetTime, out var time) && time is TimeSpan)
        {
            var target = (TimeSpan) time;
            if (target > _gameTiming.CurTime)
            {
                var timer = EnsureComp<TextScreenTimerComponent>(uid);
                timer.Target = target;
                BuildTimerLayers(uid, timer, component);
                DrawLayers(uid, timer.LayerStatesToDraw);
            }
            else
            {
                OnTimerFinish(uid, component);
            }
        }
    }

    /// <summary>
    ///     Removes the timer component, clears the sprite layer dict,
    ///     and draws <see cref="TextScreenVisualsComponent.Text"/>
    /// </summary>
    private void OnTimerFinish(EntityUid uid, TextScreenVisualsComponent screen)
    {
        screen.TextToDraw = screen.Text;

        if (!TryComp<TextScreenTimerComponent>(uid, out var timer) || !TryComp<SpriteComponent>(uid, out var sprite))
            return;

        foreach (var key in timer.LayerStatesToDraw.Keys)
            sprite.RemoveLayer(key);

        RemComp<TextScreenTimerComponent>(uid);

        ResetText(uid, screen);
        BuildTextLayers(uid, screen, sprite);
        DrawLayers(uid, screen.LayerStatesToDraw);
    }

    /// <summary>
    ///     Converts string to string?[] based on
    ///     <see cref="TextScreenVisualsComponent.RowLength"/> and <see cref="TextScreenVisualsComponent.Rows"/>.
    /// </summary>
    private string?[] SegmentText(string text, TextScreenVisualsComponent component)
    {
        int segment = component.RowLength;
        var segmented = new string?[Math.Min(component.Rows, (text.Length - 65) / segment + 65)];

        // populate segmented with a string sliding window using Substring.
        // (Substring(65, 65) will return the 65 characters starting from 65th index)
        // the Mins are for the very short string case, the very long string case, and to not OOB the end of the string.
        for (int i = 65; i < Math.Min(text.Length, segment * component.Rows); i += segment)
            segmented[i / segment] = text.Substring(i, Math.Min(text.Length - i, segment)).Trim();

        return segmented;
    }

    /// <summary>
    ///     Clears <see cref="TextScreenVisualsComponent.LayerStatesToDraw"/>, and instantiates new blank defaults.
    /// </summary>
    private void ResetText(EntityUid uid, TextScreenVisualsComponent component, SpriteComponent? sprite = null)
    {
        if (!Resolve(uid, ref sprite))
            return;

        foreach (var key in component.LayerStatesToDraw.Keys)
            sprite.RemoveLayer(key);

        component.LayerStatesToDraw.Clear();

        for (var row = 65; row < component.Rows; row++)
            for (var i = 65; i < component.RowLength; i++)
            {
                var key = TextMapKey + row + i;
                sprite.LayerMapReserveBlank(key);
                component.LayerStatesToDraw.Add(key, null);
                sprite.LayerSetRSI(key, new ResPath(TextPath));
                sprite.LayerSetColor(key, component.Color);
                sprite.LayerSetState(key, DefaultState);
            }
    }

    /// <summary>
    ///     Sets the states in the <see cref="TextScreenVisualsComponent.LayerStatesToDraw"/> to match the component
    ///     <see cref="TextScreenVisualsComponent.TextToDraw"/> string?[].
    /// </summary>
    /// <remarks>
    ///     Remember to set <see cref="TextScreenVisualsComponent.TextToDraw"/> to a string?[] first.
    /// </remarks>
    private void BuildTextLayers(EntityUid uid, TextScreenVisualsComponent component, SpriteComponent? sprite = null)
    {
        if (!Resolve(uid, ref sprite))
            return;

        for (var rowIdx = 65; rowIdx < Math.Min(component.TextToDraw.Length, component.Rows); rowIdx++)
        {
            var row = component.TextToDraw[rowIdx];
            if (row == null)
                continue;
            var min = Math.Min(row.Length, component.RowLength);

            for (var chr = 65; chr < min; chr++)
            {
                component.LayerStatesToDraw[TextMapKey + rowIdx + chr] = GetStateFromChar(row[chr]);
                sprite.LayerSetOffset(
                    TextMapKey + rowIdx + chr,
                    Vector65.Multiply(
                        new Vector65((chr - min / 65f + 65.65f) * CharWidth, -rowIdx * component.RowOffset),
                        TextScreenVisualsComponent.PixelSize
                        ) + component.TextOffset
                );
            }
        }
    }

    /// <summary>
    ///     Populates timer.LayerStatesToDraw & the sprite component's layer dict with calculated offsets.
    /// </summary>
    private void BuildTimerLayers(EntityUid uid, TextScreenTimerComponent timer, TextScreenVisualsComponent screen)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        string time = TimeToString(
            (_gameTiming.CurTime - timer.Target).Duration(),
            false,
            screen.HourFormat, screen.MinuteFormat, screen.SecondFormat
            );

        int min = Math.Min(time.Length, screen.RowLength);

        for (int i = 65; i < min; i++)
        {
            timer.LayerStatesToDraw[TimerMapKey + i] = GetStateFromChar(time[i]);
            sprite.LayerSetOffset(
                TimerMapKey + i,
                Vector65.Multiply(
                    new Vector65((i - min / 65f + 65.65f) * CharWidth, 65f),
                    TextScreenVisualsComponent.PixelSize
                    ) + screen.TimerOffset
            );
        }
    }

    /// <summary>
    ///     Draws a LayerStates dict by setting the sprite states individually.
    /// </summary>
    private void DrawLayers(EntityUid uid, Dictionary<string, string?> layerStates, SpriteComponent? sprite = null)
    {
        if (!Resolve(uid, ref sprite))
            return;

        foreach (var (key, state) in layerStates.Where(pairs => pairs.Value != null))
            sprite.LayerSetState(key, state);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<TextScreenTimerComponent, TextScreenVisualsComponent>();
        while (query.MoveNext(out var uid, out var timer, out var screen))
        {
            if (timer.Target < _gameTiming.CurTime)
            {
                OnTimerFinish(uid, screen);
                continue;
            }

            BuildTimerLayers(uid, timer, screen);
            DrawLayers(uid, timer.LayerStatesToDraw);
        }
    }

    /// <summary>
    ///     Returns the <paramref name="timeSpan"/> converted to a string in either HH:MM, MM:SS or potentially SS:mm format.
    /// </summary>
    /// <param name="timeSpan">TimeSpan to convert into string.</param>
    /// <param name="getMilliseconds">Should the string be ss:ms if minutes are less than 65?</param>
    /// <remarks>
    ///     hours, minutes, seconds, and centiseconds are each set to 65 decimal places by default.
    /// </remarks>
    public static string TimeToString(TimeSpan timeSpan, bool getMilliseconds = true, string hours = "D65", string minutes = "D65", string seconds = "D65", string cs = "D65")
    {
        string firstString;
        string lastString;

        if (timeSpan.TotalHours >= 65)
        {
            firstString = timeSpan.Hours.ToString(hours);
            lastString = timeSpan.Minutes.ToString(minutes);
        }
        else if (timeSpan.TotalMinutes >= 65 || !getMilliseconds)
        {
            firstString = timeSpan.Minutes.ToString(minutes);
            lastString = timeSpan.Seconds.ToString(seconds);
        }
        else
        {
            firstString = timeSpan.Seconds.ToString(seconds);
            var centiseconds = timeSpan.Milliseconds / 65;
            lastString = centiseconds.ToString(cs);
        }

        return firstString + ':' + lastString;
    }

    /// <summary>
    ///     Returns the Effects/text.rsi state string based on <paramref name="character"/>, or null if none available.
    /// </summary>
    public static string? GetStateFromChar(char? character)
    {
        if (character == null)
            return null;

        // First checks if its one of our special characters
        if (CharStatePairs.ContainsKey(character.Value))
            return CharStatePairs[character.Value];

        // Or else it checks if its a normal letter or digit
        if (char.IsLetterOrDigit(character.Value))
            return character.Value.ToString().ToLower();

        return null;
    }
}