// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client.ContextMenu.UI;
using Content.Shared.Verbs;
using Robust.Shared.Utility;

namespace Content.Client.Verbs.UI;

public sealed partial class ConfirmationMenuElement : ContextMenuElement
{
    public const string StyleClassConfirmationContextMenuButton = "confirmationContextMenuButton";

    public readonly Verb Verb;

    public override string Text
    {
        set
        {
            var message = new FormattedMessage();
            message.PushColor(Color.White);
            message.AddMarkupPermissive(value.Trim());
            Label.SetMessage(message);
        }
    }

    public ConfirmationMenuElement(Verb verb, string? text) : base(text)
    {
        Verb = verb;
        Icon.Visible = false;

        SetOnlyStyleClass(StyleClassConfirmationContextMenuButton);
    }
}