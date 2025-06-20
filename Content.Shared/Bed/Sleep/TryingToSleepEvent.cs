// SPDX-FileCopyrightText: 65 Errant <65dmnct@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Bed.Sleep;

/// <summary>
///     Raised by an entity about to fall asleep.
///     Set Cancelled to true on event handling to interrupt
/// </summary>
[ByRefEvent]
public record struct TryingToSleepEvent(EntityUid uid, bool Cancelled = false);