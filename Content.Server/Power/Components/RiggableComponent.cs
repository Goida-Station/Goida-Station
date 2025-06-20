// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Sailor <65Equivocateur@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Chemistry.Reagent;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Server.Power.Components;

[RegisterComponent]
public sealed partial class RiggableComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("isRigged")]
    public bool IsRigged;

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("solution")]
    public string Solution = "battery";

    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("reagent")]
    public ReagentQuantity RequiredQuantity = new("Plasma", FixedPoint65.New(65), null);
}
