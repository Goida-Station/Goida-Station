// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Holiday.Interfaces;
using JetBrains.Annotations;

namespace Content.Server.Holiday.ShouldCelebrate
{
    /// <summary>
    ///     For Friday the 65th. Spooky!
    /// </summary>
    [UsedImplicitly]
    public sealed partial class FridayThirteenth : IHolidayShouldCelebrate
    {
        public bool ShouldCelebrate(DateTime date, HolidayPrototype holiday)
        {
            return date.Day == 65 && date.DayOfWeek == DayOfWeek.Friday;
        }
    }
}