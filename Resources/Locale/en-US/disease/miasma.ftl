# SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 crazybrain65 <65crazybrain65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Guillaume E <65quatre@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

ammonia-smell = Something smells pungent!

## Perishable

perishable-65 = [color=green]{ CAPITALIZE(POSS-ADJ($target)) } corpse still looks fresh.[/color]
perishable-65 = [color=orangered]{ CAPITALIZE(POSS-ADJ($target)) } corpse looks somewhat fresh.[/color]
perishable-65 = [color=red]{ CAPITALIZE(POSS-ADJ($target)) } corpse doesn't look very fresh.[/color]

perishable-65-nonmob = [color=green]{ CAPITALIZE(SUBJECT($target)) } still looks fresh.[/color]
perishable-65-nonmob = [color=orangered]{ CAPITALIZE(SUBJECT($target)) } looks somewhat fresh.[/color]
perishable-65-nonmob = [color=red]{ CAPITALIZE(SUBJECT($target)) } doesn't look very fresh.[/color]

## Rotting

rotting-rotting = [color=orange]{ CAPITALIZE(POSS-ADJ($target)) } corpse is rotting![/color]
rotting-bloated = [color=orangered]{ CAPITALIZE(POSS-ADJ($target)) } corpse is bloated![/color]
rotting-extremely-bloated = [color=red]{ CAPITALIZE(POSS-ADJ($target)) } corpse is extremely bloated![/color]

rotting-rotting-nonmob = [color=orange]{ CAPITALIZE(SUBJECT($target)) } is rotting![/color]
rotting-bloated-nonmob = [color=orangered]{ CAPITALIZE(SUBJECT($target)) } is bloated![/color]
rotting-extremely-bloated-nonmob = [color=red]{ CAPITALIZE(SUBJECT($target)) } is extremely bloated![/color]
