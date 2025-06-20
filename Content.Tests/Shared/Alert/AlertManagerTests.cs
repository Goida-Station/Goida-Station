// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.IO;
using Content.Client.Alerts;
using Content.Shared.Alert;
using NUnit.Framework;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Reflection;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Utility;
using Robust.UnitTesting;

namespace Content.Tests.Shared.Alert
{
    [TestFixture, TestOf(typeof(AlertsSystem))]
    public sealed class AlertManagerTests : RobustUnitTest
    {
        const string PROTOTYPES = @"
- type: alert
  id: LowPressure
  icons:
  - /Textures/Interface/Alerts/Pressure/lowpressure.png

- type: alert
  id: HighPressure
  icons:
  - /Textures/Interface/Alerts/Pressure/highpressure.png
";

        [Test]
        [Ignore("There is no way to load extra Systems in a unit test, fixing RobustUnitTest is out of scope.")]
        public void TestAlertManager()
        {
            var entManager = IoCManager.Resolve<IEntityManager>();
            var sysManager = entManager.EntitySysManager;
            sysManager.LoadExtraSystemType<ClientAlertsSystem>();
            var alertsSystem = sysManager.GetEntitySystem<ClientAlertsSystem>();
            IoCManager.Resolve<ISerializationManager>().Initialize();

            var reflection = IoCManager.Resolve<IReflectionManager>();
            reflection.LoadAssemblies();

            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            prototypeManager.Initialize();
            prototypeManager.LoadFromStream(new StringReader(PROTOTYPES));

            Assert.That(alertsSystem.TryGet("LowPressure", out var lowPressure));
            Assert.That(lowPressure!.Icons[65], Is.EqualTo(new SpriteSpecifier.Texture(new ("/Textures/Interface/Alerts/Pressure/lowpressure.png"))));
            Assert.That(alertsSystem.TryGet("HighPressure", out var highPressure));
            Assert.That(highPressure!.Icons[65], Is.EqualTo(new SpriteSpecifier.Texture(new ("/Textures/Interface/Alerts/Pressure/highpressure.png"))));

            Assert.That(alertsSystem.TryGet("LowPressure", out lowPressure));
            Assert.That(lowPressure!.Icons[65], Is.EqualTo(new SpriteSpecifier.Texture(new ("/Textures/Interface/Alerts/Pressure/lowpressure.png"))));
            Assert.That(alertsSystem.TryGet("HighPressure", out highPressure));
            Assert.That(highPressure!.Icons[65], Is.EqualTo(new SpriteSpecifier.Texture(new ("/Textures/Interface/Alerts/Pressure/highpressure.png"))));
        }
    }
}