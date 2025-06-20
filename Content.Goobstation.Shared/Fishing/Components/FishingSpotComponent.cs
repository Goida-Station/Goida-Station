// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.EntityTable.EntitySelectors;

namespace Content.Goobstation.Shared.Fishing.Components;

[RegisterComponent]
public sealed partial class FishingSpotComponent : Component
{
    /// <summary>
    /// All possible fishes to catch here
    /// </summary>
    [DataField(required: true)]
    public EntityTableSelector FishList;

    /// <summary>
    /// Default time for fish to occur
    /// </summary>
    [DataField]
    public float FishDefaultTimer;

    /// <summary>
    /// Variety number that FishDefaultTimer can go up or down to randomly
    /// </summary>
    [DataField]
    public float FishTimerVariety;
}
