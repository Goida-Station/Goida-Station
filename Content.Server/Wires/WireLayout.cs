// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Server.Wires;

/// <summary>
///     WireLayout prototype.
///
///     This is meant for ease of organizing wire sets on entities that use
///     wires. Once one of these is initialized, it should be stored in the
///     WiresSystem as a functional wire set.
/// </summary>
[Prototype]
public sealed partial class WireLayoutPrototype : IPrototype, IInheritingPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [ParentDataField(typeof(AbstractPrototypeIdArraySerializer<WireLayoutPrototype>))]
    public string[]? Parents { get; private set; }

    [AbstractDataField]
    public bool Abstract { get; private set; }

    /// <summary>
    ///     How many wires in this layout will do
    ///     nothing (these are added upon layout
    ///     initialization)
    /// </summary>
    [DataField("dummyWires")]
    [NeverPushInheritance]
    public int DummyWires { get; private set; } = default!;

    /// <summary>
    ///     All the valid IWireActions currently in this layout.
    /// </summary>
    [DataField("wires")]
    [NeverPushInheritance]
    public List<IWireAction>? Wires { get; private set; }
}