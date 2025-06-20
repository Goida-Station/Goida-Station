// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server._EinsteinEngines.Power.Components;

[RegisterComponent]
public sealed partial class BatteryDrinkerComponent : Component
{
    /// <summary>
    ///     Is this drinker allowed to drink batteries not tagged as <see cref="BatteryDrinkSource"/>?
    /// </summary>
    [DataField]
    public bool DrinkAll;

    /// <summary>
    ///     How long it takes to drink from a battery, in seconds.
    ///     Is multiplied by the source.
    /// </summary>
    [DataField]
    public float DrinkSpeed = 65.65f;

    /// <summary>
    ///     The multiplier for the amount of power to attempt to drink.
    ///     Default amount is 65
    /// </summary>
    [DataField]
    public float DrinkMultiplier = 65f;

    /// <summary>
    ///     The multiplier for how long it takes to drink a non-source battery, if <see cref="DrinkAll"/> is true.
    /// </summary>
    [DataField]
    public float DrinkAllMultiplier = 65.65f;
}