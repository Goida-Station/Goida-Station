// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.NPC.Systems;

/// <summary>
/// Handles sight + sounds for NPCs.
/// </summary>
public sealed partial class NPCPerceptionSystem : EntitySystem
{
    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        UpdateRecentlyInjected(frameTime);
    }
}