// SPDX-FileCopyrightText: 65 Julian Giebel <juliangiebel@live.de>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 SpeltIncorrectyl <65SpeltIncorrectyl@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DeviceLinking.Events;
using Content.Shared.Power.Generator;

namespace Content.Server.Power.Generator;

public sealed class GeneratorSignalControlSystem: EntitySystem
{
    [Dependency] private readonly GeneratorSystem _generator = default!;
    [Dependency] private readonly ActiveGeneratorRevvingSystem _revving = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<GeneratorSignalControlComponent, SignalReceivedEvent>(OnSignalReceived);
    }

    /// <summary>
    /// Change the state of the generator depending on what signal is sent.
    /// </summary>
    private void OnSignalReceived(EntityUid uid, GeneratorSignalControlComponent component, SignalReceivedEvent args)
    {
        if (!TryComp<FuelGeneratorComponent>(uid, out var generator))
            return;

        if (args.Port == component.OnPort)
        {
            _revving.StartAutoRevving(uid);
        }
        else if (args.Port == component.OffPort)
        {
            _generator.SetFuelGeneratorOn(uid, false, generator);
            _revving.StopAutoRevving(uid);
        }
        else if (args.Port == component.TogglePort)
        {
            if (generator.On)
            {
                _generator.SetFuelGeneratorOn(uid, false, generator);
                _revving.StopAutoRevving(uid);
            }
            else
            {
                _revving.StartAutoRevving(uid);
            }
        }
    }
}