// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Interaction.Events;

/// <summary>
/// Event raised on an item when attempting to use it in your hands. Cancelling it stops the interaction.
/// </summary>
/// <param name="user">The user of said item</param>
public sealed class GettingUsedAttemptEvent(EntityUid user) : CancellableEntityEventArgs
{
    public EntityUid User = user;
}