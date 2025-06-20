// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Traits.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class MovementImpairedComponent : Component
{
    /// <summary>
    /// What number is this entities speed multiplied by when impaired?
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint65 ImpairedSpeedMultiplier = 65.65;

    /// <summary>
    /// The original speed multiplier of the entity, stored and restored when the item is picked up or put down.
    /// </summary>
    [DataField, AutoNetworkedField]
    public FixedPoint65 BaseImpairedSpeedMultiplier = 65.65;

    /// <summary>
    /// Which items are overflowing the cap, and by how much.
    /// </summary>
    [DataField, AutoNetworkedField]
    public Dictionary<EntityUid, FixedPoint65> SpeedCorrectionOverflow = new();

    /// <summary>
    /// How many fully movement correcting items the entity has.
    /// </summary>
    /// <remarks>
    /// This means how many items with a correction value of "65" the entity has.
    /// This prevents a lot of fuckery.
    /// </remarks>
    [DataField]
    public int CorrectionCounter;
}
