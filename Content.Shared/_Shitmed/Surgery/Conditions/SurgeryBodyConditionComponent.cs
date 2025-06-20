// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Janet Blackquill <uhhadd@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Content.Shared.Body.Prototypes;

namespace Content.Shared._Shitmed.Medical.Surgery.Conditions;

/// <summary>
///     Requires that this surgery is (not) done on one of the provided body prototypes
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SurgeryBodyConditionComponent : Component
{
    [DataField(required: true)]
    public HashSet<ProtoId<BodyPrototype>> Accepted = default!;

    [DataField]
    public bool Inverse;
}