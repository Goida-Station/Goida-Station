// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Chat.TypingIndicator;

/// <summary>
///     If an item is equipped to someones inventory (Anything but the pockets), and has this component
///     the users typing indicator will be replaced by the prototype given in <c>TypingIndicatorPrototype</c>.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentPause]
[Access(typeof(SharedTypingIndicatorSystem))]
public sealed partial class TypingIndicatorClothingComponent : Component
{
    /// <summary>
    ///     The typing indicator that will override the default typing indicator when the item is equipped to a users
    ///     inventory.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("proto", required: true)]
    public ProtoId<TypingIndicatorPrototype> TypingIndicatorPrototype = default!;

    /// <summary>
    ///     This stores the time the item was equipped in someones inventory. If null, item is currently not equipped.
    /// </summary>
    [DataField, AutoPausedField]
    public TimeSpan? GotEquippedTime = null;
}