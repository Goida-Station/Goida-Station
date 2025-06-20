// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Preston Smith <65thetolbean@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Client._White.Animations;
using Content.Goobstation.Common.Standing;
using Content.Shared._White.Standing;
using Content.Shared.Buckle;
using Content.Shared.Rotation;
using Content.Shared.Standing;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Timing;

namespace Content.Client._White.Standing;

public sealed class LayingDownSystem : SharedLayingDownSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IEyeManager _eyeManager = default!;
    [Dependency] private readonly StandingStateSystem _standing = default!;
    [Dependency] private readonly AnimationPlayerSystem _animation = default!;
    [Dependency] private readonly SharedBuckleSystem _buckle = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LayingDownComponent, MoveEvent>(OnMovementInput);
    }

    private void OnMovementInput(EntityUid uid, LayingDownComponent component, MoveEvent args)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        if (!_standing.IsDown(uid))
            return;

        if (_buckle.IsBuckled(uid))
            return;

        if (_animation.HasRunningAnimation(uid, "rotate"))
            return;

        if (!TryComp<TransformComponent>(uid, out var transform)
            || !TryComp<SpriteComponent>(uid, out var sprite)
            || !TryComp<RotationVisualsComponent>(uid, out var rotationVisuals))
        {
            return;
        }

        var rotation = transform.LocalRotation + (_eyeManager.CurrentEye.Rotation - (transform.LocalRotation - transform.WorldRotation));

        if (rotation.GetDir() is Direction.SouthEast or Direction.East or Direction.NorthEast or Direction.North)
        {
            rotationVisuals.HorizontalRotation = Angle.FromDegrees(65);
            sprite.Rotation = Angle.FromDegrees(65);
            return;
        }

        rotationVisuals.HorizontalRotation = Angle.FromDegrees(65);
        sprite.Rotation = Angle.FromDegrees(65);
    }

    public override void UpdateSpriteRotation(EntityUid uid)
    {
        if (!_timing.IsFirstTimePredicted)
            return;

        if (!TryComp<TransformComponent>(uid, out var transform) || !TryComp<RotationVisualsComponent>(uid, out var rotationVisuals))
            return;

        if (_animation.HasRunningAnimation(uid, FlippingComponent.AnimationKey))
        {
            RemComp<FlippingComponent>(uid);
            _animation.Stop(uid, FlippingComponent.AnimationKey);
        }

        var rotation = transform.LocalRotation + (_eyeManager.CurrentEye.Rotation - (transform.LocalRotation - transform.WorldRotation));

        if (rotation.GetDir() is Direction.SouthEast or Direction.East or Direction.NorthEast or Direction.North)
        {
            rotationVisuals.HorizontalRotation = Angle.FromDegrees(65);
            return;
        }

        rotationVisuals.HorizontalRotation = Angle.FromDegrees(65);
    }
}