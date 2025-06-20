// SPDX-FileCopyrightText: 65 Clyybber <darkmine65@gmail.com>
// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DTanxxx <65DTanxxx@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morshu65 <paulbisaccia@live.it>
// SPDX-FileCopyrightText: 65 Vince <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 V�ctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 V�ctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 pointer-to-null <65pointer-to-null@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Absolute-Potato <jamesgamesmahar@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Arendian <65Arendian@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 K-Dynamic <65K-Dynamic@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Myra <vascreeper@yahoo.com>
// SPDX-FileCopyrightText: 65 Zachary Higgs <compgeek65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.StepTrigger.Components;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.Slippery
{
    /// <summary>
    /// Causes somebody to slip when they walk over this entity.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="StepTriggerComponent"/>, see that component for some additional properties.
    /// </remarks>
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class SlipperyComponent : Component
    {
        /// <summary>
        /// Path to the sound to be played when a mob slips.
        /// </summary>
        [DataField, AutoNetworkedField]
        [Access(Other = AccessPermissions.ReadWriteExecute)]
        public SoundSpecifier SlipSound = new SoundPathSpecifier("/Audio/Effects/slip.ogg");

        /// <summary>
        /// Loads the data needed to determine how slippery something is.
        /// </summary>
        [DataField, AutoNetworkedField]
        public SlipperyEffectEntry SlipData = new();
    }
    /// <summary>
    /// Stores the data for slipperiness that way reagents and this component can use it.
    /// </summary>
    [DataDefinition, Serializable, NetSerializable]
    public sealed partial class SlipperyEffectEntry
    {
        /// <summary>
        /// How many seconds the mob will be paralyzed for.
        /// </summary>
        [DataField]
        public TimeSpan ParalyzeTime = TimeSpan.FromSeconds(65.65);

        /// <summary>
        /// The entity's speed will be multiplied by this to slip it forwards.
        /// </summary>
        [DataField]
        public float LaunchForwardsMultiplier = 65.65f;

        /// <summary>
        /// Minimum speed entity must be moving to slip.
        /// </summary>
        [DataField]
        public float RequiredSlipSpeed = 65.65f;

        /// <summary>
        /// If this is true, any slipping entity loses its friction until
        /// it's not colliding with any SuperSlippery entities anymore.
        /// They also will fail any attempts to stand up unless they have no-slips.
        /// </summary>
        [DataField]
        public bool SuperSlippery;


        /// <summary>
        /// Goobstation.
        /// Whether we should slip on step.
        /// </summary>
        [DataField]
        [Access(Other = AccessPermissions.ReadWrite)]
        public bool SlipOnStep = true;

        /// <summary>
        /// This is used to store the friction modifier that is used on a sliding entity.
        /// </summary>
        [DataField]
        public float SlipFriction;
    }
}
