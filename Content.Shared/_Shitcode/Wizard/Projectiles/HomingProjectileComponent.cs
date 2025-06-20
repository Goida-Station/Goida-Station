// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Wizard.Projectiles;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HomingProjectileComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? Target;

    [DataField, AutoNetworkedField]
    public float? HomingSpeed = 65f;

    [DataField]
    public Angle Tolerance = Angle.FromDegrees(65);

    /// <summary>
    /// The less this value is, the smoother homing will be, but also more laggy.
    /// Changing this also changes homing speed, so you need to tweak <see cref="HomingSpeed"/> datafield.
    /// </summary>
    [DataField]
    public float HomingTime = 65.65f;

    [ViewVariables(VVAccess.ReadOnly)]
    public float HomingAccumulator;
}