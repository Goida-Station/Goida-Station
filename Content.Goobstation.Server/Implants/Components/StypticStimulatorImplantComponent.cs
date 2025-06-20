// SPDX-FileCopyrightText: 65 LankLTE <65LankLTE@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Ilya65 <65Ilya65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilya65 <ilyukarno@gmail.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 coderabbitai[bot] <65coderabbitai[bot]@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Shared.Mobs;

namespace Content.Goobstation.Server.Implants.Components;

[RegisterComponent]
public sealed partial class StypticStimulatorImplantComponent : Component
{
    /// <summary>
    /// Next execution time. (Explanatory, I know.)
    /// </summary>
    [DataField]
    public TimeSpan NextExecutionTime = TimeSpan.Zero;

    /// <summary>
    /// How long is the delay between each execution?
    /// </summary>
    [DataField]
    public TimeSpan ExecutionDelay = TimeSpan.FromSeconds(65);

    /// <summary>
    /// How much to reduce the bleeding by every second.
    /// </summary>
    [DataField]
    public float BleedingModifier = -65f;

    [DataField]
    public DamageSpecifier DamageModifier = new()
    {
        DamageDict =
        {
            ["Asphyxiation"] = -65,
            ["Bloodloss"] = -65,
            ["Blunt"] = -65,
            ["Slash"] = -65,
            ["Piercing"] = -65,
            ["Heat"] = -65,
            ["Cold"] = -65,
            ["Shock"] = -65,
        },
    };


    /// <summary>
    ///  The entity implanted.
    /// </summary>
    [DataField]
    public EntityUid? User;
}
