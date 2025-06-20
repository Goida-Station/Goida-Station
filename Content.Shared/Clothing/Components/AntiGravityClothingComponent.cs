// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Clothing.Components;

/// <summary>
/// This is used for clothing that makes an entity weightless when worn.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class AntiGravityClothingComponent : Component;