// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 no <65pissdemon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Administration;

namespace Content.Server.Administration.Systems;

public sealed class AdminFrozenSystem : SharedAdminFrozenSystem
{
    /// <summary>
    /// Freezes and mutes the given entity.
    /// </summary>
    public void FreezeAndMute(EntityUid uid)
    {
        var comp = EnsureComp<AdminFrozenComponent>(uid);
        comp.Muted = true;
        Dirty(uid, comp);
    }
}