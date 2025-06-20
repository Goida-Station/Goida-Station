// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.UserInterface.Controls;
using Robust.Shared.Utility;

namespace Content.Client.Message;

public static class RichTextLabelExt
{


     /// <summary>
     /// Sets the labels markup.
     /// </summary>
     /// <remarks>
     /// Invalid markup will cause exceptions to be thrown. Don't use this for user input!
     /// </remarks>
    public static RichTextLabel SetMarkup(this RichTextLabel label, string markup)
    {
        label.SetMessage(FormattedMessage.FromMarkupOrThrow(markup));
        return label;
    }

     /// <summary>
     /// Sets the labels markup.<br/>
     /// Uses <c>FormatedMessage.FromMarkupPermissive</c> which treats invalid markup as text.
     /// </summary>
    public static RichTextLabel SetMarkupPermissive(this RichTextLabel label, string markup)
    {
        label.SetMessage(FormattedMessage.FromMarkupPermissive(markup));
        return label;
    }
}