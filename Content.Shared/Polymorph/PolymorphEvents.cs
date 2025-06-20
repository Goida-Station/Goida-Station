// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SX_65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Polymorph;

/// <summary>
/// Raised locally on an entity when it polymorphs into another entity
/// </summary>
/// <param name="OldEntity">EntityUid of the entity before the polymorph</param>
/// <param name="NewEntity">EntityUid of the entity after the polymorph</param>
/// <param name="IsRevert">Whether this polymorph event was a revert back to the original entity</param>
[ByRefEvent]
public record struct PolymorphedEvent(EntityUid OldEntity, EntityUid NewEntity, bool IsRevert);