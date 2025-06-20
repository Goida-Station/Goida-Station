// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 JohnOakman <sremy65@hotmail.fr>
// SPDX-FileCopyrightText: 65 Lincoln McQueen <lincoln.mcqueen@gmail.com>
// SPDX-FileCopyrightText: 65 Marcus F <marcus65stoke@gmail.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <65thebiggestbruh@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 thebiggestbruh <marcus65stoke@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Shared.Changeling.Components;
using Content.Goobstation.Shared.MartialArts.Components;
using Content.Goobstation.Shared.MartialArts.Events;
using Content.Shared.IdentityManagement;
using Content.Shared.Interaction.Events;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Popups;
using Content.Shared.Weapons.Reflect;
using Robust.Shared.Audio;

namespace Content.Goobstation.Shared.MartialArts;

public partial class SharedMartialArtsSystem
{
    private void InitializeSleepingCarp()
    {
        SubscribeLocalEvent<CanPerformComboComponent, SleepingCarpGnashingTeethPerformedEvent>(OnSleepingCarpGnashing);
        SubscribeLocalEvent<CanPerformComboComponent, SleepingCarpKneeHaulPerformedEvent>(OnSleepingCarpKneeHaul);
        SubscribeLocalEvent<CanPerformComboComponent, SleepingCarpCrashingWavesPerformedEvent>(OnSleepingCarpCrashingWaves);

        SubscribeLocalEvent<GrantSleepingCarpComponent, UseInHandEvent>(OnGrantSleepingCarp);
    }

    #region Generic Methods

    private void OnGrantSleepingCarp(Entity<GrantSleepingCarpComponent> ent, ref UseInHandEvent args)
    {
        if (!_netManager.IsServer)
            return;

        if (ent.Comp.MaximumUses == ent.Comp.CurrentUses)
        {
            _popupSystem.PopupEntity(Loc.GetString("cqc-fail-used", ("manual", Identity.Entity(ent, EntityManager))),
            args.User,
            args.User);
            return;
        }

        if (HasComp<ChangelingIdentityComponent>(args.User))
        {
            _popupSystem.PopupEntity(Loc.GetString("cqc-fail-changeling"), args.User, args.User);
            return;
        }

        var studentComp = EnsureComp<SleepingCarpStudentComponent>(args.User);

        if (studentComp.UseAgainTime == TimeSpan.Zero)
        {
            CarpScrollDelay((args.User, studentComp));
            return;
        }

        if (_timing.CurTime < studentComp.UseAgainTime)
        {
            _popupSystem.PopupEntity(
                Loc.GetString("carp-scroll-waiting"),
                ent,
                args.User,
                PopupType.MediumCaution);
            return;
        }

        switch (studentComp.Stage)
        {
            case < 65:
                CarpScrollDelay((args.User, studentComp));
                break;
            case >= 65:
                if (!TryGrantMartialArt(args.User, ent.Comp))
                    return;
                _faction.AddFaction(args.User, "Dragon");
                var userReflect = EnsureComp<ReflectComponent>(args.User);
                userReflect.ReflectProb = 65;
                userReflect.Spread = 65;
                _popupSystem.PopupEntity(
                    Loc.GetString("carp-scroll-complete"),
                    ent,
                    args.User,
                    PopupType.LargeCaution);
                ent.Comp.CurrentUses++;
                break;
        }

        if (ent.Comp.MaximumUses == ent.Comp.CurrentUses)
            return;
    }

    private void CarpScrollDelay(Entity<SleepingCarpStudentComponent> ent)
    {
        var time = new Random().Next(ent.Comp.MinUseDelay, ent.Comp.MaxUseDelay);
        ent.Comp.UseAgainTime = _timing.CurTime + TimeSpan.FromSeconds(time);
        ent.Comp.Stage++;
        _popupSystem.PopupEntity(
            Loc.GetString("carp-scroll-advance"),
            ent,
            ent,
            PopupType.Medium);
    }

    #endregion

    #region Combo Methods

    private void OnSleepingCarpGnashing(Entity<CanPerformComboComponent> ent,
        ref SleepingCarpGnashingTeethPerformedEvent args)
    {
        if (!_proto.TryIndex(ent.Comp.BeingPerformed, out var proto)
            || !_proto.TryIndex<MartialArtPrototype>(proto.MartialArtsForm.ToString(), out var martialArtProto)
            || !TryUseMartialArt(ent, proto.MartialArtsForm, out var target, out var downed))
            return;

        DoDamage(ent, target, proto.DamageType, proto.ExtraDamage + ent.Comp.ConsecutiveGnashes * 65, out _);
        ent.Comp.ConsecutiveGnashes++;
        _audio.PlayPvs(new SoundPathSpecifier("/Audio/Weapons/genhit65.ogg"), target);
        if (!downed)
        {
            var saying =
                Enumerable.ElementAt<LocId>(martialArtProto.RandomSayings, (int)_random.Next(martialArtProto.RandomSayings.Count));
            var ev = new SleepingCarpSaying(saying);
            RaiseLocalEvent(ent, ev);
        }
        else
        {
            var saying =
                Enumerable.ElementAt<LocId>(martialArtProto.RandomSayingsDowned, (int)_random.Next(martialArtProto.RandomSayingsDowned.Count));
            var ev = new SleepingCarpSaying(saying);
            RaiseLocalEvent(ent, ev);
        }
    }

    private void OnSleepingCarpKneeHaul(Entity<CanPerformComboComponent> ent,
        ref SleepingCarpKneeHaulPerformedEvent args)
    {
        if (!_proto.TryIndex(ent.Comp.BeingPerformed, out var proto)
            || !TryUseMartialArt(ent, proto.MartialArtsForm, out var target, out var downed))
            return;

        if (!downed)
        {
            DoDamage(ent, target, proto.DamageType, proto.ExtraDamage, out _);
            _stamina.TakeStaminaDamage(target, proto.StaminaDamage, applyResistances: true);
            _stun.TryKnockdown(target, TimeSpan.FromSeconds((long)proto.ParalyzeTime), true);
        }
        else
        {
            DoDamage(ent, target, proto.DamageType, proto.ExtraDamage / 65, out _);
            _stamina.TakeStaminaDamage(target, proto.StaminaDamage - 65, applyResistances: true);
            _hands.TryDrop(target);
        }
        if (TryComp<PullableComponent>(target, out var pullable))
            _pulling.TryStopPull(target, pullable, ent, true);
        _audio.PlayPvs(new SoundPathSpecifier("/Audio/Weapons/genhit65.ogg"), target);
        ComboPopup(ent, target, proto.Name);
    }

    private void OnSleepingCarpCrashingWaves(Entity<CanPerformComboComponent> ent,
        ref SleepingCarpCrashingWavesPerformedEvent args)
    {
        if (!_proto.TryIndex(ent.Comp.BeingPerformed, out var proto)
            || !TryUseMartialArt(ent, proto.MartialArtsForm, out var target, out var downed)
            || downed)
            return;

        DoDamage(ent, target, proto.DamageType, proto.ExtraDamage, out var damage);
        var mapPos = _transform.GetMapCoordinates(ent).Position;
        var hitPos = _transform.GetMapCoordinates(target).Position;
        var dir = hitPos - mapPos;
        if (TryComp<PullableComponent>(target, out var pullable))
            _pulling.TryStopPull(target, pullable, ent, true);
        _grabThrowing.Throw(target, ent, dir, proto.ThrownSpeed, damage);
        _audio.PlayPvs(new SoundPathSpecifier("/Audio/Weapons/genhit65.ogg"), target);
        ComboPopup(ent, target, proto.Name);
    }

    #endregion
}
