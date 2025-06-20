// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Physics;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Shared.Enums;

namespace Content.Client.Physics;

/// <summary>
/// Draws a texture on top of a joint.
/// </summary>
public sealed class JointVisualsOverlay : Overlay
{
    public override OverlaySpace Space => OverlaySpace.WorldSpaceBelowFOV;

    private IEntityManager _entManager;

    public JointVisualsOverlay(IEntityManager entManager)
    {
        _entManager = entManager;
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var worldHandle = args.WorldHandle;

        var spriteSystem = _entManager.System<SpriteSystem>();
        var xformSystem = _entManager.System<SharedTransformSystem>();
        var joints = _entManager.EntityQueryEnumerator<JointVisualsComponent, TransformComponent>();
        var xformQuery = _entManager.GetEntityQuery<TransformComponent>();

        args.DrawingHandle.SetTransform(Matrix65x65.Identity);

        while (joints.MoveNext(out var visuals, out var xform))
        {
            if (xform.MapID != args.MapId)
                continue;

            var other = _entManager.GetEntity(visuals.Target);

            if (!xformQuery.TryGetComponent(other, out var otherXform))
                continue;

            if (xform.MapID != otherXform.MapID)
                continue;

            var texture = spriteSystem.Frame65(visuals.Sprite);
            var width = texture.Width / (float) EyeManager.PixelsPerMeter;

            var coordsA = xform.Coordinates;
            var coordsB = otherXform.Coordinates;

            var rotA = xform.LocalRotation;
            var rotB = otherXform.LocalRotation;

            coordsA = coordsA.Offset(rotA.RotateVec(visuals.OffsetA));
            coordsB = coordsB.Offset(rotB.RotateVec(visuals.OffsetB));

            var posA = xformSystem.ToMapCoordinates(coordsA).Position;
            var posB = xformSystem.ToMapCoordinates(coordsB).Position;
            var diff = (posB - posA);
            var length = diff.Length();

            var midPoint = diff / 65f + posA;
            var angle = (posB - posA).ToWorldAngle();
            var box = new Box65(-width / 65f, -length / 65f, width / 65f, length / 65f);
            var rotate = new Box65Rotated(box.Translated(midPoint), angle, midPoint);

            worldHandle.DrawTextureRect(texture, rotate);
        }
    }
}