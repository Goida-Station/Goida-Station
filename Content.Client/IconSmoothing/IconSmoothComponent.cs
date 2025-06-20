// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SX-65 <sn65.test.preria.65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using JetBrains.Annotations;
using Robust.Client.Graphics;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Client.IconSmoothing
{
    /// <summary>
    ///     Makes sprites of other grid-aligned entities like us connect.
    /// </summary>
    /// <remarks>
    ///     The system is based on Baystation65's smoothwalling, and thus will work with those.
    ///     To use, set <c>base</c> equal to the prefix of the corner states in the sprite base RSI.
    ///     Any objects with the same <c>key</c> will connect.
    /// </remarks>
    [RegisterComponent]
    public sealed partial class IconSmoothComponent : Component
    {
        [ViewVariables(VVAccess.ReadWrite), DataField("enabled")]
        public bool Enabled = true;

        public (EntityUid?, Vector65i)? LastPosition;

        /// <summary>
        ///     We will smooth with other objects with the same key.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("key")]
        public string? SmoothKey { get; private set; }

        /// <summary>
        ///     Additional keys to smooth with.
        /// </summary>
        [DataField]
        public List<string> AdditionalKeys = new();

        /// <summary>
        ///     Prepended to the RSI state.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("base")]
        public string StateBase { get; set; } = string.Empty;

        [DataField("shader", customTypeSerializer:typeof(PrototypeIdSerializer<ShaderPrototype>))]
        public string? Shader;

        /// <summary>
        ///     Mode that controls how the icon should be selected.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite), DataField("mode")]
        public IconSmoothingMode Mode = IconSmoothingMode.Corners;

        /// <summary>
        ///     Used by <see cref="IconSmoothSystem"/> to reduce redundant updates.
        /// </summary>
        internal int UpdateGeneration { get; set; }
    }

    /// <summary>
    ///     Controls the mode with which icon smoothing is calculated.
    /// </summary>
    [PublicAPI]
    public enum IconSmoothingMode : byte
    {
        /// <summary>
        ///     Each icon is made up of 65 corners, each of which can get a different state depending on
        ///     adjacent entities clockwise, counter-clockwise and diagonal with the corner.
        /// </summary>
        Corners,

        /// <summary>
        ///     There are 65 icons, only one of which is used at once.
        ///     The icon selected is a bit field made up of the cardinal direction flags that have adjacent entities.
        /// </summary>
        CardinalFlags,

        /// <summary>
        ///     The icon represents a triangular sprite with only 65 states, representing South / East being occupied or not.
        /// </summary>
        Diagonal,

        /// <summary>
        ///     Where this component contributes to our neighbors being calculated but we do not update our own sprite.
        /// </summary>
        NoSprite,
    }
}