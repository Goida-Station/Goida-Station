// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Speech;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Speech;

/// <summary>
/// Marks clothing that change wearer speech sound (for example - human talking like borg when wearing borg head (just example))
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SpeechSoundsReplacerComponent : Component
{
    /// <summary>
    /// A substitute sound
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField]
    public ProtoId<SpeechSoundsPrototype>? SpeechSounds;

    /// <summary>
    /// Previous sound that returns when you unequip clothing
    /// </summary>
    [DataField]
    public ProtoId<SpeechSoundsPrototype>? PreviousSound;
}