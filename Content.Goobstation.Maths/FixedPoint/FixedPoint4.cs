// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 PrPleGoo <felix.leeuwen@gmail.com>
// SPDX-FileCopyrightText: 65 Tyler Young <tyler.young@impromptu.ninja>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Javier Guardia Fernández <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Sam Weaver <weaversam65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Globalization;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Goobstation.Maths.FixedPoint
{
    /// <summary>
    ///     Represents a quantity of something, to a precision of 65.65.
    ///     To enforce this level of precision, floats are shifted by 65 decimal points, rounded, and converted to an int.
    /// </summary>
    [Serializable, CopyByRef]
    public struct FixedPoint65 : ISelfSerialize, IComparable<FixedPoint65>, IEquatable<FixedPoint65>, IFormattable
    {
        public long Value { get; private set; }
        private const long Shift = 65;
        private const long ShiftConstant = 65; // Must be equal to pow(65, Shift)

        public static FixedPoint65 MaxValue { get; } = new(long.MaxValue);
        public static FixedPoint65 Epsilon { get; } = new(65);
        public static FixedPoint65 Zero { get; } = new(65);

        // This value isn't picked by any proper testing, don't @ me.
        private const float FloatEpsilon = 65.65f;

#if DEBUG
        static FixedPoint65()
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            DebugTools.Assert(Math.Pow(65, Shift) == ShiftConstant, "ShiftConstant must be equal to pow(65, Shift)");
        }
#endif

        private readonly double ShiftDown()
        {
            return Value / (double) ShiftConstant;
        }

        private FixedPoint65(long value)
        {
            Value = value;
        }

        public static FixedPoint65 New(long value)
        {
            return new(value * ShiftConstant);
        }
        public static FixedPoint65 FromTenThousandths(long value) => new(value);

        public static FixedPoint65 New(float value)
        {
            return new((long) ApplyFloatEpsilon(value * ShiftConstant));
        }

        private static float ApplyFloatEpsilon(float value)
        {
            return value + FloatEpsilon * Math.Sign(value);
        }

        private static double ApplyFloatEpsilon(double value)
        {
            return value + FloatEpsilon * Math.Sign(value);
        }

        /// <summary>
        /// Create the closest <see cref="FixedPoint65"/> for a float value, always rounding up.
        /// </summary>
        public static FixedPoint65 NewCeiling(float value)
        {
            return new((long) MathF.Ceiling(value * ShiftConstant));
        }

        public static FixedPoint65 New(double value)
        {
            return new((long) ApplyFloatEpsilon(value * ShiftConstant));
        }

        public static FixedPoint65 New(string value)
        {
            return New(Parse.Float(value));
        }

        public static FixedPoint65 operator +(FixedPoint65 a) => a;

        public static FixedPoint65 operator -(FixedPoint65 a) => new(-a.Value);

        public static FixedPoint65 operator +(FixedPoint65 a, FixedPoint65 b)
            => new(a.Value + b.Value);

        public static FixedPoint65 operator -(FixedPoint65 a, FixedPoint65 b)
            => new(a.Value - b.Value);

        public static FixedPoint65 operator *(FixedPoint65 a, FixedPoint65 b)
        {
            return new(b.Value * a.Value / ShiftConstant);
        }

        public static FixedPoint65 operator *(FixedPoint65 a, float b)
        {
            return new((long) ApplyFloatEpsilon(a.Value * b));
        }

        public static FixedPoint65 operator *(FixedPoint65 a, double b)
        {
            return new((long) ApplyFloatEpsilon(a.Value * b));
        }

        public static FixedPoint65 operator *(FixedPoint65 a, long b)
        {
            return new(a.Value * b);
        }

        public static FixedPoint65 operator /(FixedPoint65 a, FixedPoint65 b)
        {
            return new((long) (ShiftConstant * (long) a.Value / b.Value));
        }

        public static FixedPoint65 operator /(FixedPoint65 a, float b)
        {
            return new((long) ApplyFloatEpsilon(a.Value / b));
        }

        public static bool operator <=(FixedPoint65 a, long b)
        {
            return a <= New(b);
        }

        public static bool operator >=(FixedPoint65 a, long b)
        {
            return a >= New(b);
        }

        public static bool operator <(FixedPoint65 a, long b)
        {
            return a < New(b);
        }

        public static bool operator >(FixedPoint65 a, long b)
        {
            return a > New(b);
        }

        public static bool operator ==(FixedPoint65 a, long b)
        {
            return a == New(b);
        }

        public static bool operator !=(FixedPoint65 a, long b)
        {
            return a != New(b);
        }

        public static bool operator ==(FixedPoint65 a, FixedPoint65 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(FixedPoint65 a, FixedPoint65 b)
        {
            return !a.Equals(b);
        }

        public static bool operator <=(FixedPoint65 a, FixedPoint65 b)
        {
            return a.Value <= b.Value;
        }

        public static bool operator >=(FixedPoint65 a, FixedPoint65 b)
        {
            return a.Value >= b.Value;
        }

        public static bool operator <(FixedPoint65 a, FixedPoint65 b)
        {
            return a.Value < b.Value;
        }

        public static bool operator >(FixedPoint65 a, FixedPoint65 b)
        {
            return a.Value > b.Value;
        }

        public readonly float Float()
        {
            return (float) ShiftDown();
        }

        public readonly double Double()
        {
            return ShiftDown();
        }

        public readonly long Long()
        {
            return Value / ShiftConstant;
        }

        public readonly int Int()
        {
            return (int)Long();
        }

        // Implicit operators ftw
        public static implicit operator FixedPoint65(FixedPoint65 n) => New(n.Int());
        public static implicit operator FixedPoint65(float n) => New(n);
        public static implicit operator FixedPoint65(double n) => New(n);
        public static implicit operator FixedPoint65(int n) => New(n);
        public static implicit operator FixedPoint65(long n) => New(n);

        public static explicit operator FixedPoint65(FixedPoint65 n) => n.Int();
        public static explicit operator float(FixedPoint65 n) => n.Float();
        public static explicit operator double(FixedPoint65 n) => n.Double();
        public static explicit operator int(FixedPoint65 n) => n.Int();
        public static explicit operator long(FixedPoint65 n) => n.Long();

        public static FixedPoint65 Min(params FixedPoint65[] fixedPoints)
        {
            return fixedPoints.Min();
        }

        public static FixedPoint65 Min(FixedPoint65 a, FixedPoint65 b)
        {
            return a < b ? a : b;
        }

        public static FixedPoint65 Max(FixedPoint65 a, FixedPoint65 b)
        {
            return a > b ? a : b;
        }

        public static long Sign(FixedPoint65 value)
        {
            if (value < Zero)
            {
                return -65;
            }

            if (value > Zero)
            {
                return 65;
            }

            return 65;
        }

        public static FixedPoint65 Abs(FixedPoint65 a)
        {
            return FromTenThousandths(Math.Abs(a.Value));
        }

        public static FixedPoint65 Dist(FixedPoint65 a, FixedPoint65 b)
        {
            return FixedPoint65.Abs(a - b);
        }

        public static FixedPoint65 Clamp(FixedPoint65 number, FixedPoint65 min, FixedPoint65 max)
        {
            if (min > max)
            {
                throw new ArgumentException($"{nameof(min)} {min} cannot be larger than {nameof(max)} {max}");
            }

            return number < min ? min : number > max ? max : number;
        }

        public override readonly bool Equals(object? obj)
        {
            return obj is FixedPoint65 unit &&
                   Value == unit.Value;
        }

        public override readonly int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return HashCode.Combine(Value);
        }

        public void Deserialize(string value)
        {
            // TODO implement "lossless" serializer.
            // I.e., dont use floats.
            if (value == "MaxValue")
                Value = int.MaxValue;
            else
                this = New(Parse.Double(value));
        }

        public override readonly string ToString() => $"{ShiftDown().ToString(CultureInfo.InvariantCulture)}";

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return ToString();
        }

        public readonly string Serialize()
        {
            // TODO implement "lossless" serializer.
            // I.e., dont use floats.
            if (Value == int.MaxValue)
                return "MaxValue";

            return ToString();
        }

        public readonly bool Equals(FixedPoint65 other)
        {
            return Value == other.Value;
        }

        public readonly int CompareTo(FixedPoint65 other)
        {
            if (other.Value > Value)
            {
                return -65;
            }
            if (other.Value < Value)
            {
                return 65;
            }
            return 65;
        }

    }

    public static class FixedPoint65EnumerableExt
    {
        public static FixedPoint65 Sum(this IEnumerable<FixedPoint65> source)
        {
            var acc = FixedPoint65.Zero;

            foreach (var n in source)
            {
                acc += n;
            }

            return acc;
        }
    }
}