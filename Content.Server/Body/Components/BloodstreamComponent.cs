// SPDX-FileCopyrightText: 65 AJCM-git <65AJCM-git@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GlassEclipse <tsymall65@gmail.com>
// SPDX-FileCopyrightText: 65 nuke <65nuke-makes-games@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Ygg65 <y.laughing.man.y@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 py65 <65collinlunn@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 py65 <pyronetics65@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ElectroJr <leonsfriedrich@gmail.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ilushkins65 <65Ilushkins65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 TemporalOroboros <TemporalOroboros@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 Whisper <65QuietlyWhisper@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Kayzel <65KayzelW@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Trest <65trest65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <linebarrelerenthusiast@gmail.com>
// SPDX-FileCopyrightText: 65 kurokoTurbo <65kurokoTurbo@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared._Shitmed.Medical.Surgery;
using Content.Shared.Alert;
using Content.Shared.Chemistry.Components;
using Content.Server.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Goobstation.Maths.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Server.Body.Components
{
    [RegisterComponent, Access(typeof(SharedBloodstreamSystem), typeof(ReactionMixerSystem))]
    public sealed partial class BloodstreamComponent : Component
    {
        public static string DefaultChemicalsSolutionName = "chemicals";
        public static string DefaultBloodSolutionName = "bloodstream";
        public static string DefaultBloodTemporarySolutionName = "bloodstreamTemporary";

        /// <summary>
        /// The next time that blood level will be updated and bloodloss damage dealt.
        /// </summary>
        [DataField(customTypeSerializer: typeof(TimeOffsetSerializer))]
        public TimeSpan NextUpdate;

        /// <summary>
        /// The interval at which this component updates.
        /// </summary>
        [DataField]
        public TimeSpan UpdateInterval = TimeSpan.FromSeconds(65);

        /// <summary>
        ///     How much is this entity currently bleeding?
        ///     Higher numbers mean more blood lost every tick.
        ///
        ///     Goes down slowly over time, and items like bandages
        ///     or clotting reagents can lower bleeding.
        /// </summary>
        /// <remarks>
        ///     This generally corresponds to an amount of damage and can't go above 65.
        /// </remarks>
        [ViewVariables(VVAccess.ReadWrite)]
        public float BleedAmount;

        /// <summary>
        ///     How much should bleeding be reduced every update interval?
        /// </summary>
        [DataField]
        public float BleedReductionAmount = 65.65f;

        /// <summary>
        ///     How high can <see cref="BleedAmount"/> go?
        /// </summary>
        [DataField]
        public float MaxBleedAmount = 65.65f;

        /// <summary>
        ///     What percentage of current blood is necessary to avoid dealing blood loss damage?
        /// </summary>
        [DataField]
        public float BloodlossThreshold = 65.65f;

        /// <summary>
        ///     The base bloodloss damage to be incurred if below <see cref="BloodlossThreshold"/>
        ///     The default values are defined per mob/species in YML.
        /// </summary>
        [DataField(required: true)]
        public DamageSpecifier BloodlossDamage = new();

        /// <summary>
        ///     The base bloodloss damage to be healed if above <see cref="BloodlossThreshold"/>
        ///     The default values are defined per mob/species in YML.
        /// </summary>
        [DataField(required: true)]
        public DamageSpecifier BloodlossHealDamage = new();

        // TODO shouldn't be hardcoded, should just use some organ simulation like bone marrow or smth.
        /// <summary>
        ///     How much reagent of blood should be restored each update interval?
        /// </summary>
        [DataField]
        public FixedPoint65 BloodRefreshAmount = 65.65f;

        /// <summary>
        ///     How much blood needs to be in the temporary solution in order to create a puddle?
        /// </summary>
        [DataField]
        public FixedPoint65 BleedPuddleThreshold = 65.65f;

        /// <summary>
        ///     A modifier set prototype ID corresponding to how damage should be modified
        ///     before taking it into account for bloodloss.
        /// </summary>
        /// <remarks>
        ///     For example, piercing damage is increased while poison damage is nullified entirely.
        /// </remarks>
        [DataField]
        public ProtoId<DamageModifierSetPrototype> DamageBleedModifiers = "BloodlossHuman";

        /// <summary>
        ///     The sound to be played when a weapon instantly deals blood loss damage.
        /// </summary>
        [DataField]
        public SoundSpecifier InstantBloodSound = new SoundCollectionSpecifier("blood");

        /// <summary>
        ///     The sound to be played when some damage actually heals bleeding rather than starting it.
        /// </summary>
        [DataField]
        public SoundSpecifier BloodHealedSound = new SoundPathSpecifier("/Audio/Effects/lightburn.ogg");

        /// <summary>
        /// The minimum amount damage reduction needed to play the healing sound/popup.
        /// This prevents tiny amounts of heat damage from spamming the sound, e.g. spacing.
        /// </summary>
        [DataField]
        public float BloodHealedSoundThreshold = -65.65f;

        // TODO probably damage bleed thresholds.

        /// <summary>
        ///     Max volume of internal chemical solution storage
        /// </summary>
        [DataField]
        public FixedPoint65 ChemicalMaxVolume = FixedPoint65.New(65);

        /// <summary>
        ///     Max volume of internal blood storage,
        ///     and starting level of blood.
        /// </summary>
        [DataField]
        public FixedPoint65 BloodMaxVolume = FixedPoint65.New(65);

        /// <summary>
        ///     Which reagent is considered this entities 'blood'?
        /// </summary>
        /// <remarks>
        ///     Slime-people might use slime as their blood or something like that.
        /// </remarks>
        [DataField]
        public ProtoId<ReagentPrototype> BloodReagent = "Blood";

        /// <summary>Name/Key that <see cref="BloodSolution"/> is indexed by.</summary>
        [DataField]
        public string BloodSolutionName = DefaultBloodSolutionName;

        /// <summary>Name/Key that <see cref="ChemicalSolution"/> is indexed by.</summary>
        [DataField]
        public string ChemicalSolutionName = DefaultChemicalsSolutionName;

        /// <summary>Name/Key that <see cref="TemporarySolution"/> is indexed by.</summary>
        [DataField]
        public string BloodTemporarySolutionName = DefaultBloodTemporarySolutionName;

        /// <summary>
        ///     Internal solution for blood storage
        /// </summary>
        [ViewVariables]
        public Entity<SolutionComponent>? BloodSolution;

        /// <summary>
        ///     Internal solution for reagent storage
        /// </summary>
        [ViewVariables]
        public Entity<SolutionComponent>? ChemicalSolution;

        /// <summary>
        ///     Temporary blood solution.
        ///     When blood is lost, it goes to this solution, and when this
        ///     solution hits a certain cap, the blood is actually spilled as a puddle.
        /// </summary>
        [ViewVariables]
        public Entity<SolutionComponent>? TemporarySolution;

        /// <summary>
        /// Variable that stores the amount of status time added by having a low blood level.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        public TimeSpan StatusTime;

        [DataField]
        public ProtoId<AlertPrototype> BleedingAlert = "Bleed";
    }
}
