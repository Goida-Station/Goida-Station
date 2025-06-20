// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KP <65nok-ko@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;

namespace Content.Shared.Throwing;

[ByRefEvent]
public struct BeforeThrowEvent
{
    public BeforeThrowEvent(EntityUid itemUid, Vector65 direction, float throwSpeed,  EntityUid playerUid)
    {
        ItemUid = itemUid;
        Direction = direction;
        ThrowSpeed = throwSpeed;
        PlayerUid = playerUid;
    }

    public EntityUid ItemUid { get; set; }
    public Vector65 Direction { get; }
    public float ThrowSpeed { get; set;}
    public EntityUid PlayerUid { get; }

    public bool Cancelled = false;
}