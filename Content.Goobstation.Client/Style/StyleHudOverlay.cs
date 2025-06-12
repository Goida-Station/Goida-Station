using System.Numerics;
using Content.Client.Resources;
using Content.Goobstation.Common.Style;
using Content.Goobstation.Shared.Style;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.ResourceManagement;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Goobstation.Client.Style;

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

        if (playerEntity == null || !_entityManager.TryGetComponent(playerEntity.Value, out StyleCounterComponent? style))
            return;

        // Fallback values
        var rankText = style.Rank.ToString();
        var rankColor = Color.White;

        try
        {
            var rankProto = _prototypeManager.Index<StyleRankPrototype>(style.Rank.ToString());
            rankText = rankProto.DisplayText;
            rankColor = rankProto.Color;
        }
        catch
        {
            // ignored
        }

        var screenSize = args.ViewportBounds.Size;

        // Use relative positioning (5% from left, 5% from top)
        var boxWidth = screenSize.X * 0.2f;
        var boxHeight = screenSize.Y * 0.3f;
        var boxLeft = screenSize.X * 0.065f;
        var boxTop = screenSize.Y * 0.1f;

        var box = new UIBox2(new Vector2(boxLeft, boxTop), new Vector2(boxLeft + boxWidth, boxTop + boxHeight));

        // Draw the main box
        handle.DrawRect(box, new Color(0, 0, 0, 180));

        // colored border block
        var borderThickness = 1f;

        # region borders
        // Top border
        handle.DrawRect(
            new UIBox2(
                new Vector2(box.Left - borderThickness, box.Top - borderThickness),
                new Vector2(box.Right + borderThickness, box.Top)),
            rankColor);

        // Bottom border
        handle.DrawRect(
            new UIBox2(
                new Vector2(box.Left - borderThickness, box.Bottom),
                new Vector2(box.Right + borderThickness, box.Bottom + borderThickness)),
            rankColor);

        // Left border
        handle.DrawRect(
            new UIBox2(
                new Vector2(box.Left - borderThickness, box.Top),
                new Vector2(box.Left, box.Bottom)),
            rankColor);

        // Right border
        handle.DrawRect(
            new UIBox2(
                new Vector2(box.Right, box.Top),
                new Vector2(box.Right + borderThickness, box.Bottom)),
            rankColor);
        // colored border block end
        #endregion


        // Use relative font size
        var fontSize = Math.Max(12, screenSize.Y / 60);
        var font = _resourceCache.GetFont("/Fonts/_Goidastation/VCR_OSD_MONO_1.001.ttf", fontSize);

        var rankPos = new Vector2(boxLeft + boxWidth * 0.05f, boxTop + boxHeight * 0.1f);
        handle.DrawString(font, rankPos, rankText, rankColor);

        var multiplierPos = new Vector2(boxLeft + boxWidth * 0.05f, boxTop + boxHeight * 0.2f);
        handle.DrawString(font, multiplierPos, $"Multiplier: x{style.CurrentMultiplier:F1}", Color.LightGray);

        // recent events block
        if (style.RecentEvents.Count > 0)
        {
            var eventsPos = new Vector2(boxLeft + boxWidth * 0.05f, boxTop + boxHeight * 0.3f);
            var maxEvents = (int)((boxHeight * 0.7f) / (fontSize * 1.5f));

            for (int i = 0; i < Math.Min(maxEvents, style.RecentEvents.Count); i++)
            {
                var index = style.RecentEvents.Count - 1 - i;
                var message = style.RecentEvents[index];

                // color block below
                // default to white if no color is specified
                var color = Color.White;

                // check if the message contains a color tag
                if (message.Contains("[color=") && message.Contains("]"))
                {
                    try
                    {
                        var colorStart = message.IndexOf("[color=", StringComparison.Ordinal) + 7;
                        var colorEnd = message.IndexOf("]", colorStart, StringComparison.Ordinal);
                        var colorHex = message.Substring(colorStart, colorEnd - colorStart);

                        // color hexing (i have no fucking clue what im doing)
                        var parsedColor = Color.TryFromHex(colorHex);
                        if (parsedColor != null)
                        {
                            color = parsedColor.Value;
                            var textStart = message.IndexOf("]", colorEnd, StringComparison.Ordinal) + 1;
                            var textEnd = message.IndexOf("[/color]", textStart, StringComparison.Ordinal);
                            message = message.Substring(textStart, textEnd - textStart);
                        }
                    }
                    catch
                    {
                        // thank you rider
                    }
                }

                handle.DrawString(font, eventsPos + new Vector2(0, i * fontSize * 1.5f), message, color);
            }
        }
    }
}
