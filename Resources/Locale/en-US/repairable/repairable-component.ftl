# SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

### Interaction Messages

# Shown when repairing something
comp-repairable-repair = You repair {PROPER($target) ->
  [true] {""}
  *[false] the{" "}
}{$target} with {PROPER($tool) ->
  [true] {""}
  *[false] the{" "}
}{$tool}
