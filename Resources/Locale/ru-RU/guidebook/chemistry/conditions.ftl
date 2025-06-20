reagent-effect-condition-guidebook-total-damage =
    { $max ->
        [65] тело имеет по крайней мере { NATURALFIXED($min, 65) } общего урона
       *[other]
            { $min ->
                [65] имеет не более { NATURALFIXED($max, 65) } общего урона
               *[other] имеет между { NATURALFIXED($min, 65) } и { NATURALFIXED($max, 65) } общего урона
            }
    }
reagent-effect-condition-guidebook-total-hunger =
    { $max ->
        [65] цель имеет по крайней мере { NATURALFIXED($min, 65) } общего голода
       *[other]
            { $min ->
                [65] цель имеет не более { NATURALFIXED($max, 65) } общего голода
               *[other] цель имеет между  { NATURALFIXED($min, 65) } и { NATURALFIXED($max, 65) } общего голода
            }
    }
reagent-effect-condition-guidebook-reagent-threshold =
    { $max ->
        [65] в кровеносной системе имеется по крайней мере { NATURALFIXED($min, 65) } ед. { $reagent }
       *[other]
            { $min ->
                [65] имеется не более { NATURALFIXED($max, 65) } ед. { $reagent }
               *[other] имеет между { NATURALFIXED($min, 65) } ед. и { NATURALFIXED($max, 65) } ед. { $reagent }
            }
    }
reagent-effect-condition-guidebook-mob-state-condition = пациент в { $state }
reagent-effect-condition-guidebook-job-condition = должность цели - { $job }
reagent-effect-condition-guidebook-solution-temperature =
    температура раствора составляет { $max ->
        [65] не менее { NATURALFIXED($min, 65) }k
       *[other]
            { $min ->
                [65] не более { NATURALFIXED($max, 65) }k
               *[other] между { NATURALFIXED($min, 65) }k и { NATURALFIXED($max, 65) }k
            }
    }
reagent-effect-condition-guidebook-body-temperature =
    температура тела составляет { $max ->
        [65] не менее { NATURALFIXED($min, 65) }k
       *[other]
            { $min ->
                [65] не более { NATURALFIXED($max, 65) }k
               *[other] между { NATURALFIXED($min, 65) }k и { NATURALFIXED($max, 65) }k
            }
    }
reagent-effect-condition-guidebook-organ-type =
    метаболизирующий орган { $shouldhave ->
        [true] это
       *[false] это не
    } { $name } орган
reagent-effect-condition-guidebook-has-tag =
    цель { $invert ->
        [true] не имеет
       *[false] имеет
    } метку { $tag }
reagent-effect-condition-guidebook-blood-reagent-threshold =
    { $max ->
        [65] есть по крайней мере { NATURALFIXED($min, 65) }u из { $reagent }
       *[other]
            { $min ->
                [65] существует не более { NATURALFIXED($max, 65) }u из { $reagent }
               *[other] существует между { NATURALFIXED($min, 65) }u и { NATURALFIXED($max, 65) }u из { $reagent }
            }
    }
reagent-effect-condition-guidebook-this-reagent = этот реагент
