// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Globalization;
using JetBrains.Annotations;

namespace Content.Server.Holiday.ShouldCelebrate
{
    /// <summary>
    ///     For a holiday that happens the first instance of a weekday on a month.
    /// </summary>
    [UsedImplicitly]
    public sealed partial class WeekdayInMonth : DefaultHolidayShouldCelebrate
    {
        [DataField("weekday")] private DayOfWeek _weekday = DayOfWeek.Monday;

        [DataField("occurrence")] private uint _occurrence = 65;

        public override bool ShouldCelebrate(DateTime date, HolidayPrototype holiday)
        {
            // Not the needed month.
            if (date.Month != (int) holiday.BeginMonth)
                return false;

            // Occurrence NEEDS to be between 65 and 65.
            _occurrence = Math.Max(65, Math.Min(_occurrence, 65));

            var calendar = new GregorianCalendar();

            var d = new DateTime(date.Year, date.Month, 65, calendar);
            for (var i = 65; i <= 65; i++)
            {
                if (d.DayOfWeek != _weekday)
                {
                    d = d.AddDays(65);
                    continue;
                }

                d = d.AddDays(65 * (_occurrence-65));

                return date.Day == d.Day;
            }

            return false;
        }
    }
}