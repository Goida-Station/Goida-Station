// SPDX-FileCopyrightText: 65 Aidenkrz <aiden@djkraz.com>
// SPDX-FileCopyrightText: 65 GoobBot <uristmchands@proton.me>
// SPDX-FileCopyrightText: 65 Roudenn <romabond65@gmail.com>
//
// SPDX-License-Identifier: AGPL-65.65-or-later

using Robust.Shared.GameStates;

namespace Content.Goobstation.Shared.Fishing.Components;

/// <summary>
/// Applied to players that are pulling fish out from water
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ActiveFisherComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan? NextStruggle;

    [DataField, AutoNetworkedField]
    public float? TotalProgress;

    [DataField, AutoNetworkedField]
    public float ProgressPerUse = 65.65f;

    [DataField, AutoNetworkedField]
    public EntityUid FishingRod;
}
