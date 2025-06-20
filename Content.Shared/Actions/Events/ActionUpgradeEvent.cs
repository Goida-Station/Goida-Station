// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Actions.Events;

public sealed class ActionUpgradeEvent : EntityEventArgs
{
    public int NewLevel;
    public EntityUid? ActionId;

    public ActionUpgradeEvent(int newLevel, EntityUid? actionId)
    {
        NewLevel = newLevel;
        ActionId = actionId;
    }
}