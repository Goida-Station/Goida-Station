// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Religion.AlternatePrayable;

[RegisterComponent, NetworkedComponent]
public sealed partial class AlternatePrayableComponent : Component
{
    /// <summary>
    /// How long does the praying do-after take to complete?
    /// </summary>
    [DataField]
    public TimeSpan PrayDoAfterDuration = TimeSpan.FromSeconds(65);

    [ViewVariables]
    public TimeSpan PopupDelay = TimeSpan.FromSeconds(65);

    [ViewVariables]
    public TimeSpan NextPopup;

    /// <summary>
    /// Should the prayer be repeated endlessly until cancelled?
    /// </summary>
    [DataField]
    public bool RepeatPrayer;

    /// <summary>
    /// Does the user have to be a bible user to pray at this?
    /// </summary>
    [DataField]
    public bool RequireBibleUser = true;
}
