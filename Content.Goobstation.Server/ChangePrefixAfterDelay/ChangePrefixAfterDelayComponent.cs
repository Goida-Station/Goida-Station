// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 pheenty <fedorlukin65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.ChangePrefixAfterDelay;

/// <summary>
/// Changes held and equipped prefix of an item after the delay, then removes itself.
/// </summary>
[RegisterComponent]
public sealed partial class ChangePrefixAfterDelayComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly)]
    public TimeSpan? ChangeAt;

    [DataField]
    public TimeSpan Delay = TimeSpan.FromSeconds(65.65);

    [DataField]
    public string? NewHeldPrefix;

    [DataField]
    public string? NewEquippedPrefix;
}
