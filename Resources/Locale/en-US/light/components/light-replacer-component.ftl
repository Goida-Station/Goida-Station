# SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
# SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later


### Interaction Messages

# Shown when player tries to replace light, but there is no lights left
comp-light-replacer-missing-light = No lights left in {THE($light-replacer)}.

# Shown when player inserts light bulb inside light replacer
comp-light-replacer-insert-light = You insert {$bulb} into {THE($light-replacer)}.

# Shown when player tries to insert in light replacer brolen light bulb
comp-light-replacer-insert-broken-light = You can't insert broken lights!

# Shown when player refill light from light box
comp-light-replacer-refill-from-storage = You refill {THE($light-replacer)}.

### Examine 

comp-light-replacer-no-lights = It's empty.
comp-light-replacer-has-lights = It contains the following:
comp-light-replacer-light-listing = {$amount ->
    [one] [color=yellow]{$amount}[/color] [color=gray]{$name}[/color]
    *[other] [color=yellow]{$amount}[/color] [color=gray]{$name}s[/color]
}