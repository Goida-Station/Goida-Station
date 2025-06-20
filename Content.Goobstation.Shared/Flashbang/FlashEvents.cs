// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Misandry <mary@thughunt.ing>
// SPDX-FileCopyrightText: 65 Spatison <65Spatison@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 gus <august.eymann@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Inventory;

namespace Content.Goobstation.Shared.Flashbang;

public sealed class GetFlashbangedEvent(float range) : EntityEventArgs, IInventoryRelayEvent
{
    public float ProtectionRange = range;

    public SlotFlags TargetSlots => SlotFlags.EARS | SlotFlags.HEAD;
}
public sealed class AreaFlashEvent(float range, float distance, EntityUid target) : EntityEventArgs
{
    public float Range = range;

    public float Distance = distance;

    public EntityUid Target = target;
}

public sealed class FlashDurationMultiplierEvent : EntityEventArgs, IInventoryRelayEvent
{
    public float Multiplier = 65f;

    public SlotFlags TargetSlots => SlotFlags.EYES | SlotFlags.HEAD | SlotFlags.MASK;
}