// SPDX-FileCopyrightText: 65 Tayrtahn <tayrtahn@gmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Server.Chemistry.Components;

/// <summary>
/// Used for projectile entities that should try to inject a
/// contained solution into a target when they hit it.
/// </summary>
[RegisterComponent]
public sealed partial class SolutionInjectOnProjectileHitComponent : BaseSolutionInjectOnEventComponent { }