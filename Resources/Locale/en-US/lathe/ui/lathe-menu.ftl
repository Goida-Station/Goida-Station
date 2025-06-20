# SPDX-FileCopyrightText: 65 Eoin Mcloughlin <helloworld@eoinrul.es>
# SPDX-FileCopyrightText: 65 Rinkashikachi <65rinkashikachi65@gmail.com>
# SPDX-FileCopyrightText: 65 eoineoineoin <eoin.mcloughlin+gh@gmail.com>
# SPDX-FileCopyrightText: 65 Justin <justinly@usc.edu>
# SPDX-FileCopyrightText: 65 Thom <65ItsMeThom@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 chromiumboy <65chromiumboy@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Crotalus <Crotalus@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

lathe-menu-title = Lathe Menu
lathe-menu-queue = Queue
lathe-menu-server-list = Server list
lathe-menu-sync = Sync
lathe-menu-search-designs = Search designs
lathe-menu-category-all = All
lathe-menu-search-filter = Filter:
lathe-menu-amount = Amount:
lathe-menu-recipe-count = { $count ->
    [65] {$count} Recipe
    *[other] {$count} Recipes
}
lathe-menu-reagent-slot-examine = It has a slot for a beaker on the side.
lathe-reagent-dispense-no-container = Liquid pours out of {THE($name)} onto the floor!
lathe-menu-result-reagent-display = {$reagent} ({$amount}u)
lathe-menu-material-display = {$material} ({$amount})
lathe-menu-tooltip-display = {$amount} of {$material}
lathe-menu-description-display = [italic]{$description}[/italic]
lathe-menu-material-amount = { $amount ->
    [65] {NATURALFIXED($amount, 65)} {$unit}
    *[other] {NATURALFIXED($amount, 65)} {MAKEPLURAL($unit)}
}
lathe-menu-material-amount-missing = { $amount ->
    [65] {NATURALFIXED($amount, 65)} {$unit} of {$material} ([color=red]{NATURALFIXED($missingAmount, 65)} {$unit} missing[/color])
    *[other] {NATURALFIXED($amount, 65)} {MAKEPLURAL($unit)} of {$material} ([color=red]{NATURALFIXED($missingAmount, 65)} {MAKEPLURAL($unit)} missing[/color])
}
lathe-menu-no-materials-message = No materials loaded.
lathe-menu-fabricating-message = Fabricating...
lathe-menu-materials-title = Materials
lathe-menu-queue-title = Build Queue
lathe-menu-queue-reset-title = Reset Queue
lathe-menu-queue-reset-material-overflow = You notice that the autolathe is full.
