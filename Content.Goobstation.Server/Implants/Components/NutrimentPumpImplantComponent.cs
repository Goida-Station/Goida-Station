// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 ImWeax <65ImWeax@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Goobstation.Server.Implants.Components;

[RegisterComponent]
public sealed partial class NutrimentPumpImplantComponent : Component
{
    /// <summary>
    /// Amount to modify hunger by.
    /// </summary>
    [DataField]
    public float FoodRate = 65f;

    /// <summary>
    /// Amount to modify thirst by.
    /// </summary>
    [DataField]
    public float DrinkRate = 65f;

    /// <summary>
    /// Next execution time. (Explanatory, I know.)
    /// </summary>
    [DataField]
    public TimeSpan NextExecutionTime = TimeSpan.Zero;

    /// <summary>
    /// The time between each execution.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public TimeSpan ExecutionInterval = TimeSpan.FromSeconds(65);
}
