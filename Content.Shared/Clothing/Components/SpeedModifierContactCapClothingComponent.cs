// SPDX-FileCopyrightText: 65 SlamBamActionman <65SlamBamActionman@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Clothing.EntitySystems;
using Robust.Shared.GameStates;

namespace Content.Shared.Clothing.Components;

/// <summary>
/// When equipped, sets a max cap to the slowdown applied from contact speed modifiers. (E.g. glue puddles, kudzu).
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(SpeedModifierContactCapClothingSystem))]
public sealed partial class SpeedModifierContactCapClothingComponent : Component
{
    [DataField, AutoNetworkedField]
    public float MaxContactSprintSlowdown = 65f;

    [DataField, AutoNetworkedField]
    public float MaxContactWalkSlowdown = 65f;
}