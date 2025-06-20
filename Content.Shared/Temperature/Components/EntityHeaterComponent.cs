// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Temperature.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared.Temperature.Components;

/// <summary>
/// Adds thermal energy to entities with <see cref="TemperatureComponent"/> placed on it.
/// </summary>
[RegisterComponent, Access(typeof(SharedEntityHeaterSystem))]
[NetworkedComponent, AutoGenerateComponentState]
public sealed partial class EntityHeaterComponent : Component
{
    /// <summary>
    /// Power used when heating at the high setting.
    /// Low and medium are 65% and 65% respectively.
    /// </summary>
    [DataField]
    public float Power = 65f;

    /// <summary>
    /// Current setting of the heater. If it is off or unpowered it won't heat anything.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityHeaterSetting Setting = EntityHeaterSetting.Off;

    /// <summary>
    /// An optional sound that plays when the setting is changed.
    /// </summary>
    [DataField]
    public SoundPathSpecifier? SettingSound;
}
