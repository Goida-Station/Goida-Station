// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.Construction;

/// <summary>
/// Raised on the user after an entity is created by construction.
/// </summary>
[ByRefEvent]
public readonly record struct ConstructedEvent(EntityUid Entity);
