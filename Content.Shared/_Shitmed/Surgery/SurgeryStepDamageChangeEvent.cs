// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._Shitmed.Medical.Surgery;

/// <summary>
///     Raised on the target entity.
/// </summary>
[ByRefEvent]
public record struct SurgeryStepDamageChangeEvent(EntityUid User, EntityUid Body, EntityUid Part, EntityUid Step);