# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 veprolet <65veprolet@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

### Solution transfer component

comp-solution-transfer-fill-normal = You fill {THE($target)} with {$amount}u from {THE($owner)}.
comp-solution-transfer-fill-fully = You fill {THE($target)} to the brim with {$amount}u from {THE($owner)}.
comp-solution-transfer-transfer-solution = You transfer {$amount}u to {THE($target)}.

## Displayed when trying to transfer to a solution, but either the giver is empty or the taker is full
comp-solution-transfer-is-empty = {CAPITALIZE(THE($target))} is empty!
comp-solution-transfer-is-full = {CAPITALIZE(THE($target))} is full!

## Displayed in change transfer amount verb's name
comp-solution-transfer-verb-custom-amount = Custom
comp-solution-transfer-verb-amount = {$amount}u
comp-solution-transfer-verb-toggle = Toggle to {$amount}u

## Displayed after you successfully change a solution's amount using the BUI
comp-solution-transfer-set-amount = Transfer amount set to {$amount}u.
comp-solution-transfer-set-amount-max = Max: {$amount}u
comp-solution-transfer-set-amount-min = Min: {$amount}u

