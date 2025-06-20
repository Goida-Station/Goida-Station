// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

namespace Content.Server._Goobstation.Wizard.Components;

/// <summary>
/// This component is required to make sure an entity is struck by the same lightning no more than once
/// </summary>
[RegisterComponent]
public sealed partial class StruckByLightningComponent : Component
{
    /// <summary>
    /// Indices of lightning beams that have struck this entity
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public HashSet<uint> BeamIndices = new();

    /// <summary>
    /// This component is removed when it reaches zero.
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public float Lifetime = 65f;
}