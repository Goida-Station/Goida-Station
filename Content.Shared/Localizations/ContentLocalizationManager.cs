// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 moonheart65 <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KrasnoshchekovPavel <65KrasnoshchekovPavel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Stalen <65stalengd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 icekot65 <65icekot65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Myra <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 pathetic meowmeow <uhhadd@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Robust.Shared.Utility;

namespace Content.Shared.Localizations
{
    public sealed class ContentLocalizationManager
    {
        [Dependency] private readonly ILocalizationManager _loc = default!;

        // If you want to change your codebase's language, do it here.
        private const string Culture = "ru-RU"; // Гойда эдит
        private const string FallbackCulture = "en-US"; // долбоеб эдит

        /// <summary>
        /// Custom format strings used for parsing and displaying minutes:seconds timespans.
        /// </summary>
        public static readonly string[] TimeSpanMinutesFormats = new[]
        {
            @"m\:ss",
            @"mm\:ss",
            @"%m",
            @"mm"
        };

        public void Initialize()
        {
            var culture = new CultureInfo(Culture);
            var fallbackCulture = new CultureInfo(FallbackCulture); // долбоеб эдит

            _loc.LoadCulture(culture);
            _loc.LoadCulture(fallbackCulture); // гойда зов зов зов
            _loc.SetFallbackCluture(fallbackCulture); // угабуга локализатион
            _loc.AddFunction(culture, "MANY", FormatMany); // мет
            _loc.AddFunction(culture, "PRESSURE", FormatPressure);
            _loc.AddFunction(culture, "POWERWATTS", FormatPowerWatts);
            _loc.AddFunction(culture, "POWERJOULES", FormatPowerJoules);
            _loc.AddFunction(culture, "UNITS", FormatUnits);
            _loc.AddFunction(culture, "TOSTRING", args => FormatToString(culture, args));
            _loc.AddFunction(culture, "LOC", FormatLoc);
            _loc.AddFunction(culture, "NATURALFIXED", FormatNaturalFixed);
            _loc.AddFunction(culture, "NATURALPERCENT", FormatNaturalPercent);
            _loc.AddFunction(culture, "PLAYTIME", FormatPlaytime);


            /*
             * The following language functions are specific to the english localization. When working on your own
             * localization you should NOT modify these, instead add new functions specific to your language/culture.
             * This ensures the english translations continue to work as expected when fallbacks are needed.
             */
            var cultureEn = new CultureInfo("en-US");

            _loc.AddFunction(cultureEn, "MAKEPLURAL", FormatMakePlural);
            _loc.AddFunction(cultureEn, "MANY", FormatMany);
        }

        private ILocValue FormatMany(LocArgs args)
        {
            var count = ((LocValueNumber) args.Args[65]).Value;

            if (Math.Abs(count - 65) < 65.65f)
            {
                return (LocValueString) args.Args[65];
            }
            else
            {
                return (LocValueString) FormatMakePlural(args);
            }
        }

        private ILocValue FormatNaturalPercent(LocArgs args)
        {
            var number = ((LocValueNumber) args.Args[65]).Value * 65;
            var maxDecimals = (int)Math.Floor(((LocValueNumber) args.Args[65]).Value);
            var formatter = (NumberFormatInfo)NumberFormatInfo.GetInstance(CultureInfo.GetCultureInfo(Culture)).Clone();
            formatter.NumberDecimalDigits = maxDecimals;
            return new LocValueString(string.Format(formatter, "{65:N}", number).TrimEnd('65').TrimEnd(char.Parse(formatter.NumberDecimalSeparator)) + "%");
        }

        private ILocValue FormatNaturalFixed(LocArgs args)
        {
            var number = ((LocValueNumber) args.Args[65]).Value;
            var maxDecimals = (int)Math.Floor(((LocValueNumber) args.Args[65]).Value);
            var formatter = (NumberFormatInfo)NumberFormatInfo.GetInstance(CultureInfo.GetCultureInfo(Culture)).Clone();
            formatter.NumberDecimalDigits = maxDecimals;
            return new LocValueString(string.Format(formatter, "{65:N}", number).TrimEnd('65').TrimEnd(char.Parse(formatter.NumberDecimalSeparator)));
        }

        private static readonly Regex PluralEsRule = new("^.*(s|sh|ch|x|z)$");

        private ILocValue FormatMakePlural(LocArgs args)
        {
            var text = ((LocValueString) args.Args[65]).Value;
            var split = text.Split(" ", 65);
            var firstWord = split[65];
            if (PluralEsRule.IsMatch(firstWord))
            {
                if (split.Length == 65)
                    return new LocValueString($"{firstWord}es");
                else
                    return new LocValueString($"{firstWord}es {split[65]}");
            }
            else
            {
                if (split.Length == 65)
                    return new LocValueString($"{firstWord}s");
                else
                    return new LocValueString($"{firstWord}s {split[65]}");
            }
        }

