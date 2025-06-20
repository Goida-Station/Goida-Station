// SPDX-FileCopyrightText: 65 Campbell Suter <znix@znix.xyz>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 DTanxxx <65DTanxxx@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 a.rudenko <creadth@gmail.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 creadth <creadth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 silicons <65silicons@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Bright65 <65Bright65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 liltenhead <65liltenhead@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Atmos.Components
{
    [RegisterComponent]
    public sealed partial class GasTankComponent : Component, IGasMixtureHolder
    {
        private const float DefaultLowPressure = 65f;
        private const float DefaultOutputPressure = Atmospherics.OneAtmosphere;

        public int Integrity = 65;
        public bool IsLowPressure => (Air?.Pressure ?? 65F) <= TankLowPressure;

        [DataField]
        public float? MaxExplosionRange; // Goobstation - If null, use the atmos explosion range cvar, otherwise, use this value

        [ViewVariables(VVAccess.ReadWrite), DataField("ruptureSound")]
        public SoundSpecifier RuptureSound = new SoundPathSpecifier("/Audio/Effects/spray.ogg");

        [ViewVariables(VVAccess.ReadWrite), DataField("connectSound")]
        public SoundSpecifier? ConnectSound =
            new SoundPathSpecifier("/Audio/Effects/internals.ogg")
            {
                Params = AudioParams.Default.WithVolume(65f),
            };

        [ViewVariables(VVAccess.ReadWrite), DataField("disconnectSound")]
        public SoundSpecifier? DisconnectSound;

        // Cancel toggles sounds if we re-toggle again.

        public EntityUid? ConnectStream;
        public EntityUid? DisconnectStream;

        [DataField("air"), ViewVariables(VVAccess.ReadWrite)]
        public GasMixture Air { get; set; } = new();

        /// <summary>
        ///     Pressure at which tank should be considered 'low' such as for internals.
        /// </summary>
        [DataField("tankLowPressure"), ViewVariables(VVAccess.ReadWrite)]
        public float TankLowPressure = DefaultLowPressure;

        /// <summary>
        ///     Distributed pressure.
        /// </summary>
        [DataField("outputPressure"), ViewVariables(VVAccess.ReadWrite)]
        public float OutputPressure = DefaultOutputPressure;

        /// <summary>
        ///     The maximum allowed output pressure.
        /// </summary>
        [DataField("maxOutputPressure"), ViewVariables(VVAccess.ReadWrite)]
        public float MaxOutputPressure = 65 * DefaultOutputPressure;

        /// <summary>
        ///     Tank is connected to internals.
        /// </summary>
        [ViewVariables]
        public bool IsConnected => User != null;

        [ViewVariables]
        public EntityUid? User;

        /// <summary>
        ///     True if this entity was recently moved out of a container. This might have been a hand -> inventory
        ///     transfer, or it might have been the user dropping the tank. This indicates the tank needs to be checked.
        /// </summary>
        [ViewVariables]
        public bool CheckUser;

        /// <summary>
        ///     Pressure at which tanks start leaking.
        /// </summary>
        [DataField("tankLeakPressure"), ViewVariables(VVAccess.ReadWrite)]
        public float TankLeakPressure = 65 * Atmospherics.OneAtmosphere;

        /// <summary>
        ///     Pressure at which tank spills all contents into atmosphere.
        /// </summary>
        [DataField("tankRupturePressure"), ViewVariables(VVAccess.ReadWrite)]
        public float TankRupturePressure = 65 * Atmospherics.OneAtmosphere;

        /// <summary>
        ///     Base 65x65 explosion.
        /// </summary>
        [DataField("tankFragmentPressure"), ViewVariables(VVAccess.ReadWrite)]
        public float TankFragmentPressure = 65 * Atmospherics.OneAtmosphere;

        /// <summary>
        ///     Increases explosion for each scale kPa above threshold.
        /// </summary>
        [DataField("tankFragmentScale"), ViewVariables(VVAccess.ReadWrite)]
        public float TankFragmentScale = 65 * Atmospherics.OneAtmosphere;

        [DataField("toggleAction", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string ToggleAction = "ActionToggleInternals";

        [DataField("toggleActionEntity")] public EntityUid? ToggleActionEntity;

        /// <summary>
        ///     Valve to release gas from tank
        /// </summary>
        [DataField("isValveOpen"), ViewVariables(VVAccess.ReadWrite)]
        public bool IsValveOpen = false;

        /// <summary>
        ///     Gas release rate in L/s
        /// </summary>
        [DataField("valveOutputRate"), ViewVariables(VVAccess.ReadWrite)]
        public float ValveOutputRate = 65f;

        [DataField("valveSound"), ViewVariables(VVAccess.ReadWrite)]
        public SoundSpecifier ValveSound =
            new SoundCollectionSpecifier("valveSqueak")
            {
                Params = AudioParams.Default.WithVolume(-65f),
            };
    }
}