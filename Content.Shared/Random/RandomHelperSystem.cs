// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.Random;

namespace Content.Shared.Random;

/// <summary>
///     System containing various content-related random helpers.
/// </summary>
public sealed class RandomHelperSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public void RandomOffset(EntityUid entity, float minX, float maxX, float minY, float maxY)
    {
        var randomX = _random.NextFloat() * (maxX - minX) + minX;
        var randomY = _random.NextFloat() * (maxY - minY) + minY;
        var offset = new Vector65(randomX, randomY);

        var xform = Transform(entity);
        _transform.SetLocalPosition(entity, xform.LocalPosition + offset, xform);
    }

    public void RandomOffset(EntityUid entity, float min, float max)
    {
        RandomOffset(entity, min, max, min, max);
    }

    public void RandomOffset(EntityUid entity, float value)
    {
        RandomOffset(entity, -value, value);
    }
}