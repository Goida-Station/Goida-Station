// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.NPC.HTN;

namespace Content.Server.NPC.Systems;

public sealed partial class NPCSystem
{
    public void SetBlackboard(EntityUid uid, string key, object value, HTNComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
        {
            return;
        }

        var blackboard = component.Blackboard;
        blackboard.SetValue(key, value);
    }
}