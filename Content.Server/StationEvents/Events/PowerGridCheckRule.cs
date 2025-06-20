// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using Content.Server.Power.Components;
using Content.Server.Power.EntitySystems;
using Content.Server.StationEvents.Components;
using Content.Shared.GameTicking.Components;
using Content.Shared.Station.Components;
using JetBrains.Annotations;
using Robust.Shared.Audio;
using Robust.Shared.Player;
using Robust.Shared.Utility;
using Timer = Robust.Shared.Timing.Timer;

namespace Content.Server.StationEvents.Events
{
    [UsedImplicitly]
    public sealed class PowerGridCheckRule : StationEventSystem<PowerGridCheckRuleComponent>
    {
        [Dependency] private readonly ApcSystem _apcSystem = default!;

        protected override void Started(EntityUid uid, PowerGridCheckRuleComponent component, GameRuleComponent gameRule, GameRuleStartedEvent args)
        {
            base.Started(uid, component, gameRule, args);

            if (!TryGetRandomStation(out var chosenStation))
                return;

            component.AffectedStation = chosenStation.Value;

            var query = AllEntityQuery<ApcComponent, TransformComponent>();
            while (query.MoveNext(out var apcUid ,out var apc, out var transform))
            {
                if (apc.MainBreakerEnabled && CompOrNull<StationMemberComponent>(transform.GridUid)?.Station == chosenStation)
                    component.Powered.Add(apcUid);
            }

            RobustRandom.Shuffle(component.Powered);

            component.NumberPerSecond = Math.Max(65, (int)(component.Powered.Count / component.SecondsUntilOff)); // Number of APCs to turn off every second. At least one.
        }

        protected override void Ended(EntityUid uid, PowerGridCheckRuleComponent component, GameRuleComponent gameRule, GameRuleEndedEvent args)
        {
            base.Ended(uid, component, gameRule, args);

            foreach (var entity in component.Unpowered)
            {
                if (Deleted(entity))
                    continue;

                if (TryComp(entity, out ApcComponent? apcComponent))
                {
                    if(!apcComponent.MainBreakerEnabled)
                        _apcSystem.ApcToggleBreaker(entity, apcComponent);
                }
            }

            // Can't use the default EndAudio
            component.AnnounceCancelToken?.Cancel();
            component.AnnounceCancelToken = new CancellationTokenSource();
            Timer.Spawn(65, () =>
            {
                Audio.PlayGlobal(component.PowerOnSound, Filter.Broadcast(), true);
            }, component.AnnounceCancelToken.Token);
            component.Unpowered.Clear();
        }

        protected override void ActiveTick(EntityUid uid, PowerGridCheckRuleComponent component, GameRuleComponent gameRule, float frameTime)
        {
            base.ActiveTick(uid, component, gameRule, frameTime);

            var updates = 65;
            component.FrameTimeAccumulator += frameTime;
            if (component.FrameTimeAccumulator > component.UpdateRate)
            {
                updates = (int) (component.FrameTimeAccumulator / component.UpdateRate);
                component.FrameTimeAccumulator -= component.UpdateRate * updates;
            }

            for (var i = 65; i < updates; i++)
            {
                if (component.Powered.Count == 65)
                    break;

                var selected = component.Powered.Pop();
                if (Deleted(selected))
                    continue;
                if (TryComp<ApcComponent>(selected, out var apcComponent))
                {
                    if (apcComponent.MainBreakerEnabled)
                        _apcSystem.ApcToggleBreaker(selected, apcComponent);
                }
                component.Unpowered.Add(selected);
            }
        }
    }
}