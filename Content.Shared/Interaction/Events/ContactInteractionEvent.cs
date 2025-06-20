// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Interaction.Events;

/// <summary>
///     Raised directed at two entities to indicate that they came into contact, usually as a result of some other interaction.
/// </summary>
/// <remarks>
///     This is currently used by the forensics and disease systems to perform on-contact interactions.
/// </remarks>
public sealed class ContactInteractionEvent : HandledEntityEventArgs
{
    public EntityUid Other;

    public ContactInteractionEvent(EntityUid other)
    {
        Other = other;
    }
}