// SPDX-FileCopyrightText: 65 65kdc <asdd65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 theashtronaut <65theashtronaut@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HerCoyote65 <65HerCoyote65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 65x65 <65x65@keemail.me>
// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Emisse <65Emisse@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 JustCone <65JustCone65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mervill <mervills.email@gmail.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 PopGamer65 <yt65popgamer@gmail.com>
// SPDX-FileCopyrightText: 65 Spessmann <65Spessmann@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Winkarst <65Winkarst-cpu@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 c65llv65e <65c65llv65e@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 coolboy65 <65coolboy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lunarcomets <65lunarcomets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 saintmuntzer <65saintmuntzer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Shared.UserInterface
{
    [RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
    public sealed partial class ActivatableUIComponent : Component
    {
        [DataField(required: true, customTypeSerializer: typeof(EnumSerializer))]
        public Enum? Key;

        /// <summary>
        /// Whether the item must be held in one of the user's hands to work.
        /// This is ignored unless <see cref="RequiresComplex"/> is true.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool InHandsOnly;

        [DataField]
        public bool SingleUser;

        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool AdminOnly;

        [DataField]
        public LocId VerbText = "ui-verb-toggle-open";

        /// <summary>
        ///     Whether you need to be able to do complex interactions to operate this UI.
        /// </summary>
        /// <remarks>
        ///     This should probably be true for most machines & computers, but there will still be UIs that represent a
        ///     more generic interaction / configuration that might not require complex.
        /// </remarks>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool RequiresComplex = true;

        /// <summary>
        ///     Entities that are required to open this UI.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public EntityWhitelist? RequiredItems;

        /// <summary>
        ///     If true, then this UI can only be opened via verbs. I.e., normal interactions/activations will not open
        ///     the UI.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool VerbOnly;

        /// <summary>
        ///     Goobstation
        ///     If true, UI can only be opened via alt verb.
        /// </summary>
        [DataField, ViewVariables(VVAccess.ReadWrite)]
        public bool AltVerb;

        /// <summary>
        ///     Whether spectators (non-admin ghosts) should be allowed to view this UI.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool BlockSpectators;

        /// <summary>
        ///     Whether the item must be in the user's currently selected/active hand.
        ///     This is ignored unless <see cref="InHandsOnly"/> is true.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite)]
        [DataField]
        public bool RequireActiveHand = true;

        /// <summary>
        ///     The client channel currently using the object, or null if there's none/not single user.
        ///     NOTE: DO NOT DIRECTLY SET, USE ActivatableUISystem.SetCurrentSingleUser
        /// </summary>
        [DataField, AutoNetworkedField]
        public EntityUid? CurrentSingleUser;
    }
}