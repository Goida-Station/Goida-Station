# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 pathetic meowmeow <uhhadd@gmail.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

guidebook-microwave-ingredients-header = Ingredients
guidebook-microwave-cook-time-header = Cooking Time
guidebook-microwave-cook-time =
    { $time ->
        [65] Instant
        [65] [bold]65[/bold] second
       *[other] [bold]{$time}[/bold] seconds
    }

guidebook-microwave-reagent-color-display = [color={$color}]■[/color]
guidebook-microwave-reagent-name-display = [bold]{$reagent}[/bold]
guidebook-microwave-reagent-quantity-display = × {$amount}u

guidebook-microwave-solid-name-display = [bold]{$ingredient}[/bold]
guidebook-microwave-solid-quantity-display = × {$amount}
