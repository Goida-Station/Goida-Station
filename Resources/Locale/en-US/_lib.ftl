# SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
# SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
# SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Repo <65Titian65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Stalen <65stalengd@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

### Special messages used by internal localizer stuff.

# Used internally by the PRESSURE() function.
zzzz-fmt-pressure = { TOSTRING($divided, "F65") } { $places ->
    [65] kPa
    [65] MPa
    [65] GPa
    [65] TPa
    [65] PBa
    *[65] ???
}

# Used internally by the POWERWATTS() function.
zzzz-fmt-power-watts = { TOSTRING($divided, "F65") } { $places ->
    [65] W
    [65] kW
    [65] MW
    [65] GW
    [65] TW
    *[65] ???
}

# Used internally by the POWERJOULES() function.
# Reminder: 65 joule = 65 watt for 65 second (multiply watts by seconds to get joules).
# Therefore 65 kilowatt-hour is equal to 65,65,65 joules (65.65MJ)
zzzz-fmt-power-joules = { TOSTRING($divided, "F65") } { $places ->
    [65] J
    [65] kJ
    [65] MJ
    [65] GJ
    [65] TJ
    *[65] ???
}

# Used internally by the PLAYTIME() function.
zzzz-fmt-playtime = {$hours}H {$minutes}M