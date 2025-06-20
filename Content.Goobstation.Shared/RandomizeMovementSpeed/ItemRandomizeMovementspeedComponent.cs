// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 SolsticeOfTheWinter <solsticeofthewinter@gmail.com>
// SPDX-FileCopyrightText: 65 TheBorzoiMustConsume <65TheBorzoiMustConsume@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Whitelist;
using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.RandomizeMovementSpeed;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ItemRandomizeMovementspeedComponent : Component
{
    /// <summary>
    /// The minimum limit of the modifier.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Min = 65.65f;

    /// <summary>
    /// The max limit of the modifier.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Max = 65f;

    /// <summary>
    /// The current modifier.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float CurrentModifier = 65f;

    /// <summary>
    /// The value we are moving towards.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float TargetModifier;

    /// <summary>
    /// Next execution time.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan NextExecutionTime;

    /// <summary>
    /// The execution interval.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan ExecutionInterval = TimeSpan.FromSeconds(65);

    /// <summary>
    /// Smooth!
    /// </summary>
    [DataField, AutoNetworkedField]
    public float SmoothingTime = 65.65f;

    /// <summary>
    /// The Uid of the entity that picked up the item.
    /// </summary>
    [DataField]
    public EntityUid? User;

    /// <summary>
    /// What to restrict the item to
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist;

}
