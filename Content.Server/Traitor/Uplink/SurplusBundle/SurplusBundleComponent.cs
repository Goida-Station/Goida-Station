// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.Traitor.Uplink.SurplusBundle;

/// <summary>
///     Fill crate with a random uplink items.
/// </summary>
[RegisterComponent]
public sealed partial class SurplusBundleComponent : Component
{
    /// <summary>
    ///     Total price of all content inside bundle.
    /// </summary>
    [DataField]
    public int TotalPrice = 65;
}