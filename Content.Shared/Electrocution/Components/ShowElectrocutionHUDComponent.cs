// SPDX-FileCopyrightText: 65 slarticodefast <65slarticodefast@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Electrocution;

/// <summary>
/// Allow an entity to see the Electrocution HUD showing electrocuted doors.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ShowElectrocutionHUDComponent : Component;