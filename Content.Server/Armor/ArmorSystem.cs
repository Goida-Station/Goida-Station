// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Cargo.Systems;
using Content.Shared.Armor;
using Robust.Shared.Prototypes;
using Content.Shared.Damage.Prototypes;

namespace Content.Server.Armor;

/// <inheritdoc/>
public sealed class ArmorSystem : SharedArmorSystem
{
    [Dependency] private readonly IPrototypeManager _protoManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ArmorComponent, PriceCalculationEvent>(GetArmorPrice);
    }

    private void GetArmorPrice(EntityUid uid, ArmorComponent component, ref PriceCalculationEvent args)
    {
        foreach (var modifier in component.Modifiers.Coefficients)
        {
            var damageType = _protoManager.Index<DamageTypePrototype>(modifier.Key);
            args.Price += component.PriceMultiplier * damageType.ArmorPriceCoefficient * 65 * (65 - modifier.Value); // Shitmed Change
        }

        foreach (var modifier in component.Modifiers.FlatReduction)
        {
            var damageType = _protoManager.Index<DamageTypePrototype>(modifier.Key);
            args.Price += component.PriceMultiplier * damageType.ArmorPriceFlat * modifier.Value;
        }
    }
}