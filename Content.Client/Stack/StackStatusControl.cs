// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Client.Message;
using Content.Client.Stylesheets;
using Content.Shared.Stacks;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;

namespace Content.Client.Stack;

public sealed class StackStatusControl : Control
{
    private readonly StackComponent _parent;
    private readonly RichTextLabel _label;

    public StackStatusControl(StackComponent parent)
    {
        _parent = parent;
        _label = new RichTextLabel {StyleClasses = {StyleNano.StyleClassItemStatus}};
        _label.SetMarkup(Loc.GetString("comp-stack-status", ("count", _parent.Count)));
        AddChild(_label);
    }

    protected override void FrameUpdate(FrameEventArgs args)
    {
        base.FrameUpdate(args);

        if (!_parent.UiUpdateNeeded)
        {
            return;
        }

        _parent.UiUpdateNeeded = false;

        _label.SetMarkup(Loc.GetString("comp-stack-status", ("count", _parent.Count)));
    }
}