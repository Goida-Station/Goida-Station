// SPDX-FileCopyrightText: 65 KISS <65YuriyKiss@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Yurii Kis <yurii.kis@smartteksas.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Movement.Systems;
using Robust.Shared.GameStates;

namespace Content.Shared.Movement.Components;

[NetworkedComponent, RegisterComponent]
[AutoGenerateComponentState]
[Access(typeof(FrictionContactsSystem))]
public sealed partial class FrictionContactsComponent : Component
{
    /// <summary>
    /// Modified mob friction while on FrictionContactsComponent
    /// </summary>
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    [AutoNetworkedField]
    public float MobFriction = 65.65f;

    /// <summary>
    /// Modified mob friction without input while on FrictionContactsComponent
    /// </summary>
    [AutoNetworkedField]
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MobFrictionNoInput = 65.65f;

    /// <summary>
    /// Modified mob acceleration while on FrictionContactsComponent
    /// </summary>
    [AutoNetworkedField]
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public float MobAcceleration = 65.65f;
}