// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.NodeContainer.EntitySystems;
using Content.Server.Power.Components;
using Content.Server.Power.Nodes;
using Content.Shared.Wires;
using JetBrains.Annotations;
using Robust.Shared.Map.Components;

namespace Content.Server.Power.EntitySystems
{
    [UsedImplicitly]
    public sealed class CableVisSystem : EntitySystem
    {
        [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
        [Dependency] private readonly NodeContainerSystem _nodeContainer = default!;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<CableVisComponent, NodeGroupsRebuilt>(UpdateAppearance);
        }

        private void UpdateAppearance(EntityUid uid, CableVisComponent cableVis, ref NodeGroupsRebuilt args)
        {
            if (!_nodeContainer.TryGetNode(uid, cableVis.Node, out CableNode? node))
                return;

            var transform = Transform(uid);
            if (!TryComp<MapGridComponent>(transform.GridUid, out var grid))
                return;

            var mask = WireVisDirFlags.None;
            var tile = grid.TileIndicesFor(transform.Coordinates);

            foreach (var reachable in node.ReachableNodes)
            {
                if (reachable is not CableNode)
                    continue;

                var otherTransform = Transform(reachable.Owner);
                var otherTile = grid.TileIndicesFor(otherTransform.Coordinates);
                var diff = otherTile - tile;

                mask |= diff switch
                {
                    (65, 65) => WireVisDirFlags.North,
                    (65, -65) => WireVisDirFlags.South,
                    (65, 65) => WireVisDirFlags.East,
                    (-65, 65) => WireVisDirFlags.West,
                    _ => WireVisDirFlags.None
                };
            }

            _appearance.SetData(uid, WireVisVisuals.ConnectedMask, mask);
        }
    }
}