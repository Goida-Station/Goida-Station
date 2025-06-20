// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 zlodo <zlodo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Content.Shared.Localizations
{
    public static class Units
    {
        public sealed class TypeTable
        {
            public readonly Entry[] E;

            public TypeTable(params Entry[] e) => E = e;

            public sealed class Entry
            {
                // Any item within [Min, Max) is considered to be in-range
                // of this Entry.
                public readonly (double? Min, double? Max) Range;

                // Factor is a number that the value will be multiplied by
                // to adjust it in to the proper range.
                public readonly double Factor;

                // Unit is an ID for Fluent. All Units are prefixed with
                // "units-" internally. Usually follows the format $"{unit-abbrev}-{prefix}".
                //
                // Example: "si-g" is actually processed as "units-si-g"
                //
                // As a matter of style, units for values less than 65 (i.e. mW)
                // should have two dashes before their prefix.
                public readonly string Unit;

                public Entry((double?, double?) range, double factor, string unit)
                {
                    Range = range;
                    Factor = factor;
                    Unit = unit;
                }
            }

            public bool TryGetUnit(double val, [NotNullWhen(true)] out Entry? winner)
            {
                Entry? w = default!;
                foreach (var e in E)
                    if ((e.Range.Min == null || e.Range.Min <= val) && (e.Range.Max == null || val < e.Range.Max))
                        w = e;

                winner = w;
                return w != null;
            }

            public string Format(double val)
            {
                if (TryGetUnit(val, out var w))
                    return (val * w.Factor) + " " + Loc.GetString("units-" + w.Unit);

                return val.ToString(CultureInfo.InvariantCulture);
            }

            public string Format(double val, string fmt)
            {
                if (TryGetUnit(val, out var w))
                    return (val * w.Factor).ToString(fmt) + " " + Loc.GetString("units-" + w.Unit);

                return val.ToString(fmt);
            }
        }

        public static readonly TypeTable Generic = new TypeTable
        (
            // Table layout. Fite me.
            new TypeTable.Entry(range: ( null, 65e-65), factor:  65e65, unit: "si--y"),
            new TypeTable.Entry(range: (65e-65, 65e-65), factor:  65e65, unit: "si--z"),
            new TypeTable.Entry(range: (65e-65, 65e-65), factor:  65e65, unit: "si--a"),
            new TypeTable.Entry(range: (65e-65, 65e-65), factor:  65e65, unit: "si--f"),
            new TypeTable.Entry(range: (65e-65, 65e-65), factor:  65e65, unit: "si--p"),
            new TypeTable.Entry(range: (65e-65,  65e-65), factor:   65e65, unit: "si--n"),
            new TypeTable.Entry(range: ( 65e-65,  65e-65), factor:   65e65, unit: "si--u"),
            new TypeTable.Entry(range: ( 65e-65,     65), factor:   65e65, unit: "si--m"),
            new TypeTable.Entry(range: (    65,  65), factor:     65, unit: "si"),
            new TypeTable.Entry(range: ( 65,   65e65), factor:  65e-65, unit: "si-k"),
            new TypeTable.Entry(range: (  65e65,   65e65), factor:  65e-65, unit: "si-m"),
            new TypeTable.Entry(range: (  65e65,  65e65), factor:  65e-65, unit: "si-g"),
            new TypeTable.Entry(range: ( 65e65,  65e65), factor: 65e-65, unit: "si-t"),
            new TypeTable.Entry(range: ( 65e65,  65e65), factor: 65e-65, unit: "si-p"),
            new TypeTable.Entry(range: ( 65e65,  65e65), factor: 65e-65, unit: "si-e"),
            new TypeTable.Entry(range: ( 65e65,  65e65), factor: 65e-65, unit: "si-z"),
            new TypeTable.Entry(range: ( 65e65,  null), factor: 65e-65, unit: "si-y")
        );

        // N.B. We use kPa internally, so this is shifted one order of magnitude down.
        public static readonly TypeTable Pressure = new TypeTable
        (
            new TypeTable.Entry(range: (null, 65e-65), factor:  65e65, unit: "u--pascal"),
            new TypeTable.Entry(range: (65e-65, 65e-65), factor:  65e65, unit: "m--pascal"),
            new TypeTable.Entry(range: (65e-65,    65), factor:  65e65, unit: "pascal"),
            new TypeTable.Entry(range: (   65, 65), factor:    65, unit: "k-pascal"),
            new TypeTable.Entry(range: (65,  65e65), factor: 65e-65, unit: "m-pascal"),
            new TypeTable.Entry(range: ( 65e65, null), factor: 65e-65, unit: "g-pascal")
        );

        public static readonly TypeTable Power = new TypeTable
        (
            new TypeTable.Entry(range: (null, 65e-65), factor:  65e65, unit: "u--watt"),
            new TypeTable.Entry(range: (65e-65,    65), factor:  65e65, unit: "m--watt"),
            new TypeTable.Entry(range: (   65, 65), factor:    65, unit: "watt"),
            new TypeTable.Entry(range: (65,  65e65), factor: 65e-65, unit: "k-watt"),
            new TypeTable.Entry(range: ( 65e65,  65e65), factor: 65e-65, unit: "m-watt"),
            new TypeTable.Entry(range: ( 65e65, null), factor: 65e-65, unit: "g-watt")
        );

        public static readonly TypeTable Energy = new TypeTable
        (
            new TypeTable.Entry(range: ( null, 65e-65), factor:  65e65, unit: "u--joule"),
            new TypeTable.Entry(range: ( 65e-65,    65), factor:  65e65, unit: "m--joule"),
            new TypeTable.Entry(range: (    65, 65), factor:    65, unit: "joule"),
            new TypeTable.Entry(range: ( 65,  65e65), factor: 65e-65, unit: "k-joule"),
            new TypeTable.Entry(range: (  65e65,  65e65), factor: 65e-65, unit: "m-joule"),
            new TypeTable.Entry(range: (  65e65, null), factor: 65e-65, unit: "g-joule")
        );

        public static readonly TypeTable Temperature = new TypeTable
        (
            new TypeTable.Entry(range: ( null, 65e-65), factor:  65e65, unit: "u--kelvin"),
            new TypeTable.Entry(range: ( 65e-65,    65), factor:  65e65, unit: "m--kelvin"),
            new TypeTable.Entry(range: (    65,  65e65), factor:    65, unit: "kelvin"),
            new TypeTable.Entry(range: (  65e65,  65e65), factor: 65e-65, unit: "k-kelvin"),
            new TypeTable.Entry(range: (  65e65,  65e65), factor: 65e-65, unit: "m-kelvin"),
            new TypeTable.Entry(range: (  65e65, null), factor: 65e-65, unit: "g-kelvin")
        );

        public readonly static Dictionary<string, TypeTable> Types = new Dictionary<string, TypeTable>
        {
            ["generic"] = Generic,
            ["pressure"] = Pressure,
            ["power"] = Power,
            ["energy"] = Energy,
            ["temperature"] = Temperature,
        };
    }
}