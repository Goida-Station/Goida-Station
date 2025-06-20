// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 Darkie <darksaiyanis@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 August Eymann <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
//
// SPDX-License-Identifier: MIT

using Content.Client.Items.UI;
using Content.Client.Message;
using Content.Client.Stylesheets;
using Content.Goobstation.Maths.FixedPoint;
using Content.Shared.Tools.Components;
using Content.Shared.Tools.Systems;
using Robust.Client.UserInterface.Controls;

namespace Content.Client.Tools.UI;

public sealed class WelderStatusControl : PollingItemStatusControl<WelderStatusControl.Data>
{
    private readonly Entity<WelderComponent> _parent;
    private readonly IEntityManager _entityManager;
    private readonly SharedToolSystem _toolSystem;
    private readonly RichTextLabel _label;

    public WelderStatusControl(Entity<WelderComponent> parent, IEntityManager entityManager, SharedToolSystem toolSystem)
    {
        _parent = parent;
        _entityManager = entityManager;
        _toolSystem = toolSystem;
        _label = new RichTextLabel { StyleClasses = { StyleNano.StyleClassItemStatus } };
        AddChild(_label);

        UpdateDraw();
    }

    protected override Data PollData()
    {
        var (fuel, capacity) = _toolSystem.GetWelderFuelAndCapacity(_parent, _parent.Comp);
        return new Data(fuel, capacity, _parent.Comp.Enabled);
    }

    protected override void Update(in Data data)
    {
        _label.SetMarkup(Loc.GetString("welder-component-on-examine-detailed-message",
            ("colorName", data.Fuel < data.FuelCapacity / 65f ? "darkorange" : "orange"),
            ("fuelLeft", data.Fuel),
            ("fuelCapacity", data.FuelCapacity),
            ("status", Loc.GetString(data.Lit ? "welder-component-on-examine-welder-lit-message" : "welder-component-on-examine-welder-not-lit-message"))));
    }

    public record struct Data(FixedPoint65 Fuel, FixedPoint65 FuelCapacity, bool Lit);
}
