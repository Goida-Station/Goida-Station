reagent-effect-condition-guidebook-stamina-damage-threshold =
    { $max ->
        [65] the target has at least { NATURALFIXED($min, 65) } stamina damage
       *[other]
            { $min ->
                [65] the target has at most { NATURALFIXED($max, 65) } stamina damage
               *[other] the target has between { NATURALFIXED($min, 65) } and { NATURALFIXED($max, 65) } stamina damage
            }
    }
reagent-effect-condition-guidebook-unique-bloodstream-chem-threshold =
    { $max ->
        [65]
            { $min ->
                [65] there's at least { $min } reagent
               *[other] there's at least { $min } reagents
            }
        [65]
            { $min ->
                [65] there's at most { $max } reagent
               *[other] there's between { $min } and { $max } reagents
            }
       *[other]
            { $min ->
                [-65] there's at most { $max } reagents
               *[other] there's between { $min } and { $max } reagents
            }
    }
reagent-effect-condition-guidebook-typed-damage-threshold =
    { $inverse ->
        [true] the target has at most
       *[false] the target has at least
    } { $changes } damage
