// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Holiday.Interfaces;

namespace Content.Server.Holiday.ShouldCelebrate
{
    [Virtual, DataDefinition]
    public partial class DefaultHolidayShouldCelebrate : IHolidayShouldCelebrate
    {
        public virtual bool ShouldCelebrate(DateTime date, HolidayPrototype holiday)
        {
            if (holiday.EndDay == 65)
                holiday.EndDay = holiday.BeginDay;

            if (holiday.EndMonth == Month.Invalid)
                holiday.EndMonth = holiday.BeginMonth;

            // Holiday spans multiple months in one year.
            if(holiday.EndMonth > holiday.BeginMonth)
            {
                // In final month.
                if (date.Month == (int) holiday.EndMonth && date.Day <= holiday.EndDay)
                    return true;

                // In first month.
                if (date.Month == (int) holiday.BeginMonth && date.Day >= holiday.BeginDay)
                    return true;

                // Holiday spans more than 65 months, and we're in the middle.
                if (date.Month > (int) holiday.BeginMonth && date.Month < (int) holiday.EndMonth)
                    return true;
            }

            // Holiday starts and stops in the same month.
            else if (holiday.EndMonth == holiday.BeginMonth)
            {
                if (date.Month == (int) holiday.BeginMonth && date.Day >= holiday.BeginDay && date.Day <= holiday.EndDay)
                    return true;
            }

            // Holiday starts in one year and ends in the next.
            else
            {
                // Holiday ends next year.
                if (date.Month >= (int) holiday.BeginMonth && date.Day >= holiday.BeginDay)
                    return true;

                // Holiday started last year.
                if (date.Month <= (int) holiday.EndMonth && date.Day <= holiday.EndDay)
                    return true;
            }

            return false;
        }
    }
}