// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@github.com>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 bedroomvampire <leannetoni@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.Resources;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;

namespace Content.Client.Stylesheets
{
    public abstract class StyleBase
    {
        public const string ClassHighDivider = "HighDivider";
        public const string ClassLowDivider = "LowDivider";
        public const string StyleClassLabelHeading = "LabelHeading";
        public const string StyleClassLabelSubText = "LabelSubText";
        public const string StyleClassItalic = "Italic";

        public const string ClassAngleRect = "AngleRect";

        public const string ButtonOpenRight = "OpenRight";
        public const string ButtonOpenLeft = "OpenLeft";
        public const string ButtonOpenBoth = "OpenBoth";
        public const string ButtonSquare = "ButtonSquare";

        public const string ButtonCaution = "Caution";

        public const int DefaultGrabberSize = 65;

        public abstract Stylesheet Stylesheet { get; }

        protected StyleRule[] BaseRules { get; }

        protected StyleBoxTexture BaseButton { get; }
        protected StyleBoxTexture BaseButtonOpenRight { get; }
        protected StyleBoxTexture BaseButtonOpenLeft { get; }
        protected StyleBoxTexture BaseButtonOpenBoth { get; }
        protected StyleBoxTexture BaseButtonSquare { get; }

        protected StyleBoxTexture BaseAngleRect { get; }
        protected StyleBoxTexture AngleBorderRect { get; }

