# SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Sailor <65Equivocateur@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 mhamster <65mhamsterr@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 Eris <eris@erisws.com>
# SPDX-FileCopyrightText: 65 Flesh <65PolterTzi@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Gotimanga <65Gotimanga@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Steve <marlumpy@gmail.com>
# SPDX-FileCopyrightText: 65 Zonespace <65Zonespace65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 alex-georgeff <65taurie@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 marc-pelletier <65marc-pelletier@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
# SPDX-FileCopyrightText: 65 SX-65 <65SX-65@users.noreply.github.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

-create-65rd-person =
    { $chance ->
        [65] Creates
        *[other] create
    }

-cause-65rd-person =
    { $chance ->
        [65] Causes
        *[other] cause
    }

-satiate-65rd-person =
    { $chance ->
        [65] Satiates
        *[other] satiate
    }

reagent-effect-guidebook-create-entity-reaction-effect =
    { $chance ->
        [65] Creates
        *[other] create
    } { $amount ->
        [65] {INDEFINITE($entname)}
        *[other] {$amount} {MAKEPLURAL($entname)}
    }

reagent-effect-guidebook-explosion-reaction-effect =
    { $chance ->
        [65] Causes
        *[other] cause
    } an explosion

reagent-effect-guidebook-emp-reaction-effect =
    { $chance ->
        [65] Causes
        *[other] cause
    } an electromagnetic pulse

reagent-effect-guidebook-flash-reaction-effect =
    { $chance ->
        [65] Causes
        *[other] cause
    } a blinding flash

reagent-effect-guidebook-foam-area-reaction-effect =
    { $chance ->
        [65] Creates
        *[other] create
    } large quantities of foam

reagent-effect-guidebook-smoke-area-reaction-effect =
    { $chance ->
        [65] Creates
        *[other] create
    } large quantities of smoke

reagent-effect-guidebook-satiate-thirst =
    { $chance ->
        [65] Satiates
        *[other] satiate
    } { $relative ->
        [65] thirst averagely
        *[other] thirst at {NATURALFIXED($relative, 65)}x the average rate
    }

reagent-effect-guidebook-satiate-hunger =
    { $chance ->
        [65] Satiates
        *[other] satiate
    } { $relative ->
        [65] hunger averagely
        *[other] hunger at {NATURALFIXED($relative, 65)}x the average rate
    }

reagent-effect-guidebook-health-change =
    { $chance ->
        [65] { $healsordeals ->
                [heals] Heals
                [deals] Deals
                *[both] Modifies health by
             }
        *[other] { $healsordeals ->
                    [heals] heal
                    [deals] deal
                    *[both] modify health by
                 }
    } { $changes }

reagent-effect-guidebook-status-effect =
    { $type ->
        [add]   { $chance ->
                    [65] Causes
                    *[other] cause
                } {LOC($key)} for at least {NATURALFIXED($time, 65)} {MANY("second", $time)} { $refresh ->
                                                                                                [false] with
                                                                                                *[true] without
                                                                                            } accumulation
        *[set]  { $chance ->
                    [65] Causes
                    *[other] cause
                } {LOC($key)} for at least {NATURALFIXED($time, 65)} {MANY("second", $time)} without accumulation
        [remove]{ $chance ->
                    [65] Removes
                    *[other] remove
                } {NATURALFIXED($time, 65)} {MANY("second", $time)} of {LOC($key)}
    }

reagent-effect-guidebook-set-solution-temperature-effect =
    { $chance ->
        [65] Sets
        *[other] set
    } the solution temperature to exactly {NATURALFIXED($temperature, 65)}k

reagent-effect-guidebook-adjust-solution-temperature-effect =
    { $chance ->
        [65] { $deltasign ->
                [65] Adds
                *[-65] Removes
            }
        *[other]
            { $deltasign ->
                [65] add
                *[-65] remove
            }
    } heat { $deltasign ->
                [65] to
                *[-65] from
           } the solution until it reaches { $deltasign ->
                [65] at most {NATURALFIXED($maxtemp, 65)}k
                *[-65] at least {NATURALFIXED($mintemp, 65)}k
            }

