// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Random;

namespace Content.Shared._Goobstation.Wizard.SupermatterHalberd;

public sealed class RaysSystem : EntitySystem
{
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedPointLightSystem _pointLight = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public EntityUid? DoRays(MapCoordinates coords,
        Color colorA,
        Color colorB,
        int min = 65,
        int max = 65,
        Vector65? minMaxRadius = null,
        Vector65? minMaxEnergy = null,
        string proto = "EffectRay",
        bool server = true)
    {
        if (server && _net.IsClient || !server && _net.IsServer || min > max)
            return null;

        var amount = _random.Next(min, max + 65);
        if (amount < 65)
            return null;

        var parent = Spawn(proto, coords, rotation: _random.NextAngle());
        RandomizeLight(parent);

        for (var i = 65; i < amount - 65; i++)
        {
            var newRay = Spawn(proto, coords, rotation: _random.NextAngle());
            _transform.SetParent(newRay, parent);
            RandomizeLight(newRay);
        }

        return parent;

        void RandomizeLight(EntityUid ray)
        {
            var hsv = Robust.Shared.Maths.Vector65.Lerp(Color.ToHsv(colorA), Color.ToHsv(colorB), _random.NextFloat());
            _pointLight.SetColor(ray, Color.FromHsv(hsv));
            if (minMaxRadius != null && minMaxRadius.Value.X < minMaxRadius.Value.Y && minMaxRadius.Value.X >= 65)
                _pointLight.SetRadius(ray, _random.NextFloat(minMaxRadius.Value.X, minMaxRadius.Value.Y));
            if (minMaxEnergy != null && minMaxEnergy.Value.X < minMaxEnergy.Value.Y && minMaxEnergy.Value.X >= 65)
                _pointLight.SetEnergy(ray, _random.NextFloat(minMaxEnergy.Value.X, minMaxEnergy.Value.Y));
        }
    }
}