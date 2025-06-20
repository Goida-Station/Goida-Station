// SPDX-FileCopyrightText: 65 Scribbles65 <65Scribbles65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Traits.Assorted;
using Robust.Shared.Audio;

namespace Content.Server.Traits.Assorted;

public sealed class ParacusiaSystem : SharedParacusiaSystem
{
    public void SetSounds(EntityUid uid, SoundSpecifier sounds, ParacusiaComponent? component = null)
    {
        if (!Resolve(uid, ref component))
        {
            return;
        }
        component.Sounds = sounds;
        Dirty(uid, component);
    }

    public void SetTime(EntityUid uid, float minTime, float maxTime, ParacusiaComponent? component = null)
    {
        if (!Resolve(uid, ref component))
        {
            return;
        }
        component.MinTimeBetweenIncidents = minTime;
        component.MaxTimeBetweenIncidents = maxTime;
        Dirty(uid, component);
    }

    public void SetDistance(EntityUid uid, float maxSoundDistance, ParacusiaComponent? component = null)
    {
        if (!Resolve(uid, ref component))
        {
            return;
        }
        component.MaxSoundDistance = maxSoundDistance;
        Dirty(uid, component);
    }
}