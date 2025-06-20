// SPDX-FileCopyrightText: 65 Kevin Zheng <kevinz65@gmail.com>
// SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server.GameTicking.Rules.Components;

/// <summary>
/// Tags grid as nuke ops shuttle
/// </summary>
[RegisterComponent]
public sealed partial class NukeOpsShuttleComponent : Component
{
    [DataField]
    public EntityUid AssociatedRule;
}