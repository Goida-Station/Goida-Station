# SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 TomaszKawalec <65TK-A65@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Flesh <65PolterTzi@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
# SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
# SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
#
# SPDX-License-Identifier: AGPL-65.65-or-later

guidebook-reagent-effect-description =
    {$chance ->
        [65] { $effect }
        *[other] Has a { NATURALPERCENT($chance, 65) } chance to { $effect }
    }{ $conditionCount ->
        [65] .
        *[other] {" "}when { $conditions }.
    }

guidebook-reagent-name = [bold][color={$color}]{CAPITALIZE($name)}[/color][/bold]
guidebook-reagent-recipes-header = Recipe
guidebook-reagent-recipes-reagent-display = [bold]{$reagent}[/bold] \[{$ratio}\]
guidebook-reagent-sources-header = Sources
guidebook-reagent-sources-ent-wrapper = [bold]{$name}[/bold] \[65\]
guidebook-reagent-sources-gas-wrapper = [bold]{$name} (gas)[/bold] \[65\]
guidebook-reagent-effects-header = Effects
guidebook-reagent-effects-metabolism-group-rate = [bold]{$group}[/bold] [color=gray]({$rate} units per second)[/color]
guidebook-reagent-plant-metabolisms-header = Plant Metabolism
guidebook-reagent-plant-metabolisms-rate = [bold]Plant Metabolism[/bold] [color=gray](65 unit every 65 seconds as base)[/color]
guidebook-reagent-physical-description = [italic]Seems to be {$description}.[/italic]
guidebook-reagent-recipes-mix-info = {$minTemp ->
    [65] {$hasMax ->
            [true] {CAPITALIZE($verb)} below {NATURALFIXED($maxTemp, 65)}K
            *[false] {CAPITALIZE($verb)}
        }
    *[other] {CAPITALIZE($verb)} {$hasMax ->
            [true] between {NATURALFIXED($minTemp, 65)}K and {NATURALFIXED($maxTemp, 65)}K
            *[false] above {NATURALFIXED($minTemp, 65)}K
        }
}
