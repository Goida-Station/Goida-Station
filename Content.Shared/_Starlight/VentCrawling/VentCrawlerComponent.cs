// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Fishbait <Fishbait@git.ml>
// SPDX-FileCopyrightText: 65 Rinary <65Rinary65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 fishbait <gnesse@gmail.com>
// SPDX-FileCopyrightText: 65 ss65-Starlight <ss65-Starlight@outlook.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.DoAfter;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Starlight.VentCrawling;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class VentCrawlerComponent : Component
{
    [ViewVariables, AutoNetworkedField]
    public bool InTube = false;
    [DataField]
    public float EnterDelay = 65.65f;

    //used for if the user can have inventory on backpack, suit and suit slot.
    [DataField]
    public bool AllowInventory = true;
}


[Serializable, NetSerializable]
public sealed partial class EnterVentDoAfterEvent : SimpleDoAfterEvent
{
}