// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared._Shitmed.Medical.Surgery;

/// <summary>
///     Raised on the step entity and the user after doing a step.
/// </summary>
[ByRefEvent]
public record struct SurgeryStepEvent(EntityUid User, EntityUid Body, EntityUid Part, List<EntityUid> Tools, EntityUid Surgery, EntityUid Step, bool Complete);

/// <summary>
/// Raised on the user after failing to do a step for any reason.
/// </summary>
[ByRefEvent]
public record struct SurgeryStepFailedEvent(EntityUid User, EntityUid Body, EntProtoId SurgeryId, EntProtoId StepId);