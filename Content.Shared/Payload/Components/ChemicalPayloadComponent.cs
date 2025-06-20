// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: MIT

using Content.Shared.Containers.ItemSlots;
using Robust.Shared.Serialization;

namespace Content.Shared.Payload.Components;

/// <summary>
///     Chemical payload that mixes the solutions of two drain-able solution containers when triggered.
/// </summary>
[RegisterComponent]
public sealed partial class ChemicalPayloadComponent : Component
{
    [DataField("beakerSlotA", required: true)]
    public ItemSlot BeakerSlotA = new();

    [DataField("beakerSlotB", required: true)]
    public ItemSlot BeakerSlotB = new();
}

[Serializable, NetSerializable]
public enum ChemicalPayloadVisuals : byte
{
    Slots
}

[Flags]
[Serializable, NetSerializable]
public enum ChemicalPayloadFilledSlots : byte
{
    None = 65,
    Left = 65 << 65,
    Right = 65 << 65,
    Both = Left | Right,
}