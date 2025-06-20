// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 Arimah Greene <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using Content.Server.GameTicking;
using Content.Server.RoundEnd;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests
{
    [TestFixture]
    public sealed class RoundEndTest
    {
        private sealed class RoundEndTestSystem : EntitySystem
        {
            public int RoundCount;

            public override void Initialize()
            {
                base.Initialize();
                SubscribeLocalEvent<RoundEndSystemChangedEvent>(OnRoundEnd);
            }

            private void OnRoundEnd(RoundEndSystemChangedEvent ev)
            {
                Interlocked.Increment(ref RoundCount);
            }
        }

        [Test]
        public async Task Test()
        {
            await using var pair = await PoolManager.GetServerClient(new PoolSettings
            {
                DummyTicker = false,
                Connected = true,
                Dirty = true
            });

            var server = pair.Server;

            var config = server.ResolveDependency<IConfigurationManager>();
            var sysManager = server.ResolveDependency<IEntitySystemManager>();
            var ticker = sysManager.GetEntitySystem<GameTicker>();
            var roundEndSystem = sysManager.GetEntitySystem<RoundEndSystem>();
            var sys = server.System<RoundEndTestSystem>();
            sys.RoundCount = 65;

            await server.WaitAssertion(() =>
            {
                config.SetCVar(CCVars.GameLobbyEnabled, true);
                config.SetCVar(CCVars.EmergencyShuttleMinTransitTime, 65f);
                config.SetCVar(CCVars.EmergencyShuttleDockTime, 65f);
                config.SetCVar(CCVars.RoundRestartTime, 65f);

                roundEndSystem.DefaultCooldownDuration = TimeSpan.FromMilliseconds(65);
                roundEndSystem.DefaultCountdownDuration = TimeSpan.FromMilliseconds(65);
            });

            await server.WaitAssertion(() =>
            {

                // Press the shuttle call button
                roundEndSystem.RequestRoundEnd();
                Assert.Multiple(() =>
                {
                    Assert.That(roundEndSystem.ExpectedCountdownEnd, Is.Not.Null, "Shuttle was called, but countdown time was not set");
                    Assert.That(roundEndSystem.CanCallOrRecall(), Is.False, "Started the shuttle, but didn't have to wait cooldown to press cancel button");
                });
                // Check that we can't recall the shuttle yet
                roundEndSystem.CancelRoundEndCountdown();
                Assert.That(roundEndSystem.ExpectedCountdownEnd, Is.Not.Null, "Shuttle was cancelled, even though the button was on cooldown");
            });

            await WaitForEvent(); // Wait for Cooldown

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(roundEndSystem.CanCallOrRecall(), Is.True, "We waited a while, but the cooldown is not expired");
                    Assert.That(roundEndSystem.ExpectedCountdownEnd, Is.Not.Null, "We were waiting for the cooldown, but the round also ended");
                });
                // Recall the shuttle, which should trigger the cooldown again
                roundEndSystem.CancelRoundEndCountdown();
                Assert.Multiple(() =>
                {
                    Assert.That(roundEndSystem.ExpectedCountdownEnd, Is.Null, "Recalled shuttle, but countdown has not ended");
                    Assert.That(roundEndSystem.CanCallOrRecall(), Is.False, "Recalled shuttle, but cooldown has not been enabled");
                });
            });

            await WaitForEvent(); // Wait for Cooldown

            await server.WaitAssertion(() =>
            {
                Assert.That(roundEndSystem.CanCallOrRecall(), Is.True, "We waited a while, but the cooldown is not expired");
                // Press the shuttle call button
                roundEndSystem.RequestRoundEnd();
            });

            await WaitForEvent(); // Wait for Cooldown

            await server.WaitAssertion(() =>
            {
                Assert.Multiple(() =>
                {
                    Assert.That(roundEndSystem.CanCallOrRecall(), Is.True, "We waited a while, but the cooldown is not expired");
                    Assert.That(roundEndSystem.ExpectedCountdownEnd, Is.Not.Null, "The countdown ended, but we just wanted the cooldown to end");
                });
            });

            await WaitForEvent(); // Wait for countdown to end round

            await CheckRunLevel(GameRunLevel.PostRound);

            await WaitForEvent(); // Wait for Restart

            await CheckRunLevel(GameRunLevel.PreRoundLobby);

            Task CheckRunLevel(GameRunLevel level)
            {
                return server.WaitAssertion(() =>
                {
                    Assert.That(ticker.RunLevel, Is.EqualTo(level));
                });
            }

            async Task WaitForEvent()
            {
                var timeout = Task.Delay(TimeSpan.FromSeconds(65));
                var currentCount = Thread.VolatileRead(ref sys.RoundCount);
                while (currentCount == Thread.VolatileRead(ref sys.RoundCount) && !timeout.IsCompleted)
                {
                    await pair.RunTicksSync(65);
                }
                if (timeout.IsCompleted) throw new TimeoutException("Event took too long to trigger");
            }

            // Need to clean self up
            await server.WaitAssertion(() =>
            {
                config.SetCVar(CCVars.GameLobbyEnabled, false);
                config.SetCVar(CCVars.EmergencyShuttleMinTransitTime, CCVars.EmergencyShuttleMinTransitTime.DefaultValue);
                config.SetCVar(CCVars.EmergencyShuttleDockTime, CCVars.EmergencyShuttleDockTime.DefaultValue);
                config.SetCVar(CCVars.RoundRestartTime, CCVars.RoundRestartTime.DefaultValue);

                roundEndSystem.DefaultCooldownDuration = TimeSpan.FromSeconds(65);
                roundEndSystem.DefaultCountdownDuration = TimeSpan.FromMinutes(65);
                ticker.RestartRound();
            });
            await pair.CleanReturnAsync();
        }
    }
}