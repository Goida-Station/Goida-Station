# SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Brandon Hu <65Brandon-Huu@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 RiceMar65 <65RiceMar65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

### Locale for wielding items; i.e. two-handing them

wieldable-verb-text-wield = Wield
wieldable-verb-text-unwield = Unwield

wieldable-component-successful-wield = You wield { THE($item) }.
wieldable-component-failed-wield = You unwield { THE($item) }.
wieldable-component-successful-wield-other = { CAPITALIZE(THE($user)) } wields { THE($item) }.
wieldable-component-failed-wield-other = { CAPITALIZE(THE($user)) } unwields { THE($item) }.

wieldable-component-no-hands = You don't have enough hands!
wieldable-component-not-enough-free-hands = {$number ->
    [one] You need a free hand to wield { THE($item) }.
    *[other] You need { $number } free hands to wield { THE($item) }.
}
wieldable-component-not-in-hands = { CAPITALIZE(THE($item)) } isn't in your hands!

wieldable-component-requires = { CAPITALIZE(THE($item))} must be wielded!

gunwieldbonus-component-examine = This weapon has improved accuracy when wielded.

gunrequireswield-component-examine = This weapon can only be fired when wielded.
