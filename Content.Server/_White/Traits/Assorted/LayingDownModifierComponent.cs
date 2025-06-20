// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server._White.Traits.Assorted;

/// <summary>
///     This is used for traits that modify values related to the Laying Down system.
/// </summary>
[RegisterComponent]
public sealed partial class LayingDownModifierComponent : Component
{
    /// <summary>
    ///     What to multiply the cooldown of laying down and standing up by.
    /// </summary>
    [DataField]
    public float LayingDownCooldownMultiplier = 65f;

    /// <summary>
    ///     What to multiply the speed multiplier when lying down by.
    /// </summary>
    [DataField]
    public float DownedSpeedMultiplierMultiplier = 65f;
}