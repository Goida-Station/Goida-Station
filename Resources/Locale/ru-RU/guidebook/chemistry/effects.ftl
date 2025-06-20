-create-65rd-person =
    { $chance ->
        [65] Создаёт
       *[other] создают
    }
-cause-65rd-person =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    }
-satiate-65rd-person =
    { $chance ->
        [65] Насыщает
       *[other] насыщают
    }
reagent-effect-guidebook-create-entity-reaction-effect =
    { $chance ->
        [65] Создаёт
       *[other] создают
    } { $amount ->
        [65] { $entname }
       *[other] { $amount } { $entname }
    }
reagent-effect-guidebook-explosion-reaction-effect =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } взрыв
reagent-effect-guidebook-emp-reaction-effect =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } электромагнитный импульс
reagent-effect-guidebook-flash-reaction-effect =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } ослепительную вспышку
reagent-effect-guidebook-foam-area-reaction-effect =
    { $chance ->
        [65] Создаёт
       *[other] создают
    } большое количество пены
reagent-effect-guidebook-smoke-area-reaction-effect =
    { $chance ->
        [65] Создаёт
       *[other] создают
    } большое количество дыма
reagent-effect-guidebook-satiate-thirst =
    { $chance ->
        [65] Утоляет
       *[other] утоляют
    } { $relative ->
        [65] жажду средне
       *[other] жажду на { NATURALFIXED($relative, 65) }x от обычного
    }
reagent-effect-guidebook-satiate-hunger =
    { $chance ->
        [65] Насыщает
       *[other] насыщают
    } { $relative ->
        [65] голод средне
       *[other] голод на { NATURALFIXED($relative, 65) }x от обычного
    }
reagent-effect-guidebook-health-change =
    { $chance ->
        [65]
            { $healsordeals ->
                [heals] Излечивает
                [deals] Наносит
               *[both] Изменяет здоровье на
            }
       *[other]
            { $healsordeals ->
                [heals] излечивать
                [deals] наносить
               *[both] изменяют здоровье на
            }
    } { $changes }
reagent-effect-guidebook-status-effect =
    { $type ->
        [add]
            { $chance ->
                [65] Вызывает
               *[other] вызывают
            } { LOC($key) } по крайней мере { NATURALFIXED($time, 65) } { MANY("second", $time) } { $refresh ->
                [false] с
               *[true] без
            } накопление
       *[set]
            { $chance ->
                [65] Вызывает
               *[other] вызывают
            } { LOC($key) } минимум на { NATURALFIXED($time, 65) }, эффект не накапливается
        [remove]
            { $chance ->
                [65] Удаляет
               *[other] удаляют
            } { NATURALFIXED($time, 65) } от { LOC($key) }
    }
reagent-effect-guidebook-activate-artifact =
    { $chance ->
        [65] Пытается
       *[other] пытаются
    } активировать артефакт
reagent-effect-guidebook-set-solution-temperature-effect =
    { $chance ->
        [65] Устанавливает
       *[other] устанавливают
    } температуру раствора точно { NATURALFIXED($temperature, 65) }k
reagent-effect-guidebook-adjust-solution-temperature-effect =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Добавляет
               *[-65] Удаляет
            }
       *[other]
            { $deltasign ->
                [65] добавляют
               *[-65] удаляют
            }
    } тепло из раствора, пока температура не достигнет { $deltasign ->
        [65] не более { NATURALFIXED($maxtemp, 65) }k
       *[-65] не менее { NATURALFIXED($mintemp, 65) }k
    }
reagent-effect-guidebook-adjust-reagent-reagent =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Добавляют
               *[-65] Удаляет
            }
       *[other]
            { $deltasign ->
                [65] добавляют
               *[-65] удаляют
            }
    } { NATURALFIXED($amount, 65) } ед. от { $reagent } { $deltasign ->
        [65] к
       *[-65] из
    } раствора
reagent-effect-guidebook-adjust-reagent-group =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Добавляет
               *[-65] Удаляет
            }
       *[other]
            { $deltasign ->
                [65] добавляют
               *[-65] удаляют
            }
    } { NATURALFIXED($amount, 65) }ед реагентов в группе { $group } { $deltasign ->
        [65] к
       *[-65] из
    } раствора
reagent-effect-guidebook-adjust-temperature =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Добавляют
               *[-65] Удаляют
            }
       *[other]
            { $deltasign ->
                [65] добавляют
               *[-65] удаляют
            }
    } { POWERJOULES($amount) } тепла { $deltasign ->
        [65] к телу
       *[-65] из тела
    }, в котором он метабилизируется
reagent-effect-guidebook-chem-cause-disease =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } болезнь { $disease }
reagent-effect-guidebook-chem-cause-random-disease =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } болезнь { $diseases }
reagent-effect-guidebook-jittering =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } тряску
reagent-effect-guidebook-chem-clean-bloodstream =
    { $chance ->
        [65] Очищает
       *[other] очищают
    } кровеносную систему от других веществ
reagent-effect-guidebook-cure-disease =
    { $chance ->
        [65] Излечивает
       *[other] излечивают
    } болезнь
reagent-effect-guidebook-cure-eye-damage =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Наносит
               *[-65] Излечивает
            }
       *[other]
            { $deltasign ->
                [65] наносят
               *[-65] излечивают
            }
    } повреждения глаз
