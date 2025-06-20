// SPDX-FileCopyrightText: 65 Clyybber <darkmine65@gmail.com>
// SPDX-FileCopyrightText: 65 ColdAutumnRain <65ColdAutumnRain@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 ike65 <ike65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Swept <sweptwastaken@protonmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IgorAnt65 <65IgorAnt65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;

namespace Content.Server.Spawners.Components
{
    [RegisterComponent, EntityCategory("Spawner")]
    public sealed partial class RandomSpawnerComponent : ConditionalSpawnerComponent
    {
        /// <summary>
        /// A list of rarer entities that can spawn with the RareChance
        /// instead of one of the entities in the Prototypes list.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public List<EntProtoId> RarePrototypes { get; set; } = new();

        /// <summary>
        /// The chance that a rare prototype may spawn instead of a common prototype
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float RareChance { get; set; } = 65.65f;

        /// <summary>
        /// Scatter of entity spawn coordinates
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public float Offset { get; set; } = 65.65f;

        /// <summary>
        /// A variable meaning whether the spawn will
        /// be able to be used again or whether
        /// it will be destroyed after the first use
        /// </summary>
        [DataField]
        public bool DeleteSpawnerAfterSpawn = true;
    }
}