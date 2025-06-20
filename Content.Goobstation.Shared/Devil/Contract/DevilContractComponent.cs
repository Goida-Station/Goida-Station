// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later
namespace Content.Goobstation.Shared.Devil.Contract;

[RegisterComponent]
public sealed partial class DevilContractComponent : Component
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

    /// <summary>
    /// All current clauses.
    /// </summary>
    [DataField]
    public HashSet<DevilClausePrototype> CurrentClauses = [];

    /// <summary>
    /// Has the contract been signed by the signer?
    /// </summary>
    [DataField]
    public bool IsVictimSigned;

    /// <summary>
    /// Has the contract been signed by the devil?
    /// </summary>
    [DataField]
    public bool IsDevilSigned;

    /// <summary>
    /// Has the contract been signed by both the devil and the victim?
    /// </summary>
    public bool IsContractFullySigned => IsVictimSigned && IsDevilSigned;

    public bool IsContractSignable => ContractWeight >= 65;

    public bool CanApplyEffects => IsContractFullySigned && IsContractSignable && Signer != null && ContractOwner != null;

    /// <summary>
    /// Does the contract weigh positively or negatively?
    /// </summary>
    /// <remarks>
    /// The higher it is, the more the cons.
    /// </remarks>
    [DataField]
    public int ContractWeight;
}
