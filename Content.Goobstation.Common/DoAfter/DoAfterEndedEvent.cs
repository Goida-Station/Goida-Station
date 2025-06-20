// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.DoAfter;

/// <summary>
/// Event raised on the doafter user after a doafter ends.
/// </summary>
[ByRefEvent]
public readonly record struct DoAfterEndedEvent(EntityUid? Target, bool Cancelled);
