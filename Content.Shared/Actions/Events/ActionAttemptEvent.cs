// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Actions.Events;

/// <summary>
/// Raised before an action is used and can be cancelled to prevent it.
/// Allowed to have side effects like modifying the action component.
/// </summary>
[ByRefEvent]
public record struct ActionAttemptEvent(EntityUid User, bool Cancelled = false);