// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Weapons.ThrowableBlocker;

/// <summary>
/// Added to items that are able to block thrown objects. These items must be able to reflect projectiles for it to work.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ThrowableBlockerComponent : Component
{
    [DataField]
    public SoundSpecifier? BlockSound;
}