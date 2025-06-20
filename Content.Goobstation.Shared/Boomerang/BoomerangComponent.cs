// SPDX-FileCopyrightText: 65 ActiveMammmoth <65ActiveMammmoth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 ActiveMammmoth <kmcsmooth@gmail.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
// SPDX-FileCopyrightText: 65 keronshb <65keronshb@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Boomerang;

[NetworkedComponent, RegisterComponent, AutoGenerateComponentState]
public sealed partial class BoomerangComponent : Component
{
    /// <summary>
    /// The entity that threw this boomerang
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? Thrower;

    [DataField, AutoNetworkedField]
    public TimeSpan? TimeToReturn = TimeSpan.Zero;


    [DataField, AutoNetworkedField]
    public bool SendBack = false;
}