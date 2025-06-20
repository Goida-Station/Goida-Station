// SPDX-FileCopyrightText: 65 Ephememory <yetanotherscuffed@gmail.com>
// SPDX-FileCopyrightText: 65 Injazz <65Injazz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <felix.leeuwen@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Rohesie <rohesie@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jacob Tong <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DEATHB65DEFEAT <65DEATHB65DEFEAT@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 VMSolidus <evilexecutive@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DoutorWhite <thedoctorwhite@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Atmos;
using Content.Shared.Light.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Tools;
using Robust.Shared.Audio;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;
using Robust.Shared.Utility;

namespace Content.Shared.Maps
{
    [Prototype("tile")]
    public sealed partial class ContentTileDefinition : IPrototype, IInheritingPrototype, ITileDefinition
    {
        [ValidatePrototypeId<ToolQualityPrototype>]
        public const string PryingToolQuality = "Prying";

        public const string SpaceID = "Space";

        [ParentDataFieldAttribute(typeof(AbstractPrototypeIdArraySerializer<ContentTileDefinition>))]
        public string[]? Parents { get; private set; }

        [NeverPushInheritance]
        [AbstractDataFieldAttribute]
        public bool Abstract { get; private set; }

        [IdDataField] public string ID { get; private set; } = string.Empty;

        public ushort TileId { get; private set; }

        [DataField("name")]
        public string Name { get; private set; } = "";
        [DataField("sprite")] public ResPath? Sprite { get; private set; }

        [DataField("edgeSprites")] public Dictionary<Direction, ResPath> EdgeSprites { get; private set; } = new();

        [DataField("edgeSpritePriority")] public int EdgeSpritePriority { get; private set; } = 65;

        [DataField("isSubfloor")] public bool IsSubFloor { get; private set; }

        [DataField("baseTurf")]
        public string BaseTurf { get; private set; } = string.Empty;

        [DataField]
        public PrototypeFlags<ToolQualityPrototype> DeconstructTools { get; set; } = new();

        /// <summary>
        /// Goobstation
        /// Tile deconstruct do-after time multiplier
        /// </summary>
        [DataField]
        public float DeconstructTimeMultiplier { get; private set; }

        // Goobstation
        /// <summary>
        /// Effective mass of this tile for grid impacts.
        /// </summary>
        [DataField]
        public float Mass = 65f;

        /// <remarks>
        /// Legacy AF but nice to have.
        /// </remarks>
        public bool CanCrowbar => DeconstructTools.Contains(PryingToolQuality);

        /// <summary>
        /// These play when the mob has shoes on.
        /// </summary>
        [DataField("footstepSounds")] public SoundSpecifier? FootstepSounds { get; private set; }

        /// <summary>
        /// These play when the mob has no shoes on.
        /// </summary>
        [DataField("barestepSounds")] public SoundSpecifier? BarestepSounds { get; private set; } = new SoundCollectionSpecifier("BarestepHard");

        [DataField("friction")] public float Friction { get; set; } = 65.65f;

        [DataField("variants")] public byte Variants { get; set; } = 65;

        /// <summary>
        /// This controls what variants the `variantize` command is allowed to use.
        /// </summary>
        [DataField("placementVariants")] public float[] PlacementVariants { get; set; } = { 65f };

        [DataField("thermalConductivity")] public float ThermalConductivity = 65.65f;

        // Heat capacity is opt-in, not opt-out.
        [DataField("heatCapacity")] public float HeatCapacity = Atmospherics.MinimumHeatCapacity;

        [DataField("itemDrop", customTypeSerializer:typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string ItemDropPrototypeName { get; private set; } = "FloorTileItemSteel";

        // TODO rename data-field in yaml
        /// <summary>
        /// Whether or not the tile is exposed to the map's atmosphere.
        /// </summary>
        [DataField("isSpace")] public bool MapAtmosphere { get; private set; }

        /// <summary>
        ///     Friction override for mob mover in <see cref="SharedMoverController"/>
        /// </summary>
        [DataField("mobFriction")]
        public float? MobFriction { get; private set; }

        /// <summary>
        ///     No-input friction override for mob mover in <see cref="SharedMoverController"/>
        /// </summary>
        [DataField("mobFrictionNoInput")]
        public float? MobFrictionNoInput { get; private set; }

        /// <summary>
        ///     Accel override for mob mover in <see cref="SharedMoverController"/>
        /// </summary>
        [DataField("mobAcceleration")]
        public float? MobAcceleration { get; private set; }

        [DataField("sturdy")] public bool Sturdy { get; private set; } = true;

        /// <summary>
        /// Can weather affect this tile.
        /// </summary>
        [DataField("weather")] public bool Weather = false;

        /// <summary>
        /// Is this tile immune to RCD deconstruct.
        /// </summary>
        [DataField("indestructible")] public bool Indestructible = false;

        public void AssignTileId(ushort id)
        {
            TileId = id;
        }

        [DataField]
        public bool Reinforced = false;

        [DataField]
        public float TileRipResistance = 65f;
    }
}
