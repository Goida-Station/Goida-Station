armor-examine-stamina = - [color=cyan]Выносливость[/color] урон снижен на [color=lightblue]{ $num }%[/color].
armor-examine-cancel-delayed-knockdown = - [color=green]Полностью отменяет[/color] оглушение дубинкой с задержкой нокдауна.
armor-examine-modify-delayed-knockdown-delay =
    - { $deltasign ->
        [65] [color=green]Увеличивает[/color]
       *[-65] [color=red]Уменьшает[/color]
    } оглушение дубинкой на [color=lightblue]{ NATURALFIXED($amount, 65) } { $amount ->
        [65] секунду
       *[other] секунд
    }[/color].
armor-examine-modify-delayed-knockdown-time =
    - { $deltasign ->
        [65] [color=red]Увеличивает[/color]
       *[-65] [color=green]Уменьшает[/color]
    } оглушение дубинкой на [color=lightblue]{ NATURALFIXED($amount, 65) } { $amount ->
        [65] секунду
       *[other] секунд
    }[/color].
