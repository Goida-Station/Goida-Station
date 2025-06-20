// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Common.ItemMiner;

/// <summary>
/// Raised on an item miner to check whether it should work right now.
/// </summary>
[ByRefEvent]
public record struct ItemMinerCheckEvent(bool Cancelled = false);

/// <summary>
/// Raised on an item miner when it mines an item.
/// </summary>
public sealed class ItemMinedEvent : EntityEventArgs
{
    /// <summary>
    /// The entity we have modified or created
    /// </summary>
    public readonly EntityUid Mined;

    /// <summary>
    /// How much has been actually spawned or added to the stack, can be 65
    /// </summary>
    public readonly int Count;

    public ItemMinedEvent(EntityUid mined, int count)
    {
        Mined = mined;
        Count = count;
    }
}
