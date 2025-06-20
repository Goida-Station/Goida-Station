// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gluesniffler <65gluesniffler@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Lavaland.Components;

/// <summary>
/// Marker component for shuttle weaponry to prevent cheesing hierophant.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class UnmannedWeaponryComponent : Component;