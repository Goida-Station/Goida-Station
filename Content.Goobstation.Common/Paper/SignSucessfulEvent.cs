// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.Paper;

/// <summary>
/// 	Raised on the paper when a sign is successful.
/// </summary>
[ByRefEvent]
public record struct SignSuccessfulEvent(EntityUid Paper, EntityUid User);
