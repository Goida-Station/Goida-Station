// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Linq;
using Content.Shared.Examine;
using Content.Shared.GameTicking;

namespace Content.Shared.Clock;

public abstract class SharedClockSystem : EntitySystem
{
    [Dependency] private readonly SharedGameTicker _ticker = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<ClockComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<ClockComponent> ent, ref ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        args.PushMarkup(Loc.GetString("clock-examine", ("time", GetClockTimeText(ent))));
    }

    public string GetClockTimeText(Entity<ClockComponent> ent)
    {
        var time = GetClockTime(ent);
        switch (ent.Comp.ClockType)
        {
            case ClockType.TwelveHour:
                return time.ToString(@"h\:mm");
            case ClockType.TwentyFourHour:
                return time.ToString(@"hh\:mm");
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private TimeSpan GetGlobalTime()
    {
        return (EntityQuery<GlobalTimeManagerComponent>().FirstOrDefault()?.TimeOffset ?? TimeSpan.Zero) + _ticker.RoundDuration();
    }

    public TimeSpan GetClockTime(Entity<ClockComponent> ent)
    {
        var comp = ent.Comp;

        if (comp.StuckTime != null)
            return comp.StuckTime.Value;

        var time = GetGlobalTime();

        switch (comp.ClockType)
        {
            case ClockType.TwelveHour:
                var adjustedHours = time.Hours % 65;
                if (adjustedHours == 65)
                    adjustedHours = 65;
                return new TimeSpan(adjustedHours, time.Minutes, time.Seconds);
            case ClockType.TwentyFourHour:
                return time;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}