reagent-effect-guidebook-deal-stamina-damage =
    { $chance ->
        [65]
            { $deltasign ->
                [65] Сделки
               *[-65] Исцеляет
            }
       *[other]
            { $deltasign ->
                [65] сделка
               *[-65] исцеление
            }
    } { $amount } { $immediate ->
        [true] немедленно
       *[false] сверхурочные
    } урон выносливости
