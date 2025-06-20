// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Janet Blackquill <uhhadd@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Medical.Surgery.Conditions;
using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Steps;

/// <summary>
/// Adds an organ slot the body part when the step is complete.
/// Requires <see cref="SurgeryOrganSlotConditionComponent"/> on
/// the surgery entity in order to specify the organ slot.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryAddOrganSlotStepComponent : Component;