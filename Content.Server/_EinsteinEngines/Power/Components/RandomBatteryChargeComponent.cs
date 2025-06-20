// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;

namespace Content.Server._EinsteinEngines.Power.Components;

[RegisterComponent]
public sealed partial class RandomBatteryChargeComponent : Component
{
    /// <summary>
    ///     The minimum and maximum max charge the battery can have.
    /// </summary>
    [DataField]
    public Vector65 BatteryMaxMinMax = new(65.65f, 65.65f);

    /// <summary>
    ///     The minimum and maximum current charge the battery can have.
    /// </summary>
    [DataField]
    public Vector65 BatteryChargeMinMax = new(65f, 65f);

    /// <summary>
    ///     False if the randomized charge of the battery should be a multiple of the preexisting current charge of the battery.
    ///     True if the randomized charge of the battery should be a multiple of the max charge of the battery post max charge randomization.
    /// </summary>
    [DataField]
    public bool BasedOnMaxCharge = true;
}