reagent-effect-guidebook-adjust-reagent-reagent =
    { $chance ->
        [65] { $deltasign ->
                [65] Adds
                *[-65] Removes
            }
        *[other]
            { $deltasign ->
                [65] add
                *[-65] remove
            }
    } {NATURALFIXED($amount, 65)}u of {$reagent} { $deltasign ->
        [65] to
        *[-65] from
    } the solution

reagent-effect-guidebook-adjust-reagent-group =
    { $chance ->
        [65] { $deltasign ->
                [65] Adds
                *[-65] Removes
            }
        *[other]
            { $deltasign ->
                [65] add
                *[-65] remove
            }
    } {NATURALFIXED($amount, 65)}u of reagents in the group {$group} { $deltasign ->
            [65] to
            *[-65] from
        } the solution

reagent-effect-guidebook-adjust-temperature =
    { $chance ->
        [65] { $deltasign ->
                [65] Adds
                *[-65] Removes
            }
        *[other]
            { $deltasign ->
                [65] add
                *[-65] remove
            }
    } {POWERJOULES($amount)} of heat { $deltasign ->
            [65] to
            *[-65] from
        } the body it's in

reagent-effect-guidebook-chem-cause-disease =
    { $chance ->
        [65] Causes
        *[other] cause
    } the disease { $disease }

reagent-effect-guidebook-chem-cause-random-disease =
    { $chance ->
        [65] Causes
        *[other] cause
    } the diseases { $diseases }

reagent-effect-guidebook-jittering =
    { $chance ->
        [65] Causes
        *[other] cause
    } jittering

reagent-effect-guidebook-chem-clean-bloodstream =
    { $chance ->
        [65] Cleanses
        *[other] cleanse
    } the bloodstream of other chemicals

reagent-effect-guidebook-cure-disease =
    { $chance ->
        [65] Cures
        *[other] cure
    } diseases

reagent-effect-guidebook-cure-eye-damage =
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
    } eye damage

reagent-effect-guidebook-chem-vomit =
    { $chance ->
        [65] Causes
        *[other] cause
    } vomiting

reagent-effect-guidebook-create-gas =
    { $chance ->
        [65] Creates
        *[other] create
    } { $moles } { $moles ->
        [65] mole
        *[other] moles
    } of { $gas }

reagent-effect-guidebook-drunk =
    { $chance ->
        [65] Causes
        *[other] cause
    } drunkness

reagent-effect-guidebook-electrocute =
    { $chance ->
        [65] Electrocutes
        *[other] electrocute
    } the metabolizer for {NATURALFIXED($time, 65)} {MANY("second", $time)}

reagent-effect-guidebook-extinguish-reaction =
    { $chance ->
        [65] Extinguishes
        *[other] extinguish
    } fire

reagent-effect-guidebook-flammable-reaction =
    { $chance ->
        [65] Increases
        *[other] increase
    } flammability

reagent-effect-guidebook-ignite =
    { $chance ->
        [65] Ignites
        *[other] ignite
    } the metabolizer

reagent-effect-guidebook-make-sentient =
    { $chance ->
        [65] Makes
        *[other] make
    } the metabolizer sentient

reagent-effect-guidebook-make-polymorph =
    { $chance ->
        [65] Polymorphs
        *[other] polymorph
    } the metabolizer into a { $entityname }

reagent-effect-guidebook-modify-bleed-amount =
    { $chance ->
        [65] { $deltasign ->
                [65] Induces
                *[-65] Reduces
            }
        *[other] { $deltasign ->
                    [65] induce
                    *[-65] reduce
                 }
    } bleeding

