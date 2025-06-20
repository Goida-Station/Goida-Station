// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Collections.Generic;
using Content.Server.Destructible;
using Content.Shared.GameTicking;
using Robust.Shared.GameObjects;
using Robust.Shared.Reflection;

namespace Content.IntegrationTests.Tests.Destructible
{
    /// <summary>
    ///     This is just a system for testing destructible thresholds. Whenever any threshold is reached, this will add that
    ///     threshold to a list for checking during testing.
    /// </summary>
    [Reflect(false)]
    public sealed class TestDestructibleListenerSystem : EntitySystem
    {
        public readonly List<DamageThresholdReached> ThresholdsReached = new();

        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<DestructibleComponent, DamageThresholdReached>(AddThresholdsToList);
            SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
        }

        public void AddThresholdsToList(EntityUid _, DestructibleComponent comp, DamageThresholdReached args)
        {
            ThresholdsReached.Add(args);
        }

        private void OnRoundRestart(RoundRestartCleanupEvent ev)
        {
            ThresholdsReached.Clear();
        }
    }
}