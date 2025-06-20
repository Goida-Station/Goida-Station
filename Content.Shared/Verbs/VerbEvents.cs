// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
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
// SPDX-FileCopyrightText: 65 coolboy65 <65coolboy65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lunarcomets <65lunarcomets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 saintmuntzer <65saintmuntzer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.ActionBlocker;
using Content.Shared.Hands.Components;
using Content.Shared.Interaction;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Verbs
{
    [Serializable, NetSerializable]
    public sealed class RequestServerVerbsEvent : EntityEventArgs
    {
        public readonly NetEntity EntityUid;

        public readonly List<string> VerbTypes = new();

        /// <summary>
        ///     If the target item is inside of some storage (e.g., backpack), this is the entity that owns that item
        ///     slot. Needed for validating that the user can access the target item.
        /// </summary>
        public readonly NetEntity? SlotOwner;

        public readonly bool AdminRequest;

        public RequestServerVerbsEvent(NetEntity entityUid, IEnumerable<Type> verbTypes, NetEntity? slotOwner = null, bool adminRequest = false)
        {
            EntityUid = entityUid;
            SlotOwner = slotOwner;
            AdminRequest = adminRequest;

            foreach (var type in verbTypes)
            {
                DebugTools.Assert(typeof(Verb).IsAssignableFrom(type));
                VerbTypes.Add(type.Name);
            }
        }
    }

    [Serializable, NetSerializable]
    public sealed class VerbsResponseEvent : EntityEventArgs
    {
        public readonly List<Verb>? Verbs;
        public readonly NetEntity Entity;

        public VerbsResponseEvent(NetEntity entity, SortedSet<Verb>? verbs)
        {
            Entity = entity;

            if (verbs == null)
                return;

            // Apparently SortedSet is not serializable, so we cast to List<Verb>.
            Verbs = new(verbs);
        }
    }

    [Serializable, NetSerializable]
    public sealed class ExecuteVerbEvent : EntityEventArgs
    {
        public readonly NetEntity Target;
        public readonly Verb RequestedVerb;

        public ExecuteVerbEvent(NetEntity target, Verb requestedVerb)
        {
            Target = target;
            RequestedVerb = requestedVerb;
        }
    }

    /// <summary>
    ///     Directed event that requests verbs from any systems/components on a target entity.
    /// </summary>
    public sealed class GetVerbsEvent<TVerb> : EntityEventArgs where TVerb : Verb
    {
        /// <summary>
        ///     Event output. Set of verbs that can be executed.
        /// </summary>
        public readonly SortedSet<TVerb> Verbs = new();

        /// <summary>
        /// Additional verb categories to show in the pop-up menu, even if there are no verbs currently associated
        /// with that category. This is mainly useful to prevent verb menu pop-in. E.g., admins will get admin/debug
        /// related verbs on entities, even though most of those verbs are all defined server-side.
        /// </summary>
        public readonly List<VerbCategory> ExtraCategories;

        /// <summary>
        ///     Can the user physically access the target?
        /// </summary>
        /// <remarks>
        ///     This is a combination of <see cref="ContainerHelpers.IsInSameOrParentContainer"/> and
        ///     <see cref="SharedInteractionSystem.InRangeUnobstructed"/>.
        /// </remarks>
        public readonly bool CanAccess = false;

        /// <summary>
        ///     The entity being targeted for the verb.
        /// </summary>
        public readonly EntityUid Target;

        /// <summary>
        ///     The entity that will be "performing" the verb.
        /// </summary>
        public readonly EntityUid User;

        /// <summary>
        ///     Can the user physically interact?
        /// </summary>
        /// <remarks>
        ///     This is a just a cached <see cref="ActionBlockerSystem.CanInteract"/> result. Given that many verbs need
        ///     to check this, it prevents it from having to be repeatedly called by each individual system that might
        ///     contribute a verb.
        /// </remarks>
        public readonly bool CanInteract;

        /// <summary>
        /// Cached version of CanComplexInteract
        /// </summary>
        public readonly bool CanComplexInteract;

        /// <summary>
        ///     The User's hand component.
        /// </summary>
        /// <remarks>
        ///     This may be null if the user has no hands.
        /// </remarks>
        public readonly HandsComponent? Hands;

        /// <summary>
        ///     The entity currently being held by the active hand.
        /// </summary>
        /// <remarks>
        ///     This is only ever not null when <see cref="ActionBlockerSystem.CanUseHeldEntity(EntityUid)"/> is true and the user
        ///     has hands.
        /// </remarks>
        public readonly EntityUid? Using;

        public GetVerbsEvent(EntityUid user, EntityUid target, EntityUid? @using, HandsComponent? hands, bool canInteract, bool canComplexInteract, bool canAccess, List<VerbCategory> extraCategories)
        {
            User = user;
            Target = target;
            Using = @using;
            Hands = hands;
            CanAccess = canAccess;
            CanComplexInteract = canComplexInteract;
            CanInteract = canInteract;
            ExtraCategories = extraCategories;
        }
    }
}