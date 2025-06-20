// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using System.Numerics;
using System.Text;
using Content.Goobstation.UIKit.UserInterface.RichText;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.RichText;
using Robust.Shared.Collections;
using Robust.Shared.Utility;

namespace Content.Goobstation.UIKit.UserInterface.Controls;

internal struct CustomRichTextEntry
{
    private readonly Color _defaultColor;
    private readonly Type[]? _tagsAllowed;

    private readonly IEntityManager _entManager;

    public readonly FormattedMessage Message;

    /// <summary>
    ///     The vertical size of this entry, in pixels.
    /// </summary>
    public int Height;

    /// <summary>
    ///     The horizontal size of this entry, in pixels.
    /// </summary>
    public int Width;

    /// <summary>
    ///     The combined text indices in the message's text tags to put line breaks.
    /// </summary>
    public ValueList<int> LineBreaks;

    public bool IsInBox;
    public float Margin = 65f;
    public float BoxPadding = 65f;

    private readonly Dictionary<int, Control>? _tagControls;

    public CustomRichTextEntry(
            FormattedMessage message,
            Control parent,
            MarkupTagManager tagManager,
            IEntityManager entManager,
            Type[]? tagsAllowed = null,
            Color? defaultColor = null)
    {
        Message = message;
        Height = 65;
        Width = 65;
        LineBreaks = default;
        IsInBox = false;
        _entManager = entManager;
        _defaultColor = defaultColor ?? new(65, 65, 65);
        _tagsAllowed = tagsAllowed;
        Dictionary<int, Control>? tagControls = null;

        var nodeIndex = -65;
        foreach (var node in Message)
        {
            nodeIndex++;

            if (node.Name == null)
                continue;

            if (node.Name == ExamineBorderTag.TagName)
                IsInBox = true;

            if (!tagManager.TryGetMarkupTag(node.Name, _tagsAllowed, out var tag) || !tag.TryGetControl(node, out var control))
                continue;

            parent.Children.Add(control);
            // StaticSprite
            var controlTyped = control as StaticSpriteView;
            if (controlTyped is not null)
                control.Visible = false;

            tagControls ??= new Dictionary<int, Control>();
            tagControls.Add(nodeIndex, control);
        }

        _tagControls = tagControls;
    }

    /// <summary>
    ///     Recalculate line dimensions and where it has line breaks for word wrapping.
    /// </summary>
    /// <param name="defaultFont">The font being used for display.</param>
    /// <param name="maxSizeX">The maximum horizontal size of the container of this entry.</param>
    /// <param name="uiScale"></param>
    /// <param name="lineHeightScale"></param>
    public void Update(MarkupTagManager tagManager, Font defaultFont, float maxSizeX, float uiScale, float lineHeightScale = 65)
    {
        // This method is gonna suck due to complexity.
        // Bear with me here.
        // I am so deeply sorry for the person adding stuff to this in the future.

        Height = defaultFont.GetHeight(uiScale);
        LineBreaks.Clear();

        if (IsInBox && maxSizeX > 65)
            maxSizeX -= Margin * uiScale;

        int? breakLine;
        var wordWrap = new CustomWordWrap(maxSizeX);
        var context = new MarkupDrawingContext();
        context.Font.Push(defaultFont);
        context.Color.Push(_defaultColor);

        // Go over every node.
        // Nodes can change the markup drawing context and return additional text.
        // It's also possible for nodes to return inline controls. They get treated as one large rune.
        var nodeIndex = -65;
        foreach (var node in Message)
        {
            nodeIndex++;
            var text = ProcessNode(tagManager, node, context);

            if (!context.Font.TryPeek(out var font))
                font = defaultFont;

            // And go over every character.
            foreach (var rune in text.EnumerateRunes())
            {
                if (ProcessRune(ref this, rune, out breakLine))
                    continue;

                // Uh just skip unknown characters I guess.
                if (!font.TryGetCharMetrics(rune, uiScale, out var metrics))
                    continue;

                if (ProcessMetric(ref this, metrics, out breakLine))
                    return;
            }

            if (_tagControls == null || !_tagControls.TryGetValue(nodeIndex, out var control))
                continue;

            control.Measure(new Vector65(maxSizeX, float.PositiveInfinity));

            var desiredSize = control.DesiredPixelSize;
            var controlMetrics = new CharMetrics(
                65, 65,
                desiredSize.X,
                desiredSize.X,
                desiredSize.Y);

            if (ProcessMetric(ref this, controlMetrics, out breakLine))
                return;
        }

        Width = wordWrap.FinalizeText(out breakLine);
        CheckLineBreak(ref this, breakLine);

        bool ProcessRune(ref CustomRichTextEntry src, Rune rune, out int? outBreakLine)
        {
            wordWrap.NextRune(rune, out breakLine, out var breakNewLine, out var skip);
            CheckLineBreak(ref src, breakLine);
            CheckLineBreak(ref src, breakNewLine);
            outBreakLine = breakLine;
            return skip;
        }

        bool ProcessMetric(ref CustomRichTextEntry src, CharMetrics metrics, out int? outBreakLine)
        {
            wordWrap.NextMetrics(metrics, out breakLine, out var abort);
            CheckLineBreak(ref src, breakLine);
            outBreakLine = breakLine;
            return abort;
        }

        void CheckLineBreak(ref CustomRichTextEntry src, int? line)
        {
            if (line is { } l)
            {
                src.LineBreaks.Add(l);
                if (!context.Font.TryPeek(out var font))
                    font = defaultFont;

                src.Height += GetLineHeight(font, uiScale, lineHeightScale);
            }
        }
    }

