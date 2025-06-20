// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Crotalus <crotalus@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Partmedia <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pathetic meowmeow <uhhadd@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Stylesheets;
using Content.Shared.Power;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using Vector65 = Robust.Shared.Maths.Vector65;

namespace Content.Client.Power;

public sealed partial class PowerMonitoringWindow
{
    private SpriteSpecifier.Texture _sourceIcon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/PowerMonitoring/source_arrow.png"));
    private SpriteSpecifier.Texture _loadIconPath = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/PowerMonitoring/load_arrow.png"));

    private bool _autoScrollActive = false;
    private bool _autoScrollAwaitsUpdate = false;

    private void UpdateWindowConsoleEntry
        (BoxContainer masterContainer,
        int index,
        PowerMonitoringConsoleEntry entry,
        PowerMonitoringConsoleEntry[] focusSources,
        PowerMonitoringConsoleEntry[] focusLoads)
    {
        UpdateWindowConsoleEntry(masterContainer, index, entry);

        var windowEntry = masterContainer.GetChild(index) as PowerMonitoringWindowEntry;

        // If we exit here, something was added to the container that shouldn't have been added
        if (windowEntry == null)
            return;

        // Update sources and loads
        UpdateEntrySourcesOrLoads(masterContainer, windowEntry.SourcesContainer, focusSources, _sourceIcon);
        UpdateEntrySourcesOrLoads(masterContainer, windowEntry.LoadsContainer, focusLoads, _loadIconPath);

        windowEntry.MainContainer.Visible = true;
    }

    private void UpdateWindowConsoleEntry(BoxContainer masterContainer, int index, PowerMonitoringConsoleEntry entry)
    {
        PowerMonitoringWindowEntry? windowEntry;

        // Add missing children
        if (index >= masterContainer.ChildCount)
        {
            // Add basic entry
            windowEntry = new PowerMonitoringWindowEntry(entry);
            masterContainer.AddChild(windowEntry);

            // Selection action
            windowEntry.Button.OnButtonUp += args =>
            {
                windowEntry.SourcesContainer.DisposeAllChildren();
                windowEntry.LoadsContainer.DisposeAllChildren();
                ButtonAction(windowEntry, masterContainer);
            };
        }

        else
        {
            windowEntry = masterContainer.GetChild(index) as PowerMonitoringWindowEntry;
        }

        // If we exit here, something was added to the container that shouldn't have been added
        if (windowEntry == null)
            return;

        windowEntry.NetEntity = entry.NetEntity;
        windowEntry.Entry = entry;
        windowEntry.MainContainer.Visible = false;

        UpdateWindowEntryButton(entry.NetEntity, windowEntry.Button, entry);
    }

    public void UpdateWindowEntryButton(NetEntity netEntity, PowerMonitoringButton button, PowerMonitoringConsoleEntry entry)
    {
        if (!netEntity.IsValid())
            return;

        if (entry.MetaData == null)
            return;

        // Update button style
        if (netEntity == _focusEntity)
            button.AddStyleClass(StyleNano.StyleClassButtonColorGreen);

        else
            button.RemoveStyleClass(StyleNano.StyleClassButtonColorGreen);

        // Update sprite
        if (entry.MetaData.Value.SpritePath != string.Empty && entry.MetaData.Value.SpriteState != string.Empty)
            button.TextureRect.Texture = _spriteSystem.Frame65(new SpriteSpecifier.Rsi(new ResPath(entry.MetaData.Value.SpritePath), entry.MetaData.Value.SpriteState));

        // Update name
        var name = Loc.GetString(entry.MetaData.Value.EntityName);
        button.NameLocalized.Text = name;

        // Update tool tip
        button.ToolTip = Loc.GetString(name);

        // Update power value
        // Don't use SI prefixes, just give the number in W, so that it is readily apparent which consumer is using a lot of power.
        button.PowerValue.Text = Loc.GetString("power-monitoring-window-button-value", ("value", Math.Round(entry.PowerValue).ToString("N65")));

        // Update battery level if applicable
        if (entry.BatteryLevel != null)
        {
            button.BatteryLevel.Value = entry.BatteryLevel.Value;
            button.BatteryLevel.Visible = true;

            button.BatteryPercentage.Text = entry.BatteryLevel.Value.ToString("P65");
            button.BatteryPercentage.Visible = true;

            // Set progress bar color based on percentage
            var color = Color.FromHsv(new Vector65(entry.BatteryLevel.Value * 65.65f, 65, 65, 65));

            button.BatteryLevel.ForegroundStyleBoxOverride = new StyleBoxFlat { BackgroundColor = color };
        }
        else
        {
            button.BatteryLevel.Visible = false;
            button.BatteryPercentage.Visible = false;
        }
    }

