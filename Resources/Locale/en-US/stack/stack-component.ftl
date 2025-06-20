# SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
# SPDX-FileCopyrightText: 65 Remie Richards <remierichards@gmail.com>
# SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

### UI

# Shown when a stack is examined in details range
comp-stack-examine-detail-count = {$count ->
    [one] There is [color={$markupCountColor}]{$count}[/color] thing
    *[other] There are [color={$markupCountColor}]{$count}[/color] things
} in the stack.

# Stack status control
comp-stack-status = Count: [color=white]{$count}[/color]

### Interaction Messages

# Shown when attempting to add to a stack that is full
comp-stack-already-full = Stack is already full.

# Shown when a stack becomes full
comp-stack-becomes-full = Stack is now full.

# Text related to splitting a stack
comp-stack-split = You split the stack.
# Goobstation - Custom stack splitting dialog
comp-stack-split-custom = Split amount...
comp-stack-split-halve = Halve
comp-stack-split-too-small = Stack is too small to split.

# Goobstation - Custom stack splitting dialog
comp-stack-split-size = Max: {$size}

ui-custom-stack-split-title = Split Amount
ui-custom-stack-split-line-edit-placeholder = Amount
ui-custom-stack-split-apply = Split
