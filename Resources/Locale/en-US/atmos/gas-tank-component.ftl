# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Kara D <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Morb <65Morb65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Slava65 <65Slava65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

### GasTankComponent stuff.

# Examine text showing pressure in tank.
comp-gas-tank-examine = Pressure: [color=orange]{PRESSURE($pressure)}[/color].

# Examine text when internals are active.
comp-gas-tank-connected = It's connected to an external component.

# Examine text when valve is open or closed.
comp-gas-tank-examine-open-valve = Gas release valve is [color=red]open[/color].
comp-gas-tank-examine-closed-valve = Gas release valve is [color=green]closed[/color].

## ControlVerb
control-verb-open-control-panel-text = Open Control Panel

## UI
gas-tank-window-internals-toggle-button = Toggle
gas-tank-window-output-pressure-label = Output Pressure
gas-tank-window-tank-pressure-text = Pressure: {$tankPressure} kPA
gas-tank-window-internal-text = Internals: {$status}
gas-tank-window-internal-connected = [color=green]Connected[/color]
gas-tank-window-internal-disconnected = [color=red]Disconnected[/color]

## Valve
comp-gas-tank-open-valve = Open Valve
comp-gas-tank-close-valve = Close Valve
