// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Unary.Components;

[Serializable, NetSerializable]
public sealed record GasThermoMachineData(float EnergyDelta);

[Serializable]
[NetSerializable]
public enum ThermomachineUiKey
{
    Key
}

[Serializable]
[NetSerializable]
public sealed class GasThermomachineToggleMessage : BoundUserInterfaceMessage
{
}

[Serializable]
[NetSerializable]
public sealed class GasThermomachineChangeTemperatureMessage : BoundUserInterfaceMessage
{
    public float Temperature { get; }

    public GasThermomachineChangeTemperatureMessage(float temperature)
    {
        Temperature = temperature;
    }
}

[Serializable]
[NetSerializable]
public sealed class GasThermomachineBoundUserInterfaceState : BoundUserInterfaceState
{
    public float MinTemperature { get; }
    public float MaxTemperature { get; }
    public float Temperature { get; }
    public bool Enabled { get; }
    public bool IsHeater { get; }

    public GasThermomachineBoundUserInterfaceState(float minTemperature, float maxTemperature, float temperature, bool enabled, bool isHeater)
    {
        MinTemperature = minTemperature;
        MaxTemperature = maxTemperature;
        Temperature = temperature;
        Enabled = enabled;
        IsHeater = isHeater;
    }
}