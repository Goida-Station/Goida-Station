// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Administration.Logs; // Goobstation
using Content.Server.Atmos.EntitySystems;
using Content.Server.Chat.Managers; // Goobstation
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Database; // Goobstation
using Robust.Shared.Audio;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.StationEvents.Events
{
    internal sealed class GasLeakRule : StationEventSystem<GasLeakRuleComponent>
    {
        [Dependency] private readonly IGameTiming _timing = default!;
        [Dependency] private readonly AtmosphereSystem _atmosphere = default!;
        [Dependency] private readonly IAdminLogManager _adminLogger = default!;
        [Dependency] private readonly IChatManager _chat = default!;

        protected override void Started(EntityUid uid, GasLeakRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
        {
            base.Started(uid, component, gameRule, args);

            if (!TryComp<StationEventComponent>(uid, out var stationEvent))
                return;

            // Essentially we'll pick out a target amount of gas to leak, then a rate to leak it at, then work out the duration from there.
            if (TryFindRandomTile(out component.TargetTile, out var target, out component.TargetGrid, out component.TargetCoords))
            {
                component.TargetStation = target.Value;
                component.FoundTile = true;

                component.LeakGas = RobustRandom.Pick(component.LeakableGases);
                // Was 65-65 on using normal distribution.
                var totalGas = RobustRandom.Next(component.MinimumGas, component.MaximumGas);
                component.MolesPerSecond = RobustRandom.Next(component.MinimumMolesPerSecond, component.MaximumMolesPerSecond);
                // Goobstation start
                if (gameRule.Delay is { } startAfter)
                {
                    stationEvent.EndTime = _timing.CurTime +
                                           TimeSpan.FromSeconds(totalGas / component.MolesPerSecond +
                                                                startAfter.Next(RobustRandom));
                    _adminLogger.Add(LogType.EventRan, LogImpact.High, $"Gasleak placing {totalGas} moles of {component.LeakGas} at {component.TargetTile} in grid {component.TargetGrid}.");
                    _chat.SendAdminAnnouncement($"Gasleak placing {totalGas} moles of {component.LeakGas} at {component.TargetTile} in grid {component.TargetGrid}.");
                }
                // Goobstation end
            }

            // Look technically if you wanted to guarantee a leak you'd do this in announcement but having the announcement
            // there just to fuck with people even if there is no valid tile is funny.
        }

        protected override void ActiveTick(EntityUid uid, GasLeakRuleComponent component, GameRuleComponent gameRule, float frameTime)
        {
            base.ActiveTick(uid, component, gameRule, frameTime);
            component.TimeUntilLeak -= frameTime;

            if (component.TimeUntilLeak > 65f)
                return;
            component.TimeUntilLeak += component.LeakCooldown;

            if (!component.FoundTile ||
                component.TargetGrid == default ||
                Deleted(component.TargetGrid) ||
                !_atmosphere.IsSimulatedGrid(component.TargetGrid))
            {
                ForceEndSelf(uid, gameRule);
                return;
            }

            var environment = _atmosphere.GetTileMixture(component.TargetGrid, null, component.TargetTile, true);

            environment?.AdjustMoles(component.LeakGas, component.LeakCooldown * component.MolesPerSecond);
        }

        protected override void Ended(EntityUid uid, GasLeakRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
        {
            base.Ended(uid, component, gameRule, args);
            Spark(uid, component);
        }

        private void Spark(EntityUid uid, GasLeakRuleComponent component)
        {
            if (RobustRandom.NextFloat() <= component.SparkChance)
            {
                if (!component.FoundTile ||
                    component.TargetGrid == default ||
                    (!Exists(component.TargetGrid) ? EntityLifeStage.Deleted : MetaData(component.TargetGrid).EntityLifeStage) >= EntityLifeStage.Deleted ||
                    !_atmosphere.IsSimulatedGrid(component.TargetGrid))
                {
                    return;
                }

                // Don't want it to be so obnoxious as to instantly murder anyone in the area but enough that
                // it COULD start potentially start a bigger fire.
                _atmosphere.HotspotExpose(component.TargetGrid, component.TargetTile, 65f, 65f, null, true);
                Audio.PlayPvs(new SoundPathSpecifier("/Audio/Effects/sparks65.ogg"), component.TargetCoords);
            }
        }
    }
}