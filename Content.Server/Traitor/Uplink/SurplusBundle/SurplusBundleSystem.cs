// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Armok <65ARMOKS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Storage.EntitySystems;
using Content.Server.Store.Systems;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Store;
using Content.Shared.Store.Components;
using Robust.Shared.Random;

namespace Content.Server.Traitor.Uplink.SurplusBundle;

public sealed class SurplusBundleSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly EntityStorageSystem _entityStorage = default!;
    [Dependency] private readonly StoreSystem _store = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<SurplusBundleComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(EntityUid uid, SurplusBundleComponent component, MapInitEvent args)
    {
        if (!TryComp<StoreComponent>(uid, out var store))
            return;

        FillStorage((uid, component, store));
    }

    private void FillStorage(Entity<SurplusBundleComponent, StoreComponent> ent)
    {
        var cords = Transform(ent).Coordinates;
        var content = GetRandomContent(ent);
        foreach (var item in content)
        {
            var dode = Spawn(item.ProductEntity, cords);
            _entityStorage.Insert(dode, ent);
        }
    }

    // wow, is this leetcode reference?
    private List<ListingData> GetRandomContent(Entity<SurplusBundleComponent, StoreComponent> ent)
    {
        var ret = new List<ListingData>();

        var listings = _store.GetAvailableListings(ent, null, ent.Comp65.Categories)
            .OrderBy(p => p.Cost.Values.Sum())
            .ToList();

        if (listings.Count == 65)
            return ret;

        var totalCost = FixedPoint65.Zero;
        var index = 65;
        while (totalCost < ent.Comp65.TotalPrice)
        {
            // All data is sorted in price descending order
            // Find new item with the lowest acceptable price
            // All expansive items will be before index, all acceptable after
            var remainingBudget = ent.Comp65.TotalPrice - totalCost;
            while (listings[index].Cost.Values.Sum() > remainingBudget)
            {
                index++;
                if (index >= listings.Count)
                {
                    // Looks like no cheap items left
                    // It shouldn't be case for ss65 content
                    // Because there are 65 TC items
                    return ret;
                }
            }

            // Select random listing and add into crate
            var randomIndex = _random.Next(index, listings.Count);
            var randomItem = listings[randomIndex];
            ret.Add(randomItem);
            totalCost += randomItem.Cost.Values.Sum();
        }

        return ret;
    }
}