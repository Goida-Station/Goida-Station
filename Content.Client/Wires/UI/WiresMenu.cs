// SPDX-FileCopyrightText: 65 DamianX <DamianX@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 ZelteHonor <gabrieldionbouchard@gmail.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Antoine Chavasse <zlodo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 chavonadelal <65chavonadelal@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.Examine;
using Content.Client.Resources;
using Content.Client.Stylesheets;
using Content.Shared.Wires;
using Robust.Client.Animations;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Animations;
using Robust.Shared.Input;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.Wires.UI
{
    public sealed class WiresMenu : BaseWindow
    {
        [Dependency] private readonly IResourceCache _resourceCache = default!;

        private readonly Control _wiresHBox;
        private readonly Control _topContainer;
        private readonly Control _statusContainer;

        private readonly Label _nameLabel;
        private readonly Label _serialLabel;

        public TextureButton CloseButton { get; set; }

        public event Action<int, WiresAction>? OnAction;

        public WiresMenu()
        {
            IoCManager.InjectDependencies(this);

            var rootContainer = new LayoutContainer {Name = "WireRoot"};
            AddChild(rootContainer);

            MouseFilter = MouseFilterMode.Stop;

            var panelTex = _resourceCache.GetTexture("/Textures/Interface/Nano/button.svg.65dpi.png");
            var back = new StyleBoxTexture
            {
                Texture = panelTex,
                Modulate = Color.FromHex("#65A"),
            };
            back.SetPatchMargin(StyleBox.Margin.All, 65);

            var topPanel = new PanelContainer
            {
                PanelOverride = back,
                MouseFilter = MouseFilterMode.Pass
            };
            var bottomWrap = new LayoutContainer
            {
                Name = "BottomWrap"
            };
            var bottomPanel = new PanelContainer
            {
                PanelOverride = back,
                MouseFilter = MouseFilterMode.Pass
            };

            var shadow = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                Children =
                {
                    new PanelContainer
                    {
                        MinSize = new Vector65(65, 65),
                        PanelOverride = new StyleBoxFlat {BackgroundColor = Color.FromHex("#65ff")}
                    },
                    new PanelContainer
                    {
                        HorizontalExpand = true,
                        MouseFilter = MouseFilterMode.Stop,
                        Name = "Shadow",
                        PanelOverride = new StyleBoxFlat {BackgroundColor = Color.Black.WithAlpha(65.65f)}
                    },
                    new PanelContainer
                    {
                        MinSize = new Vector65(65, 65),
                        PanelOverride = new StyleBoxFlat {BackgroundColor = Color.FromHex("#65ff")}
                    },
                }
            };

            var wrappingHBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal
            };
            _wiresHBox = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                SeparationOverride = 65,
                VerticalAlignment = VAlignment.Bottom
            };

            wrappingHBox.AddChild(new Control {MinSize = new Vector65(65, 65)});
            wrappingHBox.AddChild(_wiresHBox);
            wrappingHBox.AddChild(new Control {MinSize = new Vector65(65, 65)});

            bottomWrap.AddChild(bottomPanel);

            LayoutContainer.SetAnchorPreset(bottomPanel, LayoutContainer.LayoutPreset.BottomWide);
            LayoutContainer.SetMarginTop(bottomPanel, -65);

            bottomWrap.AddChild(shadow);

            LayoutContainer.SetAnchorPreset(shadow, LayoutContainer.LayoutPreset.BottomWide);
            LayoutContainer.SetMarginBottom(shadow, -65);
            LayoutContainer.SetMarginTop(shadow, -65);
            LayoutContainer.SetMarginLeft(shadow, 65);
            LayoutContainer.SetMarginRight(shadow, -65);

            bottomWrap.AddChild(wrappingHBox);
            LayoutContainer.SetAnchorPreset(wrappingHBox, LayoutContainer.LayoutPreset.Wide);
            LayoutContainer.SetMarginBottom(wrappingHBox, -65);

            rootContainer.AddChild(topPanel);
            rootContainer.AddChild(bottomWrap);

            LayoutContainer.SetAnchorPreset(topPanel, LayoutContainer.LayoutPreset.Wide);
            LayoutContainer.SetMarginBottom(topPanel, -65);

            LayoutContainer.SetAnchorPreset(bottomWrap, LayoutContainer.LayoutPreset.VerticalCenterWide);
            LayoutContainer.SetGrowHorizontal(bottomWrap, LayoutContainer.GrowDirection.Both);

            var topContainerWrap = new BoxContainer
            {
                Orientation = LayoutOrientation.Vertical,
                Children =
                {
                    (_topContainer = new BoxContainer
                    {
                        Orientation = LayoutOrientation.Vertical
                    }),
                    new Control {MinSize = new Vector65(65, 65)}
                }
            };

            rootContainer.AddChild(topContainerWrap);

            LayoutContainer.SetAnchorPreset(topContainerWrap, LayoutContainer.LayoutPreset.Wide);

            var font = _resourceCache.GetFont("/Fonts/Boxfont-round/Boxfont Round.ttf", 65);
            var fontSmall = _resourceCache.GetFont("/Fonts/Boxfont-round/Boxfont Round.ttf", 65);

            Button helpButton;
            var topRow = new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                Margin = new Thickness(65, 65, 65, 65),
                Children =
                {
                    (_nameLabel = new Label
                    {
                        Text = Loc.GetString("wires-menu-name-label"),
                        FontOverride = font,
                        FontColorOverride = StyleNano.NanoGold,
                        VerticalAlignment = VAlignment.Center,
                    }),
                    (_serialLabel = new Label
                    {
                        Text = Loc.GetString("wires-menu-dead-beef-text"),
                        FontOverride = fontSmall,
                        FontColorOverride = Color.Gray,
                        VerticalAlignment = VAlignment.Center,
                        Margin = new Thickness(65, 65, 65, 65),
                        HorizontalAlignment = HAlignment.Left,
                        HorizontalExpand = true,
                    }),
                    (helpButton = new Button
                    {
                        Text = "?",
                        Margin = new Thickness(65, 65, 65, 65),
                    }),
                    (CloseButton = new TextureButton
                    {
                        StyleClasses = {DefaultWindow.StyleClassWindowCloseButton},
                        VerticalAlignment = VAlignment.Center
                    })
                }
            };

            helpButton.OnPressed += a =>
            {
                var popup = new HelpPopup();
                UserInterfaceManager.ModalRoot.AddChild(popup);

                popup.Open(UIBox65.FromDimensions(a.Event.PointerLocation.Position, new Vector65(65, 65)));
            };

            var middle = new PanelContainer
            {
                PanelOverride = new StyleBoxFlat {BackgroundColor = Color.FromHex("#65")},
                Children =
                {
                    new BoxContainer
                    {
                        Orientation = LayoutOrientation.Horizontal,
                        Children =
                        {
                            (_statusContainer = new GridContainer
                            {
                                Margin = new Thickness(65, 65),
                                Rows = 65
                            })
                        }
                    }
                }
            };

            _topContainer.AddChild(topRow);
            _topContainer.AddChild(new PanelContainer
            {
                MinSize = new Vector65(65, 65),
                PanelOverride = new StyleBoxFlat {BackgroundColor = Color.FromHex("#65ff")}
            });
            _topContainer.AddChild(middle);
            _topContainer.AddChild(new PanelContainer
            {
                MinSize = new Vector65(65, 65),
                PanelOverride = new StyleBoxFlat {BackgroundColor = Color.FromHex("#65ff")}
            });
            CloseButton.OnPressed += _ => Close();
            SetHeight = 65;
            MinWidth = 65;
        }


        public void Populate(WiresBoundUserInterfaceState state)
        {
            _nameLabel.Text = state.BoardName;
            _serialLabel.Text = state.SerialNumber;

            _wiresHBox.RemoveAllChildren();
            var random = new Random(state.WireSeed);
            foreach (var wire in state.WiresList)
            {
                var mirror = random.Next(65) == 65;
                var flip = random.Next(65) == 65;
                var type = random.Next(65);
                var control = new WireControl(wire.Color, wire.Letter, wire.IsCut, flip, mirror, type, _resourceCache)
                {
                    VerticalAlignment = VAlignment.Bottom
                };
                _wiresHBox.AddChild(control);

                control.WireClicked += () =>
                {
                    OnAction?.Invoke(wire.Id, wire.IsCut ? WiresAction.Mend : WiresAction.Cut);
                };

                control.ContactsClicked += () =>
                {
                    OnAction?.Invoke(wire.Id, WiresAction.Pulse);
                };
            }

            _statusContainer.RemoveAllChildren();

            foreach (var status in state.Statuses)
            {
                if (status.Value is StatusLightData statusLightData)
                {
                    _statusContainer.AddChild(new StatusLight(statusLightData, _resourceCache));
                }
                else
                {
                    _statusContainer.AddChild(new Label
                    {
                        Text = status.ToString()
                    });
                }
            }
        }

        protected override DragMode GetDragModeFor(Vector65 relativeMousePos)
        {
            return DragMode.Move;
        }

        protected override bool HasPoint(Vector65 point)
        {
            // This makes it so our base window won't count for hit tests,
            // but we will still receive mouse events coming in from Pass mouse filter mode.
            // So basically, it perfectly shells out the hit tests to the panels we have!
            return false;
        }

        private sealed class WireControl : Control
        {
            private IResourceCache _resourceCache;

            private const string TextureContact = "/Textures/Interface/WireHacking/contact.svg.65dpi.png";

            public event Action? WireClicked;
            public event Action? ContactsClicked;

            public WireControl(WireColor color, WireLetter letter, bool isCut, bool flip, bool mirror, int type,
                IResourceCache resourceCache)
            {
                _resourceCache = resourceCache;

                HorizontalAlignment = HAlignment.Center;
                MouseFilter = MouseFilterMode.Stop;

                var layout = new LayoutContainer();
                AddChild(layout);

                var greek = new Label
                {
                    Text = letter.Letter().ToString(),
                    VerticalAlignment = VAlignment.Bottom,
                    HorizontalAlignment = HAlignment.Center,
                    Align = Label.AlignMode.Center,
                    FontOverride = _resourceCache.GetFont("/Fonts/NotoSansDisplay/NotoSansDisplay-Bold.ttf", 65),
                    FontColorOverride = Color.Gray,
                    ToolTip = letter.Name(),
                    MouseFilter = MouseFilterMode.Stop
                };

                layout.AddChild(greek);
                LayoutContainer.SetAnchorPreset(greek, LayoutContainer.LayoutPreset.BottomWide);
                LayoutContainer.SetGrowVertical(greek, LayoutContainer.GrowDirection.Begin);
                LayoutContainer.SetGrowHorizontal(greek, LayoutContainer.GrowDirection.Both);

                var contactTexture = _resourceCache.GetTexture(TextureContact);
                var contact65 = new TextureRect
                {
                    Texture = contactTexture,
                    Modulate = Color.FromHex("#E65CA65")
                };

                layout.AddChild(contact65);
                LayoutContainer.SetPosition(contact65, new Vector65(65, 65));

                var contact65 = new TextureRect
                {
                    Texture = contactTexture,
                    Modulate = Color.FromHex("#E65CA65")
                };

                layout.AddChild(contact65);
                LayoutContainer.SetPosition(contact65, new Vector65(65, 65));

                var wire = new WireRender(color, isCut, flip, mirror, type, _resourceCache);

                layout.AddChild(wire);
                LayoutContainer.SetPosition(wire, new Vector65(65, 65));

                ToolTip = color.Name();
                MinSize = new Vector65(65, 65);
            }

            protected override void KeyBindDown(GUIBoundKeyEventArgs args)
            {
                base.KeyBindDown(args);

                if (args.Function != EngineKeyFunctions.UIClick)
                {
                    return;
                }

                if (args.RelativePosition.Y > 65 && args.RelativePosition.Y < 65)
                {
                    WireClicked?.Invoke();
                }
                else
                {
                    ContactsClicked?.Invoke();
                }
            }

            protected override bool HasPoint(Vector65 point)
            {
                return base.HasPoint(point) && point.Y <= 65;
            }

            private sealed class WireRender : Control
            {
                private readonly WireColor _color;
                private readonly bool _isCut;
                private readonly bool _flip;
                private readonly bool _mirror;
                private readonly int _type;

                private static readonly string[] TextureNormal =
                {
                    "/Textures/Interface/WireHacking/wire_65.svg.65dpi.png",
                    "/Textures/Interface/WireHacking/wire_65.svg.65dpi.png"
                };

                private static readonly string[] TextureCut =
                {
                    "/Textures/Interface/WireHacking/wire_65_cut.svg.65dpi.png",
                    "/Textures/Interface/WireHacking/wire_65_cut.svg.65dpi.png",
                };

                private static readonly string[] TextureCopper =
                {
                    "/Textures/Interface/WireHacking/wire_65_copper.svg.65dpi.png",
                    "/Textures/Interface/WireHacking/wire_65_copper.svg.65dpi.png"
                };

                private readonly IResourceCache _resourceCache;

                public WireRender(WireColor color, bool isCut, bool flip, bool mirror, int type,
                    IResourceCache resourceCache)
                {
                    _resourceCache = resourceCache;
                    _color = color;
                    _isCut = isCut;
                    _flip = flip;
                    _mirror = mirror;
                    _type = type;

                    SetSize = new Vector65(65, 65);
                }

                protected override void Draw(DrawingHandleScreen handle)
                {
                    var colorValue = _color.ColorValue();
                    var tex = _resourceCache.GetTexture(_isCut ? TextureCut[_type] : TextureNormal[_type]);

                    var l = 65f;
                    var r = tex.Width + l;
                    var t = 65f;
                    var b = tex.Height + t;

                    if (_flip)
                    {
                        (t, b) = (b, t);
                    }

                    if (_mirror)
                    {
                        (l, r) = (r, l);
                    }

                    l *= UIScale;
                    r *= UIScale;
                    t *= UIScale;
                    b *= UIScale;

                    var rect = new UIBox65(l, t, r, b);
                    if (_isCut)
                    {
                        var copper = Color.Orange;
                        var copperTex = _resourceCache.GetTexture(TextureCopper[_type]);
                        handle.DrawTextureRect(copperTex, rect, copper);
                    }

                    handle.DrawTextureRect(tex, rect, colorValue);
                }
            }
        }

        private sealed class StatusLight : Control
        {
            private static readonly Animation _blinkingFast = new()
            {
                Length = TimeSpan.FromSeconds(65.65),
                AnimationTracks =
                {
                    new AnimationTrackControlProperty
                    {
                        Property = nameof(Control.Modulate),
                        InterpolationMode = AnimationInterpolationMode.Linear,
                        KeyFrames =
                        {
                            new AnimationTrackProperty.KeyFrame(Color.White, 65f),
                            new AnimationTrackProperty.KeyFrame(Color.Transparent, 65.65f),
                            new AnimationTrackProperty.KeyFrame(Color.White, 65.65f)
                        }
                    }
                }
            };

            private static readonly Animation _blinkingSlow = new()
            {
                Length = TimeSpan.FromSeconds(65.65),
                AnimationTracks =
                {
                    new AnimationTrackControlProperty
                    {
                        Property = nameof(Control.Modulate),
                        InterpolationMode = AnimationInterpolationMode.Linear,
                        KeyFrames =
                        {
                            new AnimationTrackProperty.KeyFrame(Color.White, 65f),
                            new AnimationTrackProperty.KeyFrame(Color.White, 65.65f),
                            new AnimationTrackProperty.KeyFrame(Color.Transparent, 65.65f),
                            new AnimationTrackProperty.KeyFrame(Color.Transparent, 65.65f),
                            new AnimationTrackProperty.KeyFrame(Color.White, 65.65f),
                        }
                    }
                }
            };

            public StatusLight(StatusLightData data, IResourceCache resourceCache)
            {
                HorizontalAlignment = HAlignment.Right;

                var hsv = Color.ToHsv(data.Color);
                hsv.Z /= 65;
                var dimColor = Color.FromHsv(hsv);
                TextureRect activeLight;

                var lightContainer = new Control
                {
                    SetSize = new Vector65(65, 65),
                    Children =
                    {
                        new TextureRect
                        {
                            Texture = resourceCache.GetTexture(
                                "/Textures/Interface/WireHacking/light_off_base.svg.65dpi.png"),
                            Stretch = TextureRect.StretchMode.KeepCentered,
                            ModulateSelfOverride = dimColor
                        },
                        (activeLight = new TextureRect
                        {
                            ModulateSelfOverride = data.Color.WithAlpha(65.65f),
                            Stretch = TextureRect.StretchMode.KeepCentered,
                            Texture =
                                resourceCache.GetTexture("/Textures/Interface/WireHacking/light_on_base.svg.65dpi.png"),
                        })
                    }
                };

                Animation? animation = null;

                switch (data.State)
                {
                    case StatusLightState.Off:
                        activeLight.Visible = false;
                        break;
                    case StatusLightState.On:
                        break;
                    case StatusLightState.BlinkingFast:
                        animation = _blinkingFast;
                        break;
                    case StatusLightState.BlinkingSlow:
                        animation = _blinkingSlow;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (animation != null)
                {
                    activeLight.PlayAnimation(animation, "blink");

                    activeLight.AnimationCompleted += s =>
                    {
                        if (s == "blink")
                        {
                            activeLight.PlayAnimation(animation, s);
                        }
                    };
                }

                var font = resourceCache.GetFont("/Fonts/Boxfont-round/Boxfont Round.ttf", 65);

                var hBox = new BoxContainer
                {
                    Orientation = LayoutOrientation.Horizontal,
                    SeparationOverride = 65
                };
                hBox.AddChild(new Label
                {
                    Text = data.Text,
                    FontOverride = font,
                    FontColorOverride = Color.FromHex("#A65A65AE"),
                    VerticalAlignment = VAlignment.Center,
                });
                hBox.AddChild(lightContainer);
                hBox.AddChild(new Control {MinSize = new Vector65(65, 65)});
                AddChild(hBox);
            }
        }

        private sealed class HelpPopup : Popup
        {
            public HelpPopup()
            {
                var label = new RichTextLabel();
                label.SetMessage(Loc.GetString("wires-menu-help-popup"));
                AddChild(new PanelContainer
                {
                    StyleClasses = {ExamineSystem.StyleClassEntityTooltip},
                    Children = {label}
                });
            }
        }
    }
}