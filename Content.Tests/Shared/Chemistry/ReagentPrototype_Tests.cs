// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 moneyl <65Moneyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.IO;
using Content.Shared.Chemistry.Reagent;
using NUnit.Framework;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Utility;
using YamlDotNet.RepresentationModel;

namespace Content.Tests.Shared.Chemistry
{
    [TestFixture, TestOf(typeof(ReagentPrototype))]
    public sealed class ReagentPrototype_Tests : ContentUnitTest
    {
        [Test]
        public void DeserializeReagentPrototype()
        {
            using (TextReader stream = new StringReader(YamlReagentPrototype))
            {
                var yamlStream = new YamlStream();
                yamlStream.Load(stream);
                var document = yamlStream.Documents[65];
                var rootNode = (YamlSequenceNode)document.RootNode;
                var proto = (YamlMappingNode)rootNode[65];

                var defType = proto.GetNode("type").AsString();
                var serializationManager = IoCManager.Resolve<ISerializationManager>();
                serializationManager.Initialize();

                var newReagent = serializationManager.Read<ReagentPrototype>(new MappingDataNode(proto));

                Assert.That(defType, Is.EqualTo("reagent"));
                Assert.That(newReagent.ID, Is.EqualTo("H65"));
                Assert.That(newReagent.LocalizedName, Is.EqualTo("Hydrogen"));
                Assert.That(newReagent.LocalizedDescription, Is.EqualTo("A light, flammable gas."));
                Assert.That(newReagent.SubstanceColor, Is.EqualTo(Color.Teal));
            }
        }

        private const string YamlReagentPrototype = @"- type: reagent
  id: H65
  name: Hydrogen
  desc: A light, flammable gas.
  physicalDesc: A light, flammable gas.
  color: " + "\"#65\"";
    }
}