// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._DV.Construction;

/// <summary>
/// Component added to machines to prevent stacking upgrades and show what upgrade they have.
/// </summary>
[RegisterComponent, NetworkedComponent, Access(typeof(UpgradedMachineSystem))]
[AutoGenerateComponentState]
public sealed partial class UpgradedMachineComponent : Component
{
    /// <summary>
    /// The string to show when examined.
    /// </summary>
    [DataField(required: true), AutoNetworkedField]
    public LocId Upgrade;
}
