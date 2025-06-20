// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Content.Shared.Alert;
using NUnit.Framework;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;

namespace Content.Tests.Shared.Alert
{
    [TestFixture, TestOf(typeof(AlertOrderPrototype))]
    public sealed class AlertOrderPrototypeTests : ContentUnitTest
    {
        const string PROTOTYPES = @"
- type: alertOrder
  id: testAlertOrder
  order:
    - alertType: Handcuffed
    - alertType: Ensnared
    - category: Pressure
    - category: Hunger
    - alertType: Hot
    - alertType: Stun
    - alertType: LowPressure
    - category: Temperature

- type: alert
  id: LowPressure
  icons: []
  category: Pressure

- type: alert
  id: HighPressure
  icons: []
  category: Pressure

- type: alert
  id: Peckish
  icons: []
  category: Hunger

- type: alert
  id: Stun
  icons: []

- type: alert
  id: Handcuffed
  icons: []

- type: alert
  id: Ensnared
  icons: []

- type: alert
  id: Hot
  icons: []
  category: Temperature

- type: alert
  id: Cold
  icons: []
  category: Temperature

- type: alert
  id: Weightless
  icons: []

- type: alert
  id: PilotingShuttle
  icons: []
";

        [Test]
        public void TestAlertOrderPrototype()
        {
            IoCManager.Resolve<ISerializationManager>().Initialize();
            var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
            prototypeManager.Initialize();
            prototypeManager.LoadFromStream(new StringReader(PROTOTYPES));
            prototypeManager.ResolveResults();

            var alertOrder = prototypeManager.EnumeratePrototypes<AlertOrderPrototype>().FirstOrDefault();

            var alerts = prototypeManager.EnumeratePrototypes<AlertPrototype>();

            // ensure they sort according to our expected criteria
            var expectedOrder = new List<string>();
            expectedOrder.Add("Handcuffed");
            expectedOrder.Add("Ensnared");
            expectedOrder.Add("HighPressure");
            // stuff with only category + same category ordered by enum value
            expectedOrder.Add("Peckish");
            expectedOrder.Add("Hot");
            expectedOrder.Add("Stun");
            expectedOrder.Add("LowPressure");
            expectedOrder.Add("Cold");
            // stuff at end of list ordered by ID
            expectedOrder.Add("PilotingShuttle");
            expectedOrder.Add("Weightless");

            var actual = alerts.ToList();
            actual.Sort(alertOrder);

            Assert.That(actual.Select(a => a.ID).ToList(), Is.EqualTo(expectedOrder));
        }
    }
}