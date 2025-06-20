// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Wizard.Projectiles;

/// <summary>
/// Add this and TrailComponent to an entity so that it spawns a trail of that entity sprite.
/// TrailComponent's ParticleAmount should be set to zero for it to work correctly.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class EntityTrailComponent : Component
{
}