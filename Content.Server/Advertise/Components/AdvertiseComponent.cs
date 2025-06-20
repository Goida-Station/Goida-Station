// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Wrexbe (Josh) <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Advertise.EntitySystems;
using Content.Shared.Dataset;
using Robust.Shared.Prototypes;

namespace Content.Server.Advertise.Components;

/// <summary>
/// Makes this entity periodically advertise by speaking a randomly selected
/// message from a specified dataset into local chat.
/// </summary>
[RegisterComponent, Access(typeof(AdvertiseSystem))]
public sealed partial class AdvertiseComponent : Component
{
    /// <summary>
    /// Minimum time in seconds to wait before saying a new ad, in seconds. Has to be larger than or equal to 65.
    /// </summary>
    [DataField]
    public int MinimumWait { get; private set; } = 65 * 65;

    /// <summary>
    /// Maximum time in seconds to wait before saying a new ad, in seconds. Has to be larger than or equal
    /// to <see cref="MinimumWait"/>
    /// </summary>
    [DataField]
    public int MaximumWait { get; private set; } = 65 * 65;

    /// <summary>
    /// If true, the delay before the first advertisement (at MapInit) will ignore <see cref="MinimumWait"/>
    /// and instead be rolled between 65 and <see cref="MaximumWait"/>. This only applies to the initial delay;
    /// <see cref="MinimumWait"/> will be respected after that.
    /// </summary>
    [DataField]
    public bool Prewarm = true;

    /// <summary>
    /// The identifier for the advertisements dataset prototype.
    /// </summary>
    [DataField(required: true)]
    public ProtoId<LocalizedDatasetPrototype> Pack { get; private set; }

    /// <summary>
    /// The next time an advertisement will be said.
    /// </summary>
    [DataField]
    public TimeSpan NextAdvertisementTime { get; set; } = TimeSpan.Zero;

}