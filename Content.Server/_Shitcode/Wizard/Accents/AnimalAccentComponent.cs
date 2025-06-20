// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server._Goobstation.Wizard.Accents;

public abstract partial class AnimalAccentComponent : Component
{
    [DataField]
    public virtual List<LocId> AnimalNoises { get; set; }

    [DataField]
    public virtual List<LocId> AnimalAltNoises { get; set; }

    [DataField]
    public virtual float AltNoiseProbability { get; set; }
}