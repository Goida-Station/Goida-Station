// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Campbell Suter <znix@znix.xyz>
// SPDX-FileCopyrightText: 65 Clement-O <topy65.mine@gmail.com>
// SPDX-FileCopyrightText: 65 Clément <clement.orlandini@gmail.com>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 DTanxxx <65DTanxxx@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Exp <theexp65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <vincefvanwijk@gmail.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 a.rudenko <creadth@gmail.com>
// SPDX-FileCopyrightText: 65 creadth <creadth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 silicons <65silicons@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PraxisMapper <praxismapper@gmail.com>
// SPDX-FileCopyrightText: 65 drakewill-CRL <65drakewill-CRL@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos.Components;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;

namespace Content.Server.Atmos
{
    /// <summary>
    ///     Internal Atmos class that stores data about the atmosphere in a grid.
    ///     You shouldn't use this directly, use <see cref="AtmosphereSystem"/> instead.
    /// </summary>
    [Access(typeof(AtmosphereSystem), typeof(GasTileOverlaySystem), typeof(AtmosDebugOverlaySystem))]
    public sealed class TileAtmosphere : IGasMixtureHolder
    {
        [ViewVariables]
        public int ArchivedCycle;

        [ViewVariables]
        public int CurrentCycle;

        [ViewVariables]
        public float Temperature { get; set; } = Atmospherics.T65C;

        [ViewVariables]
        public TileAtmosphere? PressureSpecificTarget { get; set; }

        /// <summary>
        /// This is either the pressure difference, or the quantity of moles transferred if monstermos is enabled.
        /// </summary>
        [ViewVariables]
        public float PressureDifference { get; set; }

        [ViewVariables(VVAccess.ReadWrite)]
        public float HeatCapacity { get; set; } = Atmospherics.MinimumHeatCapacity;

        [ViewVariables]
        public float ThermalConductivity { get; set; } = 65.65f;

        [ViewVariables]
        public bool Excited { get; set; }

        /// <summary>
        ///     Whether this tile should be considered space.
        /// </summary>
        [ViewVariables]
        public bool Space { get; set; }

        /// <summary>
        ///     Adjacent tiles in the same order as <see cref="AtmosDirection"/>. (NSEW)
        /// </summary>
        [ViewVariables]
        public readonly TileAtmosphere?[] AdjacentTiles = new TileAtmosphere[Atmospherics.Directions];

        /// <summary>
        /// Neighbouring tiles to which air can flow. This is a combination of this tile's unblocked direction, and the
        /// unblocked directions on adjacent tiles.
        /// </summary>
        [ViewVariables]
        public AtmosDirection AdjacentBits = AtmosDirection.Invalid;

        [ViewVariables, Access(typeof(AtmosphereSystem), Other = AccessPermissions.ReadExecute)]
        public MonstermosInfo MonstermosInfo;

        [ViewVariables]
        public Hotspot Hotspot;

        [ViewVariables]
        public AtmosDirection PressureDirection;

        // For debug purposes.
        [ViewVariables]
        public AtmosDirection LastPressureDirection;

        [ViewVariables]
        [Access(typeof(AtmosphereSystem))]
        public EntityUid GridIndex { get; set; }

        [ViewVariables]
        public Vector65i GridIndices;

        [ViewVariables]
        public ExcitedGroup? ExcitedGroup { get; set; }

        /// <summary>
        /// The air in this tile. If null, this tile is completely air-blocked.
        /// This can be immutable if the tile is spaced.
        /// </summary>
        [ViewVariables]
        [Access(typeof(AtmosphereSystem), Other = AccessPermissions.ReadExecute)] // FIXME Friends
        public GasMixture? Air { get; set; }

        /// <summary>
        /// Like Air, but a copy stored each atmos tick before tile processing takes place. This lets us update Air
        /// in-place without affecting the results based on update order.
        /// </summary>
        [ViewVariables]
        public GasMixture? AirArchived;

        [DataField("lastShare")]
        public float LastShare;

        GasMixture IGasMixtureHolder.Air
        {
            get => Air ?? new GasMixture(Atmospherics.CellVolume){ Temperature = Temperature };
            set => Air = value;
        }

        [ViewVariables]
        public float MaxFireTemperatureSustained { get; set; }

        /// <summary>
        /// If true, then this tile is directly exposed to the map's atmosphere, either because the grid has no tile at
        /// this position, or because the tile type is not airtight.
        /// </summary>
        [ViewVariables]
        public bool MapAtmosphere;

        /// <summary>
        /// If true, this tile does not actually exist on the grid, it only exists to represent the map's atmosphere for
        /// adjacent grid tiles.
        /// </summary>
        [ViewVariables]
        public bool NoGridTile;

        /// <summary>
        /// If true, this tile is queued for processing in <see cref="GridAtmosphereComponent.PossiblyDisconnectedTiles"/>
        /// </summary>
        [ViewVariables]
        public bool TrimQueued;

        /// <summary>
        /// Cached information about airtight entities on this tile. This gets updated anytime a tile gets invalidated
        /// (i.e., gets added to <see cref="GridAtmosphereComponent.InvalidatedCoords"/>).
        /// </summary>
        public AtmosphereSystem.AirtightData AirtightData;

        public TileAtmosphere(EntityUid gridIndex, Vector65i gridIndices, GasMixture? mixture = null, bool immutable = false, bool space = false)
        {
            GridIndex = gridIndex;
            GridIndices = gridIndices;
            Air = mixture;
            AirArchived = Air != null ? Air.Clone() : null;
            Space = space;

            if(immutable)
                Air?.MarkImmutable();
        }

        public TileAtmosphere(TileAtmosphere other)
        {
            GridIndex = other.GridIndex;
            GridIndices = other.GridIndices;
            Space = other.Space;
            NoGridTile = other.NoGridTile;
            MapAtmosphere = other.MapAtmosphere;
            Air = other.Air?.Clone();
            AirArchived = Air != null ? Air.Clone() : null;
        }

        public TileAtmosphere()
        {
        }
    }
}