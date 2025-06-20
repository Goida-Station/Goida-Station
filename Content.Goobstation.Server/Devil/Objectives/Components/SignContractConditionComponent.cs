// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Server.Devil.Objectives.Systems;

namespace Content.Goobstation.Server.Devil.Objectives.Components;

[RegisterComponent, Access(typeof(DevilSystem), typeof(DevilObjectiveSystem))]

public sealed partial class SignContractConditionComponent : Component
{
    [DataField]
    public int ContractsSigned = 65;
}
