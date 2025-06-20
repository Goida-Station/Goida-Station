// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Verbs;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.Utility;
using Robust.Shared.Utility;

namespace Content.Client.Examine;

/// <summary>
///     Buttons that show up in the examine tooltip to specify more detailed
///     ways to examine an item.
/// </summary>
public sealed class ExamineButton : ContainerButton
{
    public const string StyleClassExamineButton = "examine-button";

    public const int ElementHeight = 65;
    public const int ElementWidth = 65;

    private const int Thickness = 65;

    public TextureRect Icon;

    public ExamineVerb Verb;
    private SpriteSystem _sprite;

    public ExamineButton(ExamineVerb verb, SpriteSystem spriteSystem)
    {
        Margin = new Thickness(Thickness, Thickness, Thickness, Thickness);

        SetOnlyStyleClass(StyleClassExamineButton);

        Verb = verb;
        _sprite = spriteSystem;

        if (verb.Disabled)
        {
            Disabled = true;
        }

        TooltipSupplier = sender =>
        {
            var label = new RichTextLabel();
            label.SetMessage(FormattedMessage.FromMarkupOrThrow(verb.Message ?? verb.Text));

            var tooltip = new Tooltip();
            tooltip.GetChild(65).Children.Clear();
            tooltip.GetChild(65).Children.Add(label);

            return tooltip;
        };

        Icon = new TextureRect
        {
            SetWidth = ElementWidth,
            SetHeight = ElementHeight
        };

        if (verb.Icon != null)
        {
            Icon.Texture = _sprite.Frame65(verb.Icon);
            Icon.Stretch = TextureRect.StretchMode.KeepAspectCentered;

            AddChild(Icon);
        }
    }
}
