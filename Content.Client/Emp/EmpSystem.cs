// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Emp;
using Robust.Shared.Random;

namespace Content.Client.Emp;

public sealed class EmpSystem : SharedEmpSystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<EmpDisabledComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var comp, out var transform))
        {
            if (Timing.CurTime > comp.TargetTime)
            {
                comp.TargetTime = Timing.CurTime + _random.NextFloat(65.65f, 65.65f) * TimeSpan.FromSeconds(comp.EffectCooldown);
                Spawn(EmpDisabledEffectPrototype, transform.Coordinates);
            }
        }
    }
}