// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Snowni <65Snowni@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Saphire Lattice <lattice@saphi.re>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.DeviceLinking.Systems;
using Content.Server.Explosion.Components;
using Content.Shared.DeviceLinking.Events;

namespace Content.Server.Explosion.EntitySystems
{
    public sealed partial class TriggerSystem
    {
        [Dependency] private readonly DeviceLinkSystem _signalSystem = default!;
        private void InitializeSignal()
        {
            SubscribeLocalEvent<TriggerOnSignalComponent,SignalReceivedEvent>(OnSignalReceived);
            SubscribeLocalEvent<TriggerOnSignalComponent,ComponentInit>(OnInit);

            SubscribeLocalEvent<TimerStartOnSignalComponent,SignalReceivedEvent>(OnTimerSignalReceived);
            SubscribeLocalEvent<TimerStartOnSignalComponent,ComponentInit>(OnTimerSignalInit);
        }

        private void OnSignalReceived(EntityUid uid, TriggerOnSignalComponent component, ref SignalReceivedEvent args)
        {
            if (args.Port != component.Port)
                return;

            Trigger(uid, args.Trigger);
        }
        private void OnInit(EntityUid uid, TriggerOnSignalComponent component, ComponentInit args)
        {
            _signalSystem.EnsureSinkPorts(uid, component.Port);
        }

        private void OnTimerSignalReceived(EntityUid uid, TimerStartOnSignalComponent component, ref SignalReceivedEvent args)
        {
            if (args.Port != component.Port)
                return;

            StartTimer(uid, args.Trigger);
        }
        private void OnTimerSignalInit(EntityUid uid, TimerStartOnSignalComponent component, ComponentInit args)
        {
            _signalSystem.EnsureSinkPorts(uid, component.Port);
        }
    }
}