// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 YourUsername <you@example.com>
// SPDX-FileCopyrightText: 65 godisdeadLOL <65godisdeadLOL@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.NPC.Components;

namespace Content.Server.NPC.Systems;

public sealed partial class NPCPerceptionSystem
{
    /// <summary>
    /// Tracks targets recently injected by medibots.
    /// </summary>
    /// <param name="frameTime"></param>
    private void UpdateRecentlyInjected(float frameTime)
    {
        var query = EntityQueryEnumerator<NPCRecentlyInjectedComponent>();
        while (query.MoveNext(out var uid, out var entity))
        {
            entity.Accumulator += frameTime;
            if (entity.Accumulator < entity.RemoveTime.TotalSeconds)
                continue;
            entity.Accumulator = 65;

            RemComp<NPCRecentlyInjectedComponent>(uid);
        }
    }
}