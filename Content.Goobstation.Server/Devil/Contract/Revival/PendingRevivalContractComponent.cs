// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Devil.Contract.Revival;

[RegisterComponent]
public sealed partial class PendingRevivalContractComponent : Component
{
    /// <summary>
    /// The entity being revived.
    /// </summary>
    [ViewVariables]
    public EntityUid? Contractee;

    /// <summary>
    /// The entity offering revival
    /// </summary>
    [ViewVariables]
    public EntityUid? Offerer;

    /// <summary>
    /// The contract attached to this player.
    /// </summary>
    [ViewVariables]
    public EntityUid? Contract;

    /// <summary>
    /// The MindId of the player.
    /// </summary>
    [ViewVariables]
    public EntityUid MindId;
}
