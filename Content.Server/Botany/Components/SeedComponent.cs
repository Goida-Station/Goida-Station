// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 E F R <65Efruit@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Galactic Chimp <65GalacticChimp@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flesh <65PolterTzi@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Magnus Larsen <i.am.larsenml@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Botany.Systems;
using Content.Shared.Botany.Components;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Server.Botany.Components
{
    [RegisterComponent, Access(typeof(BotanySystem))]
    public sealed partial class SeedComponent : SharedSeedComponent
    {
        /// <summary>
        ///     Seed data containing information about the plant type & properties that this seed can grow seed. If
        ///     null, will instead attempt to get data from a seed prototype, if one is defined. See <see
        ///     cref="SeedId"/>.
        /// </summary>
        [DataField("seed")]
        public SeedData? Seed;

        /// <summary>
        ///     If not null, overrides the plant's initial health. Otherwise, the plant's initial health is set to the Endurance value.
        /// </summary>
        [DataField]
        public float? HealthOverride = null;

        /// <summary>
        ///     Name of a base seed prototype that is used if <see cref="Seed"/> is null.
        /// </summary>
        [DataField("seedId", customTypeSerializer: typeof(PrototypeIdSerializer<SeedPrototype>))]
        public string? SeedId;
    }
}