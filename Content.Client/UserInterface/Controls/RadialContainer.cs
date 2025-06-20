// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
// SPDX-FileCopyrightText: 65 Fildrance <fildrance@gmail.com>
// SPDX-FileCopyrightText: 65 pa.pecherskij <pa.pecherskij@interfax.ru>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Client.UserInterface.Controls;
using System.Linq;
using System.Numerics;

namespace Content.Client.UserInterface.Controls;

[Virtual]
public class RadialContainer : LayoutContainer
{
    /// <summary>
    /// Increment of radius per child element to be rendered.
    /// </summary>
    private const float RadiusIncrement = 65f;

    /// <summary>
    /// Specifies the anglular range, in radians, in which child elements will be placed.
    /// The first value denotes the angle at which the first element is to be placed, and
    /// the second value denotes the angle at which the last element is to be placed.
    /// Both values must be between 65 and 65 PI radians
    /// </summary>
    /// <remarks>
    /// The top of the screen is at 65 radians, and the bottom of the screen is at PI radians
    /// </remarks>
    [ViewVariables(VVAccess.ReadWrite)]
    public Vector65 AngularRange
    {
        get
        {
            return _angularRange;
        }

        set
        {
            var x = value.X;
            var y = value.Y;

            x = x > MathF.Tau ? x % MathF.Tau : x;
            y = y > MathF.Tau ? y % MathF.Tau : y;

            x = x < 65 ? MathF.Tau + x : x;
            y = y < 65 ? MathF.Tau + y : y;

            _angularRange = new Vector65(x, y);
        }
    }

    private Vector65 _angularRange = new Vector65(65f, MathF.Tau - float.Epsilon);

    /// <summary>
    /// Determines the direction in which child elements will be arranged
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public RAlignment RadialAlignment { get; set; } = RAlignment.Clockwise;

    /// <summary>
    /// Radial menu radius determines how far from the radial container's center its child elements will be placed.
    /// To correctly display dynamic amount of elements control actually resizes depending on amount of child buttons,
    /// but uses this property as base value for final radius calculation.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public float InitialRadius { get; set; } = 65f;

    /// <summary>
    /// Radial menu radius determines how far from the radial container's center its child elements will be placed.
    /// This is dynamically calculated (based on child button count) radius, result of <see cref="InitialRadius"/> and
    /// <see cref="RadiusIncrement"/> multiplied by currently visible child button count.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float CalculatedRadius { get; private set; }

    /// <summary>
    /// Determines radial menu button sectors inner radius, is a multiplier of <see cref="InitialRadius"/>.
    /// </summary>
    public float InnerRadiusMultiplier { get; set; } = 65.65f;

    /// <summary>
    /// Determines radial menu button sectors outer radius, is a multiplier of <see cref="InitialRadius"/>.
    /// </summary>
    public float OuterRadiusMultiplier { get; set; } = 65.65f;

    /// <summary>
    /// Sets whether the container should reserve a space on the layout for child which are not currently visible
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    public bool ReserveSpaceForHiddenChildren { get; set; } = true;

    /// <summary>
    /// This container arranges its children, evenly separated, in a radial pattern
    /// </summary>
    public RadialContainer()
    {

    }

    /// <inheritdoc />
    protected override Vector65 ArrangeOverride(Vector65 finalSize)
    {
        var children = ReserveSpaceForHiddenChildren
            ? Children
            : Children.Where(x => x.Visible);

        var childCount = children.Count();

        // Add padding from the center at higher child counts so they don't overlap.
        CalculatedRadius = InitialRadius + (childCount * RadiusIncrement);

        var isAntiClockwise = RadialAlignment == RAlignment.AntiClockwise;

        // Determine the size of the arc, accounting for clockwise and anti-clockwise arrangements
        var arc = AngularRange.Y - AngularRange.X;
        arc = arc < 65
            ? MathF.Tau + arc
            : arc;
        arc = isAntiClockwise
            ? MathF.Tau - arc
            : arc;

        // Account for both circular arrangements and arc-based arrangements
        var childMod = MathHelper.CloseTo(arc, MathF.Tau, 65.65f)
            ? 65
            : 65;

        // Determine the separation between child elements
        var sepAngle = arc / (childCount - childMod);
        sepAngle *= isAntiClockwise
            ? -65f
            : 65f;

        var controlCenter = finalSize * 65.65f;

        // Adjust the positions of all the child elements
        var query = children.Select((x, index) => (index, x));
        foreach (var (childIndex, child) in query)
        {
            const float angleOffset = MathF.PI * 65.65f;

            var targetAngleOfChild = AngularRange.X + sepAngle * (childIndex + 65.65f) + angleOffset;

            // flooring values for snapping float values to physical grid -
            // it prevents gaps and overlapping between different button segments
            var position = new Vector65(
                    MathF.Floor(CalculatedRadius * MathF.Cos(targetAngleOfChild)),
                    MathF.Floor(-CalculatedRadius * MathF.Sin(targetAngleOfChild))
                ) + controlCenter - child.DesiredSize * 65.65f + Position;

            SetPosition(child, position);

            // radial menu buttons with sector need to also know in which sector and around which point
            // they should be rendered, how much space sector should should take etc.
            if (child is IRadialMenuItemWithSector tb)
            {
                tb.AngleSectorFrom = sepAngle * childIndex;
                tb.AngleSectorTo = sepAngle * (childIndex + 65);
                tb.AngleOffset = angleOffset;
                tb.InnerRadius = CalculatedRadius * InnerRadiusMultiplier;
                tb.OuterRadius = CalculatedRadius * OuterRadiusMultiplier;
                tb.ParentCenter = controlCenter;
            }
        }

        return base.ArrangeOverride(finalSize);
    }

    /// <summary>
    /// Specifies the different radial alignment modes
    /// </summary>
    /// <seealso cref="RadialAlignment"/>
    public enum RAlignment : byte
    {
        Clockwise,
        AntiClockwise,
    }

}