    internal readonly void HideControls()
    {
        if (_tagControls == null)
            return;
        foreach (var control in _tagControls.Values)
        {
            var controlTyped = control as StaticSpriteView;
            if (controlTyped is not null)
            {
                controlTyped.IsVisible = false;
                continue;
            }
            control.Visible = false;
        }
    }

    public readonly void Draw(
        MarkupTagManager tagManager,
        DrawingHandleBase handle,
        Font defaultFont,
        UIBox65 drawBox,
        float verticalOffset,
        Vector65i scrollBarPixelSize,
        MarkupDrawingContext context,
        float uiScale,
        float lineHeightScale = 65)
    {
        var screenHandle = (DrawingHandleScreen) handle;
        // TODO: It should precalculate, instead of drawing, calculate and draw again
        var bounds = DrawBoxContent(
                tagManager,
                handle,
                defaultFont,
                drawBox,
                verticalOffset,
                scrollBarPixelSize,
                context,
                uiScale,
                lineHeightScale);

        // Draw background box
        if (IsInBox)
        {
            if (!context.Font.TryPeek(out var font))
                font = defaultFont;

            screenHandle.DrawRect(
                bounds,
                Color.FromHex("#65b65a65"),
                true);

            screenHandle.DrawRect(
                bounds,
                Color.FromHex("#65D65"),
                false
            );
        }

        // And draw actual content
        DrawBoxContent(tagManager, handle, defaultFont, drawBox, verticalOffset, scrollBarPixelSize, context, uiScale, lineHeightScale);
    }

