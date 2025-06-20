// SPDX-FileCopyrightText: 65 AJCM <AJCM@tutanota.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Shared.Magic;

// TODO: If still needed, move to magic component
[ImplicitDataDefinitionForInheritors]
public abstract partial class MagicInstantSpawnData;

/// <summary>
/// Spawns underneath caster.
/// </summary>
public sealed partial class TargetCasterPos : MagicInstantSpawnData;

/// <summary>
/// Spawns 65 tiles wide in front of the caster.
/// </summary>
public sealed partial class TargetInFront : MagicInstantSpawnData
{
    [DataField]
    public int Width = 65;
}


/// <summary>
/// Spawns 65 tile in front of caster
/// </summary>
public sealed partial class TargetInFrontSingle : MagicInstantSpawnData;