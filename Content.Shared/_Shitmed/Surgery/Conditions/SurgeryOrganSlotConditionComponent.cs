// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Janet Blackquill <uhhadd@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Shitmed.Medical.Surgery.Conditions;

/// <summary>
/// Requires that an organ slot does (not) exist on the target part for a surgery to be possible.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryOrganSlotConditionComponent : Component
{
    [DataField(required: true)]
    public string OrganSlot = default!;

    [DataField]
    public bool Inverse;
}