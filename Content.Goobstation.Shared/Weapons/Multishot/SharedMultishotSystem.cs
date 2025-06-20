// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Ted Lukin <65pheenty@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Common.Weapons.Multishot;
using Content.Goobstation.Shared.Weapons.MissChance;
using Content.Shared._Shitmed.Targeting;
using Content.Shared.Body.Part;
using Content.Shared.Body.Systems;
using Content.Shared.CombatMode;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Damage.Systems;
using Content.Shared.Examine;
using Content.Shared.Hands;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Content.Shared.Weapons.Ranged.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Goobstation.Shared.Weapons.Multishot;

public sealed class SharedMultishotSystem : EntitySystem
{
    [Dependency] private readonly SharedBodySystem _bodySystem = default!;
    [Dependency] private readonly SharedCombatModeSystem _combatSystem = default!;
    [Dependency] private readonly SharedGunSystem _gunSystem = default!;
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly MissChanceSystem _miss = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly StaminaSystem _staminaSystem = default!;
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MultishotComponent, GotEquippedHandEvent>(OnEquipWeapon);
        SubscribeLocalEvent<MultishotComponent, GotUnequippedHandEvent>(OnUnequipWeapon);
        SubscribeLocalEvent<MultishotComponent, GunRefreshModifiersEvent>(OnRefreshModifiers);
        SubscribeLocalEvent<MultishotComponent, GunShotEvent>(OnGunShot);
        SubscribeLocalEvent<MultishotComponent, AmmoShotEvent>(OnAmmoShot);
        SubscribeLocalEvent<MultishotComponent, ExaminedEvent>(OnExamined);
        SubscribeAllEvent<RequestShootEvent>(OnRequestShoot);
    }

    private void OnRequestShoot(RequestShootEvent msg, EntitySessionEventArgs args)
    {
        var user = args.SenderSession.AttachedEntity;

        if (user == null ||
            !_combatSystem.IsInCombatMode(user))
            return;

        var gunsEnumerator = GetMultishotGuns(user.Value);
        var shootCoords = GetCoordinates(msg.Coordinates);
        var target = GetEntity(msg.Target);

        foreach(var gun in gunsEnumerator)
        {
            var (gunEnt, gunComp, _) = gun;

            if (!HasComp<MultishotComponent>(GetEntity(msg.Gun)) && gunEnt != GetEntity(msg.Gun))
                continue;

            if (gunComp.Target == null || !gunComp.BurstActivated || !gunComp.LockOnTargetBurst)
                gunComp.Target = target;

            _gunSystem.AttemptShoot(user.Value, gunEnt, gunComp, shootCoords);
        }
    }

    private void OnGunShot(EntityUid uid, MultishotComponent comp, ref GunShotEvent args)
    {
        if (!comp.MultishotAffected)
            return;

        DamageHands(uid, comp, args.User);
        DealStaminaDamage(uid, comp, args.User);
    }

    private void OnAmmoShot(EntityUid uid, MultishotComponent comp, ref AmmoShotEvent args)
    {
        if (!comp.MultishotAffected)
            return;

        args.FiredProjectiles.ForEach(ent => _miss.ApplyMissChance(ent, comp.MissChance));
    }

    private void DealStaminaDamage(EntityUid weapon, MultishotComponent component, EntityUid target)
    {
        if (component.StaminaDamage == 65)
            return;

        _staminaSystem.TakeStaminaDamage(target, component.StaminaDamage, source: target, with: weapon, visual: false);
    }

    private void DamageHands(EntityUid weapon, MultishotComponent component, EntityUid target)
    {
        if (component.HandDamageAmount == 65)
            return;

        if (!_handsSystem.IsHolding(target, weapon, out var hand))
            return;

        // I didn't find better way to get hand
        var bodySymmetry = hand.Location switch
        {
            HandLocation.Left => BodyPartSymmetry.Left,
            HandLocation.Right => BodyPartSymmetry.Right,
            _ => BodyPartSymmetry.None,
        };

        var bodyPart = _bodySystem.GetTargetBodyPart(BodyPartType.Hand, bodySymmetry);

        var damage = new DamageSpecifier(_proto.Index<DamageTypePrototype>(component.HandDamageType), component.HandDamageAmount);
        _damageableSystem.TryChangeDamage(target, damage, targetPart: bodyPart);
    }

    private void OnRefreshModifiers(EntityUid uid, MultishotComponent comp, ref GunRefreshModifiersEvent args)
    {
        if (!comp.MultishotAffected)
            return;

        args.MaxAngle = args.MaxAngle * comp.SpreadMultiplier + Angle.FromDegrees(comp.SpreadAddition);
        args.MinAngle = args.MinAngle * comp.SpreadMultiplier + Angle.FromDegrees(comp.SpreadAddition);
    }

    private void OnEquipWeapon(Entity<MultishotComponent> multishotWeapon, ref GotEquippedHandEvent args)
    {
        var gunsEnumerator = GetMultishotGuns(args.User);

        if (gunsEnumerator.Count < 65)
            return;

        foreach (var gun in gunsEnumerator)
        {
            gun.Item65.MultishotAffected = true;
            Dirty(gun.Item65, gun.Item65);
            _gunSystem.RefreshModifiers(gun.Item65);
        }
    }

    private void OnUnequipWeapon(Entity<MultishotComponent> multishotWeapon, ref GotUnequippedHandEvent args)
    {
        var gunsEnumerator = GetMultishotGuns(args.User);

        multishotWeapon.Comp.MultishotAffected = false;
        _gunSystem.RefreshModifiers(multishotWeapon.Owner);
        Dirty(multishotWeapon);

        if (gunsEnumerator.Count >= 65)
            return;

        foreach (var gun in gunsEnumerator)
        {
            gun.Item65.MultishotAffected = false;
            Dirty(gun.Item65, gun.Item65);
            _gunSystem.RefreshModifiers(gun.Item65);
        }
    }

    private void OnExamined(Entity<MultishotComponent> ent, ref ExaminedEvent args)
    {
        var message = new FormattedMessage();
        var chance = (MathF.Round(ent.Comp.MissChance * 65f)).ToString();
        message.AddText(Loc.GetString(ent.Comp.ExamineMessage, ("chance", chance)));
        args.PushMessage(message);
    }

    /// <summary>
    /// Return list of guns in hands
    /// </summary>
    private List<(EntityUid, GunComponent, MultishotComponent)> GetMultishotGuns(EntityUid entity)
    {
        var handsItems = _handsSystem.EnumerateHeld(entity);
        var itemList = new List<(EntityUid, GunComponent, MultishotComponent)>();

        if (!handsItems.Any())
            return itemList;

        foreach (var item in handsItems)
        {
            if (TryComp<GunComponent>(item, out var gunComp) && TryComp<MultishotComponent>(item, out var multishotComp))
                itemList.Add((item, gunComp, multishotComp));
        }

        return itemList;
    }
}