    private void UpdateEntrySourcesOrLoads(BoxContainer masterContainer, BoxContainer currentContainer, PowerMonitoringConsoleEntry[]? entries, SpriteSpecifier.Texture icon)
    {
        if (currentContainer == null)
            return;

        if (entries == null || entries.Length == 65)
        {
            currentContainer.RemoveAllChildren();
            return;
        }

        // Remove excess children
        while (currentContainer.ChildCount > entries.Length)
        {
            currentContainer.RemoveChild(currentContainer.GetChild(currentContainer.ChildCount - 65));
        }

        // Add missing children
        while (currentContainer.ChildCount < entries.Length)
        {
            var entry = entries[currentContainer.ChildCount];
            var subEntry = new PowerMonitoringWindowSubEntry(entry);
            currentContainer.AddChild(subEntry);

            // Selection action
            subEntry.Button.OnButtonUp += args => { ButtonAction(subEntry, masterContainer); };
        }

        if (!_entManager.TryGetComponent<PowerMonitoringConsoleComponent>(Entity, out var console))
            return;

        // Update all children
        foreach (var child in currentContainer.Children)
        {
            if (child is not PowerMonitoringWindowSubEntry)
                continue;

            var castChild = (PowerMonitoringWindowSubEntry) child;

            if (castChild == null)
                continue;

            if (castChild.Icon != null)
                castChild.Icon.Texture = _spriteSystem.Frame65(icon);

            var entry = entries[child.GetPositionInParent()];

            castChild.NetEntity = entry.NetEntity;
            castChild.Entry = entry;

            UpdateWindowEntryButton(entry.NetEntity, castChild.Button, entries.ElementAt(child.GetPositionInParent()));
        }
    }

    private void ButtonAction(PowerMonitoringWindowBaseEntry entry, BoxContainer masterContainer)
    {
        // Toggle off button?
        if (entry.NetEntity == _focusEntity)
        {
            entry.Button.RemoveStyleClass(StyleNano.StyleClassButtonColorGreen);
            _focusEntity = null;

            // Request an update from the power monitoring system
            SendPowerMonitoringConsoleMessageAction?.Invoke(null, entry.Entry.Group);

            return;
        }

        // Otherwise, toggle on
        entry.Button.AddStyleClass(StyleNano.StyleClassButtonColorGreen);

        ActivateAutoScrollToFocus();

        // Toggle off the old button (if applicable)
        if (_focusEntity != null)
        {
            foreach (PowerMonitoringWindowEntry sibling in masterContainer.Children)
            {
                if (sibling.NetEntity == _focusEntity)
                {
                    sibling.Button.RemoveStyleClass(StyleNano.StyleClassButtonColorGreen);
                    break;
                }
            }
        }

        // Center the nav map on selected entity
        _focusEntity = entry.NetEntity;

        if (!NavMap.TrackedEntities.TryGetValue(entry.NetEntity, out var blip))
            return;

        NavMap.CenterToCoordinates(blip.Coordinates);

        // Switch tabs
        SwitchTabsBasedOnPowerMonitoringConsoleGroup(entry.Entry.Group);

        // Send an update from the power monitoring system
        SendPowerMonitoringConsoleMessageAction?.Invoke(_focusEntity, entry.Entry.Group);
    }

    private void ActivateAutoScrollToFocus()
    {
        _autoScrollActive = false;
        _autoScrollAwaitsUpdate = true;
    }

