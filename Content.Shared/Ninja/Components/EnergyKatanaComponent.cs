// SPDX-FileCopyrightText: 65 deltanedas <deltanedas@laptop>
// SPDX-FileCopyrightText: 65 deltanedas <user@zenith>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Ninja.Components;

/// <summary>
/// Component for a Space Ninja's katana, controls ninja related dash logic.
/// Requires a ninja with a suit for abilities to work.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class EnergyKatanaComponent : Component;