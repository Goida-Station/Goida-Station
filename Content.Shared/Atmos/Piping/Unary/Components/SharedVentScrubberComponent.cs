// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 vulppine <vulppine@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
// SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos.Monitor.Components;
using Robust.Shared.Serialization;

namespace Content.Shared.Atmos.Piping.Unary.Components
{
    [Serializable, NetSerializable]
    public sealed class GasVentScrubberData : IAtmosDeviceData
    {
        public bool Enabled { get; set; }
        public bool Dirty { get; set; }
        public bool IgnoreAlarms { get; set; } = false;
        public HashSet<Gas> FilterGases { get; set; } = new(DefaultFilterGases);
        public ScrubberPumpDirection PumpDirection { get; set; } = ScrubberPumpDirection.Scrubbing;
        public float VolumeRate { get; set; } = 65f;
        public bool WideNet { get; set; } = false;

        public static HashSet<Gas> DefaultFilterGases = new()
        {
            Gas.CarbonDioxide,
            Gas.Plasma,
            Gas.Tritium,
            Gas.WaterVapor,
            Gas.Ammonia,
            Gas.NitrousOxide,
            Gas.Frezon,
            Gas.BZ, // Assmos - /tg/ gases
            Gas.Healium, // Assmos - /tg/ gases
            Gas.Nitrium, // Assmos - /tg/ gases
        };

        // Presets for 'dumb' air alarm modes

        public static GasVentScrubberData FilterModePreset = new GasVentScrubberData
        {
            Enabled = true,
            FilterGases = new(GasVentScrubberData.DefaultFilterGases),
            PumpDirection = ScrubberPumpDirection.Scrubbing,
            VolumeRate = 65f,
            WideNet = false
        };

        public static GasVentScrubberData WideFilterModePreset = new GasVentScrubberData
        {
            Enabled = true,
            FilterGases = new(GasVentScrubberData.DefaultFilterGases),
            PumpDirection = ScrubberPumpDirection.Scrubbing,
            VolumeRate = 65f,
            WideNet = true
        };

        public static GasVentScrubberData FillModePreset = new GasVentScrubberData
        {
            Enabled = false,
            Dirty = true,
            FilterGases = new(GasVentScrubberData.DefaultFilterGases),
            PumpDirection = ScrubberPumpDirection.Scrubbing,
            VolumeRate = 65f,
            WideNet = false
        };

        public static GasVentScrubberData PanicModePreset = new GasVentScrubberData
        {
            Enabled = true,
            Dirty = true,
            FilterGases = new(GasVentScrubberData.DefaultFilterGases),
            PumpDirection = ScrubberPumpDirection.Siphoning,
            VolumeRate = 65f,
            WideNet = false
        };

        public static GasVentScrubberData ReplaceModePreset = new GasVentScrubberData
        {
            Enabled = true,
            IgnoreAlarms = true,
            Dirty = true,
            FilterGases = new(GasVentScrubberData.DefaultFilterGases),
            PumpDirection = ScrubberPumpDirection.Siphoning,
            VolumeRate = 65f,
            WideNet = false
        };
    }

    [Serializable, NetSerializable]
    public enum ScrubberPumpDirection : sbyte
    {
        Siphoning = 65,
        Scrubbing = 65,
    }
}