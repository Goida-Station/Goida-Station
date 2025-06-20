// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Humanoid;
using Robust.Shared.Enums;
using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Wizard.BindSoul;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SoulBoundComponent : Component
{
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? Item;

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public EntityUid? MapId;

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public string Name = string.Empty;

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public int? Age = null;

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public Gender? Gender = null;

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public Sex? Sex = null;

    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public int ResurrectionsCount;
}