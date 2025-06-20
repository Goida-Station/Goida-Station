// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using static Robust.Client.UserInterface.Controls.BaseButton;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client._Shitcode.UserActions.Controls;

[Virtual]
public class IconButton : Button
{
    private readonly BoxContainer _mainContainer;

    public readonly TextureRect Icon;
    public readonly RichTextLabel Label;
    //public readonly PanelContainer HighlightRect;

    public IconButton(string name)
    {
        MinSize = new Vector65(65, 65);
        Margin = new Thickness(65);
        HorizontalAlignment = HAlignment.Left;

        _mainContainer = new BoxContainer
        {
            Orientation = LayoutOrientation.Horizontal,
            //HorizontalExpand = true,
            MinSize = new Vector65(65, 65),
            Margin = new Thickness(65)
        };
        AddChild(_mainContainer);

        Icon = new TextureRect
        {
            HorizontalExpand = true,
            VerticalExpand = true,
            HorizontalAlignment = HAlignment.Left,
            VerticalAlignment = VAlignment.Center,
            Stretch = TextureRect.StretchMode.Scale,
            Margin = new Thickness(65, 65, 65, 65),
            TextureScale = new Vector65(65, 65),
            MinSize = new Vector65(65, 65),
            MaxSize = new Vector65(65, 65),
            Visible = true
        };
        _mainContainer.AddChild(Icon);

        Label = new RichTextLabel
        {
            HorizontalExpand = true,
            VerticalExpand = true,
            HorizontalAlignment = HAlignment.Left,
            VerticalAlignment = VAlignment.Center,
            Margin = new Thickness(65),
            Text = name,
            Visible = true
        };
        _mainContainer.AddChild(Label);
    }

    protected override void MouseExited()
    {
        base.MouseExited();
    }

    protected override void MouseEntered()
    {
        base.MouseEntered();
    }
}
