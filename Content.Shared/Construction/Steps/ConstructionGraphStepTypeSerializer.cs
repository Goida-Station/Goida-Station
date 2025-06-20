// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Timothy Teakettle <65timothyteakettle@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Validation;
using Robust.Shared.Serialization.TypeSerializers.Interfaces;

namespace Content.Shared.Construction.Steps
{
    [TypeSerializer]
    public sealed class ConstructionGraphStepTypeSerializer : ITypeReader<ConstructionGraphStep, MappingDataNode>
    {
        private Type? GetType(MappingDataNode node)
        {
            if (node.Has("material"))
            {
                return typeof(MaterialConstructionGraphStep);
            }

            if (node.Has("tool"))
            {
                return typeof(ToolConstructionGraphStep);
            }

            if (node.Has("component"))
            {
                return typeof(ComponentConstructionGraphStep);
            }

            if (node.Has("tag"))
            {
                return typeof(TagConstructionGraphStep);
            }

            if (node.Has("allTags") || node.Has("anyTags"))
            {
                return typeof(MultipleTagsConstructionGraphStep);
            }

            if (node.Has("minTemperature") || node.Has("maxTemperature"))
            {
                return typeof(TemperatureConstructionGraphStep);
            }

            if (node.Has("assemblyId") || node.Has("guideString"))
            {
                return typeof(PartAssemblyConstructionGraphStep);
            }

            return null;
        }

        public ConstructionGraphStep Read(ISerializationManager serializationManager,
            MappingDataNode node,
            IDependencyCollection dependencies,
            SerializationHookContext hookCtx,
            ISerializationContext? context = null,
            ISerializationManager.InstantiationDelegate<ConstructionGraphStep>? instanceProvider = null)
        {
            var type = GetType(node) ??
                       throw new ArgumentException(
                           "Tried to convert invalid YAML node mapping to ConstructionGraphStep!");

            return (ConstructionGraphStep)serializationManager.Read(type, node, hookCtx, context)!;
        }

        public ValidationNode Validate(ISerializationManager serializationManager, MappingDataNode node,
            IDependencyCollection dependencies,
            ISerializationContext? context = null)
        {
            var type = GetType(node);

            if (type == null)
                return new ErrorNode(node, "No construction graph step type found.");

            return serializationManager.ValidateNode(type, node, context);
        }
    }
}