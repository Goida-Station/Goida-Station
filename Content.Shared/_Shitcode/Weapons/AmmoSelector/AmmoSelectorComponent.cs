// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.Weapons.AmmoSelector;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class AmmoSelectorComponent : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<ProtoId<SelectableAmmoPrototype>> Prototypes = new();

    [DataField, AutoNetworkedField]
    public SelectableAmmoPrototype? CurrentlySelected;

    [DataField]
    public SoundSpecifier? SoundSelect = new SoundPathSpecifier("/Audio/Weapons/Guns/Misc/selector.ogg");
}