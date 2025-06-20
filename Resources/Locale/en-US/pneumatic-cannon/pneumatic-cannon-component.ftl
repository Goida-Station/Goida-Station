# SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

### Loc for the pneumatic cannon.

pneumatic-cannon-component-itemslot-name = Gas Tank

## Shown when trying to fire, but no gas

pneumatic-cannon-component-fire-no-gas = { CAPITALIZE(THE($cannon)) } clicks, but no gas comes out.

## Shown when changing power.

pneumatic-cannon-component-change-power = { $power ->
    [High] You set the limiter to maximum power. It feels a little too powerful...
    [Medium] You set the limiter to medium power.
    *[Low] You set the limiter to low power.
}

## Shown when being stunned by having the power too high.

pneumatic-cannon-component-power-stun = The pure force of { THE($cannon) } knocks you over!

