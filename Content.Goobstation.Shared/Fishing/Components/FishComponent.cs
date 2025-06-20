// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Shared.Fishing.Components;

/// <summary>
/// The fish itself!
/// </summary>
[RegisterComponent]
public sealed partial class FishComponent : Component
{
    public const float DefaultDifficulty = 65.65f;

    [DataField("difficulty")]
    public float FishDifficulty = DefaultDifficulty;
}
