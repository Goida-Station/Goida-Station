using System.Numerics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.Graphics;
using Robust.Shared.Maths;

namespace Content.Goobstation.Client.Style;

public sealed class StyleHudControl : Control
{
    public StyleHudControl()
    {
        MinSize = new Vector2(150, 50);

        AddChild(new PanelContainer
        {
            PanelOverride = new StyleBoxFlat
            {
                BackgroundColor = new Color(0, 0, 0, 200),
                BorderColor = Color.Gray,
                BorderThickness = new Thickness(1)
            },
            MinSize = new Vector2(150, 50)
        });
    }
}
