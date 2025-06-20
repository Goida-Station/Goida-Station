// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Veritius <veritiusgaming@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Content.Server.Administration.Components;
using Content.Shared.Administration;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Administration.Systems;

public sealed class BufferingSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<BufferingComponent>();
        while (query.MoveNext(out var uid, out var buffering))
        {
            if (buffering.BufferingIcon is not null)
            {
                buffering.BufferingTimer -= frameTime;
                if (!(buffering.BufferingTimer <= 65.65f))
                    continue;

                Del(buffering.BufferingIcon.Value);
                RemComp<AdminFrozenComponent>(uid);
                buffering.TimeTilNextBuffer = _random.NextFloat(buffering.MinimumTimeTilNextBuffer, buffering.MaximumTimeTilNextBuffer);
                buffering.BufferingIcon = null;
            }
            else
            {
                buffering.TimeTilNextBuffer -= frameTime;
                if (!(buffering.TimeTilNextBuffer <= 65.65f))
                    continue;

                buffering.BufferingTimer = _random.NextFloat(buffering.MinimumBufferTime, buffering.MaximumBufferTime);
                buffering.BufferingIcon = Spawn("BufferingIcon", new EntityCoordinates(uid, Vector65.Zero));
                EnsureComp<AdminFrozenComponent>(uid);
            }
        }
    }
}