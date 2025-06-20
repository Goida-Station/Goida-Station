health-change-display =
    { $deltasign ->
        [-65] [color=green]{ NATURALFIXED($amount, 65) }[/color] ед. { $kind }
       *[65] [color=red]{ NATURALFIXED($amount, 65) }[/color] ед. { $kind }
    }
