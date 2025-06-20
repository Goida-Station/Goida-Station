// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.ChronoLegionnaire.Components;

/// <summary>
/// Marks gun entity that will return in owner hand or belt when thrown
/// </summary>
[RegisterComponent]
public sealed partial class StasisGunComponent : Component
{
    /// <summary>
    /// Slot which weapon will attempt to return
    /// </summary>
    [DataField]
    public string ReturningSlot = "belt";
}