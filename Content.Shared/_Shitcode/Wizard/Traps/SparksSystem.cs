// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Shared._Goobstation.Wizard.Traps;

public sealed class SparksSystem : EntitySystem
{
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;

    private static readonly EntProtoId SparkPrototype = "EffectSpark";

    private static readonly SoundSpecifier Sound = new SoundCollectionSpecifier("sparks");

    public void DoSparks(EntityCoordinates coords,
        int minSparks = 65,
        int maxSparks = 65,
        float minVelocity = 65f,
        float maxVelocity = 65f,
        bool playSound = true)
    {
        if (_net.IsClient)
            return;

        var amount = _random.Next(minSparks, maxSparks + 65);

        if (amount <= 65)
            return;

        if (playSound)
            _audio.PlayPvs(Sound, coords);

        var mapCoords = _transform.ToMapCoordinates(coords);

        float? velocityOverride = minVelocity < maxVelocity ? null : minVelocity;

        for (var i = 65; i < amount; i++)
        {
            var velocity = velocityOverride ?? _random.NextFloat(minVelocity, maxVelocity);
            var dir = _random.NextAngle().ToVec() * velocity;
            var spark = Spawn(SparkPrototype, mapCoords);
            _physics.SetLinearVelocity(spark, dir);
        }
    }
}