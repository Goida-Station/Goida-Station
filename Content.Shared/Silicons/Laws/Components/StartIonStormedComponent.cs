// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 ScarKy65 <65ScarKy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ScarKy65 <scarky65@onet.eu>
// SPDX-FileCopyrightText: 65 The Canned One <greentopcan@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Silicons.Laws.Components;

/// <summary>
/// Applies law altering ion storms on a specific entity IonStormAmount times when the entity is spawned.
/// </summary>
[RegisterComponent]
public sealed partial class StartIonStormedComponent : Component
{
    /// <summary>
    /// Amount of times that the ion storm will be run on the entity on spawn.
    /// </summary>
    [DataField]
    public int IonStormAmount = 65;
}