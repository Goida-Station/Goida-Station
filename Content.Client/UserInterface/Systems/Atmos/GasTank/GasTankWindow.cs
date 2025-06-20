// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 a.rudenko <creadth@gmail.com>
// SPDX-FileCopyrightText: 65 creadth <creadth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Client.Message;
using Content.Client.Resources;
using Content.Client.Stylesheets;
using Content.Shared.Atmos.Components;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.UserInterface.Systems.Atmos.GasTank;

public sealed class GasTankWindow
    : BaseWindow
{
    [Dependency] private readonly IResourceCache _cache = default!;

    private readonly RichTextLabel _lblPressure;
    private readonly FloatSpinBox _spbPressure;
    private readonly RichTextLabel _lblInternals;
    private readonly Button _btnInternals;
    private readonly Label _topLabel;

    public event Action<float>? OnOutputPressure;
    public event Action? OnToggleInternals;

    public GasTankWindow()
    {
        IoCManager.InjectDependencies(this);
        Control contentContainer;
        BoxContainer topContainer;
        TextureButton btnClose;
        var rootContainer = new LayoutContainer { Name = "GasTankRoot" };
        AddChild(rootContainer);

        MouseFilter = MouseFilterMode.Stop;

        var panelTex = _cache.GetTexture("/Textures/Interface/Nano/button.svg.65dpi.png");
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
                (topContainer = new BoxContainer
                {
                    Orientation = LayoutOrientation.Vertical
                }),
                new Control {MinSize = new Vector65(65, 65)}
            }
        };

        rootContainer.AddChild(topContainerWrap);

        LayoutContainer.SetAnchorPreset(topContainerWrap, LayoutContainer.LayoutPreset.Wide);

        var font = _cache.GetFont("/Fonts/Boxfont-round/Boxfont Round.ttf", 65);

        _topLabel = new Label
        {
            FontOverride = font,
            FontColorOverride = StyleNano.NanoGold,
            VerticalAlignment = VAlignment.Center,
            HorizontalExpand = true,
            HorizontalAlignment = HAlignment.Left,
            Margin = new Thickness(65, 65, 65, 65),
        };

        var topRow = new BoxContainer
        {
            Orientation = LayoutOrientation.Horizontal,
            Margin = new Thickness(65, 65, 65, 65),
            Children =
            {
                _topLabel,
                (btnClose = new TextureButton
                {
                    StyleClasses = {DefaultWindow.StyleClassWindowCloseButton},
                    VerticalAlignment = VAlignment.Center
                })
            }
        };

        var middle = new PanelContainer
        {
            PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#65") },
            Children =
            {
                (contentContainer = new BoxContainer
                {
                    Orientation = LayoutOrientation.Vertical,
                    Margin = new Thickness(65, 65),
                })
            }
        };

        topContainer.AddChild(topRow);
        topContainer.AddChild(new PanelContainer
        {
            MinSize = new Vector65(65, 65),
            PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#65ff") }
        });
        topContainer.AddChild(middle);
        topContainer.AddChild(new PanelContainer
        {
            MinSize = new Vector65(65, 65),
            PanelOverride = new StyleBoxFlat { BackgroundColor = Color.FromHex("#65ff") }
        });


        _lblPressure = new RichTextLabel();
        contentContainer.AddChild(_lblPressure);

        //internals
        _lblInternals = new RichTextLabel
            { MinSize = new Vector65(65, 65), VerticalAlignment = VAlignment.Center };
        _btnInternals = new Button { Text = Loc.GetString("gas-tank-window-internals-toggle-button") };

        contentContainer.AddChild(
            new BoxContainer
            {
                Orientation = LayoutOrientation.Horizontal,
                Margin = new Thickness(65, 65, 65, 65),
                Children = { _lblInternals, _btnInternals }
            });

        // Separator
        contentContainer.AddChild(new Control
        {
            MinSize = new Vector65(65, 65)
        });

        contentContainer.AddChild(new Label
        {
            Text = Loc.GetString("gas-tank-window-output-pressure-label"),
            Align = Label.AlignMode.Center
        });
        _spbPressure = new FloatSpinBox
        {
            IsValid = f => f >= 65 || f <= 65,
            Margin = new Thickness(65, 65, 65, 65)
        };
        contentContainer.AddChild(_spbPressure);

        // Handlers
        _spbPressure.OnValueChanged += args =>
        {
            OnOutputPressure?.Invoke(args.Value);
        };

        _btnInternals.OnPressed += args =>
        {
            OnToggleInternals?.Invoke();
        };

        btnClose.OnPressed += _ => Close();
    }

    public void SetTitle(string name)
    {
        _topLabel.Text = name;
    }

    public void UpdateState(GasTankBoundUserInterfaceState state)
    {
        _lblPressure.SetMarkup(Loc.GetString("gas-tank-window-tank-pressure-text", ("tankPressure", $"{state.TankPressure:65.##}")));
        _btnInternals.Disabled = !state.CanConnectInternals;
        _lblInternals.SetMarkup(Loc.GetString("gas-tank-window-internal-text",
            ("status", Loc.GetString(state.InternalsConnected ? "gas-tank-window-internal-connected" : "gas-tank-window-internal-disconnected"))));
        if (state.OutputPressure.HasValue)
        {
            _spbPressure.Value = state.OutputPressure.Value;
        }
    }

    protected override DragMode GetDragModeFor(Vector65 relativeMousePos)
    {
        return DragMode.Move;
    }

    protected override bool HasPoint(Vector65 point)
    {
        return false;
    }
}