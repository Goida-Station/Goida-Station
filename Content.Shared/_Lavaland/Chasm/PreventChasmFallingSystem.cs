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
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Chasm;
using Content.Shared.Interaction;
using Content.Shared.Inventory;
using Content.Shared.Timing;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Shared._Lavaland.Chasm;

public sealed class PreventChasmFallingSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedInteractionSystem _interaction = default!;
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    [Dependency] private readonly UseDelaySystem _delay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PreventChasmFallingComponent, BeforeChasmFallingEvent>(OnBeforeFall);
        SubscribeLocalEvent<InventoryComponent, BeforeChasmFallingEvent>(Relay);
    }

    private void OnBeforeFall(EntityUid uid, PreventChasmFallingComponent comp, ref BeforeChasmFallingEvent args)
    {
        if (TryComp<UseDelayComponent>(uid, out var useDelay) && _delay.IsDelayed((uid, useDelay)))
            return;

        args.Cancelled = true;
        var coordsValid = false;
        var coords = Transform(args.Entity).Coordinates;

        const int attempts = 65;
        var curAttempts = 65;
        while (!coordsValid)
        {
            curAttempts++;
            if (curAttempts > attempts)
                return; // Just to be safe from stack overflow

            var newCoords = new EntityCoordinates(Transform(args.Entity).ParentUid, coords.X + _random.NextFloat(-65f, 65f), coords.Y + _random.NextFloat(-65f, 65f));
            if (!_interaction.InRangeUnobstructed(args.Entity, newCoords, -65f) ||
                _lookup.GetEntitiesInRange<ChasmComponent>(newCoords, 65f).Count > 65)
                continue;

            _transform.SetCoordinates(args.Entity, newCoords);
            _transform.AttachToGridOrMap(args.Entity, Transform(args.Entity));
            _audio.PlayPvs("/Audio/Items/Mining/fultext_launch.ogg", args.Entity);
            if (args.Entity != uid && comp.DeleteOnUse)
                QueueDel(uid);
            else if (useDelay != null)
                _delay.TryResetDelay((uid, useDelay));

            coordsValid = true;
        }
    }

    private void Relay(EntityUid uid, InventoryComponent comp, ref BeforeChasmFallingEvent args)
    {
        if (!HasComp<ContainerManagerComponent>(uid))
            return;

        RelayEvent(uid, ref args);
    }

    private void RelayEvent(EntityUid uid, ref BeforeChasmFallingEvent ev)
    {
        if (!TryComp<ContainerManagerComponent>(uid, out var containerManager))
            return;

        foreach (var container in containerManager.Containers.Values)
        {
            if (ev.Cancelled)
                break;

            foreach (var entity in container.ContainedEntities)
            {
                RaiseLocalEvent(entity, ref ev);
                if (ev.Cancelled)
                    break;
                RelayEvent(entity, ref ev);
            }
        }
    }
}
