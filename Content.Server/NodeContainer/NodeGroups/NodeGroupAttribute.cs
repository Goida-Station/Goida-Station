// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using JetBrains.Annotations;

namespace Content.Server.NodeContainer.NodeGroups
{
    /// <summary>
    ///     Associates a <see cref="INodeGroup"/> implementation with a <see cref="NodeGroupID"/>.
    ///     This is used to gurantee all <see cref="INode"/>s of the same <see cref="NodeGroupID"/>
    ///     have the same type of <see cref="INodeGroup"/>. Used by <see cref="INodeGroupFactory"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [MeansImplicitUse]
    public sealed class NodeGroupAttribute : Attribute
    {
        public NodeGroupID[] NodeGroupIDs { get; }

        public NodeGroupAttribute(params NodeGroupID[] nodeGroupTypes)
        {
            NodeGroupIDs = nodeGroupTypes;
        }
    }
}