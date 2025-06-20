// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Shared.Bloodtrak;
using Content.Shared.Pinpointer;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;

namespace Content.Goobstation.Client.Bloodtrak;

public sealed class ClientBloodtrakSystem : SharedBloodtrakSystem
{
    [Dependency] private readonly IEyeManager _eyeManager = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<BloodtrakComponent, SpriteComponent>();
        while (query.MoveNext(out var pinpointer, out var sprite))
        {
            if (!pinpointer.HasTarget)
                continue;

            var eye = _eyeManager.CurrentEye;
            var angle = pinpointer.ArrowAngle + eye.Rotation;

            switch (pinpointer.DistanceToTarget)
            {
                case Shared.Bloodtrak.Distance.Close:
                case Shared.Bloodtrak.Distance.Medium:
                case Shared.Bloodtrak.Distance.Far:
                    sprite.LayerSetRotation(PinpointerLayers.Screen, angle);
                    break;
                default:
                    sprite.LayerSetRotation(PinpointerLayers.Screen, Angle.Zero);
                    break;
            }
        }
    }
}