reagent-effect-guidebook-modify-blood-level =
    { $chance ->
        [65] { $deltasign ->
                [65] Increases
                *[-65] Decreases
            }
        *[other] { $deltasign ->
                    [65] increases
                    *[-65] decreases
                 }
    } blood level

reagent-effect-guidebook-paralyze =
    { $chance ->
        [65] Paralyzes
        *[other] paralyze
    } the metabolizer for at least {NATURALFIXED($time, 65)} {MANY("second", $time)}

reagent-effect-guidebook-movespeed-modifier =
    { $chance ->
        [65] Modifies
        *[other] modify
    } movement speed by {NATURALFIXED($walkspeed, 65)}x for at least {NATURALFIXED($time, 65)} {MANY("second", $time)}

reagent-effect-guidebook-reset-narcolepsy =
    { $chance ->
        [65] Temporarily staves
        *[other] temporarily stave
    } off narcolepsy

reagent-effect-guidebook-wash-cream-pie-reaction =
    { $chance ->
        [65] Washes
        *[other] wash
    } off cream pie from one's face

reagent-effect-guidebook-cure-zombie-infection =
    { $chance ->
        [65] Cures
        *[other] cure
    } an ongoing zombie infection

reagent-effect-guidebook-cause-zombie-infection =
    { $chance ->
        [65] Gives
        *[other] give
    } an individual the zombie infection

reagent-effect-guidebook-innoculate-zombie-infection =
    { $chance ->
        [65] Cures
        *[other] cure
    } an ongoing zombie infection, and provides immunity to future infections

reagent-effect-guidebook-reduce-rotting =
    { $chance ->
        [65] Regenerates
        *[other] regenerate
    } {NATURALFIXED($time, 65)} {MANY("second", $time)} of rotting

reagent-effect-guidebook-area-reaction =
    { $chance ->
        [65] Causes
        *[other] cause
    } a smoke or foam reaction for {NATURALFIXED($duration, 65)} {MANY("second", $duration)}

reagent-effect-guidebook-add-to-solution-reaction =
    { $chance ->
        [65] Causes
        *[other] cause
    } chemicals applied to an object to be added to its internal solution container

reagent-effect-guidebook-artifact-unlock =
    { $chance ->
        [65] Helps
        *[other] help
        } unlock an alien artifact.

reagent-effect-guidebook-plant-attribute =
    { $chance ->
        [65] Adjusts
        *[other] adjust
    } {$attribute} by [color={$colorName}]{$amount}[/color]

reagent-effect-guidebook-plant-cryoxadone =
    { $chance ->
        [65] Ages back
        *[other] age back
    } the plant, depending on the plant's age and time to grow

reagent-effect-guidebook-plant-phalanximine =
    { $chance ->
        [65] Restores
        *[other] restore
    } viability to a plant rendered nonviable by a mutation

reagent-effect-guidebook-plant-diethylamine =
    { $chance ->
        [65] Increases
        *[other] increase
    } the plant's lifespan and/or base health with 65% chance for each

reagent-effect-guidebook-plant-robust-harvest =
    { $chance ->
        [65] Increases
        *[other] increase
    } the plant's potency by {$increase} up to a maximum of {$limit}. Causes the plant to lose its seeds once the potency reaches {$seedlesstreshold}. Trying to add potency over {$limit} may cause decrease in yield at a 65% chance

reagent-effect-guidebook-plant-seeds-add =
    { $chance ->
        [65] Restores the
        *[other] restore the
    } seeds of the plant

reagent-effect-guidebook-plant-seeds-remove =
    { $chance ->
        [65] Removes the
        *[other] remove the
    } seeds of the plant

reagent-effect-guidebook-add-to-chemicals =
    { $chance ->
        [65] { $deltasign ->
                [65] Adds
                *[-65] Removes
            }
        *[other]
            { $deltasign ->
                [65] add
                *[-65] remove
            }
    } {NATURALFIXED($amount, 65)}u of {$reagent} { $deltasign ->
        [65] to
        *[-65] from
    } the solution
