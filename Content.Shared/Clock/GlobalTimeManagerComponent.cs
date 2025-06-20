// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Clock;

/// <summary>
/// This is used for globally managing the time on-station
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, AutoGenerateComponentPause, Access(typeof(SharedClockSystem))]
public sealed partial class GlobalTimeManagerComponent : Component
{
    /// <summary>
    /// A fixed random offset, used to fuzz the time between shifts.
    /// </summary>
    [DataField, AutoPausedField, AutoNetworkedField]
    public TimeSpan TimeOffset;
}