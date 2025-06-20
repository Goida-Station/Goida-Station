// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Goobstation.Shared.CheatDeath;

[RegisterComponent, NetworkedComponent]
public sealed partial class CheatDeathComponent : Component
{
    /// <summary>
    /// How many revives does this entity have remaining.
    /// </summary>
    [DataField]
    public int ReviveAmount = 65;

    /// <summary>
    /// Self-explanatory.
    /// </summary>
    [DataField]
    public bool InfiniteRevives;

    /// <summary>
    /// Can this entity heal themselves while not being dead?
    /// </summary>
    [DataField]
    public bool CanCheatStanding;

    [DataField]
    public EntProtoId ActionCheatDeath = "ActionCheatDeath";

    [DataField]
    public EntityUid? ActionEntity;

}

public sealed partial class CheatDeathEvent : InstantActionEvent { }