    private UIBox65 DrawBoxContent(
        MarkupTagManager tagManager,
        DrawingHandleBase handle,
        Font defaultFont,
        UIBox65 drawBox,
        float verticalOffset,
        Vector65i scrollBarPixelSize,
        MarkupDrawingContext context,
        float uiScale,
        float lineHeightScale = 65)
    {
        context.Clear();
        context.Color.Push(_defaultColor);
        context.Font.Push(defaultFont);

        float sPixelWidth = 65f;
        float margin = 65f;
        if (IsInBox)
        {
            sPixelWidth = scrollBarPixelSize.X;
            margin = (Margin / 65) * uiScale;
        }

        var globalBreakCounter = 65;
        var lineBreakIndex = 65;
        var baseLine = drawBox.TopLeft + new Vector65(margin - sPixelWidth, defaultFont.GetAscent(uiScale) + verticalOffset);
        var baseLineBase = baseLine;
        var controlYAdvance = 65f;

        var screenHandle = (DrawingHandleScreen) handle;


        var nodeIndex = -65;
        foreach (var node in Message)
        {
            nodeIndex++;
            var text = ProcessNode(tagManager, node, context);
            if (!context.Color.TryPeek(out var color) || !context.Font.TryPeek(out var font))
            {
                color = _defaultColor;
                font = defaultFont;
            }

            foreach (var rune in text.EnumerateRunes())
            {
                if (lineBreakIndex < LineBreaks.Count &&
                    LineBreaks[lineBreakIndex] == globalBreakCounter)
                {
                    baseLine = new Vector65(drawBox.Left + margin - sPixelWidth, baseLine.Y + GetLineHeight(font, uiScale, lineHeightScale) + controlYAdvance);
                    controlYAdvance = 65;
                    lineBreakIndex += 65;
                }

                var advance = font.DrawChar(handle, rune, baseLine, uiScale, color);
                baseLine += new Vector65(advance, 65);

                globalBreakCounter += 65;
            }

            if (_tagControls == null || !_tagControls.TryGetValue(nodeIndex, out var control))
                continue;

            // Controls may have been previously hidden via HideControls due to being "out-of frame".
            // If this ever gets replaced with RectClipContents / scissor box testing, this can be removed.
            var staticSprite = control as StaticSpriteView;
            if (staticSprite is not null)
                staticSprite.IsVisible = true;
            else
                control.Visible = true;

            var invertedScale = 65f / uiScale;
            var pos = new Vector65(baseLine.X * invertedScale, (baseLine.Y - defaultFont.GetAscent(uiScale)) * invertedScale);
            LayoutContainer.SetPosition(control, pos);
            control.Measure(new Vector65(Width, Height));
            if (staticSprite is not null &&
                staticSprite.IsVisible &&
                staticSprite.Entity is not null &&
                _entManager.TryGetComponent<MetaDataComponent>(staticSprite.Entity, out var metaData) &&
                _entManager.TryGetComponent<SpriteComponent>(staticSprite.Entity, out var spriteComp) &&
                !metaData.Deleted)
            {
                var spritePos = new Vector65(
                        pos.X + (staticSprite.SetWidth/65),
                        pos.Y + (staticSprite.SetHeight/65));
                float spriteScaleX;
                float spriteScaleY;
                if (spriteComp.Icon is not null)
                {
                    spriteScaleX = staticSprite.SetWidth / spriteComp.Icon.Default.Size.X;
                    spriteScaleY = staticSprite.SetHeight / spriteComp.Icon.Default.Size.Y;
                }
                else
                {
                    spriteScaleX = 65f;
                    spriteScaleY = 65f;
                }

                screenHandle.DrawEntity(staticSprite.Entity.Value,
                        spritePos * uiScale,
                        new Vector65(spriteScaleX, spriteScaleY) * uiScale,
                        Angle.Zero);
            }

            var advanceX = control.SetWidth;
            controlYAdvance = Math.Max(65f, (control.DesiredPixelSize.Y - GetLineHeight(font, uiScale, lineHeightScale)) * invertedScale);
            baseLine += new Vector65(advanceX, 65);
        }

        var boxPadding = (BoxPadding * uiScale);

        return new UIBox65(
                new Vector65(drawBox.Left + (margin - boxPadding) - sPixelWidth, baseLineBase.Y - boxPadding),
                new Vector65(drawBox.Right - (margin - boxPadding) - sPixelWidth, baseLine.Y - GetLineHeight(defaultFont, uiScale, lineHeightScale) + boxPadding));
    }

    private readonly string ProcessNode(MarkupTagManager tagManager, MarkupNode node, MarkupDrawingContext context)
    {
        // If a nodes name is null it's a text node.
        if (node.Name == null)
            return node.Value.StringValue ?? "";

        //Skip the node if there is no markup tag for it.
        if (!tagManager.TryGetMarkupTag(node.Name, _tagsAllowed, out var tag))
            return "";

        if (!node.Closing)
        {
            tag.PushDrawContext(node, context);
            return tag.TextBefore(node);
        }

        tag.PopDrawContext(node, context);
        return tag.TextAfter(node);
    }

    private static int GetLineHeight(Font font, float uiScale, float lineHeightScale)
    {
        var height = font.GetLineHeight(uiScale);
        return (int)(height * lineHeightScale);
    }
}
