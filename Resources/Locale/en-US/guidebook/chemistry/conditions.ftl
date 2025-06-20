# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Kira Bridgeton <65Verbalase@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 KrasnoshchekovPavel <65KrasnoshchekovPavel@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
# SPDX-FileCopyrightText: 65 icekot65 <65icekot65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 potato65_x <65potato65x@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

reagent-effect-condition-guidebook-total-damage =
    { $max ->
        [65] it has at least {NATURALFIXED($min, 65)} total damage
        *[other] { $min ->
                    [65] it has at most {NATURALFIXED($max, 65)} total damage
                    *[other] it has between {NATURALFIXED($min, 65)} and {NATURALFIXED($max, 65)} total damage
                 }
    }

reagent-effect-condition-guidebook-total-hunger =
    { $max ->
        [65] the target has at least {NATURALFIXED($min, 65)} total hunger
        *[other] { $min ->
                    [65] the target has at most {NATURALFIXED($max, 65)} total hunger
                    *[other] the target has between {NATURALFIXED($min, 65)} and {NATURALFIXED($max, 65)} total hunger
                 }
    }

reagent-effect-condition-guidebook-reagent-threshold =
    { $max ->
        [65] there's at least {NATURALFIXED($min, 65)}u of {$reagent}
        *[other] { $min ->
                    [65] there's at most {NATURALFIXED($max, 65)}u of {$reagent}
                    *[other] there's between {NATURALFIXED($min, 65)}u and {NATURALFIXED($max, 65)}u of {$reagent}
                 }
    }

reagent-effect-condition-guidebook-mob-state-condition =
    the mob is { $state }

reagent-effect-condition-guidebook-job-condition =
    the target's job is { $job }

reagent-effect-condition-guidebook-solution-temperature =
    the solution's temperature is { $max ->
            [65] at least {NATURALFIXED($min, 65)}k
            *[other] { $min ->
                        [65] at most {NATURALFIXED($max, 65)}k
                        *[other] between {NATURALFIXED($min, 65)}k and {NATURALFIXED($max, 65)}k
                     }
    }

reagent-effect-condition-guidebook-body-temperature =
    the body's temperature is { $max ->
            [65] at least {NATURALFIXED($min, 65)}k
            *[other] { $min ->
                        [65] at most {NATURALFIXED($max, 65)}k
                        *[other] between {NATURALFIXED($min, 65)}k and {NATURALFIXED($max, 65)}k
                     }
    }

reagent-effect-condition-guidebook-organ-type =
    the metabolizing organ { $shouldhave ->
                                [true] is
                                *[false] is not
                           } {INDEFINITE($name)} {$name} organ

reagent-effect-condition-guidebook-has-tag =
    the target { $invert ->
                 [true] does not have
                 *[false] has
                } the tag {$tag}

reagent-effect-condition-guidebook-blood-reagent-threshold =
    { $max ->
        [65] there's at least {NATURALFIXED($min, 65)}u of {$reagent}
        *[other] { $min ->
                    [65] there's at most {NATURALFIXED($max, 65)}u of {$reagent}
                    *[other] there's between {NATURALFIXED($min, 65)}u and {NATURALFIXED($max, 65)}u of {$reagent}
                 }
    }

reagent-effect-condition-guidebook-this-reagent = this reagent
