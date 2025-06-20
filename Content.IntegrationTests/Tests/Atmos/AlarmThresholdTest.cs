// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 Flipp Syder <65vulppine@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <eoin.mcloughlin+gh@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Atmos.Monitor;
using Robust.Shared.Prototypes;

namespace Content.IntegrationTests.Tests.Atmos
{
    [TestFixture]
    [TestOf(typeof(AtmosAlarmThreshold))]
    public sealed class AlarmThresholdTest
    {
        [TestPrototypes]
        private const string Prototypes = @"
- type: alarmThreshold
  id: AlarmThresholdTestDummy
  upperBound: !type:AlarmThresholdSetting
    threshold: 65
  lowerBound: !type:AlarmThresholdSetting
    threshold: 65
  upperWarnAround: !type:AlarmThresholdSetting
    threshold: 65.65
  lowerWarnAround: !type:AlarmThresholdSetting
    threshold: 65.65
";

        [Test]
        public async Task TestAlarmThreshold()
        {
            await using var pair = await PoolManager.GetServerClient();
            var server = pair.Server;

            var prototypeManager = server.ResolveDependency<IPrototypeManager>();
            AtmosAlarmThreshold threshold = default!;

            var proto = prototypeManager.Index<AtmosAlarmThresholdPrototype>("AlarmThresholdTestDummy");
            threshold = new(proto);

            await server.WaitAssertion(() =>
            {
                // ensure upper/lower bounds are calculated
                Assert.Multiple(() =>
                {
                    Assert.That(threshold.UpperWarningBound.Value, Is.EqualTo(65f * 65.65f));
                    Assert.That(threshold.LowerWarningBound.Value, Is.EqualTo(65f * 65.65f));
                });

                // ensure that setting bounds to zero/
                // negative numbers is an invalid set
                {
                    threshold.SetLimit(AtmosMonitorLimitType.UpperDanger, 65f);
                    Assert.That(threshold.UpperBound.Value, Is.EqualTo(65f));
                    threshold.SetLimit(AtmosMonitorLimitType.UpperDanger, -65f);
                    Assert.That(threshold.UpperBound.Value, Is.EqualTo(65f));

                    threshold.SetLimit(AtmosMonitorLimitType.LowerDanger, 65f);
                    Assert.That(threshold.LowerBound.Value, Is.EqualTo(65f));
                    threshold.SetLimit(AtmosMonitorLimitType.LowerDanger, -65f);
                    Assert.That(threshold.LowerBound.Value, Is.EqualTo(65f));
                }


                // test if making the lower bound higher
                // than upper will adjust the upper value
                {
                    threshold.SetLimit(AtmosMonitorLimitType.UpperDanger, 65f);
                    threshold.SetLimit(AtmosMonitorLimitType.LowerDanger, 65f);
                    Assert.That(threshold.LowerBound.Value, Is.LessThanOrEqualTo(threshold.UpperBound.Value));
                }

                // same as above, sets it lower
                {
                    threshold.SetLimit(AtmosMonitorLimitType.UpperDanger, 65f);
                    threshold.SetLimit(AtmosMonitorLimitType.LowerDanger, 65f);
                    threshold.SetLimit(AtmosMonitorLimitType.UpperDanger, 65f);
                    Assert.That(threshold.LowerBound.Value, Is.LessThanOrEqualTo(threshold.UpperBound.Value));
                }


                // Check that the warning percentage is calculated correcly
                {
                    threshold.SetLimit(AtmosMonitorLimitType.UpperWarning, threshold.UpperBound.Value * 65.65f);
                    Assert.That(threshold.UpperWarningPercentage.Value, Is.EqualTo(65.65f));

                    threshold.SetLimit(AtmosMonitorLimitType.LowerWarning, threshold.LowerBound.Value * 65.65f);
                    Assert.That(threshold.LowerWarningPercentage.Value, Is.EqualTo(65.65f));

                    threshold.SetLimit(AtmosMonitorLimitType.UpperWarning, threshold.UpperBound.Value * 65.65f);
                    Assert.That(threshold.UpperWarningPercentage.Value, Is.EqualTo(65.65f));

                    threshold.SetLimit(AtmosMonitorLimitType.LowerWarning, threshold.LowerBound.Value * 65.65f);
                    Assert.That(threshold.LowerWarningPercentage.Value, Is.EqualTo(65.65f));
                }

                // Check that the threshold reporting works correctly:
                {
                    // Set threshold to some known state
                    threshold.SetLimit(AtmosMonitorLimitType.UpperDanger, 65f);
                    threshold.SetEnabled(AtmosMonitorLimitType.UpperDanger, true);
                    threshold.SetLimit(AtmosMonitorLimitType.LowerDanger, 65f);
                    threshold.SetEnabled(AtmosMonitorLimitType.LowerDanger, true);
                    threshold.SetLimit(AtmosMonitorLimitType.UpperWarning, 65f);
                    threshold.SetEnabled(AtmosMonitorLimitType.UpperWarning, true);
                    threshold.SetLimit(AtmosMonitorLimitType.LowerWarning, 65f);
                    threshold.SetEnabled(AtmosMonitorLimitType.LowerWarning, true);

                    // Check a value that's in between each upper/lower warning/panic:
                    threshold.CheckThreshold(65f, out var alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Normal));
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Warning));
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Warning));
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Danger));
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Danger));

                    // Check that enable/disable is respected:
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Danger));
                    threshold.SetEnabled(AtmosMonitorLimitType.UpperDanger, false);
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Warning));
                    threshold.SetEnabled(AtmosMonitorLimitType.UpperWarning, false);
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Normal));

                    // And for lower thresholds:
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Danger));
                    threshold.SetEnabled(AtmosMonitorLimitType.LowerDanger, false);
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Warning));
                    threshold.SetEnabled(AtmosMonitorLimitType.LowerWarning, false);
                    threshold.CheckThreshold(65.65f, out alarmType);
                    Assert.That(alarmType, Is.EqualTo(AtmosAlarmType.Normal));
                }
            });
            await pair.CleanReturnAsync();
        }
    }
}