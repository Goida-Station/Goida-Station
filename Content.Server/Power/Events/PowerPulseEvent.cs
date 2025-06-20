// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Power.Events;

/// <summary>
///     Invoked on a target entity, when it was pulsed with an energy.
///     For instance, interacted with an active stun baton.
/// </summary>
public sealed class PowerPulseEvent : EntityEventArgs
{
    public EntityUid? User;
    public EntityUid? Used;
}