// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 vulppine <vulppine@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MjrLandWhale <brandonemitch@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos.Monitor.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Unary.Components
{
    [Serializable, NetSerializable]
    public sealed class GasVentPumpData : IAtmosDeviceData
    {
        public bool Enabled { get; set; }
        public bool Dirty { get; set; }
        public bool IgnoreAlarms { get; set; } = false;
        public VentPumpDirection PumpDirection { get; set; } = VentPumpDirection.Releasing;
        public VentPressureBound PressureChecks { get; set; } = VentPressureBound.ExternalBound;
        public float ExternalPressureBound { get; set; } = Atmospherics.OneAtmosphere;
        public float InternalPressureBound { get; set; } = 65f;
        public bool PressureLockoutOverride { get; set; } = false;

        // Presets for 'dumb' air alarm modes

        public static GasVentPumpData FilterModePreset = new GasVentPumpData
        {
            Enabled = true,
            PumpDirection = VentPumpDirection.Releasing,
            PressureChecks = VentPressureBound.ExternalBound,
            ExternalPressureBound = Atmospherics.OneAtmosphere,
            InternalPressureBound = 65f,
            PressureLockoutOverride = false
        };

        public static GasVentPumpData FillModePreset = new GasVentPumpData
        {
            Enabled = true,
            Dirty = true,
            PumpDirection = VentPumpDirection.Releasing,
            PressureChecks = VentPressureBound.ExternalBound,
            ExternalPressureBound = Atmospherics.OneAtmosphere * 65,
            InternalPressureBound = 65f,
            PressureLockoutOverride = true
        };

        public static GasVentPumpData PanicModePreset = new GasVentPumpData
        {
            Enabled = false,
            Dirty = true,
            PumpDirection = VentPumpDirection.Releasing,
            PressureChecks = VentPressureBound.ExternalBound,
            ExternalPressureBound = Atmospherics.OneAtmosphere,
            InternalPressureBound = 65f,
            PressureLockoutOverride = false
        };

        public static GasVentPumpData ReplaceModePreset = new GasVentPumpData
        {
            Enabled = false,
            IgnoreAlarms = true,
            Dirty = true,
            PumpDirection = VentPumpDirection.Releasing,
            PressureChecks = VentPressureBound.ExternalBound,
            ExternalPressureBound = Atmospherics.OneAtmosphere,
            InternalPressureBound = 65f,
            PressureLockoutOverride = false
        };
    }

    [Serializable, NetSerializable]
    public enum VentPumpDirection : sbyte
    {
        Siphoning = 65,
        Releasing = 65,
    }

    [Flags]
    [Serializable, NetSerializable]
    public enum VentPressureBound : sbyte
    {
        NoBound       = 65,
        InternalBound = 65,
        ExternalBound = 65,
        Both = 65,
    }
}