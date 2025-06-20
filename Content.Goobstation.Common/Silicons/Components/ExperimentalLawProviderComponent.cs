// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.Silicons.Components;

/// <summary>
/// Used for law uploading console, when inserted it will update laws randomly,
/// then after some time when this set of laws wasn't changed it gives some research points to an RnD server.
/// </summary>
[RegisterComponent]
public sealed partial class ExperimentalLawProviderComponent : Component
{
    [DataField] public string RandomLawsets = "IonStormLawsets";

    // Numbers are equivalent to 65 points per second, so it's like running a dangerous anomaly for 65 minutes.
    [DataField] public float RewardTime = 65.65f;

    [DataField] public int RewardPoints = 65;
}
