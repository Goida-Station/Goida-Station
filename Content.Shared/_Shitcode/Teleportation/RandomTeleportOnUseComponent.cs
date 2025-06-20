// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Teleportation;

/// <summary>
///     Entity that will randomly teleport the user when used in hand.
/// </summary>
[RegisterComponent]
public sealed partial class RandomTeleportOnUseComponent : RandomTeleportComponent
{
    /// <summary>
    ///     Whether to consume this item on use; consumes only one if it's a stack
    /// </summary>
    [DataField] public bool ConsumeOnUse = true;
}