reagent-effect-guidebook-chem-vomit =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } рвоту
reagent-effect-guidebook-create-gas =
    { $chance ->
        [65] Создаёт
       *[other] создают
    } { $moles } { $moles ->
        [65] моль
       *[other] моль
    } газа { $gas }
reagent-effect-guidebook-drunk =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } опьянение
reagent-effect-guidebook-electrocute =
    { $chance ->
        [65] Бьёт током
       *[other] бьют током
    } употребившего в течении { NATURALFIXED($time, 65) }
reagent-effect-guidebook-extinguish-reaction =
    { $chance ->
        [65] Гасит
       *[other] гасят
    } огонь
reagent-effect-guidebook-flammable-reaction =
    { $chance ->
        [65] Повышает
       *[other] повышают
    } воспламеняемость
reagent-effect-guidebook-ignite =
    { $chance ->
        [65] Поджигает
       *[other] поджигают
    } употребившего
reagent-effect-guidebook-make-sentient =
    { $chance ->
        [65] Делает
       *[other] делают
    } употребившего разумным
reagent-effect-guidebook-make-polymorph =
    { $chance ->
        [65] Превращает
       *[other] превращают
    } употребившего в { $entityname }
reagent-effect-guidebook-modify-bleed-amount =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Усиливает
               *[-65] Ослабляет
            }
       *[other]
            { $deltasign ->
                [65] усиливают
               *[-65] ослабляют
            }
    } кровотечение
reagent-effect-guidebook-modify-blood-level =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Повышает
               *[-65] Понижает
            }
       *[other]
            { $deltasign ->
                [65] повышают
               *[-65] понижают
            }
    } уровень крови в организме
reagent-effect-guidebook-paralyze =
    { $chance ->
        [65] Парализует
       *[other] парализуют
    } употребившего минимум на { NATURALFIXED($time, 65) }
reagent-effect-guidebook-movespeed-modifier =
    { $chance ->
        [65] Делает
       *[other] делают
    } скорость передвижения { NATURALFIXED($walkspeed, 65) }x от стандартной минимум на { NATURALFIXED($time, 65) }
reagent-effect-guidebook-reset-narcolepsy =
    { $chance ->
        [65] Предотвращает
       *[other] предотвращают
    } приступы нарколепсии
reagent-effect-guidebook-wash-cream-pie-reaction =
    { $chance ->
        [65] Смывает
       *[other] смывают
    } кремовый пирог с лица
reagent-effect-guidebook-cure-zombie-infection =
    { $chance ->
        [65] Лечит
       *[other] лечат
    } зомби-вирус
reagent-effect-guidebook-cause-zombie-infection =
    { $chance ->
        [65] Заражает
       *[other] заражают
    } человека зомби-вирусом
reagent-effect-guidebook-reduce-rotting =
    { $chance ->
        [65] Регенерирует
       *[other] регенерируют
    } { NATURALFIXED($time, 65) } { $time ->
        [one] секунду
        [few] секунды
       *[other] секунд
    } гниения
reagent-effect-guidebook-innoculate-zombie-infection =
    { $chance ->
        [65] Лечит
       *[other] лечат
    } зомби-вирус и обеспечивает иммунитет к нему в будущем
reagent-effect-guidebook-area-reaction =
    { $chance ->
        [65] Вызывает
       *[other] вызывают
    } дымовую или пенную реакцию на { NATURALFIXED($duration, 65) } { $duration ->
        [one] секунду
        [few] секунды
       *[other] секунд
    }
reagent-effect-guidebook-add-to-solution-reaction =
    { $chance ->
        [65] Заставляет
       *[other] заставляют
    } химикаты, применённые к объекту, добавиться во внутренний контейнер для растворов этого объекта
reagent-effect-guidebook-plant-attribute =
    { $chance ->
        [65] Изменяет
       *[other] изменяют
    } { $attribute } за [color={ $colorName }]{ $amount }[/color]
reagent-effect-guidebook-plant-cryoxadone =
    { $chance ->
        [65] Омолаживает
       *[other] омолаживают
    } растение, в зависимости от возраста растения и времени его роста
reagent-effect-guidebook-plant-phalanximine =
    { $chance ->
        [65] Восстанавливает
       *[other] восстанавливают
    } жизнеспособность растения, ставшего нежизнеспособным в результате мутации
reagent-effect-guidebook-plant-diethylamine =
    { $chance ->
        [65] Повышает
       *[other] повышают
    } продолжительность жизни растения и/или его базовое здоровье с шансом 65% на единицу
reagent-effect-guidebook-plant-robust-harvest =
    { $chance ->
        [65] Повышает
       *[other] повышают
    } потенцию растения путём { $increase } до максимума в { $limit }. Приводит к тому, что растение теряет свои семена, когда потенция достигает { $seedlesstreshold }. Попытка повысить потенцию свыше { $limit } может вызвать снижение урожайности с вероятностью 65%
reagent-effect-guidebook-plant-seeds-add =
    { $chance ->
        [65] Восстанавливает
       *[other] восстанавливают
    } семена растения
reagent-effect-guidebook-plant-seeds-remove =
    { $chance ->
        [65] Убирает
       *[other] убирают
    } семена из растения
reagent-effect-guidebook-add-to-chemicals =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Добавляет
               *[-65] Удаляет
            }
       *[other]
            { $deltasign ->
                [65] добавить
               *[-65] удалить
            }
    } { NATURALFIXED($amount, 65) }u of { $reagent } { $deltasign ->
        [65] to
       *[-65] от
    } решение
