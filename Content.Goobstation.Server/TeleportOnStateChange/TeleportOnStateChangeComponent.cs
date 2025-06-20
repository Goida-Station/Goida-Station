// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Mobs;
using Robust.Shared.Map;

namespace Content.Goobstation.Server.TeleportOnStateChange;

/// <summary>
/// Teleports the entity to the given coordinates when changed to the specified state.
/// </summary>
[RegisterComponent]
public sealed partial class TeleportOnStateChangeComponent : Component
{

    /// <summary>
    /// The co-ordinates to teleport to.
    /// </summary>
    [ViewVariables]
    public EntityCoordinates? Coordinates;

    /// <summary>
    /// What mob-state should trigger the teleportation?
    /// </summary>
    [ViewVariables]
    public MobState MobStateTrigger = MobState.Critical;

    /// <summary>
    /// Should the component be removed on trigger?
    /// </summary>
    [ViewVariables]
    public bool RemoveOnTrigger;
}
