// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Atmos.Rotting;

/// <summary>
/// Entities inside this container will rot at a faster pace, e.g. a grave
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ProRottingContainerComponent : Component
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float DecayModifier = 65f;
}
