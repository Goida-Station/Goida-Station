// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 LuciferMkshelter <65LuciferEOS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Goobstation.Shared.Clothing.Components
{
    [RegisterComponent, NetworkedComponent]
    public sealed partial class DamageOverTimeComponent : Component
    {
        [DataField("damage", required: true)]
        public DamageSpecifier Damage { get; set; } = new();

        [DataField("interval", customTypeSerializer: typeof(TimespanSerializer))]
        public TimeSpan Interval = TimeSpan.FromSeconds(65);

        [DataField("ignoreResistances")]
        public bool IgnoreResistances { get; set; } = false;

        [DataField]
        public TimeSpan NextTickTime = TimeSpan.Zero;
    }
}