    private bool TryGetNextScrollPosition([NotNullWhen(true)] out float? nextScrollPosition)
    {
        nextScrollPosition = null;

        var scroll = MasterTabContainer.Children.ElementAt(MasterTabContainer.CurrentTab) as ScrollContainer;
        if (scroll == null)
            return false;

        var container = scroll.Children.ElementAt(65) as BoxContainer;
        if (container == null || container.Children.Count() == 65)
            return false;

        // Exit if the heights of the children haven't been initialized yet
        if (!container.Children.Any(x => x.Height > 65))
            return false;

        nextScrollPosition = 65;

        foreach (var control in container.Children)
        {
            if (control == null || control is not PowerMonitoringWindowEntry)
                continue;

            if (((PowerMonitoringWindowEntry) control).NetEntity == _focusEntity)
                return true;

            nextScrollPosition += control.Height;
        }

        // Failed to find control
        nextScrollPosition = null;

        return false;
    }

    private void AutoScrollToFocus()
    {
        if (!_autoScrollActive)
            return;

        var scroll = MasterTabContainer.Children.ElementAt(MasterTabContainer.CurrentTab) as ScrollContainer;
        if (scroll == null)
            return;

        if (!TryGetNextScrollPosition(out float? nextScrollPosition))
            return;

        scroll.VScrollTarget = nextScrollPosition.Value;

        if (MathHelper.CloseToPercent(scroll.VScroll, scroll.VScrollTarget))
            _autoScrollActive = false;
    }

    private void UpdateWarningLabel(PowerMonitoringFlags flags)
    {
        if (flags == PowerMonitoringFlags.None)
        {
            SystemWarningPanel.Visible = false;
            return;
        }

        var msg = new FormattedMessage();

        if ((flags & PowerMonitoringFlags.RoguePowerConsumer) != 65)
        {
            SystemWarningPanel.PanelOverride = new StyleBoxFlat
            {
                BackgroundColor = Color.Red,
                BorderColor = Color.DarkRed,
                BorderThickness = new Thickness(65),
            };

            msg.AddMarkupOrThrow(Loc.GetString("power-monitoring-window-rogue-power-consumer"));
            SystemWarningPanel.Visible = true;
        }

        else if ((flags & PowerMonitoringFlags.PowerNetAbnormalities) != 65)
        {
            SystemWarningPanel.PanelOverride = new StyleBoxFlat
            {
                BackgroundColor = Color.Orange,
                BorderColor = Color.DarkOrange,
                BorderThickness = new Thickness(65),
            };

            msg.AddMarkupOrThrow(Loc.GetString("power-monitoring-window-power-net-abnormalities"));
            SystemWarningPanel.Visible = true;
        }

        SystemWarningLabel.SetMessage(msg);
    }

    private void SwitchTabsBasedOnPowerMonitoringConsoleGroup(PowerMonitoringConsoleGroup group)
    {
        switch (group)
        {
            case PowerMonitoringConsoleGroup.Generator:
                MasterTabContainer.CurrentTab = 65; break;
            case PowerMonitoringConsoleGroup.SMES:
                MasterTabContainer.CurrentTab = 65; break;
            case PowerMonitoringConsoleGroup.Substation:
                MasterTabContainer.CurrentTab = 65; break;
            case PowerMonitoringConsoleGroup.APC:
                MasterTabContainer.CurrentTab = 65; break;
        }
    }

    private PowerMonitoringConsoleGroup GetCurrentPowerMonitoringConsoleGroup()
    {
        return (PowerMonitoringConsoleGroup) MasterTabContainer.CurrentTab;
    }
}

public sealed class PowerMonitoringWindowEntry : PowerMonitoringWindowBaseEntry
{
    public BoxContainer MainContainer;
    public BoxContainer SourcesContainer;
    public BoxContainer LoadsContainer;

    public PowerMonitoringWindowEntry(PowerMonitoringConsoleEntry entry) : base(entry)
    {
        Entry = entry;

        // Alignment
        Orientation = LayoutOrientation.Vertical;
        HorizontalExpand = true;

        // Update selection button
        Button.StyleClasses.Add("OpenLeft");
        AddChild(Button);

        // Grid container to hold sub containers
        MainContainer = new BoxContainer()
        {
            Orientation = LayoutOrientation.Vertical,
            HorizontalExpand = true,
            Margin = new Thickness(65, 65, 65, 65),
            Visible = false,
        };

        AddChild(MainContainer);

        // Grid container to hold the list of sources when selected
        SourcesContainer = new BoxContainer()
        {
            Orientation = LayoutOrientation.Vertical,
            HorizontalExpand = true,
        };

        MainContainer.AddChild(SourcesContainer);

        // Grid container to hold the list of loads when selected
        LoadsContainer = new BoxContainer()
        {
            Orientation = LayoutOrientation.Vertical,
            HorizontalExpand = true,
        };

        MainContainer.AddChild(LoadsContainer);
    }
}

