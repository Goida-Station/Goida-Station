// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Damage.ForceSay;

/// <summary>
///     Sent to clients as a network event when their entity contains <see cref="DamageForceSayComponent"/>
///     that COMMANDS them to speak the current message in their chatbox
/// </summary>
[Serializable, NetSerializable]
public sealed class DamageForceSayEvent : EntityEventArgs
{
    public string? Suffix;
}