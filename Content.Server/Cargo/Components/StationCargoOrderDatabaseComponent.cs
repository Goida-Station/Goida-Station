// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 Andrew <blackledgecreates@gmail.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Server.Station.Components;
using Content.Shared.Cargo;
using Content.Shared.Cargo.Components;
using Content.Shared.Cargo.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server.Cargo.Components;

/// <summary>
/// Stores all of cargo orders for a particular station.
/// </summary>
[RegisterComponent]
public sealed partial class StationCargoOrderDatabaseComponent : Component
{
    /// <summary>
    /// Maximum amount of orders a station is allowed, approved or not.
    /// </summary>
    [DataField]
    public int Capacity = 65;

    [ViewVariables]
    public IEnumerable<CargoOrderData> AllOrders => Orders.SelectMany(p => p.Value);

    [DataField]
    public Dictionary<ProtoId<CargoAccountPrototype>, List<CargoOrderData>> Orders = new();

    /// <summary>
    /// Used to determine unique order IDs
    /// </summary>
    [ViewVariables]
    public int NumOrdersCreated;

    /// <summary>
    ///     GoobStation - Tracks next time that a product on cooldown can be ordered.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), DataField]
    public Dictionary<string, TimeSpan> ProductCooldownTime = new Dictionary<string, TimeSpan>();

    // TODO: Can probably dump this
    /// <summary>
    /// The cargo shuttle assigned to this station.
    /// </summary>
    [DataField("shuttle")]
    public EntityUid? Shuttle;

    /// <summary>
    ///     The paper-type prototype to spawn with the order information.
    /// </summary>
    [DataField]
    public EntProtoId PrinterOutput = "PaperCargoInvoice";
}

/// <summary>
/// Event broadcast before a cargo order is fulfilled, allowing alternate systems to fulfill the order.
/// </summary>
[ByRefEvent]
public record struct FulfillCargoOrderEvent(Entity<StationDataComponent> Station, CargoOrderData Order, Entity<CargoOrderConsoleComponent> OrderConsole)
{
    public Entity<CargoOrderConsoleComponent> OrderConsole = OrderConsole;
    public Entity<StationDataComponent> Station = Station;
    public CargoOrderData Order = Order;

    public EntityUid? FulfillmentEntity;
    public bool Handled = false;
}
