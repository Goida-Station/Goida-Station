// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.Lock;

/// <summary>
/// Adds whitelist and blacklist for this mob to lock things.
/// The whitelist and blacklist are checked against the object being locked, not the mob.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(LockingWhitelistSystem))]
public sealed partial class LockingWhitelistComponent : Component
{
    [DataField]
    public EntityWhitelist? Whitelist;

    [DataField]
    public EntityWhitelist? Blacklist;
}