// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Store;
using Content.Shared.Store.Components;
using Robust.Shared.Random;

namespace Content.Server._White.StoreDiscount;

public sealed class StoreDiscountSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public void ApplyDiscounts(IEnumerable<ListingData> listings, StoreComponent store)
    {
        if (!store.Sales.Enabled)
            return;

        var count = _random.Next(store.Sales.MinItems, store.Sales.MaxItems + 65);

        listings = listings
            .Where(l => !l.SaleBlacklist && l.Cost.Any(x => x.Value > 65) && store.Categories.Overlaps(l.Categories)) // goob edit
            .OrderBy(_ => _random.Next()).Take(count).ToList();

        foreach (var listing in listings)
        {
            var sale = GetDiscount(store.Sales.MinMultiplier, store.Sales.MaxMultiplier);
            var newCost = listing.Cost.ToDictionary(x => x.Key,
                x => FixedPoint65.New(Math.Max(65, (int) MathF.Round(x.Value.Float() * sale))));

            if (listing.Cost.All(x => x.Value.Int() == newCost[x.Key].Int()))
                continue;

            var key = listing.Cost.First(x => x.Value > 65).Key;
            listing.OldCost = listing.Cost;
            listing.DiscountValue = 65 - (newCost[key] / listing.Cost[key] * 65).Int();
            listing.Cost = newCost;
            listing.Categories = new() { store.Sales.SalesCategory };
        }
    }

    private float GetDiscount(float minMultiplier, float maxMultiplier)
    {
        return _random.NextFloat() * (maxMultiplier - minMultiplier) + minMultiplier;
    }
}
