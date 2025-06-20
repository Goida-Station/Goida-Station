// SPDX-FileCopyrightText: 65 Nemanja <65EmoGarbage65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Interaction.Components;

/// <summary>
/// This is used for identifying entities as being able to use complex interactions with the environment.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(SharedInteractionSystem))]
public sealed partial class ComplexInteractionComponent : Component;