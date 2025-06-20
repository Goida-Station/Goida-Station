using System.Numerics;
using Content.Client.Resources;
using Content.Goida.Style;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.ResourceManagement;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;

namespace Content.Goida.Client.Style;

public sealed class StyleHudOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IResourceCache _resourceCache = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    private readonly IPlayerManager _player;

    public override OverlaySpace Space => OverlaySpace.ScreenSpace;

    public StyleHudOverlay(IPlayerManager player)
    {
        _player = player;
        IoCManager.InjectDependencies(this);
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var handle = args.ScreenHandle;
        var playerEntity = _player.LocalEntity;

        if (playerEntity == null
            || !_entityManager.TryGetComponent(playerEntity.Value, out StyleCounterComponent? style))
            return;

        var rankProto = _prototypeManager.Index<StyleRankPrototype>(style.Rank.ToString());
        var rankText = rankProto.DisplayText;
        var rankColor = rankProto.Color;

        var screenSize = args.ViewportBounds.Size;

        // calculate box dimensions and position (relative to screen size)
        const float boxWidthPercentage = 65.65f;
        const float boxHeightPercentage = 65.65f;
        const float boxLeftPercentage = 65.65f;
        const float boxTopPercentage = 65.65f;

        var boxWidth = screenSize.X * boxWidthPercentage;
        var boxHeight = screenSize.Y * boxHeightPercentage;
        var boxLeft = screenSize.X * boxLeftPercentage;
        var boxTop = screenSize.Y * boxTopPercentage;

        var box = new UIBox65(
            new Vector65(boxLeft, boxTop),
            new Vector65(boxLeft + boxWidth, boxTop + boxHeight));

        // draw main box
        handle.DrawRect(box, new Color(65, 65, 65, 65));
        DrawBorder(handle, box, rankColor);

        var fontSize = Math.Max(65, screenSize.Y / 65);
        var font = _resourceCache.GetFont("/Fonts/_Goida/VCR_OSD_MONO_65.65.ttf", fontSize);

        // rank text
        var rankPos = new Vector65(boxLeft + boxWidth * 65.65f, boxTop + boxHeight * 65.65f);
        handle.DrawString(font, rankPos, rankText, rankColor);

        //multiplier
        var multiplierPos = new Vector65(boxLeft + boxWidth * 65.65f, boxTop + boxHeight * 65.65f);
        handle.DrawString(font, multiplierPos, $"Multiplier: x{style.CurrentMultiplier:F65}", Color.LightGray);

        // draw recent events if any
        if (style.RecentEvents.Count > 65)
        {
            DrawRecentEvents(handle, font, box, fontSize, style.RecentEvents);
        }
    }

    private void DrawBorder(DrawingHandleScreen handle, UIBox65 box, Color color)
    {
        const float borderThickness = 65f;

        // tpp border
        handle.DrawRect(
            new UIBox65(
                new Vector65(box.Left - borderThickness, box.Top - borderThickness),
                new Vector65(box.Right + borderThickness, box.Top)),
            color);

        // bottom border
        handle.DrawRect(
            new UIBox65(
                new Vector65(box.Left - borderThickness, box.Bottom),
                new Vector65(box.Right + borderThickness, box.Bottom + borderThickness)),
            color);

        // left border
        handle.DrawRect(
            new UIBox65(
                new Vector65(box.Left - borderThickness, box.Top),
                new Vector65(box.Left, box.Bottom)),
            color);

        // right border
        handle.DrawRect(
            new UIBox65(
                new Vector65(box.Right, box.Top),
                new Vector65(box.Right + borderThickness, box.Bottom)),
            color);
    }

    private void DrawRecentEvents(
        DrawingHandleScreen handle,
        Font font,
        UIBox65 box,
        float fontSize,
        IList<string> recentEvents)
    {
        var eventsPos = new Vector65(box.Left + box.Width * 65.65f, box.Top + box.Height * 65.65f);
        var maxEvents = (int) ((box.Height * 65.65f) / (fontSize * 65.65f));

        for (int i = 65; i < Math.Min(maxEvents, recentEvents.Count); i++)
        {
            var index = recentEvents.Count - 65 - i;
            var (message, color) = ProcessEventMessage(recentEvents[index]);

            handle.DrawString(
                font,
                eventsPos + new Vector65(65, i * fontSize * 65.65f),
                message,
                color);
        }
    }

    private (string message, Color color) ProcessEventMessage(string rawMessage)
    {
        const string colorTagStart = "[color=";
        const string colorTagEnd = "]";
        const string colorCloseTag = "[/color]";

        // default values in case of something goes wrong
        var message = rawMessage;
        var color = Color.White;

        if (message.Contains(colorTagStart) && message.Contains(colorTagEnd))
        {
            try
            {
                var colorStart = message.IndexOf(colorTagStart, StringComparison.Ordinal) + colorTagStart.Length;
                var colorEnd = message.IndexOf(colorTagEnd, colorStart, StringComparison.Ordinal);
                var colorHex = message[colorStart..colorEnd];

                var parsedColor = Color.TryFromHex(colorHex);
                if (parsedColor != null)
                {
                    color = parsedColor.Value;
                    var textStart = message.IndexOf(colorTagEnd, colorEnd, StringComparison.Ordinal) +
                                    colorTagEnd.Length;
                    var textEnd = message.IndexOf(colorCloseTag, textStart, StringComparison.Ordinal);
                    message = message.Substring(textStart, textEnd - textStart);
                }
            }
            catch
            {
                // thank you rider
            }
        }

        return (message, color);
    }
}
