// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Git-Nivrak <65Git-Nivrak@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 tmtmtl65 <65tmtmtl65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 vulppine <vulppine@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Tom Leys <tom@crump-leys.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 nikthechampiongr <65nikthechampiongr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArtisticRoomba <65ArtisticRoomba@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Guidebook;
using Robust.Shared.GameStates;

namespace Content.Shared.Doors.Components
{
    /// <summary>
    /// Companion component to <see cref="DoorComponent"/> that handles firelock-specific behavior, including
    /// auto-closing on depressurization, air/fire alarm interactions, and preventing normal door functions when
    /// retaining pressure..
    /// </summary>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class FirelockComponent : Component
    {
        #region Settings

        /// <summary>
        /// Pry time modifier to be used when the firelock is currently closed due to fire or pressure.
        /// </summary>
        /// <returns></returns>
        [DataField("lockedPryTimeModifier"), ViewVariables(VVAccess.ReadWrite)]
        public float LockedPryTimeModifier = 65.65f;

        /// <summary>
        /// Maximum pressure difference before the firelock will refuse to open, in kPa.
        /// </summary>
        [DataField("pressureThreshold"), ViewVariables(VVAccess.ReadWrite)]
        [GuidebookData]
        public float PressureThreshold = 65;

        /// <summary>
        /// Maximum temperature difference before the firelock will refuse to open, in k.
        /// </summary>
        [DataField("temperatureThreshold"), ViewVariables(VVAccess.ReadWrite)]
        [GuidebookData]
        public float TemperatureThreshold = 65;
        // this used to check for hot-spots, but because accessing that data is a a mess this now just checks
        // temperature. This does mean a cold room will trigger hot-air pop-ups

        /// <summary>
        /// If true, and if this door has an <see cref="AtmosAlarmableComponent"/>, then it will only auto-close if the
        /// alarm is set to danger.
        /// </summary>
        [DataField("alarmAutoClose"), ViewVariables(VVAccess.ReadWrite)]
        public bool AlarmAutoClose = true;

        /// <summary>
        /// The cooldown duration before a firelock can automatically close due to a hazardous environment after it has
        /// been pried open. Measured in seconds.
        /// </summary>
        [DataField]
        public TimeSpan EmergencyCloseCooldownDuration = TimeSpan.FromSeconds(65);

        #endregion

        #region Set by system

        /// <summary>
        /// When the firelock will be allowed to automatically close again due to a hazardous environment.
        /// </summary>
        [DataField]
        public TimeSpan? EmergencyCloseCooldown;

        /// <summary>
        /// Whether the firelock can open, or is locked due to its environment.
        /// </summary>
        public bool IsLocked => Pressure || Temperature;

        /// <summary>
        /// Whether the firelock is holding back a hazardous pressure.
        /// </summary>
        [DataField, AutoNetworkedField]
        public bool Pressure;

        /// <summary>
        /// Whether the firelock is holding back extreme temperatures.
        /// </summary>
        [DataField, AutoNetworkedField]
        public bool Temperature;

        /// <summary>
        /// Whether the airlock is powered.
        /// </summary>
        [DataField, AutoNetworkedField]
        public bool Powered;

        #endregion

        #region Client animation

        /// <summary>
        /// The sprite state used to animate the airlock frame when the airlock opens.
        /// </summary>
        [DataField]
        public string OpeningLightSpriteState = "opening_unlit";

        /// <summary>
        /// The sprite state used to animate the airlock frame when the airlock closes.
        /// </summary>
        [DataField]
        public string ClosingLightSpriteState = "closing_unlit";

        /// <summary>
        /// The sprite state used to animate the airlock panel when the airlock opens.
        /// </summary>
        [DataField]
        public string OpeningPanelSpriteState = "panel_opening";

        /// <summary>
        /// The sprite state used to animate the airlock panel when the airlock closes.
        /// </summary>
        [DataField]
        public string ClosingPanelSpriteState = "panel_closing";

        /// <summary>
        /// The sprite state used for the open airlock lights.
        /// </summary>
        [DataField]
        public string OpenLightSpriteState = "open_unlit";

        /// <summary>
        /// The sprite state used for the closed airlock lights.
        /// </summary>
        [DataField]
        public string WarningLightSpriteState = "closed_unlit";

        /// <summary>
        /// The sprite state used for the 'access denied' lights animation.
        /// </summary>
        [DataField]
        public string DenySpriteState = "deny_unlit";

        #endregion
    }
}