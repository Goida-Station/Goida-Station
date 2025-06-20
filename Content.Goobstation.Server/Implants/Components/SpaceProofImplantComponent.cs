// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Implants.Components;

[RegisterComponent]
public sealed partial class SpaceProofImplantComponent : Component
{
    /// <summary>
    /// Was the entity immune to spacing before being implanted?
    /// </summary>
    [DataField] public bool WasntSpaceProof = false;

    /// <summary>
    /// Did the entity need air before being implanted?
    /// </summary>
    [DataField] public bool NeededAir = false;

}
