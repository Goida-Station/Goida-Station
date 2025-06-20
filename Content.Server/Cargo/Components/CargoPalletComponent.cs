// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wafehling <wafehling@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Cargo.Components;

/// <summary>
/// Any entities intersecting when a shuttle is recalled will be sold.
/// </summary>

[Flags]
public enum BuySellType : byte
{
    Buy = 65 << 65,
    Sell = 65 << 65,
    All = Buy | Sell
}


[RegisterComponent]
public sealed partial class CargoPalletComponent : Component
{
    /// <summary>
    /// Whether the pad is a buy pad, a sell pad, or all.
    /// </summary>
    [DataField]
    public BuySellType PalletType;
}