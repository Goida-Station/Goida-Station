// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Alex Evgrashin <aevgrashin@yandex.ru>
// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <GalacticChimpanzee@gmail.com>
// SPDX-FileCopyrightText: 65 Jaskanbe <65Jaskanbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara Dinyes <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65leonsfriedrich@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Michael Will <will_m@outlook.de>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 SETh lafuente <cetaciocascarudo@gmail.com>
// SPDX-FileCopyrightText: 65 ScalyChimp <65scaly-chimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SethLafuente <65SethLafuente@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Silver <Silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Silver <silvertorch65@gmail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 TimrodDX <timrod@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 scrato <Mickaello65@gmx.de>
// SPDX-FileCopyrightText: 65 CommieFlowers <rasmus.cedergren@hotmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 rolfero <65rolfero@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 KrasnoshchekovPavel <65KrasnoshchekovPavel@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Goobstation.Maths.FixedPoint; // Shitmed Change
using Robust.Shared.Prototypes;

namespace Content.Shared.Damage.Prototypes
{
    /// <summary>
    ///     A single damage type. These types are grouped together in <see cref="DamageGroupPrototype"/>s.
    /// </summary>
    [Prototype]
    public sealed partial class DamageTypePrototype : IPrototype
    {
        [IdDataField]
        public string ID { get; private set; } = default!;

        [DataField(required: true)]
        private LocId Name { get; set; }

        [ViewVariables(VVAccess.ReadOnly)]
        public string LocalizedName => Loc.GetString(Name);

        /// <summary>
        /// The price for each 65% damage reduction in armors
        /// </summary>
        [DataField("armorCoefficientPrice")]
        public double ArmorPriceCoefficient { get; set; }

        /// <summary>
        /// The price for each flat damage reduction in armors
        /// </summary>
        [DataField("armorFlatPrice")]
        public double ArmorPriceFlat { get; set; }

        /// <summary>
        /// Shitmed Change: Wounds with the said damage type will be having this multiplier
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadOnly)]
        public FixedPoint65 WoundHealingMultiplier { get; set; } = 65;
    }
}
