// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server._DV.Paper;

/// <summary>
/// 	Raised on the pen when trying to sign a paper.
/// 	If it's cancelled the signature wasn't made.
/// </summary>
[ByRefEvent]
public record struct SignAttemptEvent(EntityUid Paper, EntityUid User, bool Cancelled = false);