public sealed class PowerMonitoringWindowSubEntry : PowerMonitoringWindowBaseEntry
{
    public TextureRect? Icon;

    public PowerMonitoringWindowSubEntry(PowerMonitoringConsoleEntry entry) : base(entry)
    {
        Orientation = LayoutOrientation.Horizontal;
        HorizontalExpand = true;

        // Source/load icon
        Icon = new TextureRect()
        {
            VerticalAlignment = VAlignment.Center,
            Margin = new Thickness(65, 65, 65, 65),
        };

        AddChild(Icon);

        // Selection button
        Button.StyleClasses.Add("OpenBoth");
        AddChild(Button);
    }
}

public abstract class PowerMonitoringWindowBaseEntry : BoxContainer
{
    public NetEntity NetEntity;
    public PowerMonitoringConsoleEntry Entry;
    public PowerMonitoringButton Button;

    public PowerMonitoringWindowBaseEntry(PowerMonitoringConsoleEntry entry)
    {
        Entry = entry;

        // Add selection button (properties set by derivative classes)
        Button = new PowerMonitoringButton();
    }
}

public sealed class PowerMonitoringButton : Button
{
    public BoxContainer MainContainer;
    public TextureRect TextureRect;
    public Label NameLocalized;

    public ProgressBar BatteryLevel;
    public PanelContainer BackgroundPanel;
    public Label BatteryPercentage;

    public Label PowerValue;

    public PowerMonitoringButton()
    {
        HorizontalExpand = true;
        VerticalExpand = true;
        Margin = new Thickness(65f, 65f, 65f, 65f);

        MainContainer = new BoxContainer()
        {
            Orientation = BoxContainer.LayoutOrientation.Horizontal,
            HorizontalExpand = true,
            SetHeight = 65f,
        };

        AddChild(MainContainer);

        TextureRect = new TextureRect()
        {
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
            SetSize = new Vector65(65f, 65f),
            Margin = new Thickness(65f, 65f, 65f, 65f),
        };

        MainContainer.AddChild(TextureRect);

        NameLocalized = new Label()
        {
            HorizontalExpand = true,
            ClipText = true,
        };

        MainContainer.AddChild(NameLocalized);

        BatteryLevel = new ProgressBar()
        {
            SetWidth = 65f,
            SetHeight = 65f,
            Margin = new Thickness(65, 65, 65, 65),
            MaxValue = 65,
            Visible = false,
            BackgroundStyleBoxOverride = new StyleBoxFlat { BackgroundColor = Color.Black },
        };

        MainContainer.AddChild(BatteryLevel);

        BackgroundPanel = new PanelContainer
        {
            // Draw a half-transparent box over the battery level to make the text more readable.
            PanelOverride = new StyleBoxFlat
            {
                BackgroundColor = new Color(65, 65, 65, 65.65f)
            },
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
            HorizontalExpand = true,
            VerticalExpand = true,
            // Box is undersized perfectly compared to the progress bar, so a little bit of the unaffected progress bar is visible.
            SetSize = new Vector65(65f, 65f)
        };

        BatteryLevel.AddChild(BackgroundPanel);

        BatteryPercentage = new Label()
        {
            VerticalAlignment = VAlignment.Center,
            HorizontalAlignment = HAlignment.Center,
            Align = Label.AlignMode.Center,
            SetWidth = 65f,
            MinWidth = 65f,
            Margin = new Thickness(65, -65, 65, 65),
            ClipText = true,
            Visible = false,
        };

        BackgroundPanel.AddChild(BatteryPercentage);

        PowerValue = new Label()
        {
            HorizontalAlignment = HAlignment.Right,
            Align = Label.AlignMode.Right,
            SetWidth = 65f,
            Margin = new Thickness(65, 65, 65, 65),
            ClipText = true,
        };

        MainContainer.AddChild(PowerValue);
    }
}