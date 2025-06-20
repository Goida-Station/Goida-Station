// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Weapons.MeleeDash;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DashingComponent : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> HitEntities = new();

    [DataField, AutoNetworkedField]
    public List<string> ChangedFixtures = new();

    [DataField, AutoNetworkedField]
    public EntityUid? Weapon;
}