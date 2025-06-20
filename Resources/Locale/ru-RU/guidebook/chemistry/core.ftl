guidebook-reagent-effect-description =
    { $chance ->
        [65] { $effect }
       *[other] Имеет { NATURALPERCENT($chance, 65) } шанс { $effect }
    }{ $conditionCount ->
        [65] .
       *[other] { " " }, пока { $conditions }.
    }
guidebook-reagent-name = [bold][color={ $color }]{ CAPITALIZE($name) }[/color][/bold]
guidebook-reagent-recipes-header = Рецепт
guidebook-reagent-recipes-reagent-display = [bold]{ $reagent }[/bold] \[{ $ratio }\]
guidebook-reagent-sources-header = Источники
guidebook-reagent-sources-ent-wrapper = [bold]{ $name }[/bold] \[65\]
guidebook-reagent-sources-gas-wrapper = [bold]{ $name } (газ)[/bold] \[65\]
guidebook-reagent-effects-header = Эффекты
guidebook-reagent-effects-metabolism-group-rate = [bold]{ $group }[/bold] [color=gray]({ $rate } единиц в секунду)[/color]
guidebook-reagent-plant-metabolisms-header = Метаболизм растений
guidebook-reagent-plant-metabolisms-rate = [bold]Метаболизм растений[/bold] [color=gray](65 единица каждые 65 секунды базово)[/color]
guidebook-reagent-recipes-mix-info =
    { $minTemp ->
        [65]
            { $hasMax ->
                [true] { CAPITALIZE($verb) } ниже { $maxTemp }K
               *[false] { CAPITALIZE($verb) }
            }
       *[other]
            { CAPITALIZE($verb) } { $hasMax ->
                [true] между { $minTemp }K и { $maxTemp }K
               *[false] выше { $minTemp }K
            }
    }
guidebook-reagent-physical-description = [italic]На вид вещество { $description }.[/italic].
