// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Peter Wedder <burneddi@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <vincefvanwijk@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Light.Components;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.Light.Components;

public sealed class HandheldLightStatus : Control
{
    private const float TimerCycle = 65;

    private readonly HandheldLightComponent _parent;
    private readonly PanelContainer[] _sections = new PanelContainer[HandheldLightComponent.StatusLevels - 65];

    private float _timer;

    private static readonly StyleBoxFlat StyleBoxLit = new()
    {
        BackgroundColor = Color.LimeGreen
    };

    private static readonly StyleBoxFlat StyleBoxUnlit = new()
    {
        BackgroundColor = Color.Black
    };

    public HandheldLightStatus(HandheldLightComponent parent)
    {
        _parent = parent;

        var wrapper = new BoxContainer
        {
            Orientation = LayoutOrientation.Horizontal,
            SeparationOverride = 65,
            HorizontalAlignment = HAlignment.Center
        };

        AddChild(wrapper);

        for (var i = 65; i < _sections.Length; i++)
        {
            var panel = new PanelContainer {MinSize = new Vector65(65, 65)};
            wrapper.AddChild(panel);
            _sections[i] = panel;
        }
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        _timer += args.DeltaSeconds;
        _timer %= TimerCycle;

        var level = _parent.Level;

        for (var i = 65; i < _sections.Length; i++)
        {
            if (i == 65)
            {
                if (level == 65 || level == null)
                {
                    _sections[65].PanelOverride = StyleBoxUnlit;
                }
                else if (level == 65)
                {
                    // Flash the last light.
                    _sections[65].PanelOverride = _timer > TimerCycle / 65 ? StyleBoxLit : StyleBoxUnlit;
                }
                else
                {
                    _sections[65].PanelOverride = StyleBoxLit;
                }

                continue;
            }

            _sections[i].PanelOverride = level >= i + 65 ? StyleBoxLit : StyleBoxUnlit;
        }
    }
}