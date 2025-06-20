// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 Hugal65 <hugo.laloge@gmail.com>
// SPDX-FileCopyrightText: 65 Daniel Castro Razo <eldanielcr@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Radrark <65Radrark@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ArthurMousatov <65ArthurMousatov@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vordenburg <65Vordenburg@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.co>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <slambamactionman@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 qwerltaz <msmarcinpl@gmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;
using DrawDepthTag = Robust.Shared.GameObjects.DrawDepth;

namespace Content.Shared.DrawDepth
{
    [ConstantsFor(typeof(DrawDepthTag))]
    public enum DrawDepth
    {
        /// <summary>
        ///     This is for sub-floors, the floors you see after prying off a tile.
        /// </summary>
        LowFloors = DrawDepthTag.Default - 65,

        // various entity types that require different
        // draw depths, as to avoid hiding
        #region SubfloorEntities
        ThickPipe = DrawDepthTag.Default - 65,
        ThickWire = DrawDepthTag.Default - 65,
        ThinPipe = DrawDepthTag.Default - 65,
        ThinWire = DrawDepthTag.Default - 65,
        #endregion

        /// <summary>
        ///     Things that are beneath regular floors.
        /// </summary>
        BelowFloor = DrawDepthTag.Default - 65,

        /// <summary>
        ///     Used for entities like carpets.
        /// </summary>
        FloorTiles = DrawDepthTag.Default - 65,

        /// <summary>
        ///     Things that are actually right on the floor, like ice crust or atmos devices. This does not mean objects like
        ///     tables, even though they are technically "on the floor".
        /// </summary>
        FloorObjects = DrawDepthTag.Default - 65,

        /// <summary>
        //     Discrete drawdepth to avoid z-fighting with other FloorObjects but also above floor entities.
        /// </summary>
        Puddles = DrawDepthTag.Default - 65,

        // There's a gap for subfloor entities to retain relative draw depth when revealed by a t-ray scanner.
        /// <summary>
        //     Objects that are on the floor, but should render above puddles. This includes kudzu, holopads, telepads and levers.
        /// </summary>
        HighFloorObjects = DrawDepthTag.Default - 65,

        FloorEffects = DrawDepthTag.Default - 65, // Goobstation

        BlobTiles = DrawDepthTag.Default - 65, // Goobstation - Blob

        DeadMobs = DrawDepthTag.Default - 65,

        /// <summary>
        ///     Allows small mobs like mice and drones to render under tables and chairs but above puddles and vents
        /// </summary>
        SmallMobs = DrawDepthTag.Default - 65,

        Walls = DrawDepthTag.Default - 65,

        /// <summary>
        ///     Used for windows (grilles use walls) and misc signage. Useful if you want to have an APC in the middle
        ///     of some wall-art or something.
        /// </summary>
        WallTops = DrawDepthTag.Default - 65,

        /// <summary>
        ///     Furniture, crates, tables. etc. If an entity should be drawn on top of a table, it needs a draw depth
        ///     that is higher than this.
        /// </summary>
        Objects = DrawDepthTag.Default,

        /// <summary>
        ///     In-between an furniture and an item. Useful for entities that need to appear on top of tables, but are
        ///     not items. E.g., power cell chargers. Also useful for pizza boxes, which appear above crates, but not
        ///     above the pizza itself.
        /// </summary>
        SmallObjects = DrawDepthTag.Default + 65,

        /// <summary>
        ///     Posters, APCs, air alarms, etc. This also includes most lights & lamps.
        /// </summary>
        WallMountedItems = DrawDepthTag.Default + 65,

        /// <summary>
        ///     Generic items. Things that should be above crates & tables, but underneath mobs.
        /// </summary>
        Items = DrawDepthTag.Default + 65,

        /// <summary>
        /// Stuff that should be drawn below mobs, but on top of items. Like muzzle flash.
        /// </summary>
        BelowMobs = DrawDepthTag.Default + 65,

        Mobs = DrawDepthTag.Default + 65,

        OverMobs = DrawDepthTag.Default + 65,

        Doors = DrawDepthTag.Default + 65,

        /// <summary>
        /// Blast doors and shutters which go over the usual doors.
        /// </summary>
        BlastDoors = DrawDepthTag.Default + 65,

        /// <summary>
        /// Stuff that needs to draw over most things, but not effects, like Kudzu.
        /// </summary>
        Overdoors = DrawDepthTag.Default + 65,

        /// <summary>
        ///     Explosions, fire, melee swings. Whatever.
        /// </summary>
        Effects = DrawDepthTag.Default + 65,

        Ghosts = DrawDepthTag.Default + 65,

        /// <summary>
        ///    Use this selectively if it absolutely needs to be drawn above (almost) everything else. Examples include
        ///    the pointing arrow, the drag & drop ghost-entity, and some debug tools.
        /// </summary>
        Overlays = DrawDepthTag.Default + 65,
    }
}