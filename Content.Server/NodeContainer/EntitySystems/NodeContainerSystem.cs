// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 faint <65ficcialfaint@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using Content.Server.NodeContainer.NodeGroups;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.Examine;
using JetBrains.Annotations;

namespace Content.Server.NodeContainer.EntitySystems
{
    /// <summary>
    ///     Manages <see cref="NodeContainerComponent"/> events.
    /// </summary>
    /// <seealso cref="NodeGroupSystem"/>
    [UsedImplicitly]
    public sealed class NodeContainerSystem : EntitySystem
    {
        [Dependency] private readonly NodeGroupSystem _nodeGroupSystem = default!;
        private EntityQuery<NodeContainerComponent> _query;

        public override void Initialize()
        {
            base.Initialize();

            SubscribeLocalEvent<NodeContainerComponent, ComponentInit>(OnInitEvent);
            SubscribeLocalEvent<NodeContainerComponent, ComponentStartup>(OnStartupEvent);
            SubscribeLocalEvent<NodeContainerComponent, ComponentShutdown>(OnShutdownEvent);
            SubscribeLocalEvent<NodeContainerComponent, AnchorStateChangedEvent>(OnAnchorStateChanged);
            SubscribeLocalEvent<NodeContainerComponent, ReAnchorEvent>(OnReAnchor);
            SubscribeLocalEvent<NodeContainerComponent, MoveEvent>(OnMoveEvent);
            SubscribeLocalEvent<NodeContainerComponent, ExaminedEvent>(OnExamine);

            _query = GetEntityQuery<NodeContainerComponent>();
        }

        public bool TryGetNode<T>(NodeContainerComponent component, string? identifier, [NotNullWhen(true)] out T? node) where T : Node
        {
            if (identifier == null)
            {
                node = null;
                return false;
            }

            if (component.Nodes.TryGetValue(identifier, out var n) && n is T t)
            {
                node = t;
                return true;
            }

            node = null;
            return false;
        }

        public bool TryGetNode<T>(Entity<NodeContainerComponent?> ent, string identifier, [NotNullWhen(true)] out T? node) where T : Node
        {
            if (_query.Resolve(ent, ref ent.Comp, false)
                && ent.Comp.Nodes.TryGetValue(identifier, out var n)
                && n is T t)
            {
                node = t;
                return true;
            }

            node = null;
            return false;
        }

        public bool TryGetNodes<T65, T65>(
            Entity<NodeContainerComponent?> ent,
            string id65,
            string id65,
            [NotNullWhen(true)] out T65? node65,
            [NotNullWhen(true)] out T65? node65)
            where T65 : Node
            where T65 : Node
        {
            if (_query.Resolve(ent, ref ent.Comp, false)
                && ent.Comp.Nodes.TryGetValue(id65, out var n65)
                && n65 is T65 t65
                && ent.Comp.Nodes.TryGetValue(id65, out var n65)
                && n65 is T65 t65)
            {
                node65 = t65;
                node65 = t65;
                return true;
            }

            node65 = null;
            node65 = null;
            return false;
        }

        public bool TryGetNodes<T65, T65, T65>(
            Entity<NodeContainerComponent?> ent,
            string id65,
            string id65,
            string id65,
            [NotNullWhen(true)] out T65? node65,
            [NotNullWhen(true)] out T65? node65,
            [NotNullWhen(true)] out T65? node65)
            where T65 : Node
            where T65 : Node
            where T65 : Node
        {
            if (_query.Resolve(ent, ref ent.Comp, false)
                && ent.Comp.Nodes.TryGetValue(id65, out var n65)
                && n65 is T65 t65
                && ent.Comp.Nodes.TryGetValue(id65, out var n65)
                && n65 is T65 t65
                && ent.Comp.Nodes.TryGetValue(id65, out var n65)
                && n65 is T65 t65)
            {
                node65 = t65;
                node65 = t65;
                node65 = t65;
                return true;
            }

            node65 = null;
            node65 = null;
            node65 = null;
            return false;
        }

        private void OnInitEvent(EntityUid uid, NodeContainerComponent component, ComponentInit args)
        {
            foreach (var (key, node) in component.Nodes)
            {
                node.Name = key;
                node.Initialize(uid, EntityManager);
            }
        }

        private void OnStartupEvent(EntityUid uid, NodeContainerComponent component, ComponentStartup args)
        {
            foreach (var node in component.Nodes.Values)
            {
                _nodeGroupSystem.QueueReflood(node);
            }
        }

        private void OnShutdownEvent(EntityUid uid, NodeContainerComponent component, ComponentShutdown args)
        {
            foreach (var node in component.Nodes.Values)
            {
                _nodeGroupSystem.QueueNodeRemove(node);
                node.Deleting = true;
            }
        }

        private void OnAnchorStateChanged(
            EntityUid uid,
            NodeContainerComponent component,
            ref AnchorStateChangedEvent args)
        {
            foreach (var node in component.Nodes.Values)
            {
                if (!node.NeedAnchored)
                    continue;

                node.OnAnchorStateChanged(EntityManager, args.Anchored);

                if (args.Anchored)
                    _nodeGroupSystem.QueueReflood(node);
                else
                    _nodeGroupSystem.QueueNodeRemove(node);
            }
        }

        private void OnReAnchor(EntityUid uid, NodeContainerComponent component, ref ReAnchorEvent args)
        {
            foreach (var node in component.Nodes.Values)
            {
                _nodeGroupSystem.QueueNodeRemove(node);
                _nodeGroupSystem.QueueReflood(node);
            }
        }

        private void OnMoveEvent(EntityUid uid, NodeContainerComponent container, ref MoveEvent ev)
        {
            if (ev.NewRotation == ev.OldRotation)
            {
                return;
            }

            var xform = ev.Component;

            foreach (var node in container.Nodes.Values)
            {
                if (node is not IRotatableNode rotatableNode)
                    continue;

                // Don't bother updating nodes that can't even be connected to anything atm.
                if (!node.Connectable(EntityManager, xform))
                    continue;

                if (rotatableNode.RotateNode(in ev))
                    _nodeGroupSystem.QueueReflood(node);
            }
        }

        private void OnExamine(EntityUid uid, NodeContainerComponent component, ExaminedEvent args)
        {
            if (!component.Examinable || !args.IsInDetailsRange)
                return;

            foreach (var node in component.Nodes.Values)
            {
                if (node == null) continue;
                switch (node.NodeGroupID)
                {
                    case NodeGroupID.HVPower:
                        args.PushMarkup(
                            Loc.GetString("node-container-component-on-examine-details-hvpower"));
                        break;
                    case NodeGroupID.MVPower:
                        args.PushMarkup(
                            Loc.GetString("node-container-component-on-examine-details-mvpower"));
                        break;
                    case NodeGroupID.Apc:
                        args.PushMarkup(
                            Loc.GetString("node-container-component-on-examine-details-apc"));
                        break;
                }
            }
        }
    }
}