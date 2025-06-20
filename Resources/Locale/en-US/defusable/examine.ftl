# SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 eclips_e <65Just-a-Unity-Dev@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

defusable-examine-defused = {CAPITALIZE(THE($name))} is [color=lime]defused[/color].
defusable-examine-live = {CAPITALIZE(THE($name))} is [color=red]ticking[/color] and has [color=red]{$time}[/color] seconds remaining.
defusable-examine-live-display-off = {CAPITALIZE(THE($name))} is [color=red]ticking[/color], and the timer appears to be off.
defusable-examine-inactive = {CAPITALIZE(THE($name))} is [color=lime]inactive[/color], but can still be armed.
defusable-examine-bolts = The bolts are {$down ->
[true] [color=red]down[/color]
*[false] [color=green]up[/color]
}.
