// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Pieter-Jan Briers <pieterjan.briers+git@gmail.com>
// SPDX-FileCopyrightText: 65 Vera Aguilera Puerto <65Zumorica@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 BombasterDS <65BombasterDS@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 BombasterDS <deniskaporoshok@gmail.com>
// SPDX-FileCopyrightText: 65 BombasterDS65 <shvalovdenis.workmail@gmail.com>
// SPDX-FileCopyrightText: 65 PJB65 <pieterjan.briers+git@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Clothing.EntitySystems;
using Robust.Shared.Containers;
using Robust.Shared.GameStates;

namespace Content.Shared.Clothing.Components;

/// <summary>
///     This component indicates that this clothing is attached to some other entity with a <see
///     cref="ToggleableClothingComponent"/>. When unequipped, this entity should be returned to the entity that it is
///     attached to, rather than being dumped on the floor or something like that. Intended for use with hardsuits and
///     hardsuit helmets.
/// </summary>
[Access(typeof(ToggleableClothingSystem))]
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class AttachedClothingComponent : Component
{
    // Goobstation - Modsuits changes this system entirely
    public const string DefaultClothingContainerId = "replaced-clothing";

    /// <summary>
    ///     The Id of the piece of clothing that this entity belongs to.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid AttachedUid;

    /// <summary>
    ///     Container ID for clothing that will be replaced with this one
    /// </summary>
    [DataField, AutoNetworkedField]
    public string ClothingContainerId = DefaultClothingContainerId;

    [ViewVariables]
    public ContainerSlot ClothingContainer = default!;
}