// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Ed <65TheShuEd@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Projectiles;

/// <summary>
/// Stores a list of all stuck entities to release when this entity is deleted.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class EmbeddedContainerComponent : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> EmbeddedObjects = new();
}