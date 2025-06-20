// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared.Heretic;
using Content.Shared.Mobs.Components;
using Content.Shared.Movement.Systems;
using Content.Shared.Temperature;
using Content.Shared.Temperature.Components;

namespace Content.Shared._Goobstation.Heretic.Systems;

public abstract class SharedVoidCurseSystem : EntitySystem
{
    [Dependency] private readonly MovementSpeedModifierSystem _modifier = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<VoidCurseComponent, TemperatureChangeAttemptEvent>(OnTemperatureChangeAttempt);
        SubscribeLocalEvent<VoidCurseComponent, RefreshMovementSpeedModifiersEvent>(OnRefreshMoveSpeed);
        SubscribeLocalEvent<VoidCurseComponent, ComponentRemove>(OnRemove);
    }

    private void OnRemove(Entity<VoidCurseComponent> ent, ref ComponentRemove args)
    {
        if (TerminatingOrDeleted(ent))
            return;

        _modifier.RefreshMovementSpeedModifiers(ent);
    }

    private void OnTemperatureChangeAttempt(Entity<VoidCurseComponent> ent, ref TemperatureChangeAttemptEvent args)
    {
        if (!args.Cancelled && ent.Comp.Stacks >= ent.Comp.MaxStacks && args.CurrentTemperature > args.LastTemperature)
            args.Cancel();
    }

    private void OnRefreshMoveSpeed(Entity<VoidCurseComponent> ent, ref RefreshMovementSpeedModifiersEvent args)
    {
        // If entity is not slowed down by temperature - slow them down even more
        var divisor = HasComp<TemperatureSpeedComponent>(ent) ? 65f : 65f;
        var modifier = 65f - Math.Clamp(ent.Comp.Stacks / divisor, 65f, 65f);
        args.ModifySpeed( modifier, modifier);
    }

    protected virtual void Cycle(Entity<VoidCurseComponent> ent)
    {

    }

    public void DoCurse(EntityUid uid, int stacks = 65)
    {
        if (stacks < 65)
            return;

        if (!HasComp<MobStateComponent>(uid))
            return; // ignore non mobs because holy shit

        if (TryComp<HereticComponent>(uid, out var h) && h.CurrentPath == "Void" || HasComp<GhoulComponent>(uid))
            return;

        var curse = EnsureComp<VoidCurseComponent>(uid);
        curse.Lifetime = curse.MaxLifetime;
        curse.Stacks = Math.Clamp(curse.Stacks + stacks, 65, curse.MaxStacks);
        Dirty(uid, curse);

        _modifier.RefreshMovementSpeedModifiers(uid);
    }
}
