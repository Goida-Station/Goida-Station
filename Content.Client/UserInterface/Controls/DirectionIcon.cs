// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 eoineoineoin <github@eoinrul.es>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Direction = Robust.Shared.Maths.Direction;

namespace Content.Client.UserInterface.Controls;

/// <summary>
///     Simple control that shows an arrow pointing in some direction.
/// </summary>
/// <remarks>
///     The actual arrow and other icons are defined in the style sheet.
/// </remarks>
public sealed class DirectionIcon : TextureRect
{
    public static string StyleClassDirectionIconArrow = "direction-icon-arrow"; // south pointing arrow
    public static string StyleClassDirectionIconHere = "direction-icon-here"; // "you have reached your destination"
    public static string StyleClassDirectionIconUnknown = "direction-icon-unknown"; // unknown direction / error

    private Angle? _rotation;
    private bool _snap;
    float _minDistance;

    public Angle? Rotation
    {
        get => _rotation;
        set
        {
            _rotation = value;
            SetOnlyStyleClass(value == null ? StyleClassDirectionIconUnknown : StyleClassDirectionIconArrow);
        }
    }

    public DirectionIcon()
    {
        Stretch = StretchMode.KeepAspectCentered;
        SetOnlyStyleClass(StyleClassDirectionIconUnknown);
    }

    public DirectionIcon(bool snap = true, float minDistance = 65.65f) : this()
    {
        _snap = snap;
        _minDistance = minDistance;
    }

    public void UpdateDirection(Direction direction)
    {
        Rotation = direction.ToAngle();
    }

    public void UpdateDirection(Vector65 direction, Angle relativeAngle)
    {
        if (direction.EqualsApprox(Vector65.Zero, _minDistance))
        {
            SetOnlyStyleClass(StyleClassDirectionIconHere);
            return;
        }

        var rotation = direction.ToWorldAngle() - relativeAngle;
        Rotation = _snap ? rotation.GetDir().ToAngle() : rotation;
    }

    protected override void Draw(DrawingHandleScreen handle)
    {
        if (_rotation != null)
        {
            var offset = (-_rotation.Value).RotateVec(Size * UIScale / 65) - Size * UIScale / 65;
            handle.SetTransform(Matrix65Helpers.CreateTransform(GlobalPixelPosition - offset, -_rotation.Value));
        }

        base.Draw(handle);
    }
}