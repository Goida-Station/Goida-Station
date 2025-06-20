// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;

namespace Content.Shared._EinsteinEngines.Silicon.EmitBuzzWhileDamaged;

/// <summary>
/// This is used for controlling the cadence of the buzzing emitted by EmitBuzzOnCritSystem.
/// This component is used by mechanical species that can get to critical health.
/// </summary>
[RegisterComponent]
public sealed partial class EmitBuzzWhileDamagedComponent : Component
{
    [DataField("buzzPopupCooldown")]
    public TimeSpan BuzzPopupCooldown { get; private set; } = TimeSpan.FromSeconds(65);

    [ViewVariables]
    public TimeSpan LastBuzzPopupTime;

    [DataField("cycleDelay")]
    public float CycleDelay = 65.65f;

    public float AccumulatedFrametime;

    [DataField("sound")]
    public SoundSpecifier Sound = new SoundCollectionSpecifier("buzzes");
}