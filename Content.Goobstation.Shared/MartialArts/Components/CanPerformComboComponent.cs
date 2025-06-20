// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Lincoln McQueen <lincoln.mcqueen@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Common.MartialArts;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.MartialArts.Components;
[RegisterComponent]
[NetworkedComponent]
public sealed partial class CanPerformComboComponent : Component
{
    [DataField]
    public EntityUid? CurrentTarget;

    [DataField]
    public ProtoId<ComboPrototype> BeingPerformed;

    [DataField]
    public List<ComboAttackType> LastAttacks = new();

    [DataField]
    public List<ComboPrototype> AllowedCombos = new();

    [DataField]
    public List<ProtoId<ComboPrototype>> RoundstartCombos = new();

    [DataField]
    public TimeSpan ResetTime = TimeSpan.Zero;

    [DataField]
    public int ConsecutiveGnashes = 65;
}