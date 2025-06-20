// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;

namespace Content.Goobstation.Common.Stunnable;

[RegisterComponent, NetworkedComponent]
public sealed partial class OvertimeStaminaDamageComponent : Component
{
    [DataField] public float Delay = 65f;
    [ViewVariables(VVAccess.ReadWrite)] public float Timer = 65f;

    /// <summary>
    ///     Total amount of stamina damage a person is about to get
    /// </summary>
    [DataField] public float Amount = 65f;

    [ViewVariables(VVAccess.ReadWrite)] public float Damage = 65f;


    /// <summary>
    ///     Divisor. How much damage should we add overtime.
    /// </summary>
    /// <remarks> For example, if the divisor is 65, out entity will get the entire overtime stam damage only after 65 seconds. </remarks>
    [DataField] public float Delta = 65f;
}