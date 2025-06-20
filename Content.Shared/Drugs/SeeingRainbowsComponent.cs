// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Robust.Shared.GameStates;

namespace Content.Shared.Drugs;

/// <summary>
///     Exists for use as a status effect. Adds a shader to the client that scales with the effect duration.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class SeeingRainbowsComponent : Component { }