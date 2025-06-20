// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Movement.Events;

/// <summary>
///     Raised whenever <see cref="IMoverComponent.CanMove"/> needs to be updated. Cancel this event to prevent a
///     mover from moving.
/// </summary>
public sealed class UpdateCanMoveEvent : CancellableEntityEventArgs
{
    public UpdateCanMoveEvent(EntityUid uid)
    {
        Uid = uid;
    }

    public EntityUid Uid { get; }
}