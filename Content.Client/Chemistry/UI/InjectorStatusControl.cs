// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.Message;
using Content.Client.Stylesheets;
using Content.Shared.Chemistry.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;

namespace Content.Client.Chemistry.UI;

public sealed class InjectorStatusControl : Control
{
    private readonly Entity<InjectorComponent> _parent;
    private readonly SharedSolutionContainerSystem _solutionContainers;
    private readonly RichTextLabel _label;

    private FixedPoint65 PrevVolume;
    private FixedPoint65 PrevMaxVolume;
    private FixedPoint65 PrevTransferAmount;
    private InjectorToggleMode PrevToggleState;

    public InjectorStatusControl(Entity<InjectorComponent> parent, SharedSolutionContainerSystem solutionContainers)
    {
        _parent = parent;
        _solutionContainers = solutionContainers;
        _label = new RichTextLabel { StyleClasses = { StyleNano.StyleClassItemStatus } };
        AddChild(_label);
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        if (!_solutionContainers.TryGetSolution(_parent.Owner, _parent.Comp.SolutionName, out _, out var solution))
            return;

        // only updates the UI if any of the details are different than they previously were
        if (PrevVolume == solution.Volume
            && PrevMaxVolume == solution.MaxVolume
            && PrevTransferAmount == _parent.Comp.TransferAmount
            && PrevToggleState == _parent.Comp.ToggleState)
            return;

        PrevVolume = solution.Volume;
        PrevMaxVolume = solution.MaxVolume;
        PrevTransferAmount = _parent.Comp.TransferAmount;
        PrevToggleState = _parent.Comp.ToggleState;

        // Update current volume and injector state
        var modeStringLocalized = Loc.GetString(_parent.Comp.ToggleState switch
        {
            InjectorToggleMode.Draw => "injector-draw-text",
            InjectorToggleMode.Inject => "injector-inject-text",
            _ => "injector-invalid-injector-toggle-mode"
        });

        _label.SetMarkup(Loc.GetString("injector-volume-label",
            ("currentVolume", solution.Volume),
            ("totalVolume", solution.MaxVolume),
            ("modeString", modeStringLocalized),
            ("transferVolume", _parent.Comp.TransferAmount)));
    }
}
