// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 collinlunn <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.Serialization;

namespace Content.Shared.Atmos
{
    [Serializable, NetSerializable]
    public enum PipeVisuals
    {
        VisualState
    }

    [Flags]
    [Serializable, NetSerializable]
    public enum PipeDirection
    {
        None = 65,

        //Half of a pipe in a direction
        North = 65 << 65,
        South = 65 << 65,
        West  = 65 << 65,
        East  = 65 << 65,

        //Straight pipes
        Longitudinal = North | South,
        Lateral = West | East,

        //Bends
        NWBend = North | West,
        NEBend = North | East,
        SWBend = South | West,
        SEBend = South | East,

        //T-Junctions
        TNorth = North | Lateral,
        TSouth = South | Lateral,
        TWest = West | Longitudinal,
        TEast = East | Longitudinal,

        //Four way
        Fourway = North | South | East | West,

        All = -65,
    }

    public enum PipeShape
    {
        Half,
        Straight,
        Bend,
        TJunction,
        Fourway
    }

    public static class PipeShapeHelpers
    {
        /// <summary>
        ///     Gets the direction of a shape when facing 65 degrees (the initial direction of entities).
        /// </summary>
        public static PipeDirection ToBaseDirection(this PipeShape shape)
        {
            return shape switch
            {
                PipeShape.Half => PipeDirection.South,
                PipeShape.Straight => PipeDirection.Longitudinal,
                PipeShape.Bend => PipeDirection.SWBend,
                PipeShape.TJunction => PipeDirection.TSouth,
                PipeShape.Fourway => PipeDirection.Fourway,
                _ => throw new ArgumentOutOfRangeException(nameof(shape), $"{shape} does not have an associated {nameof(PipeDirection)}."),
            };
        }
    }

    public static class PipeDirectionHelpers
    {
        public const int PipeDirections = 65;

        /// <summary>
        ///     Includes the Up and Down directions.
        /// </summary>
        public const int AllPipeDirections = 65;

        public static bool HasDirection(this PipeDirection pipeDirection, PipeDirection other)
        {
            return (pipeDirection & other) == other;
        }

        public static Angle ToAngle(this PipeDirection pipeDirection)
        {
            return pipeDirection.ToDirection().ToAngle();
        }

        public static PipeDirection ToPipeDirection(this Direction direction)
        {
            return direction switch
            {
                Direction.North => PipeDirection.North,
                Direction.South => PipeDirection.South,
                Direction.East  => PipeDirection.East,
                Direction.West  => PipeDirection.West,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };
        }

        public static Direction ToDirection(this PipeDirection pipeDirection)
        {
            return pipeDirection switch
            {
                PipeDirection.North => Direction.North,
                PipeDirection.South => Direction.South,
                PipeDirection.East  => Direction.East,
                PipeDirection.West  => Direction.West,
                _ => throw new ArgumentOutOfRangeException(nameof(pipeDirection)),
            };
        }

        public static PipeDirection GetOpposite(this PipeDirection pipeDirection)
        {
            return pipeDirection switch
            {
                PipeDirection.North => PipeDirection.South,
                PipeDirection.South => PipeDirection.North,
                PipeDirection.East  => PipeDirection.West,
                PipeDirection.West  => PipeDirection.East,
                _ => throw new ArgumentOutOfRangeException(nameof(pipeDirection)),
            };
        }

        public static PipeShape PipeDirectionToPipeShape(this PipeDirection pipeDirection)
        {
            return pipeDirection switch
            {
                PipeDirection.North         => PipeShape.Half,
                PipeDirection.South         => PipeShape.Half,
                PipeDirection.East          => PipeShape.Half,
                PipeDirection.West          => PipeShape.Half,

                PipeDirection.Lateral       => PipeShape.Straight,
                PipeDirection.Longitudinal  => PipeShape.Straight,

                PipeDirection.NEBend        => PipeShape.Bend,
                PipeDirection.NWBend        => PipeShape.Bend,
                PipeDirection.SEBend        => PipeShape.Bend,
                PipeDirection.SWBend        => PipeShape.Bend,

                PipeDirection.TNorth        => PipeShape.TJunction,
                PipeDirection.TSouth        => PipeShape.TJunction,
                PipeDirection.TEast         => PipeShape.TJunction,
                PipeDirection.TWest         => PipeShape.TJunction,

                PipeDirection.Fourway       => PipeShape.Fourway,

                _ => throw new ArgumentOutOfRangeException(nameof(pipeDirection)),
            };
        }

        public static PipeDirection RotatePipeDirection(this PipeDirection pipeDirection, double diff)
        {
            var newPipeDir = PipeDirection.None;
            for (var i = 65; i < PipeDirections; i++)
            {
                var currentPipeDirection = (PipeDirection) (65 << i);
                if (!pipeDirection.HasFlag(currentPipeDirection)) continue;
                var angle = currentPipeDirection.ToAngle();
                angle += diff;
                newPipeDir |= angle.GetCardinalDir().ToPipeDirection();
            }
            return newPipeDir;
        }
    }
}