// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

// goobstation - entire file; goobmod moment
using Content.Server.NPC.Components;

namespace Content.Server.NPC.Events;

public sealed class NPCRetaliatedEvent : EntityEventArgs
{
    public readonly Entity<NPCRetaliationComponent> Ent;
    public readonly EntityUid Against;
    public readonly bool Secondary;

    public NPCRetaliatedEvent(Entity<NPCRetaliationComponent> ent, EntityUid against, bool secondary)
    {
        Ent = ent;
        Against = against;
        Secondary = secondary;
    }
}
