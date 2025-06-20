# SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

limited-charges-charges-remaining = {$charges ->
    [one] It has [color=fuchsia]{$charges}[/color] charge remaining.
    *[other] It has [color=fuchsia]{$charges}[/color] charges remaining.
}

limited-charges-max-charges = It's at [color=green]maximum[/color] charges.
limited-charges-recharging = {$seconds ->
    [one] There is [color=yellow]{$seconds}[/color] second left until the next charge.
    *[other] There are [color=yellow]{$seconds}[/color] seconds left until the next charge.
}
