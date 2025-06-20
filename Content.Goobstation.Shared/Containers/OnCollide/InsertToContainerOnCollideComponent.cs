// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Containers.OnCollide;

/// <summary>
/// When this component is added, we insert to a given container any entity we collide with
/// </summary>
[RegisterComponent, NetworkedComponent]
[Access(typeof(InsertToContainerOnCollideSystem))]
public sealed partial class InsertToContainerOnCollideComponent : Component
{
    /// <summary>
    /// ID of the target container
    /// </summary>
    [DataField("container", required: true)]
    [ViewVariables]
    public string Container = default!;

    [DataField("insertableEntities")]
    public EntityWhitelist? InsertableEntities;

    /// <summary>
    /// The minimum velocity we have to have to be able to insert something in the container.
    /// Represented in meters/tiles per second
    /// </summary>
    [DataField("requiredVelocity")]
    [ViewVariables(VVAccess.ReadWrite)]
    public float RequiredVelocity;

    /// <summary>
    /// Entities which we should never insert on collide
    /// </summary>
    [DataField("blacklistedEntities")]
    public EntityWhitelist? BlacklistedEntities;
}