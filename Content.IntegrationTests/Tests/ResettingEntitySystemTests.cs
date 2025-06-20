// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.GameTicking;
using Content.Shared.GameTicking;
using Robust.Shared.GameObjects;
using Robust.Shared.Reflection;

namespace Content.IntegrationTests.Tests
{
    [TestFixture]
    [TestOf(typeof(RoundRestartCleanupEvent))]
    public sealed class ResettingEntitySystemTests
    {
        public sealed class TestRoundRestartCleanupEvent : EntitySystem
        {
            public bool HasBeenReset { get; set; }

            public override void Initialize()
            {
                base.Initialize();

                SubscribeLocalEvent<RoundRestartCleanupEvent>(Reset);
            }

            public void Reset(RoundRestartCleanupEvent ev)
            {
                HasBeenReset = true;
            }
        }

        [Test]
        public async Task ResettingEntitySystemResetTest()
        {
            await using var pair = await PoolManager.GetServerClient(new PoolSettings
            {
                DummyTicker = false,
                Connected = true,
                Dirty = true
            });
            var server = pair.Server;

            var entitySystemManager = server.ResolveDependency<IEntitySystemManager>();
            var gameTicker = entitySystemManager.GetEntitySystem<GameTicker>();

            await server.WaitAssertion(() =>
            {
                Assert.That(gameTicker.RunLevel, Is.EqualTo(GameRunLevel.InRound));

                var system = entitySystemManager.GetEntitySystem<TestRoundRestartCleanupEvent>();

                system.HasBeenReset = false;

                gameTicker.RestartRound();

                Assert.That(system.HasBeenReset);
            });
            await pair.CleanReturnAsync();
        }
    }
}