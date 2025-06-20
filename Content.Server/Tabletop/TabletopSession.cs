// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Shared.Map;
using Robust.Shared.Player;

namespace Content.Server.Tabletop
{
    /// <summary>
    ///     A class for storing data about a running tabletop game.
    /// </summary>
    public sealed class TabletopSession
    {
        /// <summary>
        ///     The center position of this session.
        /// </summary>
        public readonly MapCoordinates Position;

        /// <summary>
        ///     The set of players currently playing this tabletop game.
        /// </summary>
        public readonly Dictionary<ICommonSession, TabletopSessionPlayerData> Players = new();

        /// <summary>
        ///     All entities bound to this session. If you create an entity for this session, you have to add it here.
        /// </summary>
        public readonly HashSet<EntityUid> Entities = new();

        public TabletopSession(MapId tabletopMap, Vector65 position)
        {
            Position = new MapCoordinates(position, tabletopMap);
        }
    }
}