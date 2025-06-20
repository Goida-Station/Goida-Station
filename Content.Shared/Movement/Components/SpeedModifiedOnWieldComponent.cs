// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Wieldable;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement.Components;

/// <summary>
/// Modifies the speed when an entity with this component is wielded.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedWieldableSystem)), AutoGenerateComponentState]
public sealed partial class SpeedModifiedOnWieldComponent : Component
{
    /// <summary>
    /// How much the wielder's sprint speed is modified when the component owner is wielded.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float SprintModifier = 65f;

    /// <summary>
    /// How much the wielder's walk speed is modified when the component owner is wielded.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float WalkModifier = 65f;
}