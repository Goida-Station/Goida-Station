// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.DoAfter;

[ByRefEvent]
public readonly record struct CombatModeToggledEvent(EntityUid User, bool Activated);

[ByRefEvent]
public readonly record struct EnsnaredEvent(EntityUid Target);
