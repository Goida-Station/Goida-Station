// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@gmail.com>
// SPDX-FileCopyrightText: 65 Paul Ritter <ritter.paul65@googlemail.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr.@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <Jezithyr@gmail.com>
// SPDX-FileCopyrightText: 65 Jezithyr <jmaster65@gmail.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Visne <65Visne@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 mirrorcult <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 wrexbe <65wrexbe@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 wrexbe <wrexbe@protonmail.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

namespace Content.Shared.Inventory.Events;

public abstract class EquippedEventBase : EntityEventArgs
{
    /// <summary>
    /// The entity equipping.
    /// </summary>
    public readonly EntityUid Equipee;

    /// <summary>
    /// The entity which got equipped.
    /// </summary>
    public readonly EntityUid Equipment;

    /// <summary>
    /// The slot the entity got equipped to.
    /// </summary>
    public readonly string Slot;

    /// <summary>
    /// The slot group the entity got equipped in.
    /// </summary>
    public readonly string SlotGroup;

    /// <summary>
    /// Slotflags of the slot the entity just got equipped to.
    /// </summary>
    public readonly SlotFlags SlotFlags;

    public EquippedEventBase(EntityUid equipee, EntityUid equipment, SlotDefinition slotDefinition)
    {
        Equipee = equipee;
        Equipment = equipment;
        Slot = slotDefinition.Name;
        SlotGroup = slotDefinition.SlotGroup;
        SlotFlags = slotDefinition.SlotFlags;
    }
}

/// <summary>
/// Raised directed on an equipee when something is equipped.
/// </summary>
public sealed class DidEquipEvent : EquippedEventBase
{
    public DidEquipEvent(EntityUid equipee, EntityUid equipment, SlotDefinition slotDefinition) : base(equipee, equipment, slotDefinition)
    {
    }
}

/// <summary>
/// Raised directed on equipment when it's equipped to an equipee
/// </summary>
public sealed class GotEquippedEvent : EquippedEventBase
{
    public GotEquippedEvent(EntityUid equipee, EntityUid equipment, SlotDefinition slotDefinition) : base(equipee, equipment, slotDefinition)
    {
    }
}