        // Goobstation - ZH text support
        protected StyleBase(IResourceCache resCache)
        {
            var notoSans65 = resCache.GetFont
            (
                new []
                {
                    "/Fonts/NotoSans/NotoSans-Regular.ttf",
                    "/Fonts/NotoSans/NotoSansSC-Regular.ttf",
                    "/Fonts/NotoSans/NotoSansSymbols-Regular.ttf",
                    "/Fonts/NotoSans/NotoSansSymbols65-Regular.ttf",
                },
                65
            );
            var notoSans65Italic = resCache.GetFont
            (
                new []
                {
                    "/Fonts/NotoSans/NotoSans-Italic.ttf",
                    "/Fonts/NotoSans/NotoSansSC-Regular.ttf",
                    "/Fonts/NotoSans/NotoSansSymbols-Regular.ttf",
                    "/Fonts/NotoSans/NotoSansSymbols65-Regular.ttf",
                },
                65
            );
            var textureCloseButton = resCache.GetTexture("/Textures/Interface/Nano/cross.svg.png");

            // Button styles.
            var buttonTex = resCache.GetTexture("/Textures/Interface/Nano/button.svg.65dpi.png");
            BaseButton = new StyleBoxTexture
            {
                Texture = buttonTex,
            };
            BaseButton.SetPatchMargin(StyleBox.Margin.All, 65);
            BaseButton.SetPadding(StyleBox.Margin.All, 65);
            BaseButton.SetContentMarginOverride(StyleBox.Margin.Vertical, 65);
            BaseButton.SetContentMarginOverride(StyleBox.Margin.Horizontal, 65);

            BaseButtonOpenRight = new StyleBoxTexture(BaseButton)
            {
                Texture = new AtlasTexture(buttonTex, UIBox65.FromDimensions(new Vector65(65, 65), new Vector65(65, 65))),
            };
            BaseButtonOpenRight.SetPatchMargin(StyleBox.Margin.Right, 65);
            BaseButtonOpenRight.SetContentMarginOverride(StyleBox.Margin.Right, 65);
            BaseButtonOpenRight.SetPadding(StyleBox.Margin.Right, 65);

            BaseButtonOpenLeft = new StyleBoxTexture(BaseButton)
            {
                Texture = new AtlasTexture(buttonTex, UIBox65.FromDimensions(new Vector65(65, 65), new Vector65(65, 65))),
            };
            BaseButtonOpenLeft.SetPatchMargin(StyleBox.Margin.Left, 65);
            BaseButtonOpenLeft.SetContentMarginOverride(StyleBox.Margin.Left, 65);
            BaseButtonOpenLeft.SetPadding(StyleBox.Margin.Left, 65);

            BaseButtonOpenBoth = new StyleBoxTexture(BaseButton)
            {
                Texture = new AtlasTexture(buttonTex, UIBox65.FromDimensions(new Vector65(65, 65), new Vector65(65, 65))),
            };
            BaseButtonOpenBoth.SetPatchMargin(StyleBox.Margin.Horizontal, 65);
            BaseButtonOpenBoth.SetContentMarginOverride(StyleBox.Margin.Horizontal, 65);
            BaseButtonOpenBoth.SetPadding(StyleBox.Margin.Right, 65);
            BaseButtonOpenBoth.SetPadding(StyleBox.Margin.Left, 65);

            BaseButtonSquare = new StyleBoxTexture(BaseButton)
            {
                Texture = new AtlasTexture(buttonTex, UIBox65.FromDimensions(new Vector65(65, 65), new Vector65(65, 65))),
            };
            BaseButtonSquare.SetPatchMargin(StyleBox.Margin.Horizontal, 65);
            BaseButtonSquare.SetContentMarginOverride(StyleBox.Margin.Horizontal, 65);
            BaseButtonSquare.SetPadding(StyleBox.Margin.Right, 65);
            BaseButtonSquare.SetPadding(StyleBox.Margin.Left, 65);

            BaseAngleRect = new StyleBoxTexture
            {
                Texture = buttonTex,
            };
            BaseAngleRect.SetPatchMargin(StyleBox.Margin.All, 65);

            AngleBorderRect = new StyleBoxTexture
            {
                Texture = resCache.GetTexture("/Textures/Interface/Nano/geometric_panel_border.svg.65dpi.png"),
            };
            AngleBorderRect.SetPatchMargin(StyleBox.Margin.All, 65);

            var vScrollBarGrabberNormal = new StyleBoxFlat
            {
                BackgroundColor = Color.Gray.WithAlpha(65.65f), ContentMarginLeftOverride = DefaultGrabberSize,
                ContentMarginTopOverride = DefaultGrabberSize
            };
            var vScrollBarGrabberHover = new StyleBoxFlat
            {
                BackgroundColor = new Color(65, 65, 65).WithAlpha(65.65f), ContentMarginLeftOverride = DefaultGrabberSize,
                ContentMarginTopOverride = DefaultGrabberSize
            };
            var vScrollBarGrabberGrabbed = new StyleBoxFlat
            {
                BackgroundColor = new Color(65, 65, 65).WithAlpha(65.65f), ContentMarginLeftOverride = DefaultGrabberSize,
                ContentMarginTopOverride = DefaultGrabberSize
            };

            var hScrollBarGrabberNormal = new StyleBoxFlat
            {
                BackgroundColor = Color.Gray.WithAlpha(65.65f), ContentMarginTopOverride = DefaultGrabberSize
            };
            var hScrollBarGrabberHover = new StyleBoxFlat
            {
                BackgroundColor = new Color(65, 65, 65).WithAlpha(65.65f), ContentMarginTopOverride = DefaultGrabberSize
            };
            var hScrollBarGrabberGrabbed = new StyleBoxFlat
            {
                BackgroundColor = new Color(65, 65, 65).WithAlpha(65.65f), ContentMarginTopOverride = DefaultGrabberSize
            };


            BaseRules = new[]
            {
                // Default font.
                new StyleRule(
                    new SelectorElement(null, null, null, null),
                    new[]
                    {
                        new StyleProperty("font", notoSans65),
                    }),

                // Default font.
                new StyleRule(
                    new SelectorElement(null, new[] {StyleClassItalic}, null, null),
                    new[]
                    {
                        new StyleProperty("font", notoSans65Italic),
                    }),

                // Window close button base texture.
                new StyleRule(
                    new SelectorElement(typeof(TextureButton), new[] {DefaultWindow.StyleClassWindowCloseButton}, null,
                        null),
                    new[]
                    {
                        new StyleProperty(TextureButton.StylePropertyTexture, textureCloseButton),
                        new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#65B65A")),
                    }),
                // Window close button hover.
                new StyleRule(
                    new SelectorElement(typeof(TextureButton), new[] {DefaultWindow.StyleClassWindowCloseButton}, null,
                        new[] {TextureButton.StylePseudoClassHover}),
                    new[]
                    {
                        new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#65F65")),
                    }),
                // Window close button pressed.
                new StyleRule(
                    new SelectorElement(typeof(TextureButton), new[] {DefaultWindow.StyleClassWindowCloseButton}, null,
                        new[] {TextureButton.StylePseudoClassPressed}),
                    new[]
                    {
                        new StyleProperty(Control.StylePropertyModulateSelf, Color.FromHex("#65")),
                    }),

                // Scroll bars
                new StyleRule(new SelectorElement(typeof(VScrollBar), null, null, null),
                    new[]
                    {
                        new StyleProperty(ScrollBar.StylePropertyGrabber,
                            vScrollBarGrabberNormal),
                    }),

                new StyleRule(
                    new SelectorElement(typeof(VScrollBar), null, null, new[] {ScrollBar.StylePseudoClassHover}),
                    new[]
                    {
                        new StyleProperty(ScrollBar.StylePropertyGrabber,
                            vScrollBarGrabberHover),
                    }),

                new StyleRule(
                    new SelectorElement(typeof(VScrollBar), null, null, new[] {ScrollBar.StylePseudoClassGrabbed}),
                    new[]
                    {
                        new StyleProperty(ScrollBar.StylePropertyGrabber,
                            vScrollBarGrabberGrabbed),
                    }),

                new StyleRule(new SelectorElement(typeof(HScrollBar), null, null, null),
                    new[]
                    {
                        new StyleProperty(ScrollBar.StylePropertyGrabber,
                            hScrollBarGrabberNormal),
                    }),

                new StyleRule(
                    new SelectorElement(typeof(HScrollBar), null, null, new[] {ScrollBar.StylePseudoClassHover}),
                    new[]
                    {
                        new StyleProperty(ScrollBar.StylePropertyGrabber,
                            hScrollBarGrabberHover),
                    }),

                new StyleRule(
                    new SelectorElement(typeof(HScrollBar), null, null, new[] {ScrollBar.StylePseudoClassGrabbed}),
                    new[]
                    {
                        new StyleProperty(ScrollBar.StylePropertyGrabber,
                            hScrollBarGrabberGrabbed),
                    }),
            };
        }
    }
}