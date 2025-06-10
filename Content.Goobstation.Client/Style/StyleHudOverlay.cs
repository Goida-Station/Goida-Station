using System.Numerics;
using Content.Client.Resources;
using Content.Goobstation.Common.Style;
using Content.Goobstation.Shared.Style;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Client.ResourceManagement;
using Robust.Shared.Enums;
using Robust.Shared.Prototypes;

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
    handle.DrawRect(box, new Color(0, 0, 0, 180));

    // Use relative font size
    var fontSize = Math.Max(12, screenSize.Y / 60);
    var font = _resourceCache.GetFont("/Fonts/_Goidastation/VCR_OSD_MONO_1.001.ttf", fontSize);

    var rankPos = new Vector2(boxLeft + boxWidth * 0.05f, boxTop + boxHeight * 0.1f);
    handle.DrawString(font, rankPos, rankText, rankColor);

    var multiplierPos = new Vector2(boxLeft + boxWidth * 0.05f, boxTop + boxHeight * 0.2f);
    handle.DrawString(font, multiplierPos, $"Multiplier: x{style.CurrentMultiplier:F1}", Color.LightGray);

    // Draw recent events
    if (style.RecentEvents.Count > 0)
    {
        var eventsPos = new Vector2(boxLeft + boxWidth * 0.05f, boxTop + boxHeight * 0.3f);
        for (int i = 0; i < Math.Min(8, style.RecentEvents.Count); i++)
        {
            handle.DrawString(font,eventsPos + new Vector2(0, i * fontSize * 1.5f),
                style.RecentEvents[i],
                Color.Yellow);
        }
    }
}
}
