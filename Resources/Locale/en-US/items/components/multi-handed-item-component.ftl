# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

multi-handed-item-pick-up-fail = {$number -> 
    [one] You need one more free hand to pick up { THE($item) }.
    *[other] You need { $number } more free hands to pick up { THE($item) }.
}
