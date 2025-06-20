// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Cargo.Components;

/// <summary>
/// This is used for setting a static, unchanging price for an object.
/// </summary>
[RegisterComponent]
public sealed partial class StaticPriceComponent : Component
{
    /// <summary>
    /// The price of the object this component is on.
    /// </summary>
    [DataField("price", required: true)]
    public double Price;
}