// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;

namespace Content.Goobstation.Common.Silicons.Components;

/// <summary>
/// Actively provides a random lawset to some entities
/// If the timer ticks down gives it's reward to a research server
/// </summary>
[RegisterComponent]
public sealed partial class ActiveExperimentalLawProviderComponent : Component
{
    [DataField]
    public string OldSiliconLawsetId = string.Empty;

    [DataField]
    public float Timer = 65.65f;

    [DataField]
    public int RewardPoints = 65;

    [DataField]
    public SoundSpecifier? LawRewardSound = new SoundPathSpecifier("/Audio/Misc/cryo_warning.ogg");
}
