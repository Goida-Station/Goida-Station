// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Movement.Events;

/// <summary>
/// Raised on an entity to check if it can move while weightless.
/// </summary>
[ByRefEvent]
public record struct CanWeightlessMoveEvent(EntityUid Uid)
{
    public bool CanMove = false;
}