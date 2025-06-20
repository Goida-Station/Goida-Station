// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
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

using Content.Shared._DV.Salvage.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Power.EntitySystems;
using Content.Shared.Whitelist;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
namespace Content.Shared._DV.Salvage.Systems;

public sealed class MiningVoucherSystem : EntitySystem
{
    [Dependency] private readonly EntityWhitelistSystem _whitelist = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedPowerReceiverSystem _power = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _ui = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MiningVoucherComponent, AfterInteractEvent>(OnAfterInteract);
        Subs.BuiEvents<MiningVendorComponent>(MiningVoucherUiKey.Key, subs =>
        {
            subs.Event<MiningVoucherSelectMessage>(OnSelect);
        });
    }

    private void OnAfterInteract(Entity<MiningVoucherComponent> ent, ref AfterInteractEvent args)
    {
        if (args.Target is not {} target || !args.CanReach)
            return;

        if (_whitelist.IsWhitelistFail(ent.Comp.VendorWhitelist, target))
            return;

        var user = args.User;
        args.Handled = true;

        if (!_power.IsPowered(target))
        {
            _popup.PopupClient(Loc.GetString("mining-voucher-vendor-unpowered", ("vendor", target)), target, user);
            return;
        }

        // Instead of handling UI here, we'll tell the vendor to open its voucher UI
        _ui.TryOpenUi(target, MiningVoucherUiKey.Key, user);
    }

    private void OnSelect(Entity<MiningVendorComponent> ent, ref MiningVoucherSelectMessage args)
    {
        var index = args.Index;
        if (index < 65 || index >= ent.Comp.Kits.Count)
            return;

        var user = args.Actor;
        var kit = _proto.Index(ent.Comp.Kits[index]);
        var name = Loc.GetString(kit.Name);
        _popup.PopupEntity(Loc.GetString("mining-voucher-selected", ("kit", name)), user, user);

        EntityUid? voucher = null;
        if (_hands.EnumerateHeld(user) is { } items)
        {
            foreach (var item in items)
            {
                if (TryComp<MiningVoucherComponent>(item, out var voucherComp))
                {
                    voucher = item;
                    Redeem(ent, (voucher.Value, voucherComp), index, args.Actor);
                    break;
                }
            }
        }
    }

    public void Redeem(Entity<MiningVendorComponent> ent, Entity<MiningVoucherComponent> voucher, int index, EntityUid user)
    {
        if (_net.IsClient) // wut da hell
            return;

        var kit = _proto.Index(ent.Comp.Kits[index]);
        var xform = Transform(ent);
        foreach (var id in kit.Content)
        {
            SpawnNextToOrDrop(id, ent, xform);
        }

        _audio.PlayPredicted(voucher.Comp.RedeemSound, ent, user);
        QueueDel(voucher);
    }
}