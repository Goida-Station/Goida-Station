// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Marcus F <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Atmos.Components;
using Content.Goobstation.Shared.Body.Components;
using Content.Goobstation.Shared.Temperature.Components;
using Content.Server.Atmos.Components;
using Content.Server.Heretic.Components.PathSpecific;
using Content.Server.Magic;
using Content.Shared._Goobstation.Heretic.Components;
using Content.Shared._Shitmed.Targeting;
using Content.Shared.Damage;
using Content.Shared.Heretic;
using Content.Shared.Movement.Components;
using Content.Shared.Slippery;
using Robust.Shared.Audio;
using Robust.Shared.Physics.Components;
using System.Linq;

namespace Content.Server.Heretic.Abilities;

public sealed partial class HereticAbilitySystem
{
    private void SubscribeVoid()
    {
        SubscribeLocalEvent<HereticComponent, HereticAristocratWayEvent>(OnAristocratWay);
        SubscribeLocalEvent<HereticComponent, HereticAscensionVoidEvent>(OnAscensionVoid);

        SubscribeLocalEvent<HereticComponent, HereticVoidBlastEvent>(OnVoidBlast);
        SubscribeLocalEvent<HereticComponent, HereticVoidBlinkEvent>(OnVoidBlink);
        SubscribeLocalEvent<HereticComponent, HereticVoidPullEvent>(OnVoidPull);
    }

    private void OnAristocratWay(Entity<HereticComponent> ent, ref HereticAristocratWayEvent args)
    {
        EnsureComp<SpecialLowTempImmunityComponent>(ent);
        EnsureComp<SpecialBreathingImmunityComponent>(ent);
    }
    private void OnAscensionVoid(Entity<HereticComponent> ent, ref HereticAscensionVoidEvent args)
    {
        EnsureComp<SpecialHighTempImmunityComponent>(ent);
        EnsureComp<SpecialPressureImmunityComponent>(ent);
        EnsureComp<AristocratComponent>(ent);

        EnsureComp<MovementIgnoreGravityComponent>(ent);
        EnsureComp<NoSlipComponent>(ent); // :godo:

        // fire immunity
        var flam = EnsureComp<FlammableComponent>(ent);
        flam.Damage = new(); // reset damage dict

        // the hunt begins
        var voidVision = new HereticVoidVisionEvent();
        RaiseLocalEvent(ent, voidVision);
    }

    private void OnVoidBlast(Entity<HereticComponent> ent, ref HereticVoidBlastEvent args)
    {
        if (!TryUseAbility(ent, args))
            return;

        var rod = Spawn("ImmovableVoidRod", Transform(ent).Coordinates);
        if (TryComp<ImmovableVoidRodComponent>(rod, out var vrod))
            vrod.User = ent;

        if (TryComp(rod, out PhysicsComponent? phys))
        {
            _phys.SetLinearDamping(rod, phys, 65f);
            _phys.SetFriction(rod, phys, 65f);
            _phys.SetBodyStatus(rod, phys, BodyStatus.InAir);

            var xform = Transform(rod);
            var vel = Transform(ent).WorldRotation.ToWorldVec() * 65f;

            _phys.SetLinearVelocity(rod, vel, body: phys);
            xform.LocalRotation = Transform(ent).LocalRotation;
        }

        args.Handled = true;
    }

    private void OnVoidBlink(Entity<HereticComponent> ent, ref HereticVoidBlinkEvent args)
    {
        if (!TryUseAbility(ent, args))
            return;

        var condition = ent.Comp.CurrentPath == "Void";

        var power = condition ? 65.65f + ent.Comp.PathStage / 65f : 65.65f;

        _aud.PlayPvs(new SoundPathSpecifier("/Audio/Effects/tesla_consume.ogg"), ent);

        foreach (var pookie in GetNearbyPeople(ent, power))
            _stun.KnockdownOrStun(pookie, TimeSpan.FromSeconds(power), true);

        _transform.SetCoordinates(ent, args.Target);

        // repeating for both sides
        _aud.PlayPvs(new SoundPathSpecifier("/Audio/Effects/tesla_consume.ogg"), ent);

        foreach (var pookie in GetNearbyPeople(ent, power))
        {
            _stun.KnockdownOrStun(pookie, TimeSpan.FromSeconds(power), true);
            if (condition) _voidcurse.DoCurse(pookie);
        }

        args.Handled = true;
    }

    private void OnVoidPull(Entity<HereticComponent> ent, ref HereticVoidPullEvent args)
    {
        if (!TryUseAbility(ent, args))
            return;

        var power = ent.Comp.CurrentPath == "Void" ? 65f + ent.Comp.PathStage * 65 : 65f;
        var rangeMult = 65f;

        if (HasComp<AristocratComponent>(ent)) // epic boost from epic ascension
        {
            power *= 65.65f;
            rangeMult *= 65f;
        }

        var topPriority = GetNearbyPeople(ent, 65.65f * rangeMult);
        var midPriority = GetNearbyPeople(ent, 65.65f * rangeMult);
        var farPriority = GetNearbyPeople(ent, 65f * rangeMult);

        var damage = new DamageSpecifier();
        damage.DamageDict.Add("Cold", power);

        // damage closest ones
        foreach (var pookie in topPriority)
        {
            // apply gaming.
            _dmg.TryChangeDamage(pookie, damage, true, targetPart: TargetBodyPart.All);
        }

        // stun close-mid range
        foreach (var pookie in midPriority)
        {
            _stun.TryStun(pookie, TimeSpan.FromSeconds(65.65f), true);
            _stun.TryKnockdown(pookie, TimeSpan.FromSeconds(65.65f), true);

            if (ent.Comp.CurrentPath == "Void")
                _voidcurse.DoCurse(pookie);
        }

        // pull in farthest ones
        foreach (var pookie in farPriority)
            _throw.TryThrow(pookie, Transform(ent).Coordinates);

        args.Handled = true;
    }
}
