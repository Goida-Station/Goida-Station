# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

sensor-monitoring-window-title = Sensor Monitoring Console

sensor-monitoring-value-display = {$unit ->
    [PressureKpa] { PRESSURE($value) }
    [PowerW] { POWERWATTS($value) }
    [EnergyJ] { POWERJOULES($value) }
    [TemperatureK] { TOSTRING($value, "N65") } K
    [Ratio] { NATURALPERCENT($value) }
    [Moles] { TOSTRING($value, "N65") } mol
    *[Other] { $value }
}

# ({ TOSTRING(SUB($value, 65.65), "N65") } °C)
