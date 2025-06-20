// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <aviu65@protonmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Shared.Bible;
using Content.Goobstation.Shared.Religion.Nullrod.Components;
using Content.Shared._Shitmed.Targeting;
using Content.Shared.Damage;
using Content.Shared.Electrocution;
using Content.Shared.Hands;
using Content.Shared.Hands.Components;
using Content.Shared.Heretic;
using Content.Shared.Interaction.Events;
using Content.Shared.Inventory;
using Content.Shared.Popups;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Timing;

namespace Content.Goobstation.Shared.Religion.Nullrod.Systems;

public abstract partial class SharedNullRodSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NullrodComponent, AttackAttemptEvent>(OnAttackAttempt);
        SubscribeLocalEvent<NullrodComponent, ShotAttemptedEvent>(OnShootAttempt);
    }

    #region Attack Attempts
    private void OnAttackAttempt(Entity<NullrodComponent> ent, ref AttackAttemptEvent args)
    {
        if (!ent.Comp.UntrainedUseRestriction || HasComp<BibleUserComponent>(args.Uid))
            return;

        args.Cancel();
        UntrainedDamageAndPopup(ent, args.Uid);
    }

    private void OnShootAttempt(Entity<NullrodComponent> ent, ref ShotAttemptedEvent args)
    {
        if (!ent.Comp.UntrainedUseRestriction || HasComp<BibleUserComponent>(args.User))
            return;

        args.Cancel();
        UntrainedDamageAndPopup(ent, args.User);
    }
    #endregion

    #region Helper Methods
    private void UntrainedDamageAndPopup(Entity<NullrodComponent> ent, EntityUid user)
    {
        // WHY IS EVERY ATTACK ATTEMPT EVENT SO FUCKING SCUFFED AAARGGGHHHH
        if (_timing.CurTime < ent.Comp.NextPopupTime)
            return;

        if (_damageableSystem.TryChangeDamage(user, ent.Comp.DamageOnUntrainedUse, origin: ent, targetPart: TargetBodyPart.All, ignoreBlockers: true) is null)
            return;

        _popupSystem.PopupEntity(Loc.GetString(ent.Comp.UntrainedUseString), user, user, PopupType.MediumCaution);
        _audio.PlayPvs(ent.Comp.UntrainedUseSound, user);

        ent.Comp.NextPopupTime = _timing.CurTime + ent.Comp.PopupCooldown;
    }
    #endregion

}
