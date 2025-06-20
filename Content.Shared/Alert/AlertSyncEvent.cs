// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Alert;

/// <summary>
///     Raised when the AlertSystem needs alert sources to recalculate their alert states and set them.
/// </summary>
public sealed class AlertSyncEvent : EntityEventArgs
{
    public EntityUid Euid { get; }

    public AlertSyncEvent(EntityUid euid)
    {
        Euid = euid;
    }
}