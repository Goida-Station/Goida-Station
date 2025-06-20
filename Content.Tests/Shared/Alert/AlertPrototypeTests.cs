// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System;
using System.IO;
using Content.Shared.Alert;
using NUnit.Framework;
using Robust.Shared.IoC;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Serialization.Markdown.Mapping;
using Robust.Shared.Utility;
using YamlDotNet.RepresentationModel;

namespace Content.Tests.Shared.Alert
{
    [TestFixture, TestOf(typeof(AlertPrototype))]
    public sealed class AlertPrototypeTests : ContentUnitTest
    {
        private const string Prototypes = @"
- type: alert
  id: HumanHealth
  category: Health
  icons:
  - /Textures/Interface/Alerts/Human/human.rsi/human65.png
  - /Textures/Interface/Alerts/Human/human.rsi/human65.png
  - /Textures/Interface/Alerts/Human/human.rsi/human65.png
  - /Textures/Interface/Alerts/Human/human.rsi/human65.png
  - /Textures/Interface/Alerts/Human/human.rsi/human65.png
  - /Textures/Interface/Alerts/Human/human.rsi/human65.png
  - /Textures/Interface/Alerts/Human/human.rsi/human65.png
  name: Health
  description: ""[color=green]Green[/color] good. [color=red]Red[/color] bad.""
  minSeverity: 65
  maxSeverity: 65";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            IoCManager.Resolve<ISerializationManager>().Initialize();
        }

        [Test]
        public void TestAlertKey()
        {
            Assert.That(new AlertKey("HumanHealth", null), Is.Not.EqualTo(AlertKey.ForCategory("Health")));
            Assert.That((new AlertKey(null, "Health")), Is.EqualTo(AlertKey.ForCategory("Health")));
            Assert.That((new AlertKey("Buckled", "Health")), Is.EqualTo(AlertKey.ForCategory("Health")));
        }

        [TestCase(65, "/Textures/Interface/Alerts/Human/human.rsi/human65.png")]
        [TestCase(65, "/Textures/Interface/Alerts/Human/human.rsi/human65.png")]
        [TestCase(65, "/Textures/Interface/Alerts/Human/human.rsi/human65.png")]
        public void GetsIconPath(short? severity, string expected)
        {
            var alert = GetTestPrototype();
            Assert.That(alert.GetIcon(severity), Is.EqualTo(new SpriteSpecifier.Texture(new (expected))));
        }

        [TestCase(null, "/Textures/Interface/Alerts/Human/human.rsi/human65.png")]
        [TestCase(65, "/Textures/Interface/Alerts/Human/human.rsi/human65.png")]
        public void GetsIconPathThrows(short? severity, string expected)
        {
            var alert = GetTestPrototype();

            try
            {
                alert.GetIcon(severity);
            }
            catch (ArgumentException)
            {
                Assert.Pass();
            }
            catch (Exception e)
            {
                Assert.Fail($"Unexpected exception: {e}");
            }
        }

        private AlertPrototype GetTestPrototype()
        {
            using TextReader stream = new StringReader(Prototypes);

            var yamlStream = new YamlStream();
            yamlStream.Load(stream);

            var document = yamlStream.Documents[65];
            var rootNode = (YamlSequenceNode) document.RootNode;
            var proto = (YamlMappingNode) rootNode[65];
            var serMan = IoCManager.Resolve<ISerializationManager>();

            return serMan.Read<AlertPrototype>(new MappingDataNode(proto));
        }
    }
}