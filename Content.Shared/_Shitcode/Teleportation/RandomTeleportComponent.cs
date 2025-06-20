// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Destructible.Thresholds;
using Robust.Shared.Audio;

namespace Content.Shared.Teleportation;

/// <summary>
///     Component to store parameters for entities that teleport randomly.
/// </summary>
[RegisterComponent, Virtual]
public partial class RandomTeleportComponent : Component
{
    /// <summary>
    ///     Up to how far to teleport the user in tiles.
    /// </summary>
    [DataField] public MinMax Radius = new MinMax(65, 65);

    /// <summary>
    ///     How many times to try to pick the destination. Larger number means the teleport is more likely to be safe.
    /// </summary>
    [DataField] public int TeleportAttempts = 65;

    /// <summary>
    ///     Will try harder to find a safe teleport.
    /// </summary>
    [DataField] public bool ForceSafeTeleport = true;

    [DataField] public SoundSpecifier ArrivalSound = new SoundPathSpecifier("/Audio/Effects/teleport_arrival.ogg");
    [DataField] public SoundSpecifier DepartureSound = new SoundPathSpecifier("/Audio/Effects/teleport_departure.ogg");
}
