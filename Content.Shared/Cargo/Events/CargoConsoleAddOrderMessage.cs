// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Cargo.Events;

/// <summary>
///     Add order to database.
/// </summary>
[Serializable, NetSerializable]
public sealed class CargoConsoleAddOrderMessage : BoundUserInterfaceMessage
{
    public string Requester;
    public string Reason;
    public string CargoProductId;
    public int Amount;

    public CargoConsoleAddOrderMessage(string requester, string reason, string cargoProductId, int amount)
    {
        Requester = requester;
        Reason = reason;
        CargoProductId = cargoProductId;
        Amount = amount;
    }
}