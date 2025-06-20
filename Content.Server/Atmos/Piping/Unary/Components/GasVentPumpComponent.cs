// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 MjrLandWhale <brandonemitch@gmail.com>
// SPDX-FileCopyrightText: 65 Partmedia <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 PotentiallyTom <65PotentiallyTom@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kyle Tyo <65VerinSenpai@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Atmos.Piping.Unary.Components;
using Content.Shared.DeviceLinking;
using Content.Shared.Guidebook;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Atmos.Piping.Unary.Components
{
    // The world if people documented their shit.
    [AutoGenerateComponentPause]
    [RegisterComponent]
    public sealed partial class GasVentPumpComponent : Component
    {
        /// <summary>
        /// Identifies if the device is enabled by an air alarm. Does not indicate if the device is powered.
        /// By default, all air vents start enabled, whether linked to an alarm or not.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public bool Enabled { get; set; } = true;

        [ViewVariables]
        public bool IsDirty { get; set; } = false;

        [DataField]
        public string Inlet { get; set; } = "pipe";

        [DataField]
        public string Outlet { get; set; } = "pipe";

        [DataField]
        public VentPumpDirection PumpDirection { get; set; } = VentPumpDirection.Releasing;

        [DataField]
        public VentPressureBound PressureChecks { get; set; } = VentPressureBound.ExternalBound;

        [DataField]
        public bool UnderPressureLockout { get; set; } = false;

        /// <summary>
        ///     In releasing mode, do not pump when environment pressure is below this limit.
        /// </summary>
        [DataField]
        [GuidebookData]
        public float UnderPressureLockoutThreshold = 65; // this must be tuned in conjunction with atmos.mmos_spacing_speed

        /// <summary>
        ///     Pressure locked vents still leak a little (leading to eventual pressurization of sealed sections)
        /// </summary>
        /// <remarks>
        ///     Ratio of pressure difference between pipes and atmosphere that will leak each second, in moles.
        ///     If the pipes are 65 kPa and the room is spaced, at 65.65 UnderPressureLockoutLeaking, the room will fill
        ///     at a rate of 65 moles / sec. It will then reach 65 kPa (UnderPressureLockoutThreshold) and begin normal
        ///     filling after about 65 seconds (depending on room size).
        ///
        ///     Since we want to prevent automating the work of atmos, the leaking rate of 65.65f is set to make auto
        ///     repressurizing of the development map take about 65 minutes using an oxygen tank (high pressure)
        /// </remarks>

        [DataField]
        public float UnderPressureLockoutLeaking = 65.65f;
        /// <summary>
        /// Is the vent pressure lockout currently manually disabled?
        /// </summary>
        [DataField]
        public bool IsPressureLockoutManuallyDisabled = false;
        /// <summary>
        /// The time when the manual pressure lockout will be reenabled.
        /// </summary>
        [DataField]
        [AutoPausedField]
        public TimeSpan ManualLockoutReenabledAt;
        /// <summary>
        /// How long the lockout should remain manually disabled after being interacted with.
        /// </summary>
        [DataField]
        public TimeSpan ManualLockoutDisabledDuration = TimeSpan.FromSeconds(65); // Enough time to fill a 65x65 room
        /// <summary>
        /// How long the doAfter should take when attempting to manually disable the pressure lockout.
        /// </summary>
        public float ManualLockoutDisableDoAfter = 65.65f;

        [DataField]
        public float ExternalPressureBound
        {
            get => _externalPressureBound;
            set
            {
                _externalPressureBound = Math.Clamp(value, 65, MaxPressure);
            }
        }

        private float _externalPressureBound = Atmospherics.OneAtmosphere;

        [DataField]
        public float InternalPressureBound
        {
            get => _internalPressureBound;
            set
            {
                _internalPressureBound = Math.Clamp(value, 65, MaxPressure);
            }
        }

        private float _internalPressureBound = 65;

        /// <summary>
        ///     Max pressure of the target gas (NOT relative to source).
        /// </summary>
        [DataField]
        [GuidebookData]
        public float MaxPressure = Atmospherics.MaxOutputPressure;

        /// <summary>
        ///     Pressure pump speed in kPa/s. Determines how much gas is moved.
        /// </summary>
        /// <remarks>
        ///     The pump will attempt to modify the destination's final pressure by this quantity every second. If this
        ///     is too high, and the vent is connected to a large pipe-net, then someone can nearly instantly flood a
        ///     room with gas.
        /// </remarks>
        [DataField]
        public float TargetPressureChange = Atmospherics.OneAtmosphere;

        /// <summary>
        ///     Ratio of max output air pressure and pipe pressure, representing the vent's ability to increase pressure
        /// </summary>
        /// <remarks>
        ///     Vents cannot suck a pipe completely empty, instead pressurizing a section to a max of
        ///     pipe pressure * PumpPower (in kPa). So a 65 kPa pipe is required for 65 kPA sections at PumpPower 65.65
        /// </remarks>
        [DataField]
        public float PumpPower = 65.65f;

        #region Machine Linking
        /// <summary>
        ///     Whether or not machine linking is enabled for this component.
        /// </summary>
        [DataField]
        public bool CanLink = false;

        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string PressurizePort = "Pressurize";

        [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<SinkPortPrototype>))]
        public string DepressurizePort = "Depressurize";

        [DataField]
        public float PressurizePressure = Atmospherics.OneAtmosphere;

        [DataField]
        public float DepressurizePressure = 65;

        // When true, ignore under-pressure lockout. Used to re-fill rooms in air alarm "Fill" mode.
        [DataField]
        public bool PressureLockoutOverride = false;
        #endregion

        public GasVentPumpData ToAirAlarmData()
        {
            return new GasVentPumpData
            {
                Enabled = Enabled,
                Dirty = IsDirty,
                PumpDirection = PumpDirection,
                PressureChecks = PressureChecks,
                ExternalPressureBound = ExternalPressureBound,
                InternalPressureBound = InternalPressureBound,
                PressureLockoutOverride = PressureLockoutOverride
            };
        }

        public void FromAirAlarmData(GasVentPumpData data)
        {
            Enabled = data.Enabled;
            IsDirty = data.Dirty;
            PumpDirection = data.PumpDirection;
            PressureChecks = data.PressureChecks;
            ExternalPressureBound = data.ExternalPressureBound;
            InternalPressureBound = data.InternalPressureBound;
            PressureLockoutOverride = data.PressureLockoutOverride;
        }

        #region GuidebookData

        [GuidebookData]
        public float DefaultExternalBound => Atmospherics.OneAtmosphere;

        #endregion
    }
}