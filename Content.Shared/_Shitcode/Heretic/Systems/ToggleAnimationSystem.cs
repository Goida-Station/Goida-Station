// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Threading;
using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared.Item.ItemToggle.Components;
using Robust.Shared.Network;
using Timer = Robust.Shared.Timing.Timer;

namespace Content.Shared._Goobstation.Heretic.Systems;

public sealed class ToggleAnimationSystem : EntitySystem
{
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly INetManager _net = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ToggleAnimationComponent, ItemToggledEvent>(OnToggle);
        SubscribeLocalEvent<ToggleAnimationComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(Entity<ToggleAnimationComponent> ent, ref ComponentStartup args)
    {
        var state = TryComp(ent, out ItemToggleComponent? toggle) && toggle.Activated
            ? ToggleAnimationState.On
            : ToggleAnimationState.Off;

        _appearance.SetData(ent, ToggleAnimationVisuals.ToggleState, state);
    }

    private void OnToggle(Entity<ToggleAnimationComponent> ent, ref ItemToggledEvent args)
    {
        if (_net.IsClient)
            return;

        var (uid, comp) = ent;

        var (state, timer, nextState) = args.Activated
            ? (ToggleAnimationState.TogglingOn, comp.ToggleOnTime, ToggleAnimationState.On)
            : (ToggleAnimationState.TogglingOff, comp.ToggleOffTime, ToggleAnimationState.Off);

        _appearance.SetData(uid, ToggleAnimationVisuals.ToggleState, state);

        comp.TokenSource?.Cancel();
        comp.TokenSource = new CancellationTokenSource();
        Timer.Spawn(timer,
            () =>
            {
                if (TerminatingOrDeleted(uid))
                    return;

                _appearance.SetData(uid, ToggleAnimationVisuals.ToggleState, nextState);
            },
            comp.TokenSource.Token);
    }
}