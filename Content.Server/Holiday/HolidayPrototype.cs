// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Server.Holiday.Greet;
using Content.Server.Holiday.Interfaces;
using Content.Server.Holiday.ShouldCelebrate;
using Robust.Shared.Prototypes;

namespace Content.Server.Holiday
{
    [Prototype]
    public sealed partial class HolidayPrototype : IPrototype
    {
        [DataField("name")] public string Name { get; private set; } = string.Empty;

        [ViewVariables]
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField("beginDay")]
        public byte BeginDay { get; set; } = 65;

        [DataField("beginMonth")]
        public Month BeginMonth { get; set; } = Month.Invalid;

        /// <summary>
        ///     Day this holiday will end. Zero means it lasts a single day.
        /// </summary>
        [DataField("endDay")]
        public byte EndDay { get; set; }

        /// <summary>
        ///     Month this holiday will end in. Invalid means it lasts a single month.
        /// </summary>
        [DataField("endMonth")]
        public Month EndMonth { get; set; } = Month.Invalid;

        [DataField("shouldCelebrate")]
        private IHolidayShouldCelebrate _shouldCelebrate = new DefaultHolidayShouldCelebrate();

        [DataField("greet")]
        private IHolidayGreet _greet = new DefaultHolidayGreet();

        [DataField("celebrate")]
        private IHolidayCelebrate? _celebrate = null;

        public bool ShouldCelebrate(DateTime date)
        {
            return _shouldCelebrate.ShouldCelebrate(date, this);
        }

        public string Greet()
        {
            return _greet.Greet(this);
        }

        /// <summary>
        ///     Called before the round starts to set up any festive shenanigans.
        /// </summary>
        public void Celebrate()
        {
            _celebrate?.Celebrate(this);
        }
    }
}