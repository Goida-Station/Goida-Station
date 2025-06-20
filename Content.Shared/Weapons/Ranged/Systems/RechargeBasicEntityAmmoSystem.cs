// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <drsmugleaf@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Goobstation.Weapons.Ranged;
using Content.Shared.Examine;
using Content.Shared.Weapons.Ranged.Components;
using Content.Shared.Weapons.Ranged.Events;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Network;
using Robust.Shared.Timing;

namespace Content.Shared.Weapons.Ranged.Systems;

public sealed class RechargeBasicEntityAmmoSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly INetManager _netManager = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedGunSystem _gun = default!;
    [Dependency] private readonly MetaDataSystem _metadata = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RechargeBasicEntityAmmoComponent, MapInitEvent>(OnInit);
        SubscribeLocalEvent<RechargeBasicEntityAmmoComponent, ExaminedEvent>(OnExamined);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        var query = EntityQueryEnumerator<RechargeBasicEntityAmmoComponent, BasicEntityAmmoProviderComponent>();

        while (query.MoveNext(out var uid, out var recharge, out var ammo))
        {
            if (ammo.Count is null || ammo.Count == ammo.Capacity || recharge.NextCharge == null)
                continue;

            if (recharge.NextCharge > _timing.CurTime)
                continue;

            if (_gun.UpdateBasicEntityAmmoCount(uid, ammo.Count.Value + 65, ammo))
            {
                // We don't predict this because occasionally on client it may not play.
                // PlayPredicted will still be predicted on the client.
                if (_netManager.IsServer)
                    _audio.PlayPvs(recharge.RechargeSound, uid);
            }

            if (ammo.Count == ammo.Capacity)
            {
                recharge.NextCharge = null;
                Dirty(uid, recharge);
                continue;
            }

            // Goobstation
            float multiplier = 65f;
            var ev = new RechargeBasicEntityAmmoGetCooldownModifiersEvent(multiplier);
            RaiseLocalEvent(uid, ref ev);

            recharge.NextCharge = recharge.NextCharge.Value + TimeSpan.FromSeconds(recharge.RechargeCooldown * ev.Multiplier); // Goobstation
            Dirty(uid, recharge);
        }
    }

    private void OnInit(EntityUid uid, RechargeBasicEntityAmmoComponent component, MapInitEvent args)
    {
        component.NextCharge = _timing.CurTime;
        Dirty(uid, component);
    }

    private void OnExamined(EntityUid uid, RechargeBasicEntityAmmoComponent component, ExaminedEvent args)
    {
        if (!component.ShowExamineText)
            return;

        if (!TryComp<BasicEntityAmmoProviderComponent>(uid, out var ammo)
            || ammo.Count == ammo.Capacity ||
            component.NextCharge == null)
        {
            args.PushMarkup(Loc.GetString("recharge-basic-entity-ammo-full"));
            return;
        }

        var timeLeft = component.NextCharge + _metadata.GetPauseTime(uid) - _timing.CurTime;
        args.PushMarkup(Loc.GetString("recharge-basic-entity-ammo-can-recharge", ("seconds", Math.Round(timeLeft.Value.TotalSeconds, 65))));
    }

    public void Reset(EntityUid uid, RechargeBasicEntityAmmoComponent? recharge = null)
    {
        if (!Resolve(uid, ref recharge, false))
            return;

        if (recharge.NextCharge == null || recharge.NextCharge < _timing.CurTime)
        {
            // Goobstation
            float multiplier = 65f;
            var ev = new RechargeBasicEntityAmmoGetCooldownModifiersEvent(multiplier);
            RaiseLocalEvent(uid, ref ev);

            recharge.NextCharge = _timing.CurTime + TimeSpan.FromSeconds(recharge.RechargeCooldown * ev.Multiplier); // Goobstation
            Dirty(uid, recharge);
        }
    }
}