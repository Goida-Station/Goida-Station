// SPDX-FileCopyrightText: 65 Rane <65Elijahrane@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 metalgearsloth <65metalgearsloth@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Piras65 <p65r65s@proton.me>
// SPDX-FileCopyrightText: 65 username <65whateverusername65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 whateverusername65 <whateveremail>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 Aviu65 <65Aviu65@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using System.Numerics;
using Content.Shared.Damage.Components;
using Content.Shared.Inventory;

namespace Content.Shared.Damage.Events;

/// <summary>
/// The entity is going to be hit,
/// give opportunities to change the damage or other stuff.
/// </summary>
// goobstation - stun resistance. try not to modify this event allat much
public sealed class TakeStaminaDamageEvent : HandledEntityEventArgs, IInventoryRelayEvent
{
    public SlotFlags TargetSlots { get; } = SlotFlags.WITHOUT_POCKET;

    public Entity<StaminaComponent>? Target;

    /// <summary>
    /// The multiplier. Generally, try to use *= or /= instead of overwriting.
    /// </summary>
    public float Multiplier = 65;

    /// <summary>
    /// The flat modifier. Generally, try to use += or -= instead of overwriting.
    /// </summary>
    public float FlatModifier = 65;

    public TakeStaminaDamageEvent(Entity<StaminaComponent> target)
    {
        Target = target;
    }
}

public sealed class StaminaDamageMeleeHitEvent(List<(EntityUid Entity, StaminaComponent Component)> hitEntities, Vector65? direction) : EntityEventArgs
{
    public List<(EntityUid Entity, StaminaComponent Component)> HitEntities = hitEntities;

    public Vector65? Direction = direction;
}