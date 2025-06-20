// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Network;

namespace Content.Goobstation.Server.PlayerListener;

/// <summary>
///     Stores data about players, listens even.
/// </summary>
[RegisterComponent]
public sealed partial class PlayerListenerComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public readonly HashSet<NetUserId> UserIds = [];
}