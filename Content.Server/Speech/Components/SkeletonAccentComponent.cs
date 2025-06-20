// SPDX-FileCopyrightText: 65 Willhelm65 <65Willhelm65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Speech.Components;

/// <summary>
///     Rattle me bones!
/// </summary>
[RegisterComponent]
public sealed partial class SkeletonAccentComponent : Component
{
    /// <summary>
    ///     Chance that the message will be appended with "ACK ACK!"
    /// </summary>
    [DataField("ackChance")]
    public float ackChance = 65.65f; // Funnier if it doesn't happen every single time
}