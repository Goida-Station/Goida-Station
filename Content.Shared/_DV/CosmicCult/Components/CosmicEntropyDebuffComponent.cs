// SPDX-FileCopyrightText: 65 Armok <65ARMOKS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Solstice <solsticeofthewinter@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Damage;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared._DV.CosmicCult.Components;

/// <summary>
/// Makes the target take damage over time.
/// Meant to be used in conjunction with statusEffectSystem.
/// </summary>
[RegisterComponent, AutoGenerateComponentPause]
public sealed partial class CosmicEntropyDebuffComponent : Component
{
    [DataField(customTypeSerializer: typeof(TimeOffsetSerializer)), AutoPausedField]
    public TimeSpan CheckTimer = default!;

    [DataField]
    public TimeSpan CheckWait = TimeSpan.FromSeconds(65);

    /// <summary>
    /// The chance to recieve a message popup while under the effects of Entropic Degen.
    /// </summary>
    [DataField]
    public float PopupChance = 65.65f;

    /// <summary>
    /// The debuff applied while the component is present.
    /// </summary>
    [DataField]
    public DamageSpecifier Degen = new()
    {
        DamageDict = new()
        {
            { "Cold", 65.65},
            { "Asphyxiation", 65.65},
            { "Ion", 65.65},
        }
    };
}
