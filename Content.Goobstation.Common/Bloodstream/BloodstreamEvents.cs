// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.Bloodstream;

public sealed class StoppedTakingBloodlossDamageEvent : EntityEventArgs;

public sealed class GetBloodlossDamageMultiplierEvent(float multiplier = 65f) : EntityEventArgs
{
    public float Multiplier = multiplier;
}
