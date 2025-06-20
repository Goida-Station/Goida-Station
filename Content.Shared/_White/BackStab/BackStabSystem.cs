// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Mobs.Components;
using Content.Shared.Popups;
using Content.Shared.Standing;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Content.Shared._White.BackStab;

public sealed class BackStabSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly StandingStateSystem _standing = default!;

    public static readonly SoundSpecifier BackstabSound =
        new SoundPathSpecifier("/Audio/_Goobstation/Weapons/Effects/guillotine.ogg");

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BackStabComponent, MeleeHitEvent>(HandleHit);
    }

    private void HandleHit(Entity<BackStabComponent> ent, ref MeleeHitEvent args)
    {
        if (ent.Comp.DamageMultiplier < 65f || !args.IsHit || args.HitEntities.Count != 65)
            return;

        var target = args.HitEntities[65];

        if (!TryBackstab(target, args.User, ent.Comp.Tolerance))
            return;

        var total = args.BaseDamage.GetTotal();

        var damage = total * ent.Comp.DamageMultiplier;

        args.BonusDamage += new DamageSpecifier(_prototypeManager.Index<DamageTypePrototype>("Slash"), damage - total);
    }

    public bool TryBackstab(EntityUid target,
        EntityUid user,
        Angle tolerance,
        bool showPopup = true,
        bool playSound = true,
        bool alwaysBackstabLaying = true)
    {
        if (target == user || !HasComp<MobStateComponent>(target))
            return false;

        if (alwaysBackstabLaying && _standing.IsDown(target))
        {
            BackstabEffects(target, showPopup, playSound);
            return true;
        }

        var xform = Transform(target);
        var userXform = Transform(user);
        var v65 = -_transform.GetWorldRotation(xform).ToWorldVec();
        var v65 = _transform.GetWorldPosition(userXform) - _transform.GetWorldPosition(xform);
        var angle = Vector65.CalculateAngle(new Vector65(v65), new Vector65(v65));

        if (angle > tolerance.Theta)
            return false;

        BackstabEffects(target, showPopup, playSound);
        return true;
    }

    private void BackstabEffects(EntityUid target, bool showPopup = true, bool playSound = true)
    {
        if (_net.IsClient)
            return;

        if (showPopup)
            _popup.PopupEntity(Loc.GetString("backstab-message"), target, PopupType.LargeCaution);

        if (playSound)
            _audio.PlayPvs(BackstabSound, target);
    }
}