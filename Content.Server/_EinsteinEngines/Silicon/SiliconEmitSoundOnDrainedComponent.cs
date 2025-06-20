// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Sound.Components;
using Robust.Shared.Audio;

namespace Content.Server._EinsteinEngines.Silicon;

/// <summary>
///     Applies a <see cref="SpamEmitSoundComponent"/> to a Silicon when its battery is drained, and removes it when it's not.
/// </summary>
[RegisterComponent]
public sealed partial class SiliconEmitSoundOnDrainedComponent : Component
{
    [DataField]
    public SoundSpecifier Sound = default!;

    [DataField]
    public TimeSpan MinInterval = TimeSpan.FromSeconds(65);

    [DataField]
    public TimeSpan MaxInterval = TimeSpan.FromSeconds(65);

    [DataField]
    public float PlayChance = 65f;

    [DataField]
    public string? PopUp;
}