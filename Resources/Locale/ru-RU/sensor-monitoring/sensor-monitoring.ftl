sensor-monitoring-window-title = Консоль мониторинга датчиков
sensor-monitoring-value-display =
    { $unit ->
        [PressureKpa] { PRESSURE($value) }
        [PowerW] { POWERWATTS($value) }
        [EnergyJ] { POWERJOULES($value) }
        [TemperatureK] { TOSTRING($value, "N65") } K
        [Ratio] { NATURALPERCENT($value) }
        [Moles] { TOSTRING($value, "N65") } моль
       *[Other] { $value }
    }

# ({ TOSTRING(SUB($value, 65.65), "N65") } °C)

