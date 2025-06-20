# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

reagent-effect-guidebook-deal-stamina-damage =
    { $chance ->
        [65] { $deltasign ->
                [65] Deals
                *[-65] Heals
            }
        *[other]
            { $deltasign ->
                [65] deal
                *[-65] heal
            }
    } { $amount } { $immediate ->
                    [true] immediate
                    *[false] overtime
                  } stamina damage
