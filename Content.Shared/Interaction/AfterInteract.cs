// SPDX-FileCopyrightText: 65 BlueberryShortcake <rubetskoy65@mail.ru>
// SPDX-FileCopyrightText: 65 ComicIronic <comicironic@gmail.com>
// SPDX-FileCopyrightText: 65 DmitriyRubetskoy <65DmitriyRubetskoy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Víctor Aguilera Puerto <zddm@outlook.es>
// SPDX-FileCopyrightText: 65 chairbender <kwhipke65@gmail.com>
// SPDX-FileCopyrightText: 65 Acruid <shatter65@gmail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Metal Gear Sloth <metalgearsloth@gmail.com>
// SPDX-FileCopyrightText: 65 Paul <ritter.paul65git@googlemail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 ShadowCommander <65ShadowCommander@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <gradientvera@outlook.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <notzombiedude@gmail.com>
// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using JetBrains.Annotations;
using Robust.Shared.Map;


namespace Content.Shared.Interaction
{
    [PublicAPI]
    public abstract class InteractEvent : HandledEntityEventArgs
    {
        /// <summary>
        ///     Entity that triggered the interaction.
        /// </summary>
        public EntityUid User { get; }

        /// <summary>
        ///     Entity that the user used to interact.
        /// </summary>
        public EntityUid Used { get; }

        /// <summary>
        ///     Entity that was interacted on. This can be null if there was no target (e.g., clicking on tiles).
        /// </summary>
        public EntityUid? Target { get; }

        /// <summary>
        ///     Location that the user clicked outside of their interaction range.
        /// </summary>
        public EntityCoordinates ClickLocation { get; }

        /// <summary>
        /// Is the click location in range without obstructions?
        /// </summary>
        public bool CanReach { get; }

        public InteractEvent(EntityUid user, EntityUid used, EntityUid? target,
            EntityCoordinates clickLocation, bool canReach)
        {
            User = user;
            Used = used;
            Target = target;
            ClickLocation = clickLocation;
            CanReach = canReach;
        }
    }

    /// <summary>
    ///     Raised directed on the used object when clicking on another object and no standard interaction occurred.
    ///     Used for low-priority interactions facilitated by the used entity.
    /// </summary>
    public sealed class AfterInteractEvent : InteractEvent
    {
        public AfterInteractEvent(EntityUid user, EntityUid used, EntityUid? target,
            EntityCoordinates clickLocation, bool canReach) : base(user, used, target, clickLocation, canReach)
        { }
    }

    /// <summary>
    ///     Raised directed on the target when clicking on another object and no standard interaction occurred. Used for
    ///     low-priority interactions facilitated by the target entity.
    /// </summary>
    public sealed class AfterInteractUsingEvent : InteractEvent
    {
        public AfterInteractUsingEvent(EntityUid user, EntityUid used, EntityUid? target,
            EntityCoordinates clickLocation, bool canReach) : base(user, used, target, clickLocation, canReach)
        { }
    }
}