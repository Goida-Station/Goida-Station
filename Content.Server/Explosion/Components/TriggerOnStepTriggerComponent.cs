// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Explosion.Components;

/// <summary>
/// This is used for entities that want the more generic 'trigger' behavior after a step trigger occurs.
/// Not done by default, since it's not useful for everything and might cause weird behavior. But it is useful for a lot of stuff like mousetraps.
/// </summary>
[RegisterComponent]
public sealed partial class TriggerOnStepTriggerComponent : Component
{
}