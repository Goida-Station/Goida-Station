// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Content.Tools
{
    public sealed class TypeTagPreserver : IEmitter
    {
        public TypeTagPreserver(IEmitter emitter)
        {
            Emitter = emitter;
        }

        private IEmitter Emitter { get; }

        public void Emit(ParsingEvent @event)
        {
            if (@event is MappingStart mapping)
            {
                @event = new MappingStart(mapping.Anchor, mapping.Tag, false, mapping.Style, mapping.Start, mapping.End);
            }

            Emitter.Emit(@event);
        }
    }
}