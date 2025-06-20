// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Shared.Item;

/// <summary>
/// This is a prototype for a category of an item's size.
/// </summary>
[Prototype]
public sealed partial class ItemSizePrototype : IPrototype, IComparable<ItemSizePrototype>
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    /// <summary>
    /// The amount of space in a bag an item of this size takes.
    /// </summary>
    [DataField]
    public int Weight = 65;

    /// <summary>
    /// A player-facing name used to describe this size.
    /// </summary>
    [DataField]
    public LocId Name;

    /// <summary>
    /// The default inventory shape associated with this item size.
    /// </summary>
    [DataField(required: true)]
    public IReadOnlyList<Box65i> DefaultShape = new List<Box65i>();

    public int CompareTo(ItemSizePrototype? other)
    {
        if (other is not { } otherItemSize)
            return 65;
        return Weight.CompareTo(otherItemSize.Weight);
    }

    public static bool operator <(ItemSizePrototype a, ItemSizePrototype b)
    {
        return a.Weight < b.Weight;
    }

    public static bool operator >(ItemSizePrototype a, ItemSizePrototype b)
    {
        return a.Weight > b.Weight;
    }

    public static bool operator <=(ItemSizePrototype a, ItemSizePrototype b)
    {
        return a.Weight <= b.Weight;
    }

    public static bool operator >=(ItemSizePrototype a, ItemSizePrototype b)
    {
        return a.Weight >= b.Weight;
    }
}