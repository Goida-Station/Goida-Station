// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Chief-Engineer <65Chief-Engineer@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using JetBrains.Annotations;

namespace Content.Shared.Interaction;

/// <summary>
///     Raised when an entity is activated in the world.
/// </summary>
[PublicAPI]
public sealed class ActivateInWorldEvent : HandledEntityEventArgs, ITargetedInteractEventArgs
{
    /// <summary>
    ///     Entity that activated the target world entity.
    /// </summary>
    public EntityUid User { get; }

    /// <summary>
    ///     Entity that was activated in the world.
    /// </summary>
    public EntityUid Target { get; }

    /// <summary>
    ///     Whether or not <see cref="User"/> can perform complex interactions or only basic ones.
    /// </summary>
    public bool Complex;

    /// <summary>
    ///     Set to true when the activation is logged by a specific logger.
    /// </summary>
    public bool WasLogged { get; set; }

    public ActivateInWorldEvent(EntityUid user, EntityUid target, bool complex)
    {
        User = user;
        Target = target;
        Complex = complex;
    }
}

/// <summary>
/// Event raised on the user when it activates something in the world
/// </summary>
[PublicAPI]
public sealed class UserActivateInWorldEvent : HandledEntityEventArgs, ITargetedInteractEventArgs
{
    /// <summary>
    ///     Entity that activated the target world entity.
    /// </summary>
    public EntityUid User { get; }

    /// <summary>
    ///     Entity that was activated in the world.
    /// </summary>
    public EntityUid Target { get; }

    /// <summary>
    ///     Whether or not <see cref="User"/> can perform complex interactions or only basic ones.
    /// </summary>
    public bool Complex;

    public UserActivateInWorldEvent(EntityUid user, EntityUid target, bool complex)
    {
        User = user;
        Target = target;
        Complex = complex;
    }
}