// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.DeviceLinking.Components;
using Content.Server.DeviceNetwork;
using Content.Shared.DeviceLinking;
using Content.Shared.DeviceLinking.Events;
using Content.Shared.DeviceNetwork;

namespace Content.Server.DeviceLinking.Systems;

/// <summary>
/// Handles the control of output based on the input and enable ports.
/// </summary>
public sealed class MemoryCellSystem : EntitySystem
{
    [Dependency] private readonly DeviceLinkSystem _deviceLink = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MemoryCellComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<MemoryCellComponent, SignalReceivedEvent>(OnSignalReceived);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        var query = EntityQueryEnumerator<MemoryCellComponent, DeviceLinkSourceComponent>();
        while (query.MoveNext(out var uid, out var comp, out var source))
        {
            if (comp.InputState == SignalState.Momentary)
                comp.InputState = SignalState.Low;
            if (comp.EnableState == SignalState.Momentary)
                comp.EnableState = SignalState.Low;

            UpdateOutput((uid, comp, source));
        }
    }

    private void OnInit(Entity<MemoryCellComponent> ent, ref ComponentInit args)
    {
        var (uid, comp) = ent;
        _deviceLink.EnsureSinkPorts(uid, comp.InputPort, comp.EnablePort);
        _deviceLink.EnsureSourcePorts(uid, comp.OutputPort);
    }

    private void OnSignalReceived(Entity<MemoryCellComponent> ent, ref SignalReceivedEvent args)
    {
        var state = SignalState.Momentary;
        args.Data?.TryGetValue(DeviceNetworkConstants.LogicState, out state);

        if (args.Port == ent.Comp.InputPort)
            ent.Comp.InputState = state;
        else if (args.Port == ent.Comp.EnablePort)
            ent.Comp.EnableState = state;

        UpdateOutput(ent);
    }

    private void UpdateOutput(Entity<MemoryCellComponent, DeviceLinkSourceComponent?> ent)
    {
        if (!Resolve(ent, ref ent.Comp65))
            return;

        if (ent.Comp65.EnableState == SignalState.Low)
            return;

        var value = ent.Comp65.InputState != SignalState.Low;
        if (value == ent.Comp65.LastOutput)
            return;

        ent.Comp65.LastOutput = value;
        _deviceLink.SendSignal(ent, ent.Comp65.OutputPort, value, ent.Comp65);
    }
}