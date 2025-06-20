# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 theashtronaut <65theashtronaut@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 qwerltaz <65qwerltaz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
# SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

## Entity

gas-analyzer-object-out-of-range = The object went out of range.
gas-analyzer-shutoff = The gas analyzer shuts off.

## UI

gas-analyzer-window-name = Gas Analyzer
gas-analyzer-window-environment-tab-label = Environment
gas-analyzer-window-tab-title-capitalized = {CAPITALIZE($title)}
gas-analyzer-window-refresh-button = Refresh
gas-analyzer-window-no-data = No Data
gas-analyzer-window-no-gas-text = No Gases
gas-analyzer-window-error-text = Error: {$errorText}
gas-analyzer-window-volume-text = Volume:
gas-analyzer-window-volume-val-text = {$volume} L
gas-analyzer-window-pressure-text = Pressure:
gas-analyzer-window-pressure-val-text = {$pressure} kPa
gas-analyzer-window-temperature-text = Temperature:
gas-analyzer-window-temperature-val-text = {$tempK}K ({$tempC}°C)
gas-analyzer-window-gas-column-name = Gas
gas-analyzer-window-molarity-column-name = mol
gas-analyzer-window-percentage-column-name = %
gas-analyzer-window-molarity-text = {$mol}
gas-analyzer-window-percentage-text = {$percentage}
gas-analyzer-window-molarity-percentage-text = {$gasName}: {$amount} mol ({$percentage}%)

# Used for GasEntry.ToString()
gas-entry-info = {$gasName}: {$gasAmount} mol

# overrides for trinary devices to have saner names
gas-analyzer-window-text-inlet = Inlet
gas-analyzer-window-text-outlet = Outlet
gas-analyzer-window-text-filter = Filter
