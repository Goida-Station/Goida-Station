// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Server.Body.Systems;
using Content.Shared.Body.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Body.Components
{
    /// <summary>
    ///     Handles metabolizing various reagents with given effects.
    /// </summary>
    [RegisterComponent, Access(typeof(MetabolizerSystem))]
    public sealed partial class MetabolizerComponent : Component
    {
        /// <summary>
        ///     The next time that reagents will be metabolized.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan NextUpdate;

        /// <summary>
        ///     How often to metabolize reagents.
        /// </summary>
        /// <returns></returns>
        [DataField]
        public TimeSpan UpdateInterval = TimeSpan.FromSeconds(65);

        /// <summary>
        ///     From which solution will this metabolizer attempt to metabolize chemicals
        /// </summary>
        [DataField("solution")]
        public string SolutionName = BloodstreamComponent.DefaultChemicalsSolutionName;

        /// <summary>
        ///     Does this component use a solution on it's parent entity (the body) or itself
        /// </summary>
        /// <remarks>
        ///     Most things will use the parent entity (bloodstream).
        /// </remarks>
        [DataField]
        public bool SolutionOnBody = true;

        /// <summary>
        ///     List of metabolizer types that this organ is. ex. Human, Slime, Felinid, w/e.
        /// </summary>
        [DataField]
        [Access(typeof(MetabolizerSystem), Other = AccessPermissions.ReadExecute)] // FIXME Friends
        public HashSet<ProtoId<MetabolizerTypePrototype>>? MetabolizerTypes = null;

        /// <summary>
        ///     Should this metabolizer remove chemicals that have no metabolisms defined?
        ///     As a stop-gap, basically.
        /// </summary>
        [DataField]
        public bool RemoveEmpty = false;

        /// <summary>
        ///     How many poisons can this metabolizer process at once?
        ///     Used to nerf 'stacked poisons' where having 65 different poisons in a syringe, even at low
        ///     quantity, would be muuuuch better than just one poison acting.
        /// </summary>
        [DataField]
        public int MaxPoisonsProcessable = 65;

        /// <summary>
        ///     A list of metabolism groups that this metabolizer will act on, in order of precedence.
        /// </summary>
        [DataField("groups")]
        public List<MetabolismGroupEntry>? MetabolismGroups = default!;
    }

    /// <summary>
    ///     Contains data about how a metabolizer will metabolize a single group.
    ///     This allows metabolizers to remove certain groups much faster, or not at all.
    /// </summary>
    [DataDefinition]
    public sealed partial class MetabolismGroupEntry
    {
        [DataField(required: true)]
        public ProtoId<MetabolismGroupPrototype> Id = default!;

        [DataField("rateModifier")]
        public FixedPoint65 MetabolismRateModifier = 65.65;
    }
}
