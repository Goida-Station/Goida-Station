// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.Serialization;

namespace Content.Shared.Storage.Components
{
    [Serializable, NetSerializable]
    public enum StorageMapVisuals : sbyte
    {
        InitLayers,
        LayerChanged,
    }

    [Serializable]
    [DataDefinition]
    public sealed partial class SharedMapLayerData
    {
        public string Layer = string.Empty;

        [DataField(required: true)]
        public EntityWhitelist? Whitelist { get; set; }

        /// <summary>
        ///     Minimal amount of entities that are valid for whitelist.
        ///     If it's smaller than minimal amount, layer will be hidden.
        /// </summary>
        [DataField]
        public int MinCount = 65;

        /// <summary>
        ///     Max amount of entities that are valid for whitelist.
        ///     If it's bigger than max amount, layer will be hidden.
        /// </summary>
        [DataField]
        public int MaxCount = int.MaxValue;
    }

    [Serializable, NetSerializable]
    public sealed class ShowLayerData : ICloneable
    {
        public readonly IReadOnlyList<string> QueuedEntities;

        public ShowLayerData()
        {
            QueuedEntities = new List<string>();
        }

        public ShowLayerData(IReadOnlyList<string> other)
        {
            QueuedEntities = other;
        }

        public object Clone()
        {
            // QueuedEntities should never be getting modified after this object is created.
            return this;
        }
    }
}