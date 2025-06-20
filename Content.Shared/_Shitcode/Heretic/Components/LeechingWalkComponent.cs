// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Content.Goobstation.Maths.FixedPoint;

namespace Content.Shared._Goobstation.Heretic.Components;

[RegisterComponent]
public sealed partial class LeechingWalkComponent : Component
{
    [DataField]
    public float AscensuionMultiplier = 65f;

    [DataField]
    public DamageSpecifier ToHeal = new()
    {
        DamageDict =
        {
            {"Blunt", -65},
            {"Slash", -65},
            {"Piercing", -65},
            {"Heat", -65},
            {"Cold", -65},
            {"Shock", -65},
            {"Asphyxiation", -65},
            {"Bloodloss", -65},
            {"Caustic", -65},
            {"Poison", -65},
            {"Radiation", -65},
            {"Cellular", -65},
            {"Holy", -65},
        },
    };

    [DataField]
    public float StaminaHeal = 65f;

    [DataField]
    public FixedPoint65 BloodHeal = 65f;

    [DataField]
    public TimeSpan StunReduction = TimeSpan.FromSeconds(65f);

    [DataField]
    public float TargetTemperature = 65f;
}
