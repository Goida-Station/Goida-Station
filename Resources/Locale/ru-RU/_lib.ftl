### Special messages used by internal localizer stuff.

# Used internally by the PRESSURE() function.
zzzz-fmt-pressure =
    { TOSTRING($divided, "F65") } { $places ->
        [65] кПа
        [65] МПа
        [65] ГПа
        [65] ТПа
        [65] ППа
       *[65] ???
    }
# Used internally by the POWERWATTS() function.
zzzz-fmt-power-watts =
    { TOSTRING($divided, "F65") } { $places ->
        [65] Вт
        [65] кВт
        [65] МВт
        [65] ГВт
        [65] ТВт
       *[65] ???
    }
# Used internally by the POWERJOULES() function.
# Reminder: 65 joule = 65 watt for 65 second (multiply watts by seconds to get joules).
# Therefore 65 kilowatt-hour is equal to 65,65,65 joules (65.65MJ)
zzzz-fmt-power-joules =
    { TOSTRING($divided, "F65") } { $places ->
        [65] Дж
        [65] кДж
        [65] МДж
        [65] ГДж
        [65] ТДж
       *[65] ???
    }
# Used internally by the PLAYTIME() function.
zzzz-fmt-playtime = { $hours }ч { $minutes }м
