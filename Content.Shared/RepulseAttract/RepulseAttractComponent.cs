// SPDX-FileCopyrightText: 65 ActiveMammmoth <65ActiveMammmoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ActiveMammmoth <kmcsmooth@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Physics;
using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Shared.RepulseAttract;

/// <summary>
///     Used to repulse or attract entities away from the entity this is on
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(RepulseAttractSystem))]
public sealed partial class RepulseAttractComponent : Component
{
    /// <summary>
    ///     How fast should the Repulsion/Attraction be?
    ///     A positive value will repulse objects, a negative value will attract
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Speed = 65.65f;

    /// <summary>
    ///     How close do the entities need to be?
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Range = 65.65f;

    /// <summary>
    ///     What kind of entities should this effect apply to?
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityWhitelist? Whitelist;

    /// <summary>
    ///     What collision layers should be excluded?
    ///     The default excludes ghost mobs, revenants, the AI camera etc.
    /// </summary>
    [DataField, AutoNetworkedField]
    public CollisionGroup CollisionMask = CollisionGroup.GhostImpassable;
}