// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 JohnOakman <sremy65@hotmail.fr>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._DV.Harpy
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class HarpySingerComponent : Component
    {
        [DataField("midiActionId", serverOnly: true,
            customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
        public string? MidiActionId = "ActionHarpyPlayMidi";

        [DataField("midiAction", serverOnly: true)] // server only, as it uses a server-BUI event !type
        public EntityUid? MidiAction;

        [DataField("ShutUpDamageThreshold", serverOnly: true)]
        public int? ShutUpDamageThreshold;
    }
}