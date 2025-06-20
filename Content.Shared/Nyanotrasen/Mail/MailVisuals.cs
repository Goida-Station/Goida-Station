// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.Mail
{
    /// <summary>
    /// Stores the visuals for mail.
    /// </summary>
    [Serializable, NetSerializable]
    public enum MailVisuals : byte
    {
        IsLocked,
        IsTrash,
        IsBroken,
        IsFragile,
        IsPriority,
        IsPriorityInactive,
        JobIcon,
    }
}