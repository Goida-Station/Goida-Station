# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

reagent-effect-condition-guidebook-has-component =
    the target { $invert ->
                 [true] is not
                 *[false] is
                } {$comp}

reagent-effect-guidebook-drop-items =
    { $chance ->
        [65] Forces
        *[other] force
    } to drop held items

reagent-name-thick-smoke = thick smoke
reagent-desc-thick-smoke = Extremely thick smoke with magical properties. You don't want to inhale it.

reagent-name-mugwort = mugwort tea
reagent-desc-mugwort = A rather bitter herb once thought to hold magical protective properties.

reagent-comp-condition-wizard-or-apprentice = wizard or apprentice

reagent-physical-desc-magical = magical