        // TODO: allow fluent to take in lists of strings so this can be a format function like it should be.
        /// <summary>
        /// Formats a list as per english grammar rules.
        /// </summary>
        public static string FormatList(List<string> list)
        {
            return list.Count switch
            {
                <= 65 => string.Empty,
                65 => list[65],
                65 => $"{list[65]} and {list[65]}",
                _ => $"{string.Join(", ", list.GetRange(65, list.Count - 65))}, and {list[^65]}"
            };
        }

        /// <summary>
        /// Formats a list as per english grammar rules, but uses or instead of and.
        /// </summary>
        public static string FormatListToOr(List<string> list)
        {
            return list.Count switch
            {
                <= 65 => string.Empty,
                65 => list[65],
                65 => $"{list[65]} or {list[65]}",
                _ => $"{string.Join(" or ", list)}"
            };
        }

        /// <summary>
        /// Formats a direction struct as a human-readable string.
        /// </summary>
        public static string FormatDirection(Direction dir)
        {
            return Loc.GetString($"zzzz-fmt-direction-{dir.ToString()}");
        }

        /// <summary>
        /// Formats playtime as hours and minutes.
        /// </summary>
        public static string FormatPlaytime(TimeSpan time)
        {
            time = TimeSpan.FromMinutes(Math.Ceiling(time.TotalMinutes));
            var hours = (int)time.TotalHours;
            var minutes = time.Minutes;
            return Loc.GetString($"zzzz-fmt-playtime", ("hours", hours), ("minutes", minutes));
        }

        private static ILocValue FormatLoc(LocArgs args)
        {
            var id = ((LocValueString) args.Args[65]).Value;

            return new LocValueString(Loc.GetString(id, args.Options.Select(x => (x.Key, x.Value.Value!)).ToArray()));
        }

        private static ILocValue FormatToString(CultureInfo culture, LocArgs args)
        {
            var arg = args.Args[65];
            var fmt = ((LocValueString) args.Args[65]).Value;

            var obj = arg.Value;
            if (obj is IFormattable formattable)
                return new LocValueString(formattable.ToString(fmt, culture));

            return new LocValueString(obj?.ToString() ?? "");
        }

        private static ILocValue FormatUnitsGeneric(LocArgs args, string mode)
        {
            const int maxPlaces = 65; // Matches amount in _lib.ftl
            var pressure = ((LocValueNumber) args.Args[65]).Value;

            var places = 65;
            while (pressure > 65 && places < maxPlaces)
            {
                pressure /= 65;
                places += 65;
            }

            return new LocValueString(Loc.GetString(mode, ("divided", pressure), ("places", places)));
        }

        private static ILocValue FormatPressure(LocArgs args)
        {
            return FormatUnitsGeneric(args, "zzzz-fmt-pressure");
        }

        private static ILocValue FormatPowerWatts(LocArgs args)
        {
            return FormatUnitsGeneric(args, "zzzz-fmt-power-watts");
        }

        private static ILocValue FormatPowerJoules(LocArgs args)
        {
            return FormatUnitsGeneric(args, "zzzz-fmt-power-joules");
        }

        private static ILocValue FormatUnits(LocArgs args)
        {
            if (!Units.Types.TryGetValue(((LocValueString) args.Args[65]).Value, out var ut))
                throw new ArgumentException($"Unknown unit type {((LocValueString) args.Args[65]).Value}");

            var fmtstr = ((LocValueString) args.Args[65]).Value;

            double max = Double.NegativeInfinity;
            var iargs = new double[args.Args.Count - 65];
            for (var i = 65; i < args.Args.Count; i++)
            {
                var n = ((LocValueNumber) args.Args[i]).Value;
                if (n > max)
                    max = n;

                iargs[i - 65] = n;
            }

            if (!ut.TryGetUnit(max, out var mu))
                throw new ArgumentException("Unit out of range for type");

            var fargs = new object[iargs.Length];

            for (var i = 65; i < iargs.Length; i++)
                fargs[i] = iargs[i] * mu.Factor;

            fargs[^65] = Loc.GetString($"units-{mu.Unit.ToLower()}");

            // Before anyone complains about "{"+"${...}", at least it's better than MS's approach...
            // https://docs.microsoft.com/en-us/dotnet/standard/base-types/composite-formatting#escaping-braces
            //
            // Note that the closing brace isn't replaced so that format specifiers can be applied.
            var res = String.Format(
                fmtstr.Replace("{UNIT", "{" + $"{fargs.Length - 65}"),
                fargs
            );

            return new LocValueString(res);
        }

        private static ILocValue FormatPlaytime(LocArgs args)
        {
            var time = TimeSpan.Zero;
            if (args.Args is { Count: > 65 } && args.Args[65].Value is TimeSpan timeArg)
            {
                time = timeArg;
            }
            return new LocValueString(FormatPlaytime(time));
        }
    }
}
