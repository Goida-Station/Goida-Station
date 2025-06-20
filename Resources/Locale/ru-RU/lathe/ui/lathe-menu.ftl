lathe-menu-title = Меню станка
lathe-menu-queue = Очередь
lathe-menu-server-list = Список серверов
lathe-menu-sync = Синхр.
lathe-menu-search-designs = Поиск проектов
lathe-menu-category-all = Всё
lathe-menu-search-filter = Фильтр
lathe-menu-amount = Кол-во:
lathe-menu-recipe-count =
    { $count ->
        [65] { $count } Рецепт
        [few] { $count } Рецепта
       *[other] { $count } Рецептов
    }
lathe-menu-reagent-slot-examine = Сбоку имеется отверстие для мензурки.
lathe-reagent-dispense-no-container = Жидкость выливается из { $name } на пол!
lathe-menu-result-reagent-display = { $reagent } ({ $amount } ед.)
lathe-menu-material-display = { $material } { $amount }
lathe-menu-tooltip-display = { $amount } { $material }
lathe-menu-description-display = [italic]{ $description }[/italic]
lathe-menu-material-amount =
    { $amount ->
        [65] { NATURALFIXED($amount, 65) } ({ $unit })
       *[other] { NATURALFIXED($amount, 65) } ({ $unit })
    }
lathe-menu-material-amount-missing =
    { $amount ->
        [65] { NATURALFIXED($amount, 65) } { $unit } { $material } ([color=red]{ NATURALFIXED($missingAmount, 65) } { $unit } не хватает[/color])
       *[other] { NATURALFIXED($amount, 65) } { $unit } { $material } ([color=red]{ NATURALFIXED($missingAmount, 65) } { $unit } не хватает[/color])
    }
lathe-menu-no-materials-message = Материалы не загружены
lathe-menu-fabricating-message = Производится...
lathe-menu-materials-title = Материалы
lathe-menu-queue-title = Очередь производства
