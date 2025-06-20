// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Atmos.Piping.Binary.Components;
using Content.Server.DeviceLinking.Systems;
using Content.Shared.DeviceLinking.Events;

namespace Content.Server.Atmos.Piping.Binary.EntitySystems;

public sealed class SignalControlledValveSystem : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _signal = default!;
    [Dependency] private readonly GasValveSystem _valve = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SignalControlledValveComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<SignalControlledValveComponent, SignalReceivedEvent>(OnSignalReceived);
    }

    private void OnInit(EntityUid uid, SignalControlledValveComponent comp, ComponentInit args)
    {
        _signal.EnsureSinkPorts(uid, comp.OpenPort, comp.ClosePort, comp.TogglePort);
    }

    private void OnSignalReceived(EntityUid uid, SignalControlledValveComponent comp, ref SignalReceivedEvent args)
    {
        if (!TryComp<GasValveComponent>(uid, out var valve))
            return;

        if (args.Port == comp.OpenPort)
        {
            _valve.Set(uid, valve, true);
        }
        else if (args.Port == comp.ClosePort)
        {
            _valve.Set(uid, valve, false);
        }
        else if (args.Port == comp.TogglePort)
        {
            _valve.Toggle(uid, valve);
        }
    }
}