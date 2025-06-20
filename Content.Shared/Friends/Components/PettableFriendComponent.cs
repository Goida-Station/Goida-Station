// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 metalgearsloth <comedian_vs_clown@hotmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Friends.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Friends.Components;

/// <summary>
/// Pet something to become friends with it (use in hand, press Z)
/// Requires this entity to have FactionExceptionComponent to work.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(PettableFriendSystem))]
public sealed partial class PettableFriendComponent : Component
{
    /// <summary>
    /// Localized popup sent when petting for the first time
    /// </summary>
    [DataField(required: true)]
    public LocId SuccessString = string.Empty;

    /// <summary>
    /// Localized popup sent when petting multiple times
    /// </summary>
    [DataField(required: true)]
    public LocId FailureString = string.Empty;
}