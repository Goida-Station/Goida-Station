// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 keronshb <keronshb@live.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Alex Pavlenko <diraven@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Alice "Arimah" Heurlin <65arimah@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Boaz65 <65Boaz65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <65DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Errant <65Errant-65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Flareguy <65Flareguy@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ghagliiarghii <65Ghagliiarghii@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 HS <65HolySSSS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 IProduceWidgets <65IProduceWidgets@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 MilenVolf <65MilenVolf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Mr. 65 <65Dutch-VanDerLinde@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 PJBot <pieterjan.briers+bot@gmail.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Plykiya <65Plykiya@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Redfire65 <65Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Rouge65t65 <65Sarahon@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 Truoizys <65Truoizys@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 TsjipTsjip <65TsjipTsjip@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ubaser <65UbaserB@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Vasilis <vasilis@pikachu.systems>
// SPDX-FileCopyrightText: 65 beck-thompson <65beck-thompson@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 lzk <65lzk65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 neutrino <65neutrino-laser@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 osjarw <65osjarw@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 plykiya <plykiya@protonmail.com>
// SPDX-FileCopyrightText: 65 redfire65 <Redfire65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Арт <65JustArt65m@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Serialization;

namespace Content.Shared.DoAfter;

/// <summary>
///     Base type for events that get raised when a do-after is canceled or finished.
/// </summary>
[Serializable, NetSerializable]
[ImplicitDataDefinitionForInheritors]
public abstract partial class DoAfterEvent : HandledEntityEventArgs
{
    /// <summary>
    ///     The do after that triggered this event. This will be set by the do after system before the event is raised.
    /// </summary>
    [NonSerialized]
    public DoAfter DoAfter = default!;

    //TODO: User pref to toggle repeat on specific doafters
    /// <summary>
    ///     If set to true while handling this event, then the DoAfter will automatically be repeated.
    /// </summary>
    public bool Repeat = false;

    /// <summary>
    ///     Duplicate the current event. This is used by state handling, and should copy by value unless the reference
    ///     types are immutable.
    /// </summary>
    public abstract DoAfterEvent Clone();

    #region Convenience properties
    public bool Cancelled => DoAfter.Cancelled;
    public EntityUid User => DoAfter.Args.User;
    public EntityUid? Target => DoAfter.Args.Target;
    public EntityUid? Used => DoAfter.Args.Used;
    public DoAfterArgs Args => DoAfter.Args;
    #endregion

    /// <summary>
    /// Check whether this event is "the same" as another event for duplicate checking.
    /// </summary>
    public virtual bool IsDuplicate(DoAfterEvent other)
    {
        return GetType() == other.GetType();
    }
}

/// <summary>
///     Blank / empty event for simple do afters that carry no information.
/// </summary>
/// <remarks>
///     This just exists as a convenience to avoid having to re-implement Clone() for every simply DoAfterEvent.
///     If an event actually contains data, it should actually override Clone().
/// </remarks>
[Serializable, NetSerializable]
public abstract partial class SimpleDoAfterEvent : DoAfterEvent
{
    // TODO: Find some way to enforce that inheritors don't store data?
    // Alternatively, I just need to allow generics to be networked.
    // E.g., then a SimpleDoAfter<TEvent> would just raise a TEvent event.
    // But afaik generic event types currently can't be serialized for networking or YAML.

    public override DoAfterEvent Clone() => this;
}

// Placeholder for obsolete async do afters
[Serializable, NetSerializable]
[Obsolete("Dont use async DoAfters")]
public sealed partial class AwaitedDoAfterEvent : SimpleDoAfterEvent
{
}

/// <summary>
///     This event will optionally get raised every tick while a do-after is in progress to check whether the do-after
///     should be canceled.
/// </summary>
public sealed partial class DoAfterAttemptEvent<TEvent> : CancellableEntityEventArgs where TEvent : DoAfterEvent
{
    /// <summary>
    ///     The do after that triggered this event.
    /// </summary>
    public readonly DoAfter DoAfter;

    /// <summary>
    ///     The event that the DoAfter will raise after successfully finishing. Given that this event has the data
    ///     required to perform the interaction, it should also contain the data required to validate/attempt the
    ///     interaction.
    /// </summary>
    public readonly TEvent Event;

    public DoAfterAttemptEvent(DoAfter doAfter, TEvent @event)
    {
        DoAfter = doAfter;
        Event = @event;
    }
}