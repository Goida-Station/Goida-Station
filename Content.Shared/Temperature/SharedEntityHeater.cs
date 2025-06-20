// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Temperature;

[Serializable, NetSerializable]
public enum EntityHeaterVisuals
{
    Setting
}

/// <summary>
/// What heat the heater is set to, if on at all.
/// </summary>
[Serializable, NetSerializable]
public enum EntityHeaterSetting
{
    Off,
    Low,
    Medium,
    High
}