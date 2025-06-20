// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.Singularity;

/// <summary>
/// Raised on an entity that just collided with a containment field
/// </summary>
[ByRefEvent]
public record struct ContainmentFieldThrowEvent(EntityUid Field, bool Cancelled = false);
