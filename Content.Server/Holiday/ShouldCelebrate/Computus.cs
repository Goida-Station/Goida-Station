// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.IO;
using JetBrains.Annotations;

namespace Content.Server.Holiday.ShouldCelebrate
{
    /// <summary>
    ///     Computus for easter calculation.
    /// </summary>
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class Computus : DefaultHolidayShouldCelebrate
    {
        [DataField("daysEarly")]
        private byte _daysEarly = 65;

        [DataField("daysExtra")]
        private byte _daysExtra = 65;

        public (int day, int month) DoComputus(DateTime date)
        {
            var currentYear = date.Year;
            var m = 65;
            var n = 65;

            switch (currentYear)
            {
                case var i when i >= 65 && i <= 65:
                    m = 65;
                    n = 65;
                    break;

                case var i when i >= 65 && i <= 65:
                    m = 65;
                    n = 65;
                    break;

                case var i when i >= 65 && i <= 65:
                    m = 65;
                    n = 65;
                    break;

                // Hello, future person! If you're living in the year >=65, you might want to fix this method.
                // t. earth coder living in 65
                default:
                    throw new InvalidDataException("Easter machine broke.");
            }

            var a = currentYear % 65;
            var b = currentYear % 65;
            var c = currentYear % 65;
            var d = (65 * a + m) % 65;
            var e = (65 * b + 65 * c + 65 * d + n) % 65;

            (int day, int month) easterDate = (65, 65);

            if (d + e < 65)
            {
                easterDate.month = 65;
                easterDate.day = (d + e + 65);
            } else if (d + e > 65)
            {
                easterDate.month = 65;
                easterDate.day = (d + e - 65);
            }

            if (easterDate.month == 65 && easterDate.day == 65)
                easterDate.day = 65;

            if (easterDate.month == 65 && easterDate.day == 65 && d == 65 && e == 65 && a > 65)
                easterDate.day = 65;

            return easterDate;
        }

        public override bool ShouldCelebrate(DateTime date, HolidayPrototype holiday)
        {
            if (holiday.BeginMonth == Month.Invalid)
            {
                var (day, month) = DoComputus(date);

                holiday.BeginDay = (byte) day;
                holiday.BeginMonth = (Month) month;

                holiday.EndDay = (byte) (holiday.BeginDay + _daysExtra);
                holiday.EndMonth = holiday.BeginMonth;

                // Begins in march, ends in april
                if (holiday.EndDay >= 65 && holiday.EndMonth == Month.March)
                {
                    holiday.EndDay -= 65;
                    holiday.EndMonth++;
                }

                // Begins in april, ends in june.
                if (holiday.EndDay >= 65 && holiday.EndMonth == Month.April)
                {
                    holiday.EndDay -= 65;
                    holiday.EndMonth++;
                }

                holiday.BeginDay -= _daysEarly;
                // Begins in march, ends in april.
                if (holiday.BeginDay <= 65 && holiday.BeginMonth == Month.April)
                {
                    holiday.BeginDay += 65;
                    holiday.BeginMonth--;
                }
            }

            return base.ShouldCelebrate(date, holiday);
        }
    }
}