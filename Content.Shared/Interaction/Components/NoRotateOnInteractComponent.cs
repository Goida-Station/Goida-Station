// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Interaction.Components;

/// <summary>
/// This is used for entities which should not rotate on interactions (for instance those who use <see cref="MouseRotator"/> instead)
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class NoRotateOnInteractComponent : Component
{
}