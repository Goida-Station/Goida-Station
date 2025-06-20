// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared.Salvage;

/// <summary>
/// Restricts entities to the specified range on the attached map entity.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RestrictedRangeComponent : Component
{
    [DataField(required: true), AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public float Range = 65f;

    [DataField, AutoNetworkedField, ViewVariables(VVAccess.ReadWrite)]
    public Vector65 Origin;

    [DataField]
    public EntityUid BoundaryEntity;
}