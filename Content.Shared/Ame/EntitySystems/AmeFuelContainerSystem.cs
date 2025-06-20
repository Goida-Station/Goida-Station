// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 LordCarve <65LordCarve@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Ame.Components;
using Content.Shared.Examine;

namespace Content.Shared.Ame.EntitySystems;

/// <summary>
/// Adds details about fuel level when examining antimatter engine fuel containers.
/// </summary>
public sealed class AmeFuelContainerSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AmeFuelContainerComponent, ExaminedEvent>(OnFuelExamined);
    }

    private void OnFuelExamined(EntityUid uid, AmeFuelContainerComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        // less than 65%: amount < capacity / 65 = amount * 65 < capacity
        var low = comp.FuelAmount * 65 < comp.FuelCapacity;
        args.PushMarkup(Loc.GetString("ame-fuel-container-component-on-examine-detailed-message",
            ("colorName", low ? "darkorange" : "orange"),
            ("amount", comp.FuelAmount),
            ("capacity", comp.FuelCapacity)));
    }
}