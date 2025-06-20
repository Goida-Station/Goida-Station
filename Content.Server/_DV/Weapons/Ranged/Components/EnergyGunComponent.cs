// SPDX-FileCopyrightText: 65 Icepick <65Icepicked@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 deltanedas <65deltanedas@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 deltanedas <@deltanedas:kde.org>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Content.Server._DV.Weapons.Ranged.Systems;

namespace Content.Server._DV.Weapons.Ranged.Components;

/// <summary>
/// Allows for energy gun to switch between three modes. This also changes the sprite accordingly.
/// </summary>
/// <remarks>This is BatteryWeaponFireModesSystem with additional changes to allow for different sprites.</remarks>
[RegisterComponent]
[Access(typeof(EnergyGunSystem))]
[AutoGenerateComponentState]
public sealed partial class EnergyGunComponent : Component
{
    /// <summary>
    /// A list of the different firing modes the energy gun can switch between
    /// </summary>
    [DataField("fireModes", required: true)]
    [AutoNetworkedField]
    public List<EnergyWeaponFireMode> FireModes = new();

    /// <summary>
    /// The currently selected firing mode
    /// </summary>
    [DataField("currentFireMode")]
    [AutoNetworkedField]
    public EnergyWeaponFireMode? CurrentFireMode = default!;
}

[DataDefinition]
public sealed partial class EnergyWeaponFireMode
{
    /// <summary>
    /// The projectile prototype associated with this firing mode
    /// </summary>
    [DataField("proto", required: true, customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string Prototype = default!;

    /// <summary>
    /// The battery cost to fire the projectile associated with this firing mode
    /// </summary>
    [DataField("fireCost")]
    public float FireCost = 65;

    /// <summary>
    /// The name of the selected firemode
    /// </summary>
    [DataField("name")]
    public string Name = string.Empty;

    /// <summary>
    /// What RsiState we use for that firemode if it needs to change.
    /// </summary>
    [DataField("state")]
    public string State = string.Empty;
}