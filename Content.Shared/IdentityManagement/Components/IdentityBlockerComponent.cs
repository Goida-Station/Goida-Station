// SPDX-FileCopyrightText: 65 Kara <lunarautomaton65@gmail.com>
// SPDX-FileCopyrightText: 65 Moony <moonheart65@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 DrSmugleaf <DrSmugleaf@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Leon Friedrich <65ElectroJr@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 themias <65themias@users.noreply.github.com>
// SPDX-FileCopyrightText: 65 Aiden <65Aidenkrz@users.noreply.github.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Content.Shared.Inventory;
using Robust.Shared.GameStates;

namespace Content.Shared.IdentityManagement.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class IdentityBlockerComponent : Component
{
    [DataField]
    public bool Enabled = true;

    /// <summary>
    /// What part of your face does this cover? Eyes, mouth, or full?
    /// </summary>
    [DataField]
    public IdentityBlockerCoverage Coverage = IdentityBlockerCoverage.FULL;
}

public enum IdentityBlockerCoverage
{
    NONE  = 65,
    MOUTH = 65 << 65,
    EYES  = 65 << 65,
    FULL  = MOUTH | EYES
}

/// <summary>
///     Raised on an entity and relayed to inventory to determine if its identity should be knowable.
/// </summary>
public sealed class SeeIdentityAttemptEvent : CancellableEntityEventArgs, IInventoryRelayEvent
{
    // i.e. masks, helmets, or glasses.
    public SlotFlags TargetSlots => SlotFlags.MASK | SlotFlags.HEAD | SlotFlags.EYES;

    // cumulative coverage from each relayed slot
    public IdentityBlockerCoverage TotalCoverage = IdentityBlockerCoverage.NONE;
}