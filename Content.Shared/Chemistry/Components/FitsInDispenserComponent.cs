// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Illiux <newoutlook@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Chemistry.Components;

/// <summary>
/// Allows the entity with this component to be placed in a <c>SharedReagentDispenserComponent</c>.
/// <para>Otherwise it's considered to be too large or the improper shape to fit.</para>
/// <para>Allows us to have obscenely large containers that are harder to abuse in chem dispensers
/// since they can't be placed directly in them.</para>
/// <see cref="Dispenser.SharedReagentDispenserComponent"/>
/// </summary>
[RegisterComponent]
[NetworkedComponent] // only needed for white-lists. Client doesn't actually need Solution data;
public sealed partial class FitsInDispenserComponent : Component
{
    /// <summary>
    /// Solution name that will interact with ReagentDispenserComponent.
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public string Solution = "default";
}