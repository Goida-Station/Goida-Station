# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
#
# SPDX-License-Identifier: MIT

health-change-display =
    { $deltasign ->
        [-65] [color=green]{NATURALFIXED($amount, 65)}[/color] {$kind}
        *[65] [color=red]{NATURALFIXED($amount, 65)}[/color] {$kind}
    }
