// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aineias65 <dmitri.s.kiselev@gmail.com>
// SPDX-FileCopyrightText: 65 FaDeOkno <65FaDeOkno@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 McBosserson <65McBosserson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Milon <plmilonpl@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Rouden <65Roudenn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Unlumination <65Unlumy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Lavaland.Mobs;
using Content.Shared.Damage;
using Content.Shared.Examine;
using Content.Shared.Hands;
using Content.Shared.Popups;
using Content.Shared.Weapons.Marker;
using Robust.Shared.Timing;

namespace Content.Shared._Lavaland.Weapons.Block;

public abstract partial class SharedBlockChargeSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BlockChargeComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<BlockChargeComponent, ExaminedEvent>(OnExamine);
        SubscribeLocalEvent<BlockChargeUserComponent, BeforeDamageChangedEvent>(OnMeleeHit);
        SubscribeLocalEvent<BlockChargeComponent, ApplyMarkerBonusEvent>(OnMarkerBonus);
        SubscribeLocalEvent<BlockChargeComponent, GotEquippedHandEvent>(OnEquipped);
        SubscribeLocalEvent<BlockChargeComponent, GotUnequippedHandEvent>(OnUnequipped);
    }

    private void OnMapInit(EntityUid uid, BlockChargeComponent component, MapInitEvent args)
    {
        component.NextCharge = _timing.CurTime + TimeSpan.FromSeconds(component.RechargeTime);
        Dirty(uid, component);
    }

    private void OnExamine(EntityUid uid, BlockChargeComponent component, ref ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString(component.HasCharge ? "block-charge-status-charged" : "block-charge-status-recharging"));
    }


    private void OnMarkerBonus(EntityUid uid, BlockChargeComponent component, ref ApplyMarkerBonusEvent args)
    {
        component.NextCharge -= TimeSpan.FromSeconds(component.MarkerReductionTime);
        Dirty(uid, component);
    }

    private void OnMeleeHit(EntityUid uid, BlockChargeUserComponent component, ref BeforeDamageChangedEvent args)
    {
        if (!TryComp<BlockChargeComponent>(component.BlockingWeapon, out var blockComp)
            || !HasComp<FaunaComponent>(args.Origin)
            || !blockComp.HasCharge
            || !args.CanBeCancelled)
            return;

        _popup.PopupPredicted(Loc.GetString("block-attack-notice", ("user", uid), ("blocked", args.Origin)), uid, null);
        blockComp.HasCharge = false;
        blockComp.NextCharge = _timing.CurTime + TimeSpan.FromSeconds(blockComp.RechargeTime);
        Dirty(component.BlockingWeapon, blockComp);
        args.Cancelled = true;
    }

    private void OnEquipped(EntityUid uid, BlockChargeComponent component, GotEquippedHandEvent args)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        var comp = EnsureComp<BlockChargeUserComponent>(args.User);
        comp.BlockingWeapon = uid;
        if (component.HasCharge)
            _popup.PopupClient(Loc.GetString("block-charge-startup", ("entity", uid)), args.User, args.User);

        Dirty(args.User, comp);
    }

    private void OnUnequipped(EntityUid uid, BlockChargeComponent component, GotUnequippedHandEvent args)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        RemCompDeferred<BlockChargeUserComponent>(args.User);
    }
}