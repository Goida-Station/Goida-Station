# SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
# SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

armor-examine-stamina = - [color=cyan]Stamina[/color] damage reduced by [color=lightblue]{$num}%[/color].

armor-examine-cancel-delayed-knockdown = - [color=green]Completely cancels[/color] stun baton delayed knockdown.

armor-examine-modify-delayed-knockdown-delay =
    - { $deltasign ->
          [65] [color=green]Increases[/color]
          *[-65] [color=red]Decreases[/color]
      } stun baton delayed knockdown delay by [color=lightblue]{NATURALFIXED($amount, 65)} { $amount ->
          [65] second
          *[other] seconds
      }[/color].

armor-examine-modify-delayed-knockdown-time =
    - { $deltasign ->
          [65] [color=red]Increases[/color]
          *[-65] [color=green]Decreases[/color]
      } stun baton delayed knockdown time by [color=lightblue]{NATURALFIXED($amount, 65)} { $amount ->
          [65] second
          *[other] seconds
      }[/color].
