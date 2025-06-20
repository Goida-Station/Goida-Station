// SPDX-FileCopyrightText: 65 Moony <moony@hellomouse.net>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Hands.Systems;
using Content.Server.Popups;
using Content.Shared.Interaction;
using Content.Shared.Storage;
using Robust.Shared.Player;

namespace Content.Server.Holiday.Christmas;

/// <summary>
/// This handles handing out items from item givers.
/// </summary>
public sealed class LimitedItemGiverSystem : EntitySystem
{
    [Dependency] private readonly HandsSystem _hands = default!;
    [Dependency] private readonly HolidaySystem _holiday = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<LimitedItemGiverComponent, InteractHandEvent>(OnInteractHand);
    }

    private void OnInteractHand(EntityUid uid, LimitedItemGiverComponent component, InteractHandEvent args)
    {
        if (!TryComp<ActorComponent>(args.User, out var actor))
            return;

        if (component.GrantedPlayers.Contains(actor.PlayerSession.UserId) || (component.RequiredHoliday is not null && !_holiday.IsCurrentlyHoliday(component.RequiredHoliday)))
        {
            _popup.PopupEntity(Loc.GetString(component.DeniedPopup), uid, args.User);
            return;
        }

        var toGive = EntitySpawnCollection.GetSpawns(component.SpawnEntries);
        var coords = Transform(args.User).Coordinates;

        foreach (var item in toGive)
        {
            if (item is null)
                continue;

            var spawned = Spawn(item, coords);
            _hands.PickupOrDrop(args.User, spawned);
        }

        component.GrantedPlayers.Add(actor.PlayerSession.UserId);
        _popup.PopupEntity(Loc.GetString(component.ReceivedPopup), uid, args.User);
    }
}