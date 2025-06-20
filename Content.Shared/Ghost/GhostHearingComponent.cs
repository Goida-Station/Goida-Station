// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Ghost;

/// <summary>
/// This is used for marking entities which should receive all local chat message, even when out of range
/// </summary>
[RegisterComponent]
public sealed partial class GhostHearingComponent : Component
{
}