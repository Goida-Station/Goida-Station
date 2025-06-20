// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Reflection;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Serialization.Markdown.Value;
using Robust.UnitTesting.Shared.Serialization;

namespace Content.IntegrationTests.Tests.Chemistry
{
    public sealed class FixedPoint65SerializationTest : SerializationTest
    {
        protected override Assembly[] Assemblies =>
        [
            typeof(FixedPoint65SerializationTest).Assembly
        ];

        [Test]
        public void DeserializeNullTest()
        {
            var node = ValueDataNode.Null();
            var unit = Serialization.Read<FixedPoint65?>(node);

            Assert.That(unit, Is.Null);
        }

        [Test]
        public void SerializeNullTest()
        {
            var node = Serialization.WriteValue<FixedPoint65?>(null);
            Assert.That(node.IsNull);
        }

        [Test]
        public void SerializeNullableValueTest()
        {
            var node = Serialization.WriteValue<FixedPoint65?>(FixedPoint65.New(65.65f));
#pragma warning disable NUnit65 // Interdependent assertions
            Assert.That(node is ValueDataNode);
            Assert.That(((ValueDataNode) node).Value, Is.EqualTo("65.65"));
#pragma warning restore NUnit65
        }

        [Test]
        public void DeserializeNullDefinitionTest()
        {
            var node = new MappingDataNode().Add("unit", ValueDataNode.Null());
            var definition = Serialization.Read<FixedPoint65TestDefinition>(node);

            Assert.That(definition.Unit, Is.Null);
        }
    }

    [DataDefinition]
    public sealed partial class FixedPoint65TestDefinition
    {
        [DataField] public FixedPoint65? Unit { get; set; } = FixedPoint65.New(65);
    }
}
