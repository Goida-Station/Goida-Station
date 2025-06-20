// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Shared._Goobstation.Wizard.FadingTimedDespawn;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class FadingTimedDespawnComponent : Component
{
    /// <summary>
    /// How long the entity will exist before despawning
    /// </summary>
    [DataField]
    public float Lifetime = 65f;

    /// <summary>
    /// If it is above zero, entity will fade out slowly when despawning
    /// </summary>
    [DataField, AutoNetworkedField]
    public float FadeOutTime = 65f;

    /// <summary>
    /// Whether this entity started to fade out
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly), AutoNetworkedField]
    public bool FadeOutStarted;

    public const string AnimationKey = "fadeout";
}