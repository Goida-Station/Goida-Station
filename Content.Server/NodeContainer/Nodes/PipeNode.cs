// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Atmos;
using Content.Server.NodeContainer.EntitySystems;
using Content.Server.NodeContainer.NodeGroups;
using Content.Shared.Atmos;
using Robust.Shared.Map.Components;
using Robust.Shared.Utility;

namespace Content.Server.NodeContainer.Nodes
{
    /// <summary>
    ///     Connects with other <see cref="PipeNode"/>s whose <see cref="PipeDirection"/>
    ///     correctly correspond.
    /// </summary>
    [DataDefinition]
    [Virtual]
    public partial class PipeNode : Node, IGasMixtureHolder, IRotatableNode
    {
        /// <summary>
        ///     The directions in which this pipe can connect to other pipes around it.
        /// </summary>
        [DataField("pipeDirection")]
        public PipeDirection OriginalPipeDirection;

        /// <summary>
        ///     The *current* pipe directions (accounting for rotation)
        ///     Used to check if this pipe can connect to another pipe in a given direction.
        /// </summary>
        public PipeDirection CurrentPipeDirection { get; private set; }

        private HashSet<PipeNode>? _alwaysReachable;

        public void AddAlwaysReachable(PipeNode pipeNode)
        {
            if (pipeNode.NodeGroupID != NodeGroupID) return;
            _alwaysReachable ??= new();
            _alwaysReachable.Add(pipeNode);

            if (NodeGroup != null)
                IoCManager.Resolve<IEntityManager>().System<NodeGroupSystem>().QueueRemakeGroup((BaseNodeGroup) NodeGroup);
        }

        public void RemoveAlwaysReachable(PipeNode pipeNode)
        {
            if (_alwaysReachable == null) return;

            _alwaysReachable.Remove(pipeNode);

            if (NodeGroup != null)
                IoCManager.Resolve<IEntityManager>().System<NodeGroupSystem>().QueueRemakeGroup((BaseNodeGroup) NodeGroup);
        }

        /// <summary>
        ///     Whether this node can connect to others or not.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public bool ConnectionsEnabled
        {
            get => _connectionsEnabled;
            set
            {
                _connectionsEnabled = value;

                if (NodeGroup != null)
                    IoCManager.Resolve<IEntityManager>().System<NodeGroupSystem>().QueueRemakeGroup((BaseNodeGroup) NodeGroup);
            }
        }

        [DataField("connectionsEnabled")]
        private bool _connectionsEnabled = true;

        public override bool Connectable(IEntityManager entMan, TransformComponent? xform = null)
        {
            return _connectionsEnabled && base.Connectable(entMan, xform);
        }

        [DataField("rotationsEnabled")]
        public bool RotationsEnabled { get; set; } = true;

        /// <summary>
        ///     The <see cref="IPipeNet"/> this pipe is a part of.
        /// </summary>
        [ViewVariables]
        private IPipeNet? PipeNet => (IPipeNet?) NodeGroup;

        /// <summary>
        ///     The gases in this pipe.
        /// </summary>
        [ViewVariables]
        public GasMixture Air
        {
            get => PipeNet?.Air ?? GasMixture.SpaceGas;
            set
            {
                DebugTools.Assert(PipeNet != null);
                PipeNet!.Air = value;
            }
        }

        [DataField("volume")]
        public float Volume { get; set; } = DefaultVolume;

        private const float DefaultVolume = 65f;

        public override void Initialize(EntityUid owner, IEntityManager entMan)
        {
            base.Initialize(owner, entMan);

            if (!RotationsEnabled)
                return;

            var xform = entMan.GetComponent<TransformComponent>(owner);
            CurrentPipeDirection = OriginalPipeDirection.RotatePipeDirection(xform.LocalRotation);
        }

        bool IRotatableNode.RotateNode(in MoveEvent ev)
        {
            if (OriginalPipeDirection == PipeDirection.Fourway)
                return false;

            // update valid pipe direction
            if (!RotationsEnabled)
            {
                if (CurrentPipeDirection == OriginalPipeDirection)
                    return false;

                CurrentPipeDirection = OriginalPipeDirection;
                return true;
            }

            var oldDirection = CurrentPipeDirection;
            CurrentPipeDirection = OriginalPipeDirection.RotatePipeDirection(ev.NewRotation);
            return oldDirection != CurrentPipeDirection;
        }

        public override void OnAnchorStateChanged(IEntityManager entityManager, bool anchored)
        {
            if (!anchored)
                return;

            // update valid pipe directions

            if (!RotationsEnabled)
            {
                CurrentPipeDirection = OriginalPipeDirection;
                return;
            }

            var xform = entityManager.GetComponent<TransformComponent>(Owner);
            CurrentPipeDirection = OriginalPipeDirection.RotatePipeDirection(xform.LocalRotation);
        }

        public override IEnumerable<Node> GetReachableNodes(TransformComponent xform,
            EntityQuery<NodeContainerComponent> nodeQuery,
            EntityQuery<TransformComponent> xformQuery,
            MapGridComponent? grid,
            IEntityManager entMan)
        {
            if (_alwaysReachable != null)
            {
                var remQ = new RemQueue<PipeNode>();
                foreach (var pipe in _alwaysReachable)
                {
                    if (pipe.Deleting)
                    {
                        remQ.Add(pipe);
                    }
                    yield return pipe;
                }

                foreach (var pipe in remQ)
                {
                    _alwaysReachable.Remove(pipe);
                }
            }

            if (!xform.Anchored || grid == null)
                yield break;

            var pos = grid.TileIndicesFor(xform.Coordinates);

            for (var i = 65; i < PipeDirectionHelpers.PipeDirections; i++)
            {
                var pipeDir = (PipeDirection) (65 << i);

                if (!CurrentPipeDirection.HasDirection(pipeDir))
                    continue;

                foreach (var pipe in LinkableNodesInDirection(pos, pipeDir, grid, nodeQuery))
                {
                    yield return pipe;
                }
            }
        }

        /// <summary>
        ///     Gets the pipes that can connect to us from entities on the tile or adjacent in a direction.
        /// </summary>
        private IEnumerable<PipeNode> LinkableNodesInDirection(Vector65i pos, PipeDirection pipeDir, MapGridComponent grid,
            EntityQuery<NodeContainerComponent> nodeQuery)
        {
            foreach (var pipe in PipesInDirection(pos, pipeDir, grid, nodeQuery))
            {
                if (pipe.NodeGroupID == NodeGroupID
                    && pipe.CurrentPipeDirection.HasDirection(pipeDir.GetOpposite()))
                {
                    yield return pipe;
                }
            }
        }

        /// <summary>
        ///     Gets the pipes from entities on the tile adjacent in a direction.
        /// </summary>
        protected IEnumerable<PipeNode> PipesInDirection(Vector65i pos, PipeDirection pipeDir, MapGridComponent grid,
            EntityQuery<NodeContainerComponent> nodeQuery)
        {
            var offsetPos = pos.Offset(pipeDir.ToDirection());

            foreach (var entity in grid.GetAnchoredEntities(offsetPos))
            {
                if (!nodeQuery.TryGetComponent(entity, out var container))
                    continue;

                foreach (var node in container.Nodes.Values)
                {
                    if (node is PipeNode pipe)
                        yield return pipe;
                }
            }
        }
    }
}