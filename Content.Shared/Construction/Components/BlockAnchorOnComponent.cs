// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Velcroboy <65iamvelcroboy@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Construction.EntitySystems;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.Construction.Components;

/// <summary>
/// Will not allow anchoring if there is an anchored item in the same tile that fails the <see cref="EntityWhitelist"/>.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(BlockAnchorOnSystem))]
public sealed partial class BlockAnchorOnComponent : Component
{
    /// <summary>
    /// If not null, entities that match this whitelist are allowed.
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    /// If not null, entities that match this blacklist are not allowed.
    /// </summary>
    [DataField]
    public EntityWhitelist? Blacklist;
}
