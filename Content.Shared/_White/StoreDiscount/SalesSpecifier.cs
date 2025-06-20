// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared._White.StoreDiscount;

[DataDefinition]
public sealed partial class SalesSpecifier
{
    [DataField]
    public bool Enabled { get; private set; }

    [DataField]
    public float MinMultiplier { get; private set; }

    [DataField]
    public float MaxMultiplier { get; private set; }

    [DataField]
    public int MinItems { get; private set; }

    [DataField]
    public int MaxItems { get; private set; }

    [DataField]
    public string SalesCategory { get; private set; } = string.Empty;

    public SalesSpecifier()
    {
    }

    public SalesSpecifier(bool enabled, float minMultiplier, float maxMultiplier, int minItems, int maxItems,
        string salesCategory)
    {
        Enabled = enabled;
        MinMultiplier = minMultiplier;
        MaxMultiplier = maxMultiplier;
        MinItems = minItems;
        MaxItems = maxItems;
        SalesCategory = salesCategory;
    }
}