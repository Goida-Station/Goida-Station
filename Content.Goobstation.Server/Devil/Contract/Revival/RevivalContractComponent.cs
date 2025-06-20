// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Devil.Contract.Revival;

[RegisterComponent]
public sealed partial class RevivalContractComponent : Component
{
    /// <summary>
    /// The entity who signed the paper, AKA, the entity who has the effects applied.
    /// </summary>
    [DataField]
    public EntityUid? Signer;

    /// <summary>
    /// The entity who created the contract, AKA, the entity who gains the soul.
    /// </summary>
    [DataField]
    public EntityUid? ContractOwner;

}
